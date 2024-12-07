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

        public CordinatorController(INotificationsService notificationsService,IFYPCommitteService fYPCommitteService, IEvaluationService evaluationService, ICompanyService companyService, IProjectService projectService, IUserService userService, UserManager<AppUser> userManager, IRoomAllotmentService supervisorService, IProposalDefenseService proposalDefense, IRoomService designationService, IStudentGroupService studentGroupService)
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
        public async Task<IActionResult> ChangeStatus(string Id, string status)
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
                // Redirect to the desired page or action after a successful update
                return RedirectToAction("Projects", "Supervisor");
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
            if (getUser != null)
            {
                var CheckExternal = studentGroups.Where(x => x.ExternalId == getUser.Id);
                var checkExternalGroup = CheckExternal.Where(x => x.ID.ToString() == model.Group);
                if (checkExternalGroup != null)
                {
                    return RedirectToAction("Evaluation", "Cordinator", new { batch = model.Batch, ViewValue = "Existed" });
                }
                else
                {
                    var fetchGroup = studentGroups.FirstOrDefault(x => x.ID.ToString() == model.Group);
                    fetchGroup.ExternalId = getUser.Id;
                    await _studentGroupService.UpdateAsync(fetchGroup);
                }
            }
            else
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
                var result = await _userManager.CreateAsync(newUser, "BahriaExternals123@@");
                if (result.Succeeded)
                {
                    var role = await _userManager.AddToRoleAsync(newUser, Roles.External.ToString());
                    if (role != null)
                    {
                        var getUsers = await _userService.GetAllAsync();
                        var user = getUsers.Where(x => x.Email == model.Email).FirstOrDefault();
                        if (user != null)
                        {
                            var FetchGroup = studentGroups.Where(x => x.ID.ToString() == model.Group).FirstOrDefault();
                            if (FetchGroup.ExternalId == null)
                            {
                                FetchGroup.ExternalId = user.Id;
                                await _studentGroupService.UpdateAsync(FetchGroup);
                                return RedirectToAction("ExternalsAccount", "Cordinator", new { batch = model.Batch, ViewValue = "Added" });
                            }

                        }
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
                    return RedirectToAction("ExternalsAccount", "Cordinator", new { batch = model.Batch,  ViewValue = "Updated" });
                }
            }
            return RedirectToAction("ExternalsAccount", "Cordinator", new {batch = model.Batch, ViewValue = "NoUser" });


        }

        [HttpGet]
        public async Task<IActionResult> CreateNotifications(string Batch)
        {
            // Fetch only the filtered groups from the service (filter applied at database level)
            var groups = await _studentGroupService.GetAllAsync();
            if (groups != null)
            {
                var filteredGroups = groups.Select(x=>x.Batch).Distinct().ToList();
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

    }


}

