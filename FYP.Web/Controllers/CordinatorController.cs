using FYP.Databases;
using FYP.Entities;
using FYP.Models;
using FYP.Services;
using FYP.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing;
using System.Globalization;
using System.Security.Claims;

namespace FYP.Web.Controllers
{
    public class CordinatorController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoomAllotmentService _supervisorService;
        private readonly IProposalDefenseService _proposalDefenseService;
        private readonly IRoomService _designationService;
        private readonly IStudentGroupService _studentGroupService;
        private readonly IProjectService _projectService;
        private readonly ICompanyService _companyService;
        private readonly IEvaluationService _evaluationService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFYPCommitteService _fYPCommitteService;
        private readonly INotificationsService _notificationsService;
        private readonly IEvaluationCriteriaService _evaluationCriteriaService;

        public CordinatorController(IEvaluationCriteriaService evaluationCriteriaService,INotificationsService notificationsService, IFYPCommitteService fYPCommitteService, IEvaluationService evaluationService, ICompanyService companyService, IProjectService projectService, IUserService userService, UserManager<AppUser> userManager, IRoomAllotmentService supervisorService, IProposalDefenseService proposalDefense, IRoomService designationService, IStudentGroupService studentGroupService)
        {
            _userService = userService;
            _userManager = userManager;
            _proposalDefenseService = proposalDefense;
            _supervisorService = supervisorService;
            _studentGroupService = studentGroupService;
            _designationService = designationService;
            _projectService = projectService;
            _companyService = companyService;
            _evaluationService = evaluationService;
            _fYPCommitteService = fYPCommitteService;
            _notificationsService = notificationsService;
            _evaluationCriteriaService = evaluationCriteriaService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> StudentGroup()
        {
            var studentGroup = await _studentGroupService.GetAllAsync();
            var existingFypCommitee = await _fYPCommitteService.GetAllAsync();
            var user = studentGroup.FirstOrDefault();
            var allGroups = await _studentGroupService.GetAllAsync();
            if (user != null)
            {
                var userGroup = allGroups
                    .Where(x => x.student1LID == user.student1LID || x.student2ID == user.student2ID || x.student3ID == user.student3ID)
                    .FirstOrDefault();
                var supervisorId = userGroup.SupervisorID;
            }
            var distinctBatches = studentGroup.Select(x => x.Batch).Distinct().ToList();
            var distinctYears = studentGroup.Select(x => x.Year).Distinct().ToList();
            var model = new StudentGroupViewModel();
            if (studentGroup != null)
            {
                model.StudentGroups = studentGroup.Select(x => new StudentGroupViewModel
                {
                    Id = x.ID,
                    Name = x.Name,
                    Batch = x.Batch,
                    student1LID = x.student1LID,
                    student2ID = x.student2ID,
                    student3ID = x.student3ID,
                    CoSupervisorID = x.CoSupervisorID,
                    SupervisorID = x.SupervisorID,
                    supervisorname = _userService.GetAllAsync().Result.Where(y => y.Id == x.SupervisorID).FirstOrDefault().Name,
                    LeaderName = _userManager.FindByIdAsync(x.student1LID).Result?.Email,
                    member1 = _userManager.FindByIdAsync(x.student2ID).Result?.Email,
                    Member2 = _userManager.FindByIdAsync(x.student3ID).Result?.Email,
                }).ToList();
            }
            if (distinctBatches != null)
            {
                model.Batches = distinctBatches;
            }
            if (distinctYears != null)
            {
                model.Years = distinctYears;
            }


            return PartialView(model);
        }
        public async Task<IActionResult> Projects()
        {
            var LoggedInUser = _userManager.GetUserId(User);
            var FetchUser = await _userManager.FindByIdAsync(LoggedInUser);
            var FetchUserDepartment = FetchUser.Department;
            var studentGroup = await _studentGroupService.GetAllAsync();
            var user = studentGroup.FirstOrDefault();
            var allGroups = await _studentGroupService.GetAllAsync();
            var userGroup = allGroups
                .Where(x => x.student1LID == user.student1LID || x.student2ID == user.student2ID || x.student3ID == user.student3ID)
                .FirstOrDefault();
            var supervisorId = userGroup.SupervisorID;
            var project = await _projectService.GetAllAsync();
            var projects = project.Where(x => x.SupervsiorApproved == "Approved" && x.projectGroup == FetchUserDepartment);
            var distinctBatches = projects.Select(x => x.batch).Distinct().ToList();

            var model = new ProjectViewModel
            {
                projects = projects.Select(x => new ProjectViewModel
                {
                    ProId = x.ID.ToString(),
                    code = x.code,
                    Specialization = x.Specialization,
                    Status = x.Status,
                    Summary = x.Summary,
                    commiteeId = x.commiteeId,
                    companyID = x.companyID,
                    ExpectedResults = x.ExpectedResults,
                    groupId = x.groupId,
                    objectives = x.objectives,
                    Others = x.Others,
                    Id = x.ID,
                    Title = x.Title,
                    Tools = x.Tools,
                    ProjectCategory = x.ProjectCategory,
                    projectGroup = x.projectGroup,
                    batch = x.batch,
                    groupname = userGroup.Name,
                    supervisorname = _userManager.FindByIdAsync(supervisorId).Result?.Name,
                    changeTitleFormStatus = x.changeTitleFormStatus,

                }).ToList(),


            };
            if (distinctBatches != null)
            {
                model.batches = distinctBatches;
            }

            return PartialView(model);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeStatus(string Id, string status, string Data)
        {
            // Fetch the project by ID
            var project = (await _projectService.GetAllAsync()).FirstOrDefault(x => x.ID.ToString() == Id);

            if (project == null)
            {
                // Handle the case where the project is not found
                return NotFound();
            }

            // Change the status of the project
            project.Status = status;

            // Update the project asynchronously
            var result = _projectService.UpdateAsync(project);

            if (result.IsCompletedSuccessfully)
            {
                if (Data == "NG")
                {
                     return RedirectToAction("Projects", "Supervisor");
                }
                else
                {
                    return RedirectToAction("StudentGroupAlongMarks", "cordinator");

                }
            }

            // In case the update fails, handle it accordingly (e.g., show an error message)
            return BadRequest("Failed to update the project status.");
        }


        public async Task<IActionResult> StudentGroupbyBatch(string batch)
        {
            var studentGroups = (await _studentGroupService.GetAllAsync())
                                .Where(x => x.Batch == batch);
            var existingFypCommitee = await _fYPCommitteService.GetAllAsync();

            var studentGroup = (await _studentGroupService.GetAllAsync());
            var user = studentGroups.FirstOrDefault();
            if (user == null)
            {
                return BadRequest("User not found.");
            }   

            var allGroups = await _studentGroupService.GetAllAsync();
            var userGroup = allGroups
                .Where(x => x.student1LID == user.student1LID || x.student2ID == user.student2ID || x.student3ID == user.student3ID)
                .FirstOrDefault();
            var distinctBatches = studentGroup.Select(x => x.Batch).Distinct().ToList();
            var distinctYears = studentGroup.Select(x => x.Year).Distinct().ToList();

            if (userGroup == null)
            {
                return BadRequest("User group not found.");
            }

            var supervisorName = (await _userManager.FindByIdAsync(userGroup.SupervisorID))?.Name;
            var leaderName = (await _userManager.FindByIdAsync(user.student1LID))?.Name;
            var member1Name = (await _userManager.FindByIdAsync(user.student2ID))?.Name;
            var member2Name = (await _userManager.FindByIdAsync(user.student3ID))?.Name;

            var model = new StudentGroupViewModel()
            {
                StudentGroups = studentGroups.Select(x => new StudentGroupViewModel
                {
                    Id = x.ID,
                    Name = x.Name,
                    Batch = x.Batch,
                    student1LID = x.student1LID,
                    student2ID = x.student2ID,
                    student3ID = x.student3ID,
                    CoSupervisorID = x.CoSupervisorID,
                    SupervisorID = x.SupervisorID,
                    supervisorname = supervisorName,
                    LeaderName = leaderName,
                    member1 = member1Name,
                    Member2 = member2Name,
                    Member1Name = _userManager.FindByIdAsync(existingFypCommitee
                .Where(y => y.groupID == x.ID.ToString())
                .FirstOrDefault()?.Member1ID).Result?.Name ?? "Not Assigned",

                    Member2Name = _userManager.FindByIdAsync(existingFypCommitee
                .Where(y => y.groupID == x.ID.ToString())
                .FirstOrDefault()?.Member2ID).Result?.Name ?? "Not Assigned",
                }).ToList(),
                Batches = distinctBatches,
                Years = distinctYears

            };

            return PartialView("StudentGroup", model);
        }
        public IActionResult ProjectGroupbyBatch(string batch)
        {
            var studentGroup = _studentGroupService.GetAllAsync().Result;
            var user = studentGroup.FirstOrDefault();
            var allGroups = _studentGroupService.GetAllAsync().Result;
            var userGroup = allGroups
                .Where(x => x.student1LID == user.student1LID || x.student2ID == user.student2ID || x.student3ID == user.student3ID)
                .FirstOrDefault();
            var supervisorId = userGroup.SupervisorID;

            var projects = _projectService.GetAllAsync().Result.Where(x => x.batch == batch);
            var distinctBatches = projects.Select(x => x.batch).Distinct().ToList();
            var model = new ProjectViewModel
            {
                projects = projects.Select(x => new ProjectViewModel
                {
                    code = x.code,
                    Specialization = x.Specialization,
                    Status = x.Status,
                    Summary = x.Summary,
                    commiteeId = x.commiteeId,
                    companyID = x.companyID,
                    ExpectedResults = x.ExpectedResults,
                    groupId = x.groupId,
                    objectives = x.objectives,
                    Others = x.Others,
                    Id = x.ID,
                    Title = x.Title,
                    Tools = x.Tools,
                    ProjectCategory = x.ProjectCategory,
                    projectGroup = x.projectGroup,
                    batch = x.batch,
                    groupname = userGroup.Name,
                    supervisorname = _userManager.FindByIdAsync(supervisorId).Result?.Name
                }).ToList(),
                batches = distinctBatches
            };
            return PartialView("projects", model);
        }

        public async Task<IActionResult> Evaluation(string Etype, int marks, string batch, string ViewValue)
        {

            var group = await _studentGroupService.GetAllAsync();

            if (group != null && group.Any()) // Check if group is not null and has data
            {
                var distinctBatches = group.Select(x => x.Batch).Distinct().ToList();

                // Get the most recent batch (if any)
                var fetchLastBatch = group.OrderByDescending(x => x.Batch).FirstOrDefault();

                var existingEvaluations = _evaluationService.GetAllAsync().Result
                    .FirstOrDefault(e => e.PBatch == fetchLastBatch?.Batch); // Safe check for null fetchLastBatch

                // Initialize the model with either the fetched data or default values
                var model = new EvaluationViewModel()
                {
                    EvaluationName = existingEvaluations?.EvaluationName ?? "Proposal", // Default value
                    batches = distinctBatches,
                    PBatch = existingEvaluations?.PBatch ?? "", // Default value
                    LastDate = existingEvaluations?.LastDate ?? DateTime.Now, // Default value
                };
                var Evaluation = await _evaluationService.GetAllAsync();
                if (Evaluation != null)
                {
                    model.EvaluationList = Evaluation.Select(x => new EvaluationViewModel
                    {
                        EvaluationName = x.EvaluationName,
                        PBatch = x.PBatch,
                        LastDate = x.LastDate
                    }).ToList();
                }
                if (ViewValue != null)
                {
                    model.success = ViewValue;
                }

                if (!string.IsNullOrEmpty(batch) && !string.IsNullOrEmpty(Etype))
                {
                    var existingEvaluation = _evaluationService.GetAllAsync().Result
                        .FirstOrDefault(e => e.PBatch == batch && e.EvaluationName == Etype);

                    if (existingEvaluation != null) // Make sure the existing evaluation is found before updating the model
                    {
                        model.EvaluationName = existingEvaluation.EvaluationName;
                        model.Marks = existingEvaluation.Marks;
                        model.PBatch = existingEvaluation.PBatch;
                        model.LastDate = existingEvaluation.LastDate;
                    }
                    model.EvaluationList = Evaluation.Where(x => x.PBatch == batch).Select(x => new EvaluationViewModel
                    {
                        EvaluationName = x.EvaluationName,
                        PBatch = x.PBatch,
                        LastDate = x.LastDate
                    }).ToList();
                }

                return PartialView(model);
            }

            // Return empty model if no group found
            
            var emptyModel = new EvaluationViewModel
            {
                batches = null,
                PBatch = null,
                LastDate = DateTime.Now
            };

            return PartialView(emptyModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetEvaluationData(string batch, string evaluationName)
        {

            if (batch != null && evaluationName != null)
            {
                // Fetch evaluation data for the selected batch and EvaluationName from the database
                var evaluation = (await _evaluationService.GetAllAsync())
                                .FirstOrDefault(e => e.PBatch == batch && e.EvaluationName == evaluationName);
                if (evaluation != null)
                {
                    return Json(new
                    {
                        success = true,
                        marks = evaluation.Marks,
                        batch = evaluation.PBatch,
                        lastDate = evaluation.LastDate.ToString("dd/MM/yyyy")  // Format as dd/MM/yyyy
                    });
                }
            }
            else
            {

                return Json(new
                {
                    success = false,
                    lastDate = DateTime.Now.ToString("dd/MM/yyyy")  // Format as dd/MM/yyyy
                });
            }
            return Json(new { success = false });

        }

        [HttpPost]
        public IActionResult Evaluation([FromBody] EvaluationViewModel model)
        {

            // Check if the evaluation exists in the database by ID
            var existingEvaluation = _evaluationService.GetAllAsync().Result
                                        .FirstOrDefault(e => e.PBatch == model.PBatch && e.EvaluationName == model.EvaluationName);

            if (existingEvaluation != null)
            {
                // Update existing evaluation
                existingEvaluation.Marks = model.Marks;
                existingEvaluation.LastDate = model.LastDate;

                _evaluationService.UpdateAsync(existingEvaluation);
                return RedirectToAction("Evaluation", "Cordinator", new { batch = existingEvaluation.PBatch, Etype = existingEvaluation.EvaluationName, ViewValue = "Updated" });

            }
            else
            {
                // Create new evaluation
                var evaluation = new EvaluationViewModel
                {
                    EvaluationName = model.EvaluationName,
                    Marks = model.Marks,
                    PBatch = model.PBatch,
                    LastDate = model.LastDate
                };
                var evaluate = new Evaluation
                {
                    EvaluationName = evaluation.EvaluationName,
                    Marks = evaluation.Marks,
                    PBatch = evaluation.PBatch,
                    LastDate = evaluation.LastDate
                };
                _evaluationService.AddAsync(evaluate);
                return RedirectToAction("Evaluation", "Cordinator", new { batch = evaluate.PBatch, Etype = evaluate.EvaluationName, ViewValue = "Added" });

            }




        }


        [HttpGet]
        public async Task<IActionResult> ExternalsAccount(string batch, string ViewValue, string Group)
        {


            var userGroup = await _studentGroupService.GetAllAsync();
            var project = await _projectService.GetAllAsync();

            if (userGroup == null || project == null)
                return BadRequest("Failed to fetch data."); // Handle the case when data is null.

            var viewModel = new ExternalEvaluation();
            var fetchLastBatch = userGroup.OrderByDescending(x => x.Batch).FirstOrDefault();
            var distinctBatches = userGroup.Select(x => x.Batch).Distinct().ToList();

            viewModel.Batches = distinctBatches;
            if (ViewValue != null)
            {
                viewModel.successmodel = ViewValue;
            }

            if (!string.IsNullOrEmpty(batch))
            {
                var fetchGroups = userGroup.Where(x => x.Batch == batch).ToList();
                var projects = project.Where(x => x.batch == batch).ToList();

                // Only include groups with associated projects
                viewModel.studentGroups = fetchGroups.Where(group => group.ExternalId == null)
                    .Where(group => projects.Any(p => p.groupId == group.ID.ToString())) // Filter groups with projects
                    .Select(group => new StudentGroupViewModel()
                    {
                        Batch = group.Batch,
                        Name = group.Name + " - " + projects.FirstOrDefault(p => p.groupId == group.ID.ToString())?.Title,
                        Id = group.ID,


                    }).ToList();
                viewModel.ExternalGroups = fetchGroups.Where(group => group.ExternalId != null)
                   .Select(group => new StudentGroupViewModel()
                   {
                       Batch = group.Batch,
                       Name = group.Name,
                       projectname = projects.FirstOrDefault(p => p.groupId == group.ID.ToString())?.Title,
                       Id = group.ID,
                       ExternalId = group.ExternalId,
                       ExternalEmail = _userManager.FindByIdAsync(group.ExternalId).Result?.Email,
                       ActiveState = _userManager.FindByIdAsync(group.ExternalId).Result?.ActiveState,

                   }).ToList();

                viewModel.Batch = batch;

            }



            return PartialView(viewModel); // Pass the view model to the partial view.
        }

        [HttpPost]
        public async Task<IActionResult> ExternalsAccount(ExternalEvaluation model)
        {
            var getUser = await _userManager.FindByEmailAsync(model.Email);
            var studentGroups = await _studentGroupService.GetAllAsync();
            var fypCommitteeGroups = await _fYPCommitteService.GetAllAsync();

            if (getUser != null) // User already exists
            {
                // Check if the user is already assigned to the group
                var isAssignedToStudentGroup = studentGroups.Any(x => x.ExternalId == getUser.Id && x.ID.ToString() == model.Group);
                var isAssignedToFypGroup = fypCommitteeGroups.Any(x => x.ExternalId == getUser.Id && x.groupID.ToString() == model.Group);

                if (isAssignedToStudentGroup || isAssignedToFypGroup)
                {
                    return RedirectToAction("Evaluation", "Cordinator", new { batch = model.Batch, ViewValue = "Existed" });
                }

                // Assign to student group
                var fetchStudentGroup = studentGroups.FirstOrDefault(x => x.ID.ToString() == model.Group);
                if (fetchStudentGroup != null)
                {
                    fetchStudentGroup.ExternalId = getUser.Id;
                    await _studentGroupService.UpdateAsync(fetchStudentGroup);
                }

                // Assign to FYP committee group
                var fetchFypGroup = fypCommitteeGroups.FirstOrDefault(x => x.groupID.ToString() == model.Group && x.EvaluationID == "Final");
                if (fetchFypGroup != null)
                {
                    fetchFypGroup.ExternalId = getUser.Id;
                    await _fYPCommitteService.UpdateAsync(fetchFypGroup);
                }

                return RedirectToAction("ExternalsAccount", "Cordinator", new { batch = model.Batch, ViewValue = "Added" });
            }
            else // Create a new user
            {
                var email = model.Email.Trim().ToLower();
                var nameFromEmail = email.Split('@')[0]; // Extract name from email

                var newUser = new AppUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Name = nameFromEmail,
                    Role = "External",
                    Designation = "External",
                    ActiveState = "Active",
                    DateofCreation = DateOnly.FromDateTime(DateTime.Now),
                };

                var createResult = await _userManager.CreateAsync(newUser, "BahriaExternals123@@");
                if (createResult.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(newUser, Roles.External.ToString());
                    if (roleResult.Succeeded)
                    {
                        var fetchStudentGroup = studentGroups.FirstOrDefault(x => x.ID.ToString() == model.Group);
                        var fetchFypGroup = fypCommitteeGroups.FirstOrDefault(x => x.groupID.ToString() == model.Group && x.EvaluationID == "Final");

                        if (fetchStudentGroup != null && fetchStudentGroup.ExternalId == null)
                        {
                            fetchStudentGroup.ExternalId = newUser.Id;
                            await _studentGroupService.UpdateAsync(fetchStudentGroup);
                        }

                        if (fetchFypGroup != null && fetchFypGroup.ExternalId == null)
                        {
                            fetchFypGroup.ExternalId = newUser.Id;
                            await _fYPCommitteService.UpdateAsync(fetchFypGroup);
                        }

                        return RedirectToAction("ExternalsAccount", "Cordinator", new { batch = model.Batch, ViewValue = "Added" });
                    }
                }
            }

            return RedirectToAction("ExternalsAccount", "Cordinator", new { batch = model.Batch, ViewValue = "Failed" });
        }


        [HttpPost]
        public async Task<IActionResult> EditExternalsAccount(ExternalEvaluation model)
        {
            var user = await _userManager.FindByIdAsync(model.ExternalId.ToString());
            if (model.isBlock == "True")
            {
                if (user != null)
                {

                    user.ActiveState = "Blocked";
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ExternalsAccount", "Cordinator", new { batch = model.Batch, ViewValue = "Updated" });
                    }
                }
            }
            if (model.isBlock == "makeActive")
            {
                if (user != null)
                {

                    user.ActiveState = "Active";
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ExternalsAccount", "Cordinator", new { batch = model.Batch, ViewValue = "Updated" });
                    }
                }
            }
            if (user != null)
            {
                user.DateofCreation = DateOnly.FromDateTime(DateTime.Now);
                user.UserName = model.Email;
                user.Email = model.Email;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ExternalsAccount", "Cordinator", new { batch = model.Batch, ViewValue = "Updated" });
                }
            }
            return RedirectToAction("ExternalsAccount", "Cordinator", new { batch = model.Batch, ViewValue = "NoUser" });


        }

