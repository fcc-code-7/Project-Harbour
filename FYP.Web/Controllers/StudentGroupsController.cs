using FYP.Databases;
using FYP.Entities;
using FYP.Models;
using FYP.Services;
using FYP.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

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
        private readonly IRoomService _roomService;
        private readonly IRoomInChargeService _roomInChargeService;
        private readonly IEvaluationService _evaluationService;

        public StudentGroupsController(IEvaluationService evaluationService,IRoomInChargeService roomInChargeService, IRoomService roomService, IFYPCommitteService fYPCommitteService, IChangeSupervisorFormService changeSupervisorFormService, IUserService userService, ISupervisorService supervisorService, IStudentService studentService, IStudentGroupService studentGroupService, UserManager<AppUser> userManager, IProjectService projectService, IProposalDefenseService proposalDefense)
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
            _roomService = roomService;
            _roomInChargeService = roomInChargeService;
            _evaluationService = evaluationService;
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
                        return RedirectToAction("StudentGroups", "studentgroups");
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
            string date = DateTime.Now.ToString("dd-MM-yy"); // Example string date
            DateTime parsedDate = DateTime.ParseExact(date, "dd-MM-yy", CultureInfo.InvariantCulture);
            model.AppointedDate = parsedDate;
            model.AppointedTime = new TimeOnly(9, 0);  // Set to 9:00 AM
            return PartialView(model);
        }

        public async Task<IActionResult> FetchBatches(string id, string batch, string assignRequest, string EvalType,string room)
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
                var currentFypCommitte = fypCommitte.Where(x => x.groupID == id && x.EvaluationID == EvalType ).FirstOrDefault();

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
                var fetchRoomGroup = fypCommitte.Where(x => x.Room == room  && x.EvaluationID == EvalType).FirstOrDefault();
                if (fetchLastRoom != null && fetchRoomGroup!=null)
                {
                    if (fetchLastRoom.Room == room && fetchRoomGroup.groupID != id)
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
                        AppointedDate = evaluation.Where(x=>x.EvaluationName == "Mid").FirstOrDefault().LastDate,
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
                        AppointedDate = evaluation.Where(x => x.EvaluationName == "Final").FirstOrDefault().LastDate,
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
        public async Task<IActionResult> FetchRoom(string room , string isRoom , string id)
        {
            var Room = await _roomService.GetAllAsync();
            var loggedUser = _userManager.GetUserId(User);
            var fetchUser = await _userManager.FindByIdAsync(loggedUser);
            var fetchRoom = Room.FirstOrDefault(x => x.ID.ToString() == id );
            var fetchExistingRoom = Room.FirstOrDefault(x=>x.RoomNo.Trim().ToUpper() == room.Trim().ToUpper());
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
        //[HttpPost]
        //public async Task<IActionResult> FetchRoomIncharges(FYPCommitteViewModel incharge)
        //{
        //    var RoomIncharge = await _roomInChargeService.GetAllAsync();
        //    var fetchRoomIncharge = RoomIncharge.FirstOrDefault(x => x.ID == incharge.Id);
        //    var fetchExistingRoomIncharge = RoomIncharge.FirstOrDefault(x => x.Email == incharge.RoomInChargeEmail);
        //    var model = new FYPCommitteViewModel();
        //    if (fetchExistingRoomIncharge != null)
        //    {
        //        if (fetchExistingRoomIncharge.Email == incharge.RoomInChargeEmail && fetchExistingRoomIncharge.Name == incharge.RoomInChargeName )
        //        { 
        //                return RedirectToAction("Rooms", "StudentGroups", new { ViewValue = "Existed" }); 
        //        }
        //    }
        //    else
        //    {
        //        if (fetchRoomIncharge != null)
        //        {
        //            if (incharge.istrue == "true")
        //            {
        //            fetchRoomIncharge.Email = incharge.RoomInChargeEmail;
        //            fetchRoomIncharge.Name = incharge.RoomInChargeName;
        //            }
        //            fetchRoomIncharge.RoomAlloted = incharge.Room;
        //            fetchRoomIncharge.Evaluation = incharge.EvaluationID;
        //            await _roomInChargeService.UpdateAsync(fetchRoomIncharge);
        //            model.Rooms = Room.Where(x => x.Department == fetchUser.Department).Select(x => new RoomViewModel
        //            {
        //                Id = x.ID.ToString(),
        //                RoomNo = x.RoomNo,

        //            }).ToList();

        //            if (isRoom == "true")
        //            {
        //                return RedirectToAction("Rooms", "StudentGroups", new { ViewValue = "Updated" });
        //            }
        //            else
        //            {
        //                return Json(new { success = true, message = "Room Updated!", room = model });
        //            }


        //        }
        //        else
        //        {
        //            var result = _roomService.AddAsync(new Room { RoomNo = room.ToUpper().Trim(), Department = fetchUser.Department });
        //            var fetchRoomagain = await _roomService.GetAllAsync();
        //            if (result.IsCompletedSuccessfully)
        //            {
        //                model.Rooms = fetchRoomagain.Where(x => x.Department == fetchUser.Department).Select(x => new RoomViewModel
        //                {
        //                    Id = x.ID.ToString(),
        //                    RoomNo = x.RoomNo

        //                }).ToList();
        //                if (isRoom == "true")
        //                {
        //                    return RedirectToAction("Rooms", "StudentGroups", new { ViewValue = "Added" });
        //                }
        //                else
        //                {
        //                    return Json(new { success = true, message = "Room added successfully!", room = model });
        //                }
        //            }
        //        }
        //    }
        //    return Json(new { success = false, message = "Room not added successfully!" });
        //}
      

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
                Evaluation = x.Evaluation,
                Email = x.Email,
                Name = x.Name,
                RoomAlloted = x.RoomAlloted
            }).ToList();

            // Fetch all rooms
            var rooms = await _roomService.GetAllAsync();

            // Filter rooms that are not allotted to any incharge
            var fetchRooms = rooms
                .Where(room => !RoomIncharge.Any(incharge => incharge.RoomAlloted == room.RoomNo))
                .Select(x => new RoomViewModel
                {
                    Id = x.ID.ToString(),
                    RoomNo = x.RoomNo
                }).ToList();

            // Prepare the ViewModel
            var model = new FYPCommitteViewModel();
            model.roomInCharges = fetchRoomsIncharge;
            model.Rooms = fetchRooms;

            return PartialView(model);
        }
    }
}
