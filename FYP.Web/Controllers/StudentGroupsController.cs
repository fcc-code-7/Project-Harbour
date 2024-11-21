using FYP.Databases;
using FYP.Entities;
using FYP.Models;
using FYP.Repositories.Interfaces;
using FYP.Services;
using FYP.Services.Implementation;
using FYP.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace FYP.Web.Controllers
{
    public class StudentGroupsController : Controller
    {
        private readonly IStudentGroupService _studentGroupService;
        private readonly IProjectService _projectService;
        private readonly IProposalDefenseService _proposalDefenseService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IRoomAllotmentService _roomAllotmentService;
        private readonly IUserService _userService;
        private readonly IChangeSupervisorFormService _changeSupervisorFormService;
        private readonly IStudentService _studentService;
        private readonly IFYPCommitteService _fYPCommitteService;
        private readonly IRoomService _roomService;
        private readonly IRoomInChargeService _roomInChargeService;
        private readonly IEvaluationService _evaluationService;
        private readonly IEvaluationCriteriaService _evaluationCriteriaService;

        public StudentGroupsController(IEvaluationCriteriaService evaluationCriteriaService ,IRoomAllotmentService roomAllotmentService, IEvaluationService evaluationService, IRoomInChargeService roomInChargeService, IRoomService roomService, IFYPCommitteService fYPCommitteService, IChangeSupervisorFormService changeSupervisorFormService, IUserService userService, IRoomAllotmentService supervisorService, IStudentService studentService, IStudentGroupService studentGroupService, UserManager<AppUser> userManager, IProjectService projectService, IProposalDefenseService proposalDefense)
        {
            _studentGroupService = studentGroupService;
            _userManager = userManager;
            _projectService = projectService;
            _proposalDefenseService = proposalDefense;
            _studentService = studentService;
            _roomAllotmentService = roomAllotmentService;
            _changeSupervisorFormService = changeSupervisorFormService;
            _userService = userService;
            _fYPCommitteService = fYPCommitteService;
            _roomService = roomService;
            _roomInChargeService = roomInChargeService;
            _evaluationService = evaluationService;
            _evaluationCriteriaService = evaluationCriteriaService;
        }
        public async Task<IActionResult> StudentGroups(string istrue)
        {
            var model = new StudentGroupViewModel();
            var fetchUser = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            var Groups = await _studentGroupService.GetAllAsync();
            var studentGroups = Groups.Where(x => x.SupervisorID == fetchUser.Id).ToList();
            if (istrue != "True")
            {
                if (studentGroups.Any())
                {
                    var fetchLastBatch = studentGroups.LastOrDefault().Batch;

                if (fetchLastBatch != null)
                {

                    model.Batch = fetchLastBatch;
                    return RedirectToAction("StudentGroupbyBatch", "StudentGroups", new { batch = fetchLastBatch });
                }
                }

            }
            var distinctBatches = studentGroups.Select(x => x.Batch).Distinct().ToList();
            var distinctYears = studentGroups.Select(x => x.Year).Distinct().ToList();
            var project = await _projectService.GetAllAsync();

            model.StudentGroups = studentGroups.Select(x => new StudentGroupViewModel
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
                projectname = project.Where(y => y.projectGroup == x.Name).Select(y => y.Title).FirstOrDefault(),
                LeaderName = _userService.GetByIdAsync(x.student1LID).Result?.Email,
                member1 = _userService.GetByIdAsync(x.student2ID).Result?.Email,
                Member2 = _userService.GetByIdAsync(x.student3ID).Result?.Email ?? "Not Exist",
                supervisorname = _userManager.FindByIdAsync(x.SupervisorID).Result.Name
            }
            ).ToList();
            model.Batches = distinctBatches;
            model.Years = distinctYears;


            return PartialView(model);
        }
        public async Task<IActionResult> StudentGroupbyBatch(string batch)
            {
            if (batch == "All Groups")
            {
                return RedirectToAction("StudentGroups", "StudentGroups", new { istrue = "True" });
            }
            var fetchUser = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            var Groups = await _studentGroupService.GetAllAsync();
            var studentGroups = Groups.Where(x => x.SupervisorID == fetchUser.Id && x.Batch == batch).ToList();
            var distinctBatches = Groups.Where(x => x.SupervisorID == fetchUser.Id).Select(x => x.Batch).Distinct().ToList();
            var distinctYears = Groups.Where(x => x.SupervisorID == fetchUser.Id).Select(x => x.Year).Distinct().ToList();
            var project = await _projectService.GetAllAsync();
            var model = new StudentGroupViewModel()
            {
                StudentGroups = studentGroups.Select(x => new StudentGroupViewModel
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
                    projectname = project.Where(y => y.projectGroup == x.Name).Select(y => y.Title).FirstOrDefault(),
                    LeaderName = _userService.GetByIdAsync(x.student1LID).Result.Email,
                    member1 = _userService.GetByIdAsync(x.student2ID).Result?.Email,
                    Member2 = _userService.GetByIdAsync(x.student3ID).Result?.Email ?? "Not Exist",
                    supervisorname = _userManager.FindByIdAsync(x.SupervisorID).Result.Name
                }
                ).ToList(),
                Batches = distinctBatches,
                Years = distinctYears,
                Batch = batch

            };

            return PartialView("StudentGroups", model);
        }

        [HttpPost]
        public async Task<IActionResult> StudentGroups([FromBody] StudentGroupViewModel model)
        {
            if (model == null)
            {
                return Json(new { success = false, message = "Invalid data!" });
            }

            // Create and process Student 1
            var student1 = new AppUser
            {
                Email = model.student1LEmail,
                UserName = model.student1LEmail,
                Designation = "Student",
                Role = "Student"
            };

            var result1 = await _userManager.CreateAsync(student1, "Bahria123@@");
            if (!result1.Succeeded) return Json(new { success = false, message = "Failed to create Student 1!" });

            await _userManager.AddToRoleAsync(student1, Roles.Student.ToString());
            var studenta = await _userManager.FindByEmailAsync(model.student1LEmail);

            var studentData1 = new Student
            {
                studentId = studenta.Id,
                ENo = null,
                RegNo = 0,
                Semester = null
            };

            // Create and process Student 2
            var student2 = new AppUser
            {
                Email = model.student2Email,
                UserName = model.student2Email,
                Designation = "Student",
                Role = "Student"
            };

            var result2 = await _userManager.CreateAsync(student2, "Bahria123@@");
            if (!result2.Succeeded) return Json(new { success = false, message = "Failed to create Student 2!" });

            await _userManager.AddToRoleAsync(student2, Roles.Student.ToString());
            var studentb = await _userManager.FindByEmailAsync(model.student2Email);

            var studentData2 = new Student
            {
                studentId = studentb.Id,
                ENo = null,
                RegNo = 0,
                Semester = null
            };

            // Create Student Group
            var userId = _userManager.GetUserId(User);
            var usergroup = new StudentGroup
            {
                student1LID = studenta.Id,
                student2ID = studentb.Id,
                SupervisorID = userId,
                Name = model.Name,
                companyID = null,
                CordinatorID = null,
                CoSupervisorID = null,
                Batch = model.Batch,
                Year = model.Year
            };

            // Optional Student 3
            if (!string.IsNullOrEmpty(model.student3Email))
            {
                var student3 = new AppUser
                {
                    Email = model.student3Email,
                    UserName = model.student3Email,
                    Designation = "Student",
                    Role = "Student"
                };

                var result3 = await _userManager.CreateAsync(student3, "Bahria123@@");
                if (result3.Succeeded)
                {
                    await _userManager.AddToRoleAsync(student3, Roles.Student.ToString());
                    var studentc = await _userManager.FindByEmailAsync(model.student3Email);

                    var studentData3 = new Student
                    {
                        studentId = studentc.Id,
                        ENo = null,
                        RegNo = 0,
                        Semester = null
                    };

                    usergroup.student3ID = studentc.Id; // Associate Student 3 with the group
                    await _studentService.AddAsync(studentData3);
                }
            }

            // Save all data
            await _studentService.AddAsync(studentData1);
            await _studentService.AddAsync(studentData2);
            await _studentGroupService.AddAsync(usergroup);

            return RedirectToAction("StudentGroups", "studentgroups");
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
            string date = DateTime.Now.ToString("dd-MM-yy"); // Example string date
            DateTime parsedDate = DateTime.ParseExact(date, "dd-MM-yy", CultureInfo.InvariantCulture);
            model.AppointedDate = parsedDate;
            model.AppointedTime = new TimeOnly(9, 0);  // Set to 9:00 AM
            return PartialView(model);
        }

        public async Task<IActionResult> FetchBatches(string id, string batch, string assignRequest, string EvalType, string room)
        {
            var LoggedInUser = _userManager.GetUserId(User);
            var FetchUser = await _userManager.FindByIdAsync(LoggedInUser);
            var FetchUserDepartment = FetchUser.Department;
            var allGroups = await _studentGroupService.GetAllAsync();
            var supervisorList = await _userManager.GetUsersInRoleAsync("Supervisor");
            var RoomsServices = await _roomService.GetAllAsync();

            var model = new FYPCommitteViewModel();
            string date = DateTime.Now.ToString("dd-MM-yy"); // Example string date
            DateTime parsedDate = DateTime.ParseExact(date, "dd-MM-yy", CultureInfo.InvariantCulture);
            model.AppointedDate = parsedDate;
            model.AppointedTime = new TimeOnly(9, 0);  // Set to 9:00 
            if (!string.IsNullOrEmpty(batch))
            {
                var Evaluation = await _evaluationService.GetAllAsync();
                //var fypCommittee = await _fYPCommitteService.GetAllAsync();
                //var fetchFypCommittee = fypCommittee.Where(x => x.batch == batch);
                var fetchEvaluation = Evaluation.Where(x => x.PBatch == batch && x.EvaluationName == EvalType);
                model.Evaluations = new List<string> { "Proposal", "Mid", "Final" };
                //if (fetchFypCommittee.Any())
                //{
                //    model.AppointedDate = fetchFypCommittee.Select(x => x.AppointedDate).FirstOrDefault();
                //    model.Lapse = fetchFypCommittee.Select(x => x.Lapse).FirstOrDefault();
                //}
                if (fetchEvaluation.Any())
                {

                    model.AppointedDate = fetchEvaluation.Select(x => x.LastDate).FirstOrDefault();
                    if (fetchEvaluation.FirstOrDefault().EvaluationName == "Proposal")
                    {
                        model.EvaluationID = "Proposal";
                        model.AppointedDate = fetchEvaluation.Select(x => x.LastDate).FirstOrDefault();
                    }
                    if (fetchEvaluation.FirstOrDefault().EvaluationName == "Mid")
                    {
                        model.EvaluationID = "Mid";
                        model.AppointedDate = fetchEvaluation.Select(x => x.LastDate).FirstOrDefault();
                    }
                    if (fetchEvaluation.FirstOrDefault().EvaluationName == "Final")
                    {
                        model.EvaluationID = "Final";
                        model.AppointedDate = fetchEvaluation.Select(x => x.LastDate).FirstOrDefault();
                    }
                }
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
                var currentFypCommitte = fypCommitte.Where(x => x.groupID == id && x.EvaluationID == EvalType).FirstOrDefault();

                if (currentFypCommitte != null)
                {
                    model.Member1ID = currentFypCommitte.Member1ID;
                    model.Member2ID = currentFypCommitte.Member2ID;
                    model.groupID = currentFypCommitte.groupID;
                    model.batch = currentFypCommitte.batch;
                    model.Room = currentFypCommitte.Room;
                    model.EvaluationID = currentFypCommitte.EvaluationID;
                    // Convert AppointedDate to string in dd-MM-yy format
                    string appointedDateString = currentFypCommitte.AppointedDate.ToString("dd-MM-yy");

                    string appointedTimeString = currentFypCommitte.AppointedTime.ToString("HH:mm:ss"); // Format for time
                                                                                                        // Parse AppointedDate from string back to DateTime
                    DateTime parsedAppointedDate = DateTime.ParseExact(appointedDateString, "dd-MM-yy", CultureInfo.InvariantCulture);

                    // If AppointedTime is a TimeSpan or a specific time format
                    TimeOnly parsedAppointedTime = TimeOnly.ParseExact(appointedTimeString, "HH:mm:ss", CultureInfo.InvariantCulture);
                    model.AppointedDate = parsedAppointedDate;
                    model.AppointedTime = parsedAppointedTime;

                }
                else
                {
                    model.groupID = id;
                    model.batch = batch;

                }
            }
            if (RoomsServices.Any())
            {
                model.Rooms = RoomsServices.Select(x => new RoomViewModel
                {
                    Id = x.ID.ToString(),
                    RoomNo = x.RoomNo
                }).ToList();
            }

            if (id != null && batch != null && EvalType != null && room != null)
            {
                var fypCommitte = await _fYPCommitteService.GetAllAsync();
                var fetchLastRoom = fypCommitte.Where(x => x.Room == room).LastOrDefault();
                var fetchRoomGroup = fypCommitte.Where(x => x.Room == room && x.EvaluationID == EvalType).FirstOrDefault();
                if (fetchLastRoom != null && fetchRoomGroup != null)
                {
                    if (fetchLastRoom.Room == room && fetchRoomGroup.groupID != id && fetchLastRoom.batch == batch)
                    {
                        if (EvalType == "Proposal")
                        {
                            TimeOnly time = fetchLastRoom.AppointedTime;
                            TimeOnly newTime = time.AddMinutes(30);
                            model.AppointedTime = newTime;
                        }
                        else if (EvalType == "Mid")
                        {
                            TimeOnly time = fetchLastRoom.AppointedTime;
                            TimeOnly newTime = time.AddMinutes(45);
                            model.AppointedTime = newTime;
                        }
                        else if (EvalType == "Final")
                        {
                            TimeOnly time = fetchLastRoom.AppointedTime;
                            TimeOnly newTime = time.AddMinutes(45);
                            model.AppointedTime = newTime;
                        }
                    }

                }
                else
                {
                    model.AppointedTime = new TimeOnly(9, 0);  // Set to 9:00 AM

                }
            }



            model.batches = allGroups.Select(x => x.Batch).Distinct().ToList();

            model.EvaluationID = EvalType;

            return PartialView("Evaluators", model);

        }

        [HttpPost]
        public async Task<IActionResult> FypCommitee(FYPCommitteViewModel model)
        {
            var getFYPCommitte = await _fYPCommitteService.GetAllAsync();
            var fypCommitte = getFYPCommitte.FirstOrDefault(x => x.groupID == model.groupID && x.EvaluationID == model.EvaluationID);

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
                if (fypCommitte.AppointedTime != model.AppointedTime)
                {
                    TimeOnly time = model.AppointedTime;
                    fypCommitte.AppointedTime = time;
                    isUpdated = true;
                }

                if (fypCommitte.Room != model.Room)
                {
                    fypCommitte.Room = model.Room;
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
                    return RedirectToAction("FetchBatches", "studentgroups", new { id = model.groupID, batch = model.batch, EvalType = model.EvaluationID });
                }

            }
            else
            {
                // Initialize a new FYPCommitte instance if it doesn't exist
                // Convert AppointedDate to string in dd-MM-yy format
                string appointedDateString = model.AppointedDate.ToString("dd-MM-yy");

                // Convert AppointedTime to string (this assumes AppointedTime is also DateTime or TimeSpan)
                string appointedTimeString = model.AppointedTime.ToString("HH:mm:ss"); // Format for time
                                                                                       // Parse AppointedDate from string back to DateTime
                DateTime parsedAppointedDate = DateTime.ParseExact(appointedDateString, "dd-MM-yy", CultureInfo.InvariantCulture);

                var evaluation = await _evaluationService.GetAllAsync();

                model.AppointedDate = parsedAppointedDate;
                TimeOnly time = model.AppointedTime;
                var newFypCommitte = new FYPCommitte
                {
                    Member1ID = model.Member1ID,
                    Member2ID = model.Member2ID,
                    groupID = model.groupID,
                    batch = model.batch,
                    AppointedDate = parsedAppointedDate,
                    AppointedTime = time,
                    Room = model.Room,
                    EvaluationID = model.EvaluationID,



                };

                if (model.EvaluationID == "Proposal")
                {
                    TimeOnly newTime = time.AddMinutes(30);
                    newFypCommitte.Endime = newTime;
                }
                if (model.EvaluationID == "Mid")
                {
                    TimeOnly newTime = time.AddMinutes(45);
                    newFypCommitte.Endime = newTime;
                }
                if (model.EvaluationID == "Final")
                {
                    TimeOnly newTime = time.AddMinutes(45);
                    newFypCommitte.Endime = newTime;
                }
                await _fYPCommitteService.AddAsync(newFypCommitte);
                var getFYPCommitteMid = await _fYPCommitteService.GetAllAsync();
                var fypCommitteMid = getFYPCommitteMid.FirstOrDefault(x => x.groupID == model.groupID && x.EvaluationID == "Proposal");
                if (fypCommitteMid != null)
                {
                    var newFypCommitteMid = new FYPCommitte
                    {
                        Member1ID = fypCommitteMid.Member1ID,
                        Member2ID = fypCommitteMid.Member2ID,
                        groupID = model.groupID,
                        batch = model.batch,
                        AppointedTime = fypCommitteMid.AppointedTime,
                        Room = fypCommitteMid.Room,
                        EvaluationID = "Mid"
                    };

                    TimeOnly newMidTime = fypCommitteMid.AppointedTime.AddMinutes(45);
                    newFypCommitteMid.Endime = newMidTime;

                    await _fYPCommitteService.AddAsync(newFypCommitteMid);
                }
                var getFYPCommitteFinal = await _fYPCommitteService.GetAllAsync();
                var fypCommitteFinal = getFYPCommitteFinal.FirstOrDefault(x => x.groupID == model.groupID && x.EvaluationID == "Mid");
                if (fypCommitteFinal != null)
                {
                    var newFypCommitteFinal = new FYPCommitte
                    {
                        Member1ID = fypCommitteFinal.Member1ID,
                        Member2ID = fypCommitteFinal.Member2ID,
                        groupID = model.groupID,
                        batch = fypCommitteFinal.batch,
                        AppointedTime = fypCommitteFinal.AppointedTime,
                        Room = fypCommitteFinal.Room,
                        EvaluationID = "Final"
                    };

                    TimeOnly newFinalTime = fypCommitteFinal.AppointedTime.AddMinutes(45);
                    newFypCommitteFinal.Endime = newFinalTime;

                    await _fYPCommitteService.AddAsync(newFypCommitteFinal);
                }
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

        [HttpPost]
        public async Task<IActionResult> FetchRoom(string room, string isRoom, string id)
        {
            var Room = await _roomService.GetAllAsync();
            var loggedUser = _userManager.GetUserId(User);
            var fetchUser = await _userManager.FindByIdAsync(loggedUser);
            var fetchRoom = Room.FirstOrDefault(x => x.ID.ToString() == id);
            var fetchExistingRoom = Room.FirstOrDefault(x => x.RoomNo.Trim().ToUpper() == room.Trim().ToUpper());
            var model = new FYPCommitteViewModel();
            if (fetchExistingRoom != null)
            {
                if (fetchExistingRoom.RoomNo == room.Trim().ToUpper())
                {
                    if (isRoom == "true")
                    {
                        return RedirectToAction("Rooms", "StudentGroups", new { ViewValue = "Existed" });
                    }
                    else
                    {
                        return Json(new { success = true, message = "Room already exists!", room = model });
                    }
                }
            }
            else
            {
                if (fetchRoom != null)
                {
                    fetchRoom.RoomNo = room;
                    await _roomService.UpdateAsync(fetchRoom);
                    model.Rooms = Room.Where(x => x.Department == fetchUser.Department).Select(x => new RoomViewModel
                    {
                        Id = x.ID.ToString(),
                        RoomNo = x.RoomNo,

                    }).ToList();

                    if (isRoom == "true")
                    {
                        return RedirectToAction("Rooms", "StudentGroups", new { ViewValue = "Updated" });
                    }
                    else
                    {
                        return Json(new { success = true, message = "Room Updated!", room = model });
                    }


                }
                else
                {
                    var result = _roomService.AddAsync(new Room { RoomNo = room.ToUpper().Trim(), Department = fetchUser.Department });
                    var fetchRoomagain = await _roomService.GetAllAsync();
                    if (result.IsCompletedSuccessfully)
                    {
                        model.Rooms = fetchRoomagain.Where(x => x.Department == fetchUser.Department).Select(x => new RoomViewModel
                        {
                            Id = x.ID.ToString(),
                            RoomNo = x.RoomNo

                        }).ToList();
                        if (isRoom == "true")
                        {
                            return RedirectToAction("Rooms", "StudentGroups", new { ViewValue = "Added" });
                        }
                        else
                        {
                            return Json(new { success = true, message = "Room added successfully!", room = model });
                        }
                    }
                }
            }
            return Json(new { success = false, message = "Room not added successfully!" });
        }


        [HttpPost]
        public async Task<IActionResult> DeleteRoom(string id)
        {
            var result = _roomService.DeleteAsync(id);
            if (result.IsCompletedSuccessfully)
            {
                return RedirectToAction("Rooms", "StudentGroups", new { ViewValue = "Deleted" });
            }
            else
            {
                return RedirectToAction("Rooms", "StudentGroups", new { ViewValue = "Failed" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteRoomIncharge(string id)
        {
            var Allotment = await _roomAllotmentService.GetAllAsync();
            var roomAllotment = Allotment.Where(x => x.InchargeId == id).FirstOrDefault();

            if (roomAllotment != null)
            {
                // If there's an associated room allotment, don't allow deletion
                return RedirectToAction("RoomIncharges", "StudentGroups", new { ViewValue = "InchargeAssigned" });
            }
            var result = _roomInChargeService.DeleteAsync(id);
            if (result.IsCompletedSuccessfully)
            {
                return RedirectToAction("RoomIncharges", "StudentGroups", new { ViewValue = "Deleted" });
            }
            else
            {
                return RedirectToAction("RoomIncharges", "StudentGroups", new { ViewValue = "Failed" });
            }
        }



        [HttpGet]
        public async Task<IActionResult> Rooms(string ViewValue)
        {
            if (ViewValue != null)
            {
                ViewBag.success = ViewValue;
            }
            var Rooms = await _roomService.GetAllAsync();
            var fetchRooms = Rooms.Select(x => new RoomViewModel
            {
                Id = x.ID.ToString(),
                RoomNo = x.RoomNo
            }).ToList();
            var model = new FYPCommitteViewModel();
            model.Rooms = fetchRooms;
            return PartialView(model);
        }
        [HttpGet]
        public async Task<IActionResult> RoomIncharges(string ViewValue)
        {
            if (ViewValue != null)
            {
                ViewBag.success = ViewValue;
            }

            // Fetch all room incharges
            var RoomIncharge = await _roomInChargeService.GetAllAsync();
            var fetchRoomsIncharge = RoomIncharge.Select(x => new RoomInChargeViewModel
            {
                Id = x.ID.ToString(),
                Email = x.Email,
                Name = x.Name,
            }).ToList();
            var groups = await _studentGroupService.GetAllAsync();


            // Prepare the ViewModel
            var model = new FYPCommitteViewModel();
            model.roomInCharges = fetchRoomsIncharge;
            if (groups != null)
            {
                model.batches = groups.Select(x => x.Batch).Distinct().ToList();
            }

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> FetchRoomIncharges(FYPCommitteViewModel incharge)
        {
            var RoomIncharge = await _roomInChargeService.GetAllAsync();
            var fetchRoomIncharge = RoomIncharge.FirstOrDefault(x => x.ID == incharge.Id);
            var fetchExistingRoomIncharge = RoomIncharge.FirstOrDefault(x => x.Email == incharge.RoomInChargeEmail);
            var model = new FYPCommitteViewModel();
            if (incharge.RoomInChargeEmail==null && incharge.RoomInChargeName == null || incharge.RoomInChargeEmail == null || incharge.RoomInChargeName == null)
            {
                return RedirectToAction("RoomIncharges", "StudentGroups", new { ViewValue = "Null" });

            }
            if (fetchExistingRoomIncharge != null)
            {
                if (fetchExistingRoomIncharge.Email == incharge.RoomInChargeEmail && fetchExistingRoomIncharge.Name == incharge.RoomInChargeName)
                {
                    return RedirectToAction("RoomIncharges", "StudentGroups", new { ViewValue = "Existed" });
                }
            }
            if (fetchRoomIncharge != null)
            {
                if (incharge.istrue == "true")
                {
                    fetchRoomIncharge.Email = incharge.RoomInChargeEmail;
                    fetchRoomIncharge.Name = incharge.RoomInChargeName;
                }

                await _roomInChargeService.UpdateAsync(fetchRoomIncharge);
                return RedirectToAction("RoomIncharges", "StudentGroups", new { ViewValue = "Updated" });
            }
            else
            {
                var result = _roomInChargeService.AddAsync(new RoomInCharge { Email = incharge.RoomInChargeEmail, Name = incharge.RoomInChargeName });
                var fetchRoomInchargeagain = await _roomService.GetAllAsync();
                if (result.IsCompletedSuccessfully)
                {
                    return RedirectToAction("RoomIncharges", "StudentGroups", new { ViewValue = "Added" });
                }
            }

            return Json(new { success = false, message = "Incharge not Updated successfully!" });
        }


        public async Task<IActionResult> AssignIncharge(string Batch, string Evaluation, string ViewValue, string name)
        {
            var RoomIncharge = await _roomInChargeService.GetAllAsync();
            var fetchRoomIncharge = RoomIncharge.ToList();
            var Room = await _roomService.GetAllAsync();
            var groups = await _studentGroupService.GetAllAsync();
            var roomalloted = await _roomAllotmentService.GetAllAsync();
            if (ViewValue != null)
            {
                ViewBag.success = ViewValue;
            }

            if (Batch != null && Evaluation == null && roomalloted != null)
            {
                if (fetchRoomIncharge != null)
                {
                    var fetchRoomInchargebyBatch = roomalloted.Where(x => x.Batch == Batch).ToList();

                    var model = new RoomInChargeViewModel()
                    {
                        //roomInCharges = fetchRoomIncharge.Select(x =>
                        //{
                        //    // Find the room assignment for this in-charge for the specified batch
                        //    var assignedRoom = fetchRoomInchargebyBatch.FirstOrDefault(r => r.InchargeId == x.ID.ToString());
                        //    var roomDetails = assignedRoom != null ? Room.FirstOrDefault(r => r.ID.ToString() == assignedRoom.Room) : null;

                        //    return new RoomInChargeViewModel
                        //    {
                        //        Id = x.ID.ToString(),
                        //        Name = x.Name + (assignedRoom != null ? $" (Assigned - Room {roomDetails?.RoomNo})" : "")
                        //    };
                        //}).ToList(),

                        //Rooms = Room.Select(x =>
                        //{
                        //    // Check if the room is allocated for the specific batch and in-charge
                        //    var isAllocated = roomalloted.Any(r => r.Room == x.ID.ToString() && r.Batch == Batch );

                        //    return new RoomViewModel
                        //    {
                        //        Id = x.ID.ToString(),
                        //        RoomNo = isAllocated ? x.RoomNo + " (Allocated)" : x.RoomNo
                        //    };
                        //}).ToList(),


                        Batchs = groups.Select(x => x.Batch).Distinct().ToList(),
                        Batch = Batch,

                        AllotedRoomIncharges = roomalloted
                                .Where(x => !string.IsNullOrEmpty(x.Room) && x.Batch == Batch)
                                .Select(x => new RoomInChargeViewModel
                                {
                                    Id = x.ID.ToString(),
                                    Name = RoomIncharge.FirstOrDefault(r => r.ID.ToString() == x.InchargeId)?.Name,
                                    RoomAlloted = Room.FirstOrDefault(r => r.ID.ToString() == x.Room)?.RoomNo,
                                    Evaluation = x.Evaluation,
                                    Batch = x.Batch

                                }).ToList(),
                    };

                    return PartialView(model);
                }
            }

            if (Batch != null && Evaluation != null && roomalloted != null)
            {
                var fetchRoomInchargebyBatch = roomalloted.Where(x => x.Batch == Batch).ToList();

                if (fetchRoomIncharge != null)
                {

                    var model = new RoomInChargeViewModel()
                    {
                        roomInCharges = fetchRoomIncharge.Select(x =>
                        {
                            // Find the room assignment for this in-charge for the specified batch and evaluation
                            var assignedRoom = fetchRoomInchargebyBatch.FirstOrDefault(r => r.InchargeId == x.ID.ToString() && r.Evaluation == Evaluation);
                            var roomDetails = assignedRoom != null ? Room.FirstOrDefault(r => r.ID.ToString() == assignedRoom.Room) : null;

                            return new RoomInChargeViewModel
                            {
                                Id = x.ID.ToString(),
                                Name = x.Name + (assignedRoom != null ? $" (Assigned - Room {roomDetails?.RoomNo})" : "")
                            };
                        }).ToList(),

                        Rooms = Room.Select(x =>
                        {
                            // Check if the room is allocated for the specific batch and in-charge
                            var isAllocated = roomalloted.Any(r => r.Room == x.ID.ToString() && r.Batch == Batch && r.Evaluation == Evaluation);

                            return new RoomViewModel
                            {
                                Id = x.ID.ToString(),
                                RoomNo = isAllocated ? x.RoomNo + " (Allocated)" : x.RoomNo
                            };
                        }).ToList(),

                        Batchs = groups.Select(x => x.Batch).Distinct().ToList(),
                        Batch = Batch,

                        AllotedRoomIncharges = roomalloted
                            .Where(x => !string.IsNullOrEmpty(x.Room) && x.Batch == Batch && x.Evaluation == Evaluation)
                            .Select(x => new RoomInChargeViewModel
                            {
                                Id = x.ID.ToString(),
                                Evaluation = x.Evaluation,
                                Name = RoomIncharge.FirstOrDefault(r => r.ID.ToString() == x.InchargeId)?.Name,
                                RoomAlloted = Room.FirstOrDefault(r => r.ID.ToString() == x.Room)?.RoomNo,
                                Batch = x.Batch
                            }).ToList(),

                        Evaluation = Evaluation
                    };
                    if (name != null)
                    {
                        var fetchIncharge = roomalloted.Where(x => x.InchargeId == name && x.Batch == Batch && x.Evaluation == Evaluation);
                        if (fetchRoomIncharge != null)
                        {
                            var fetchRoomInchargeRoom = fetchIncharge.Select(x => x.Room).FirstOrDefault();
                            model.RoomAlloted = fetchRoomInchargeRoom;
                        }
                    }
                    return PartialView(model);
                }
            }

            if (fetchRoomIncharge != null)
            {
                var roomsAllocatedForBatch = roomalloted.Select(x => x.Room).Distinct().ToList();
                var fetchgroups = groups.Select(x => x.Batch).Distinct().ToList();
                if (fetchgroups != null)
                {
                    var model = new RoomInChargeViewModel()
                    {
                        Batchs = fetchgroups,
                        roomInCharges = null,
                        Batch = null,
                    };
                    return PartialView(model);

                }
            }

            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> AssignIncharge(RoomInChargeViewModel incharge)
        {
            if (incharge.Batch == null || incharge.RoomAlloted == null || incharge.Name == null || incharge.Evaluation == null)
            {
                return RedirectToAction("AssignIncharge", "StudentGroups", new
                {
                    Batch = incharge.Batch,
                    Evaluation = incharge.Evaluation,
                    name = incharge.Name,
                    ViewValue = "SelectFirst"
                });
            }
            var fetchRoom = await _roomAllotmentService.GetAllAsync();
            var fetchExistingRoom = fetchRoom.FirstOrDefault(x =>
                x.Batch == incharge.Batch &&
                x.Evaluation == incharge.Evaluation &&
                x.InchargeId == incharge.Name);

            var fetchExistingInchargebyRoom = fetchRoom.FirstOrDefault(x =>
                x.Batch == incharge.Batch &&
                x.Evaluation == incharge.Evaluation &&
                x.Room == incharge.RoomAlloted);

            // Condition 1: If in-charge ID and room are the same
            if (fetchExistingRoom != null && fetchExistingRoom.Room == incharge.RoomAlloted)
            {
                return RedirectToAction("AssignIncharge", "StudentGroups", new
                {
                    Batch = incharge.Batch,
                    Evaluation = incharge.Evaluation,
                    ViewValue = "Existed"
                });
            }

            // Condition 2: If the in-charge exists but the room exists and is assigned to someone else
            if (fetchExistingRoom != null && fetchExistingInchargebyRoom != null && fetchExistingInchargebyRoom.InchargeId != incharge.Name)
            {
                return RedirectToAction("AssignIncharge", "StudentGroups", new
                {
                    Batch = incharge.Batch,
                    Evaluation = incharge.Evaluation,
                    ViewValue = "Assigned"
                });
            }

            // Condition 3: If the in-charge does not exist but the room exists and is assigned to someone else
            if (fetchExistingRoom == null && fetchExistingInchargebyRoom != null)
            {
                return RedirectToAction("AssignIncharge", "StudentGroups", new
                {
                    Batch = incharge.Batch,
                    Evaluation = incharge.Evaluation,
                    ViewValue = "Assigned"
                });
            }

            // If no existing room assignments were found, create a new one
            var newAllotment = new RoomAllotment
            {
                InchargeId = incharge.Name,
                Room = incharge.RoomAlloted,
                Batch = incharge.Batch,
                Evaluation = incharge.Evaluation
            };
            await _roomAllotmentService.AddAsync(newAllotment);
            return RedirectToAction("AssignIncharge", "StudentGroups", new
            {
                Batch = incharge.Batch,
                Evaluation = incharge.Evaluation,
                ViewValue = "Success"
            });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteRoomAllotment(string id, string Evaluation, string Batch)
        {
            // Fetch the room allotment using the provided ID
            var roomAllotment = await _roomAllotmentService.GetAllAsync();
            var RoomAllotmentToDelete = roomAllotment.FirstOrDefault(x => x.ID.ToString() == id);

            // Check if the room allotment was found
            if (RoomAllotmentToDelete != null)
            {
                // Remove the room allotment from the database

                await _roomAllotmentService.DeleteAsync(id);
                return RedirectToAction("AssignIncharge", "StudentGroups", new
                {
                    Evaluation = Evaluation,
                    Batch = Batch,
                    ViewValue = "Deleted"
                });
            }
            else
            {
                // Handle the case where the room allotment was not found
                return RedirectToAction("AssignIncharge", "StudentGroups", new { ViewValue = "NotFound" });
            }
        }
        public async Task<IActionResult> ExportAllotedRoomInchargesToExcel(string Batch, string Evaluation)
        {
            var RoomIncharge = await _roomInChargeService.GetAllAsync();
            var Rooms = await _roomService.GetAllAsync();
            var roomalloted = await _roomAllotmentService.GetAllAsync();
            var roomallotedbybatch = roomalloted.FirstOrDefault(x => x.Batch == Batch && x.Evaluation == Evaluation);

            if (roomallotedbybatch == null)
            {
                return Json(new { success = false, message = "Data does not exist!" });
            }

            // If data exists, respond with success
            return Json(new { success = true });
        }

        public async Task<IActionResult> GroupEvaluation(string groupId ,  string ViewValue , string EvalName)
        {
            if (ViewValue != null)
            {
                ViewBag.success = ViewValue;
            }
            var currentUserId = _userManager.GetUserId(User);
            var currentDate = DateTime.Today;
            var committees = await _fYPCommitteService.GetAllAsync();
            // Filter committees based on AppointedDate and logged-in user
            var filteredCommittees = committees.Where
                                                           (c=>c.Member1ID == currentUserId || c.Member2ID == currentUserId && c.AppointedDate == currentDate).ToList();
            var viewModel = new EvaluationMarksViewModel();

                var groups = await _studentGroupService.GetAllAsync();
            if (filteredCommittees != null)
            {
                var filteredGroupIds = filteredCommittees.Select(c => c.groupID).Distinct().ToList();
                var filteredGroups = groups.Where(g => filteredGroupIds.Contains(g.ID.ToString())).ToList();
                viewModel.Groups = filteredGroups;
            }
            if (groupId !=null)
            {
                var evaluations = await _evaluationService.GetAllAsync();
                var filteredEvaluations = filteredCommittees
                    .Where(c => c.groupID == groupId && c.AppointedDate == currentDate)
                    .FirstOrDefault().EvaluationID;
                viewModel.EvalName = filteredEvaluations;
                viewModel.GroupId = groupId;

                var fetchGroup = groups.Where(x => x.ID.ToString() == groupId).FirstOrDefault();
                var userGet = await _userService.GetAllAsync();
                viewModel.stu1 = userGet.Where(x=>x.Id.ToString() == fetchGroup.student1LID).FirstOrDefault().Name;
                viewModel.stu2 = userGet.Where(x=>x.Id.ToString() == fetchGroup.student2ID).FirstOrDefault().Name;
                viewModel.stu3 = userGet.Where(x=>x.Id.ToString() == fetchGroup.student3ID).FirstOrDefault().Name;
                if (groupId != null )
                {
                    var evaluationCriteriaList = await _evaluationCriteriaService.GetAllAsync();

                    // Fetch the existing record based on GroupId and EvalName
                    var existingRecord = evaluationCriteriaList
                        .FirstOrDefault(e => e.GId == groupId && e.SubmissionDate == currentDate && e.CommiteeID == currentUserId);
                    if (existingRecord != null)
                    {
                        viewModel.qno1 = existingRecord.Q1Marks;
                        viewModel.qno2 = existingRecord.Q2Marks;
                        viewModel.qno3 = existingRecord.Q3Marks;
                        viewModel.qno4 = existingRecord.Q4Marks;
                        viewModel.qno5 = existingRecord.Q5Marks;
                        viewModel.qno6 = existingRecord.Q6Marks;
                        viewModel.qno7 = existingRecord.Q7Marks;
                        viewModel.qno8 = existingRecord.Q8Marks;
                        viewModel.Remarks = existingRecord.Remarks;
                    }
                }


            }

            return PartialView(viewModel);

        }

        [HttpGet]
        public async Task<IActionResult> CheckSubmissionEligibility(string groupId, string evalName)
        {
            // Fetch the record from the database
            var EvaluationCriteria = await _evaluationCriteriaService.GetAllAsync();
            var record = EvaluationCriteria.FirstOrDefault(e => e.GId == groupId && e.EvalName == evalName );

            if (record != null && record.SubmissionTime.HasValue)
            {
                // Calculate the time elapsed since the first submission
                var timeElapsed = DateTime.UtcNow - record.SubmissionTime.Value;

                // If less than 30 minutes have passed, allow assigning marks
                if (timeElapsed.TotalMinutes < 30)
                {
                    return Json(new { canAssign = true, minutesLeft = 30 - timeElapsed.TotalMinutes });
                }

                // If 30 minutes have passed, permanently lock the button
                return Json(new { canAssign = false, isLocked = true });
            }

            // If no record exists, allow assigning marks
            return Json(new { canAssign = true, isLocked = false });
        }

        [HttpPost]
        public async Task<IActionResult> AssignEvaluationMarks([FromBody] EvaluationMarksViewModel viewModel)
        {
           

            // Store the question marks in an array and calculate TotalMarks
            var questionMarks = new string[] { viewModel.qno1, viewModel.qno2, viewModel.qno3,
                                       viewModel.qno4, viewModel.qno5, viewModel.qno6,
                                       viewModel.qno7, viewModel.qno8 };
            decimal totalMarks = questionMarks
                .Select(q => string.IsNullOrEmpty(q) ? 0 : Convert.ToDecimal(q)) // Convert each to decimal or 0
                .Sum(); // Sum all the marks

            viewModel.TotalMarks = totalMarks;


            // Set the calculated TotalMarks in the viewModel
            viewModel.TotalMarks = totalMarks;

            var evaluationCriteriaList = await _evaluationCriteriaService.GetAllAsync();
            var currentUserId = _userManager.GetUserId(User);
            var group = await _studentGroupService.GetAllAsync();
            var fetchBatch = group.Where(x => x.ID.ToString() == viewModel.GroupId).FirstOrDefault().Batch;
            // Fetch the existing record based on GroupId and EvalName
            var existingRecord = evaluationCriteriaList
                .FirstOrDefault(e => e.GId == viewModel.GroupId && e.EvalName == viewModel.EvalName && e.CommiteeID == currentUserId);

            if (existingRecord != null)
            {
                // Update existing record with new data
                existingRecord.Q1Marks = viewModel.qno1;
                existingRecord.Q2Marks = viewModel.qno2;
                existingRecord.Q3Marks = viewModel.qno3;
                existingRecord.Q4Marks = viewModel.qno4;
                existingRecord.Q5Marks = viewModel.qno5;
                existingRecord.Q6Marks = viewModel.qno6;
                existingRecord.Q7Marks = viewModel.qno7;
                existingRecord.Q8Marks = viewModel.qno8;

                if (!string.IsNullOrEmpty(viewModel.Remarks))
                {
                    existingRecord.Remarks = viewModel.Remarks;
                }

                existingRecord.TotalMarks = viewModel.TotalMarks;
                existingRecord.SubmissionTime = DateTime.UtcNow;
                existingRecord.SubmissionDate = DateTime.Today;

                await _evaluationCriteriaService.UpdateAsync(existingRecord);

                return RedirectToAction("GroupEvaluation", "StudentGroups", new
                {
                    groupId = viewModel.GroupId,
                    ViewValue = "Updated"
                });
            }
            else
            {
                // Create a new record if no existing one was found
                var newEvaluation = new EvaluationCriteria
                {
                    GId = viewModel.GroupId,
                    Batch = fetchBatch,
                    EvalName = viewModel.EvalName,
                    Q1Marks = viewModel.qno1,
                    Q2Marks = viewModel.qno2,
                    Q3Marks = viewModel.qno3,
                    Q4Marks = viewModel.qno4,
                    Q5Marks = viewModel.qno5,
                    Q6Marks = viewModel.qno6,
                    Q7Marks = viewModel.qno7,
                    Q8Marks = viewModel.qno8,
                    Remarks = viewModel.Remarks,
                    TotalMarks = viewModel.TotalMarks,
                    CommiteeID = currentUserId,
                    SubmissionTime = DateTime.UtcNow,
                    SubmissionDate = DateTime.Today
                };

                await _evaluationCriteriaService.AddAsync(newEvaluation);

                return RedirectToAction("GroupEvaluation", "StudentGroups", new
                {
                    groupId = viewModel.GroupId,
                    ViewValue = "Assigned"
                });
            }
        }


        [HttpPost]
        public async Task<IActionResult> FilterGroups(string GroupWord, string Batch)
        {
            var model = new StudentGroupViewModel();
            var currentUserId = _userManager.GetUserId(User);  // Get the current user ID

            // Fetch all groups
            var groups = await _studentGroupService.GetAllAsync();

            // If GroupWord is empty, we only filter by SupervisorID and Batch
            var filteredGroups = groups
                .Where(g => g.SupervisorID == currentUserId) // Filter by SupervisorID
                .Where(g => string.IsNullOrWhiteSpace(GroupWord) || g.Name.Contains(GroupWord, StringComparison.OrdinalIgnoreCase)) // If GroupWord is not empty, filter by group name
                .Where(g => string.IsNullOrEmpty(Batch) || g.Batch == Batch) // If Batch is selected, filter by Batch
                .ToList();

            // Get project details
            var project = await _projectService.GetAllAsync();

            var studentGroupViewModels = new List<StudentGroupViewModel>();

            foreach (var g in filteredGroups)
            {
                var leaderTask = _userService.GetByIdAsync(g.student1LID);
                var member1Task = _userService.GetByIdAsync(g.student2ID);
                var member2Task = _userService.GetByIdAsync(g.student3ID);
                var supervisorTask = _userManager.FindByIdAsync(g.SupervisorID);

                // Prepare tasks to fetch user details
                await Task.WhenAll(leaderTask, member1Task, member2Task, supervisorTask);

                var leader = await leaderTask;
                var member1 = await member1Task;
                var member2 = await member2Task;
                var supervisor = await supervisorTask;

                studentGroupViewModels.Add(new StudentGroupViewModel
                {
                    GrouID = g.ID.ToString(),
                    groupId = g.ID.ToString(),
                    companyID = g.companyID,
                    Batch = g.Batch,
                    CordinatorID = g.CordinatorID,
                    CoSupervisorID = g.CoSupervisorID,
                    Id = g.ID,
                    Name = g.Name,
                    student1LID = g.student1LID,
                    student2ID = g.student2ID,
                    student3ID = g.student3ID,
                    SupervisorID = g.SupervisorID,
                    Year = g.Year,
                    projectname = project.Where(p => p.projectGroup == g.Name).Select(p => p.Title).FirstOrDefault(),

                    // Fetching member names
                    LeaderName = leader?.Email ?? "Not Found",
                    member1 = member1?.Email ?? "Not Found",
                    Member2 = member2?.Email ?? "Not Found",

                    // Fetching supervisor name
                    supervisorname = supervisor?.Name ?? "Not Found"
                });
            }

            model.StudentGroups = studentGroupViewModels;

            return Json(model.StudentGroups);
        }






        public async Task<IActionResult> DownloadExcel(string Batch, string Evaluation)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var RoomIncharge = await _roomInChargeService.GetAllAsync();
            var Rooms = await _roomService.GetAllAsync();
            var roomalloted = await _roomAllotmentService.GetAllAsync();

            var roomAllotments = roomalloted
                .Where(x => !string.IsNullOrEmpty(x.Room) && x.Batch == Batch && x.Evaluation == Evaluation)
                .Select(x => new RoomInChargeViewModel
                {
                    Id = x.ID.ToString(),
                    Name = RoomIncharge.FirstOrDefault(r => r.ID.ToString() == x.InchargeId)?.Name,
                    RoomAlloted = Rooms.FirstOrDefault(r => r.ID.ToString() == x.Room)?.RoomNo,
                    Evaluation = x.Evaluation,
                    Batch = x.Batch
                }).ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Room Incharges");
                worksheet.Cells[1, 1].Value = "Name";
                worksheet.Cells[1, 2].Value = "Room Alloted";
                worksheet.Cells[1, 3].Value = "Evaluation";
                worksheet.Cells[1, 4].Value = "Batch";
                worksheet.Row(1).Style.Font.Bold = true;

                int row = 2;
                foreach (var item in roomAllotments)
                {
                    worksheet.Cells[row, 1].Value = item.Name;
                    worksheet.Cells[row, 2].Value = item.RoomAlloted;
                    worksheet.Cells[row, 3].Value = item.Evaluation;
                    worksheet.Cells[row, 4].Value = item.Batch;
                    row++;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string excelName = $"AllotedRoomIncharges-{Batch}-{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }

    }
}