        [HttpGet]
        public async Task<IActionResult> CreateNotifications(string Batch)
        {
            // Fetch only the filtered groups from the service (filter applied at database level)
            var groups = await _studentGroupService.GetAllAsync();
            if (groups != null)
            {
                var filteredGroups = groups.Select(x => x.Batch).Distinct().ToList();
                var model = new NotificationsViewModel();
                model.Batches = filteredGroups;

                return PartialView(model);
            }
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> CreateNotifications([FromBody] NotificationsViewModel model)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var notification = new Notifications
            {
                Subject = model.Subject,
                Message = model.Message,
                UserType = model.UserType,
                Date = DateOnly.FromDateTime(DateTime.Now).ToString(),
                SenderId = currentUser.Id,
                Batch = model.Batch
            };
            await _notificationsService.AddAsync(notification);
            return RedirectToAction("Notifications", "Cordinator", new { Batch = model.Batch });
        }
        [HttpGet]
        public async Task<IActionResult> GetMessageDetails(string id)
        {
            var Fetchnotification = await _notificationsService.GetAllAsync();
            var notification = Fetchnotification.Where(x => x.ID.ToString() == id).FirstOrDefault();
            var model = new NotificationsViewModel();
            model.Message = notification.Message;
            model.Subject = notification.Subject;
            model.Date = notification.Date;


            if (notification == null)
            {
                return NotFound();
            }

            // Return the partial view with the message data
            return PartialView(model);
        }
        [HttpGet]
        public async Task<IActionResult> Notifications()
        {
            var notifications = await _notificationsService.GetAllAsync();
            var studentGroups = await _studentGroupService.GetAllAsync(); // Assuming you have a service to fetch student groups
            var model = new NotificationsViewModel();

            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get current user ID
            string userBatch = studentGroups
                .FirstOrDefault(g => g.student1LID == currentUserId
                                  || g.student2ID == currentUserId
                                  || g.student3ID == currentUserId)
                ?.Batch;

            if (notifications != null && notifications.Any())
            {
                if (User.IsInRole("Student") && !string.IsNullOrEmpty(userBatch))
                {
                    // Only show messages for the student's batch
                    var filteredNotifications = notifications
                        .Where(x => x.Batch == userBatch && x.UserType == "Student")
                        .OrderByDescending(x => x.Date);

                    model.NotificationsViewModels = filteredNotifications.Select(x => new NotificationsViewModel
                    {
                        Id = x.ID,
                        Subject = x.Subject,
                        Message = x.Message,
                        UserType = x.UserType,
                        Date = x.Date,
                        SenderId = x.SenderId,
                        Batch = x.Batch
                    }).ToList();

                }
                else if (User.IsInRole("Supervisor") && !string.IsNullOrEmpty(userBatch))
                {
                    var filteredNotifications = notifications
                       .Where(x => x.Batch == userBatch && x.UserType == "Supervisor")
                       .OrderByDescending(x => x.Date);

                    model.NotificationsViewModels = filteredNotifications.Select(x => new NotificationsViewModel
                    {
                        Id = x.ID,
                        Subject = x.Subject,
                        Message = x.Message,
                        UserType = x.UserType,
                        Date = x.Date,
                        SenderId = x.SenderId,
                        Batch = x.Batch
                    }).ToList();
                }
                else 
                {
                    // Other roles (e.g., Coordinator or Supervisor)
                    var filteredNotifications = notifications.OrderByDescending(x => x.Date);

                    model.NotificationsViewModels = filteredNotifications.Select(x => new NotificationsViewModel
                    {
                        Id = x.ID,
                        Subject = x.Subject,
                        Message = x.Message,
                        UserType = x.UserType,
                        Date = x.Date,
                        SenderId = x.SenderId,
                        Batch = x.Batch
                    }).ToList();
                }
            }

            return PartialView(model);
        }



