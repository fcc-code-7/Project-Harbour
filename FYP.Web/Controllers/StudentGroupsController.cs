using FYP.Databases;
using FYP.Entities;
using FYP.Models;
using FYP.Services;
using FYP.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FYP.Web.Controllers
{
    public class StudentGroupsController : Controller
    {
        private readonly IStudentGroupService _studentGroupService;
        private readonly IProjectService _projectService;
        private readonly IProposalDefenseService _proposalDefenseService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ISupervisorService _supervisorService;
        private readonly IUserService _userService;
        private readonly IChangeSupervisorFormService _changeSupervisorFormService;
        private readonly IStudentService _studentService;
        private readonly IFYPCommitteService _fYPCommitteService;

        public StudentGroupsController(IFYPCommitteService fYPCommitteService, IChangeSupervisorFormService changeSupervisorFormService, IUserService userService, ISupervisorService supervisorService, IStudentService studentService, IStudentGroupService studentGroupService, UserManager<AppUser> userManager, IProjectService projectService, IProposalDefenseService proposalDefense)
        {
            _studentGroupService = studentGroupService;
            _userManager = userManager;
            _projectService = projectService;
            _proposalDefenseService = proposalDefense;
            _studentService = studentService;
            _supervisorService = supervisorService;
            _changeSupervisorFormService = changeSupervisorFormService;
            _userService = userService;
            _fYPCommitteService = fYPCommitteService;
        }
        public async Task<IActionResult> StudentGroups()
        {
            var studentGroups = await _studentGroupService.GetAllAsync();
            var user = _userManager.GetUserId(User);
            var distinctBatches = studentGroups.Select(x => x.Batch).Distinct().ToList();
            var distinctYears = studentGroups.Select(x => x.Year).Distinct().ToList();

            var model = new StudentGroupViewModel()
            {
                StudentGroups = studentGroups.Where(x => x.SupervisorID == user).Select(x => new StudentGroupViewModel
                {
                    GrouID = x.ID.ToString(),
                    groupId = x.ID.ToString(),
                    companyID = x.companyID,
                    Batch = x.Batch,
                    CordinatorID = x.CordinatorID,
                    CoSupervisorID = x.CoSupervisorID,
                    Id = x.ID,
                    Name = x.Name,
                    student1LID = x.student1LID,
                    student2ID = x.student2ID,
                    student3ID = x.student3ID,
                    SupervisorID = x.SupervisorID,
                    Year = x.Year,
                    projectname = _projectService.GetAllAsync().Result.Where(y => y.projectGroup == x.Name).Select(y => y.Title).FirstOrDefault(),
                    LeaderName = _userService.GetByIdAsync(x.student1LID).Result.Email,
                    member1 = _userService.GetByIdAsync(x.student2ID).Result.Email,
                    Member2 = _userService.GetByIdAsync(x.student3ID).Result.Email,
                    supervisorname = _userManager.FindByIdAsync(x.SupervisorID).Result.Name
                }
                ).ToList(),
                Batches = distinctBatches,
                Years = distinctYears
            };

            return PartialView(model);
        }
        public async Task<IActionResult> StudentGroupbyBatch(string batch)
        {
            if (batch == "All Projects")
            {
                return RedirectToAction("StudentGroups");
            }
            var userid = _userManager.GetUserId(User);
            var studentGroups = (await _studentGroupService.GetAllAsync())
                                .Where(x => x.Batch == batch && x.SupervisorID == userid);
            var studentGroup = (await _studentGroupService.GetAllAsync());
            var user = studentGroups.FirstOrDefault();
            var allGroups = await _studentGroupService.GetAllAsync();
            var userGroup = allGroups
                .Where(x => x.student1LID == user.student1LID || x.student2ID == user.student2ID || x.student3ID == user.student3ID)
                .FirstOrDefault();
            var distinctBatches = studentGroup.Select(x => x.Batch).Distinct().ToList();
            var distinctYears = studentGroup.Select(x => x.Year).Distinct().ToList();
            var supervisorName = (await _userManager.FindByIdAsync(userGroup.SupervisorID))?.Name;
            var leaderName = (await _userManager.FindByIdAsync(user.student1LID))?.Email;
            var member1Name = (await _userManager.FindByIdAsync(user.student2ID))?.Email;
            var member2Name = (await _userManager.FindByIdAsync(user.student3ID))?.Email;

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
                    Member2 = member2Name
                }).ToList(),
                Batches = distinctBatches,
                Years = distinctYears

            };

            return PartialView("StudentGroups", model);
        }
        [HttpPost]
        public async Task<IActionResult> StudentGroups([FromBody] StudentGroupViewModel model)
        {
            if (model != null)
            {
                var student = new AppUser()
                {
                    Email = model.student1LEmail,
                    UserName = model.student1LEmail,
                    Designation = "Student",
                    Role = "Student"

                };
                var student2 = new AppUser()
                {
                    Email = model.student2Email,
                    UserName = model.student2Email,
                    Designation = "Student",
                    Role = "Student"


                };
                var student3 = new AppUser()
                {
                    Email = model.student3Email,
                    UserName = model.student3Email,
                    Designation = "Student",
                    Role = "Student"


                };
                var result = await _userManager.CreateAsync(student, "Bahria123@@");
                var result2 = await _userManager.CreateAsync(student2, "Bahria123@@");
                var result3 = await _userManager.CreateAsync(student3, "Bahria123@@");

                if (result.Succeeded && result2.Succeeded || result3.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(student, Roles.Student.ToString());
                    var roleResult2 = await _userManager.AddToRoleAsync(student2, Roles.Student.ToString());
                    var roleResult3 = await _userManager.AddToRoleAsync(student3, Roles.Student.ToString());


                    if (roleResult.Succeeded && roleResult2.Succeeded || roleResult3.Succeeded)
                    {
                        var studenta = await _userManager.FindByEmailAsync(model.student1LEmail);
                        var studentb = await _userManager.FindByEmailAsync(model.student2Email);
                        var studentc = await _userManager.FindByEmailAsync(model.student3Email);
                        var userId = _userManager.GetUserId(User);
                        var studentData1 = new Student()
                        {
                            studentId = studenta.Id,
                            ENo = null,
                            RegNo = 0,
                            Semester = null

                        };
                        var studentData2 = new Student()
                        {
                            studentId = studentb.Id,
                            ENo = null,
                            RegNo = 0,
                            Semester = null

                        };
                        var studentData3 = new Student()
                        {
                            studentId = studentc.Id,
                            ENo = null,
                            RegNo = 0,
                            Semester = null
                        };
                        var usergroup = new StudentGroup
                        {
                            student1LID = studenta.Id,
                            student2ID = studentb.Id,
                            student3ID = studentc.Id,
                            SupervisorID = userId,
                            Name = model.Name,
                            companyID = null,
                            CordinatorID = null,
                            CoSupervisorID = null,

                            Batch = model.Batch,
                            Year = model.Year,

                        };
                        await _studentService.AddAsync(studentData1);
                        await _studentService.AddAsync(studentData2);
                        await _studentService.AddAsync(studentData3);
                        await _studentGroupService.AddAsync(usergroup);
                        return RedirectToAction("StudentGroups", "Supervisor");
                    }

                }
            }

            // Return an error response
            return Json(new { success = false, message = "Invalid data!" });
        }


        //EDIT GROUP GET
        [HttpGet]
        public async Task<IActionResult> EditGroup(string id)
        {
            // Fetch the group and related student data by ID
            var group = _studentGroupService.GetAllAsync().Result.Where(x => x.ID.ToString() == id).FirstOrDefault();
            if (group == null)
            {
                return Json(new { success = false, message = "Invalid Group!" });
            }

            var viewModel = new StudentGroupViewModel
            {
                Id = group.ID,
                groupId = group.ID.ToString(),
                Name = group.Name,
                Batch = group.Batch,
                Year = group.Year,
                student1LEmail = (await _userManager.FindByIdAsync(group.student1LID))?.Email,
                student2Email = (await _userManager.FindByIdAsync(group.student2ID))?.Email,
                student3Email = (await _userManager.FindByIdAsync(group.student3ID))?.Email
            };
            return PartialView(viewModel);
        }

        //EDIT GROUP POST

        [HttpPost]
        public async Task<IActionResult> EditGroup([FromBody] StudentGroupViewModel model)
        {

            var group = _studentGroupService.GetAllAsync().Result.Where(x => x.ID.ToString() == model.groupId).FirstOrDefault();
            if (group == null)
            {
                return NotFound();
            }

            group.Name = model.Name;
            // Update the emails if necessary
            var student1 = await _userManager.FindByIdAsync(group.student1LID);
            var student2 = await _userManager.FindByIdAsync(group.student2ID);
            var student3 = await _userManager.FindByIdAsync(group.student3ID);

            if (student1.Email != model.student1LEmail)
            {
                student1.Email = model.student1LEmail;
                student1.UserName = model.student1LEmail;
                await _userManager.UpdateAsync(student1);
            }
            if (student2.Email != model.student2Email)
            {
                student2.Email = model.student2Email;
                student2.UserName = model.student2Email;
                await _userManager.UpdateAsync(student2);
            }
            if (student3.Email != model.student3Email)
            {
                student3.Email = model.student3Email;
                student3.UserName = model.student3Email;
                await _userManager.UpdateAsync(student3);
            }

            var result = _studentGroupService.UpdateAsync(group);
            if (result != null)
            {
                return RedirectToAction("StudentGroups", "studentgroups");

            }
            return Json(new { success = false, message = "Invalid data!" });


        }

        [HttpGet]
        public async Task<IActionResult> ChangeSupervisor(string groupId)
        {
            var currentUserId = _userManager.GetUserId(User);
            var group = _studentGroupService.GetAllAsync().Result.Where(x => x.ID.ToString() == groupId).FirstOrDefault();
            var currentUserName = _userService.GetByIdAsync(currentUserId).Result.Name;
            var studentGroups = await _studentGroupService.GetAllAsync();
            var supervisorGroupCount = studentGroups
                .GroupBy(x => x.SupervisorID)
                .Select(g => new { SupervisorID = g.Key, GroupCount = g.Count() })
                .ToList();
            var allSupervisors = await _userManager.GetUsersInRoleAsync("Supervisor");
            // Filter supervisors assigned to zero or one group, excluding the logged-in supervisor
            var filteredSupervisors = allSupervisors
                .Where(user => supervisorGroupCount
                    .Where(sc => sc.SupervisorID == user.Id)
                    .Select(sc => sc.GroupCount)
                    .FirstOrDefault() <= 1
                    && user.Id != currentUserId)  // Exclude the logged-in supervisor
                .Select(user => new SupervisorsViewModel
                {
                    UserId = user.Id,
                    name = user.Name
                })
                .ToList();

            // Create the view model with filtered supervisors
            var model = new ChangeSupervisorFormViewModel
            {
                GroupId = groupId,
                groupName = group.Name,
                oldsupervsiorname = currentUserName,
                oldsupervisorId = currentUserId,
                Supervisors = filteredSupervisors
            };

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeSupervisor(ChangeSupervisorFormViewModel model)
        {
            var findGroup = _studentGroupService.GetAllAsync().Result.Where(x => x.ID.ToString() == model.GroupId).FirstOrDefault();
            findGroup.changeSupervisorForm = true;
            await _studentGroupService.UpdateAsync(findGroup);
            var changeSupervisor = new ChangeSupervisorForm()
            {
                CurrentDate = DateTime.Now,
                GroupId = model.GroupId,
                oldsupervisorId = model.oldsupervisorId,
                NewSupervsiorId = model.NewSupervsiorId,
                OtherReason = model.OtherReason,
                Reason = model.Reason,
            }; ;
            var result = _changeSupervisorFormService.AddAsync(changeSupervisor);
            if (result != null)
            {
                RedirectToAction("StudentGroups", "Supervsior");
            }
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Evaluators()
        {
            var LoggedInUser = _userManager.GetUserId(User);
            var FetchUser = await _userManager.FindByIdAsync(LoggedInUser);
            var FetchUserDepartment = FetchUser.Department;

            // Fetch groups once
            var allGroups = await _studentGroupService.GetAllAsync();
            var supervisorList = await _userManager.GetUsersInRoleAsync("Supervisor");

            var model = new FYPCommitteViewModel();
            // Populate distinct batches
            model.batches = allGroups.Select(x => x.Batch).Distinct().ToList();

            return PartialView(model);
        }

        public async Task<IActionResult> FetchBatches(string id, string batch , string assignRequest)
        {
            var LoggedInUser = _userManager.GetUserId(User);
            var FetchUser = await _userManager.FindByIdAsync(LoggedInUser);
            var FetchUserDepartment = FetchUser.Department;
            var allGroups = await _studentGroupService.GetAllAsync();
            var supervisorList = await _userManager.GetUsersInRoleAsync("Supervisor");
           
              
            
            var model = new FYPCommitteViewModel();
            if (!string.IsNullOrEmpty(batch))
            {
                var filteredGroups = allGroups.Where(x => x.Batch == batch).ToList();
                if (filteredGroups.Any())
                {
                    model.studentGroups = filteredGroups.Select(x => new FetchGroupListViewModel
                    {
                        groupID = x.ID.ToString(),
                        groupName = x.Name
                    }).ToList();
                    
                }
                model.batch = batch;
                // Get supervisors for the department
                if (supervisorList.Any() && !string.IsNullOrEmpty(id))
                {
                    var fetchCurrentGroup = allGroups.Where(x => x.ID.ToString() == id).FirstOrDefault();
                    var fetchCurrentGroupSupervisor = allGroups.Where(x => x.SupervisorID == fetchCurrentGroup.SupervisorID).FirstOrDefault().SupervisorID;
                    var departmentSupervisors = supervisorList
                        .Where(x => x.Department == FetchUserDepartment)
                        .Select((supervisor, index) => new { supervisor, index })
                        .ToList();

                    model.SupervisorList1 = departmentSupervisors
                        .Where(x => x.index % 2 == 0 && x.supervisor.Id != fetchCurrentGroupSupervisor.ToString())
                        .Select(x => new FetchSupervisorListViewModel
                        {
                            SupervisorID = x.supervisor.Id,
                            SupervisorName = x.supervisor.Name
                        }).ToList();

                    model.SupervisorList2 = departmentSupervisors
                        .Where(x => x.index % 2 != 0 && x.supervisor.Id != fetchCurrentGroupSupervisor.ToString())
                        .Select(x => new FetchSupervisorListViewModel
                        {
                            SupervisorID = x.supervisor.Id,
                            SupervisorName = x.supervisor.Name
                        }).ToList();
                    model.assignRequest = assignRequest;
                }
            }

            // Handle id filtering
            if (!string.IsNullOrEmpty(id))
            {
                var fypCommitte = await _fYPCommitteService.GetAllAsync();
                var currentFypCommitte = fypCommitte.Where(x => x.groupID == id).FirstOrDefault();

                if (currentFypCommitte != null)
                {
                    model.Member1ID = currentFypCommitte.Member1ID;
                    model.Member2ID = currentFypCommitte.Member2ID;
                    model.groupID = currentFypCommitte.groupID;
                    model.batch = currentFypCommitte.batch;
                }
                else
                {
                    model.groupID = id;
                    model.batch = batch;
                }
            }
            // Populate distinct batches
            model.batches = allGroups.Select(x => x.Batch).Distinct().ToList();

            return PartialView("Evaluators", model);

        }

        [HttpPost]
        public async Task<IActionResult> FypCommitee([FromBody] FYPCommitteViewModel model)
        {
            var getFYPCommitte = await _fYPCommitteService.GetAllAsync();
            var fypCommitte = getFYPCommitte.FirstOrDefault(x => x.groupID == model.groupID);

            if (fypCommitte != null)
            {
                bool isUpdated = false;

                if (fypCommitte.Member1ID != model.Member1ID)
                {
                    fypCommitte.Member1ID = model.Member1ID;
                    isUpdated = true;
                }

                if (fypCommitte.Member2ID != model.Member2ID)
                {
                    fypCommitte.Member2ID = model.Member2ID;
                    isUpdated = true;
                }
                

                // If either member has been updated, proceed to update in the database
                if (isUpdated)
                {
                    await _fYPCommitteService.UpdateAsync(fypCommitte);
                }

                // Redirect after the update
                if (model.assignRequest == "true")
                {
                    return RedirectToAction("StudentGroup", "cordinator");
                }
                else
                {
                return RedirectToAction("FetchBatches", "studentgroups", new { id = model.groupID, batch = model.batch });
                }

            }
            else
            {
                // Initialize a new FYPCommitte instance if it doesn't exist
                var newFypCommitte = new FYPCommitte
                {
                    Member1ID = model.Member1ID,
                    Member2ID = model.Member2ID,
                    groupID = model.groupID,
                    batch = model.batch
                };

                await _fYPCommitteService.AddAsync(newFypCommitte);
                if (model.assignRequest == "true")
                {
                    return RedirectToAction("StudentGroups", "studentgroups");
                }
                else
                {
                    return RedirectToAction("FetchBatches", "studentgroups", new { id = model.groupID, batch = model.batch });
                }
            }
        }

    }
}