        [HttpPost]
        public async Task<IActionResult> DeleteMessage(string id)
        {
            var notifications = await _notificationsService.GetAllAsync();
            var fetchNotification = notifications.Where(x => x.ID.ToString() == id).FirstOrDefault();
            if (fetchNotification != null)
            {
                await _notificationsService.DeleteAsync(fetchNotification.ID.ToString());
                return RedirectToAction("Notifications", "Cordinator");
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> SearchMessages(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Search query cannot be empty.");

            var notifications = await _notificationsService.GetAllAsync();

            var filteredNotifications = notifications
                .Where(n =>
                    (n.Message != null && n.Message.Contains(query, StringComparison.OrdinalIgnoreCase)) ||
                    (n.Subject != null && n.Subject.Contains(query, StringComparison.OrdinalIgnoreCase)) ||
                    (n.Date != null && n.Date.Contains(query, StringComparison.OrdinalIgnoreCase))
                )
                .Select(n => new
                {
                    Id = n.ID, // Assuming Id exists in the model
                    n.Subject,
                    n.Message,
                    n.Date
                })
                .ToList();

            return Ok(filteredNotifications);
        }
        [HttpPost]
        public async Task<IActionResult> AssignMarks([FromBody] EvaluationMarksViewModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.GroupId) || model.SupervisorMarks < 0 || model.CordinatorMarks < 0)
            {
                return BadRequest("Invalid data.");
            }

            // Retrieve the group by GroupId
            var evaluation = (await _evaluationCriteriaService.GetAllAsync())
                             .FirstOrDefault(e => e.GId == model.GroupId);

            if (evaluation == null)
            {
                return NotFound("Group not found.");
            }

            // Assign marks based on the role
            if (User.IsInRole("Supervisor"))
            {
                if (model.SupervisorMarks > 20)
                    return BadRequest("Marks exceed maximum allowed for Supervisor.");

                evaluation.SupervisorMarks = model.SupervisorMarks;
            }
            else if (User.IsInRole("Cordinator"))
            {
                if (model.CordinatorMarks > 10)
                    return BadRequest("Marks exceed maximum allowed for Coordinator.");

                evaluation.CordinatorMarks = model.CordinatorMarks;
            }
            else
            {
                return BadRequest("Invalid role.");
            }

            // Save changes to the database
            await _evaluationCriteriaService.UpdateAsync(evaluation);

            return Ok(new
            {
                message = "Marks assigned successfully.",
                evaluation.GId,
                evaluation.SupervisorMarks,
                evaluation.CordinatorMarks
            });
        }

        public async Task<IActionResult> StudentGroupAlongMarks(string batch, string ViewValue)
        {
            if (ViewValue != null)
            {
                ViewBag.success = ViewValue;
            }

            // Fetch all necessary data upfront
            var studentGroups = await _studentGroupService.GetAllAsync();
            var evaluationCriteria = await _evaluationCriteriaService.GetAllAsync();
            var fetchEvaluation = await _evaluationService.GetAllAsync();
            var fetchProjects = await _projectService.GetAllAsync();
            var currentUser = await _userManager.GetUserAsync(User); // Get the current logged-in user
            var userRole = (await _userManager.GetRolesAsync(currentUser)).FirstOrDefault(); // Get the role of the user

            // If a batch is selected, filter the student groups by batch
            if (!string.IsNullOrEmpty(batch))
            {
                studentGroups = studentGroups.Where(x => x.Batch == batch).ToList();
                ViewBag.batch = batch;
            }

            // Initialize the model list
            if (studentGroups != null)
            {
                // Role-specific filtering
                if (userRole == "Supervisor")
                {
                    studentGroups = studentGroups.Where(x => x.SupervisorID == currentUser.Id).ToList();
                }
                else if (userRole == "Student")
                {
                    studentGroups = studentGroups.Where(x =>
                        x.student1LID == currentUser.Id || x.student2ID == currentUser.Id || x.student3ID == currentUser.Id)
                        .ToList();
                }

                // Get distinct batches
                var batches = studentGroups.Select(sg => sg.Batch).Distinct().ToList();

                // Get all user IDs from the groups
                var userIds = studentGroups.SelectMany(x => new[] { x.student1LID, x.student2ID, x.student3ID })
                                           .Where(id => id != null).Distinct();

                // Fetch all users asynchronously
                var users = (await Task.WhenAll(userIds.Select(id => _userManager.FindByIdAsync(id))))
                            .ToDictionary(user => user?.Id, user => user);

                // Construct the view model for each group
                var model = studentGroups.Select(x =>
                {
                    var leaderUser = x.student1LID != null && users.ContainsKey(x.student1LID) ? users[x.student1LID] : null;
                    var member1User = x.student2ID != null && users.ContainsKey(x.student2ID) ? users[x.student2ID] : null;
                    var member2User = x.student3ID != null && users.ContainsKey(x.student3ID) ? users[x.student3ID] : null;

                    var groupCriteria = evaluationCriteria.FirstOrDefault(y => y.GId == x.ID.ToString());
                    var project = fetchProjects.FirstOrDefault(y => y.groupId == x.ID.ToString());

                    var midLast = fetchEvaluation
                        .FirstOrDefault(y => y.PBatch == x.Batch && y.EvaluationName == "Mid")?.LastDate
                        .ToString("dd-MM-yyyy") ?? "No Date Available";

                    var propLast = fetchEvaluation
                        .FirstOrDefault(y => y.PBatch == x.Batch && y.EvaluationName == "Proposal")?.LastDate
                        .ToString("dd-MM-yyyy") ?? "No Date Available";

                    var finalLast = fetchEvaluation
                        .FirstOrDefault(y => y.PBatch == x.Batch && y.EvaluationName == "Final")?.LastDate
                        .ToString("dd-MM-yyyy") ?? "No Date Available";
                    DateTime proposalSubmissionDate, midSubmissionDate, finalSubmissionDate;
                    DateTime propLastDate, midLastDate, finalLastDate;

                    // Parse the project's submission dates
                    DateTime.TryParse(project?.ProposalSubmissionDate, out proposalSubmissionDate);
                    DateTime.TryParse(project?.MidSubmissionDate, out midSubmissionDate);
                    DateTime.TryParse(project?.FinalSubmissionDate, out finalSubmissionDate);

                    // Parse the "last date" strings
                    DateTime.TryParse(propLast, out propLastDate);
                    DateTime.TryParse(midLast, out midLastDate);
                    DateTime.TryParse(finalLast, out finalLastDate);

                    var propPpt = project?.PropPPT ?? "No submission found";
                    var propReport = project?.PropReport ?? "No submission found";
                    var midPpt = project?.MidPPT ?? "No submission found";
                    var midReport = project?.MidReport ?? "No submission found";
                    var finalDocs = project?.FinalDocs ?? "No submission found";
                    var status = project?.Status ?? "No Status";

                    return new StudentGroupViewModel
                    {
                        Id = x.ID,
                        Name = x.Name,
                        Batch = x.Batch,
                        student1LID = x.student1LID,
                        student2ID = x.student2ID,
                        student3ID = x.student3ID,
                        CoSupervisorID = x.CoSupervisorID,
                        SupervisorID = x.SupervisorID,

                        LeaderEnrollement = leaderUser?.Email ?? "Don't Exist",
                        member1Enrollment = member1User?.Email ?? "Don't Exist",
                        Member2Enrollment = member2User?.Email ?? "Don't Exist",

                        LeaderName = leaderUser?.Name ?? "Don't Exist",
                        member1 = member1User?.Name ?? "Don't Exist",
                        Member2 = member2User?.Name ?? "Don't Exist",

                        StudentsProposalMarks = groupCriteria?.StudentsProposalMarks ?? 0,
                        Student1MidMarks = groupCriteria?.Student1MidMarks ?? 0,
                        Student2MidMarks = groupCriteria?.Student2MidMarks ?? 0,
                        Student3MidMarks = groupCriteria?.Student3MidMarks ?? 0,
                        Student1FinalMarks = groupCriteria?.Student1FinalMarks ?? 0,
                        Student2FinalMarks = groupCriteria?.Student2FinalMarks ?? 0,
                        Student3FinalMarks = groupCriteria?.Student3FinalMarks ?? 0,
                        CordinatorMarks = groupCriteria?.CordinatorMarks ?? 0,
                        SupervisorMarks = groupCriteria?.SupervisorMarks ?? 0,

                        ProposalRemarks = evaluationCriteria.FirstOrDefault(y => y.GId == x.ID.ToString() && y.EvalName == "Proposal")?.Remarks ?? "No Remarks",
                        MidRemarks = evaluationCriteria.FirstOrDefault(y => y.GId == x.ID.ToString() && y.EvalName == "Mid")?.Remarks ?? "No Remarks",
                        FinalRemarks = evaluationCriteria.FirstOrDefault(y => y.GId == x.ID.ToString() && y.EvalName == "Final")?.Remarks ?? "No Remarks",

                        projectname = project?.Title ?? "No Project",
                        supervisorname = _userManager.FindByIdAsync(x.SupervisorID).Result?.Name ?? "No Supervisor",
                        Approved = project?.Status ?? "Pending",

                        MidLast = midLast,
                        PropLast = propLast,
                        FinalLast = finalLast,
                        CheckProjectstatus = status,
                        PropPPT = propPpt,
                        PropReport = propReport,
                        MidPPT = midPpt,
                        MidReport = midReport,
                        FinalDocs = finalDocs,
                        PropSubCheck = proposalSubmissionDate <= propLastDate ? "Yes" : "No",
                        MidSubCheck = midSubmissionDate <= midLastDate ? "Yes" : "No",
                        FinalSubCheck = finalSubmissionDate <= finalLastDate ? "Yes" : "No",
                        CheckProject = project == null ? "No" : "Yes"

                    };
                }).ToList();

                // Set distinct batches in the ViewBag for dropdown
                ViewBag.Batches = batches;

                // Return the model to the partial view
                return PartialView(model);
            }

            // If no groups found, return an empty view or a view with a message
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> SearchStudentGroupAlongMarks(string batch, string ViewValue, string searchKeyword)
        {
            // Handle ViewBag value for success messages
            if (!string.IsNullOrEmpty(ViewValue))
            {
                ViewBag.success = ViewValue;
            }

            // Fetch all required data upfront
            var studentGroups = await _studentGroupService.GetAllAsync();
            var evaluationCriteria = await _evaluationCriteriaService.GetAllAsync();
            var fetchProjects = await _projectService.GetAllAsync();
            var fetchEvaluation = await _evaluationService.GetAllAsync();
            var currentUser = await _userManager.GetUserAsync(User);
            var userRole = (await _userManager.GetRolesAsync(currentUser)).FirstOrDefault();

            // Apply filters based on role
            if (userRole == "Supervisor")
            {
                studentGroups = studentGroups.Where(x => x.SupervisorID == currentUser.Id).ToList();
            }
            else if (userRole == "Student")
            {
                studentGroups = studentGroups.Where(x =>
                    x.student1LID == currentUser.Id || x.student2ID == currentUser.Id || x.student3ID == currentUser.Id)
                    .ToList();
            }

            // Filter by batch
            if (!string.IsNullOrEmpty(batch))
            {
                studentGroups = studentGroups.Where(x => x.Batch == batch).ToList();
                ViewBag.batch = batch; // Store the selected batch
            }

            // Search by keyword (group name, project title, supervisor name)
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                studentGroups = studentGroups.Where(x =>
                    x.Name.Contains(searchKeyword, StringComparison.OrdinalIgnoreCase) ||
                    fetchProjects.Any(p => p.groupId == x.ID.ToString() && p.Title.Contains(searchKeyword, StringComparison.OrdinalIgnoreCase)) ||
                    _userManager.FindByIdAsync(x.SupervisorID).Result?.Name.Contains(searchKeyword, StringComparison.OrdinalIgnoreCase) == true)
                    .ToList();

                ViewBag.searchKeyword = searchKeyword; // Store the search keyword
            }

            // Pre-fetch all users for better performance
            var userIds = studentGroups.SelectMany(x => new[] { x.student1LID, x.student2ID, x.student3ID })
                                        .Where(id => id != null)
                                        .Distinct();

            var users = (await Task.WhenAll(userIds.Select(id => _userManager.FindByIdAsync(id)))).ToDictionary(user => user?.Id, user => user);

            // Build the view model
            var model = studentGroups.Select(x =>
            {
                var leaderUser = x.student1LID != null && users.ContainsKey(x.student1LID) ? users[x.student1LID] : null;
                var member1User = x.student2ID != null && users.ContainsKey(x.student2ID) ? users[x.student2ID] : null;
                var member2User = x.student3ID != null && users.ContainsKey(x.student3ID) ? users[x.student3ID] : null;

                var groupCriteria = evaluationCriteria.FirstOrDefault(y => y.GId == x.ID.ToString());
                var project = fetchProjects.FirstOrDefault(y => y.groupId == x.ID.ToString());

                var midLast = fetchEvaluation
                    .FirstOrDefault(y => y.PBatch == x.Batch && y.EvaluationName == "Mid")?.LastDate
                    .ToString("dd-MM-yyyy") ?? "No Date Available";

                var propLast = fetchEvaluation
                    .FirstOrDefault(y => y.PBatch == x.Batch && y.EvaluationName == "Proposal")?.LastDate
                    .ToString("dd-MM-yyyy") ?? "No Date Available";

                var finalLast = fetchEvaluation
                    .FirstOrDefault(y => y.PBatch == x.Batch && y.EvaluationName == "Final")?.LastDate
                    .ToString("dd-MM-yyyy") ?? "No Date Available";

                DateTime proposalSubmissionDate, midSubmissionDate, finalSubmissionDate;
                DateTime propLastDate, midLastDate, finalLastDate;

                // Parse the project's submission dates
                DateTime.TryParse(project?.ProposalSubmissionDate, out proposalSubmissionDate);
                DateTime.TryParse(project?.MidSubmissionDate, out midSubmissionDate);
                DateTime.TryParse(project?.FinalSubmissionDate, out finalSubmissionDate);

                // Parse the "last date" strings
                DateTime.TryParse(propLast, out propLastDate);
                DateTime.TryParse(midLast, out midLastDate);
                DateTime.TryParse(finalLast, out finalLastDate);

                var propPpt = project?.PropPPT ?? "No submission found";
                var propReport = project?.PropReport ?? "No submission found";
                var midPpt = project?.MidPPT ?? "No submission found";
                var midReport = project?.MidReport ?? "No submission found";
                var finalDocs = project?.FinalDocs ?? "No submission found";
                var status = project?.Status ?? "No Status";

                return new StudentGroupViewModel
                {
                    Id = x.ID,
                    Name = x.Name,
                    Batch = x.Batch,
                    student1LID = x.student1LID,
                    student2ID = x.student2ID,
                    student3ID = x.student3ID,
                    CoSupervisorID = x.CoSupervisorID,
                    SupervisorID = x.SupervisorID,

                    LeaderEnrollement = leaderUser?.Email ?? "Don't Exist",
                    member1Enrollment = member1User?.Email ?? "Don't Exist",
                    Member2Enrollment = member2User?.Email ?? "Don't Exist",

                    LeaderName = leaderUser?.Name ?? "Don't Exist",
                    member1 = member1User?.Name ?? "Don't Exist",
                    Member2 = member2User?.Name ?? "Don't Exist",

                    StudentsProposalMarks = groupCriteria?.StudentsProposalMarks ?? 0,
                    Student1MidMarks = groupCriteria?.Student1MidMarks ?? 0,
                    Student2MidMarks = groupCriteria?.Student2MidMarks ?? 0,
                    Student3MidMarks = groupCriteria?.Student3MidMarks ?? 0,
                    Student1FinalMarks = groupCriteria?.Student1FinalMarks ?? 0,
                    Student2FinalMarks = groupCriteria?.Student2FinalMarks ?? 0,
                    Student3FinalMarks = groupCriteria?.Student3FinalMarks ?? 0,
                    CordinatorMarks = groupCriteria?.CordinatorMarks ?? 0,
                    SupervisorMarks = groupCriteria?.SupervisorMarks ?? 0,

                    ProposalRemarks = groupCriteria?.Remarks ?? "No Remarks",
                    projectname = project?.Title ?? "No Project",
                    supervisorname = _userManager.FindByIdAsync(x.SupervisorID).Result?.Name ?? "No Supervisor",
                    Approved = project?.Status ?? "Pending",
                    
                    PropPPT = propPpt,
                    PropReport = propReport,
                    MidPPT = midPpt,
                    MidReport = midReport,
                    FinalDocs = finalDocs,

                    MidLast = midLast,
                    PropLast = propLast,
                    FinalLast = finalLast,

                    CheckProjectstatus = status,
                    PropSubCheck = proposalSubmissionDate <= propLastDate ? "Yes" : "No",
                    MidSubCheck = midSubmissionDate <= midLastDate ? "Yes" : "No",
                    FinalSubCheck = finalSubmissionDate <= finalLastDate ? "Yes" : "No",
                    CheckProject = project == null ? "No" : "Yes"
                };
            }).ToList();

            // Store distinct batches in ViewBag for dropdowns
            ViewBag.Batches = studentGroups.Select(x => x.Batch).Distinct().ToList();

            return PartialView("StudentGroupAlongMarks", model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignSupandCordMarks(int cMarks, int sMarks , string groupid)
        {
            var fetchEvaluation = await _evaluationCriteriaService.GetAllAsync();
            var evaluation = fetchEvaluation.FirstOrDefault(x => x.GId == groupid && x.EvalName == "Proposal");

            if (fetchEvaluation != null)
            {
                if (cMarks != 0)
                {

                evaluation.CordinatorMarks = cMarks;
                }
                else
                {
                 evaluation.SupervisorMarks = sMarks;
                }
                await _evaluationCriteriaService.UpdateAsync(evaluation);
                return RedirectToAction("StudentGroupAlongMarks", "Cordinator", new { ViewValue = "Updated" });
            }
            return RedirectToAction("StudentGroupAlongMarks", "Cordinator", new { ViewValue = "Failed" });
        }



    }


}

