using FYP.Entities;
using FYP.Services;
using FYP.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileSystemGlobbing;
using System.Globalization;

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

        public CordinatorController(IFYPCommitteService fYPCommitteService, IEvaluationService evaluationService, ICompanyService companyService, IProjectService projectService, IUserService userService, UserManager<AppUser> userManager, IRoomAllotmentService supervisorService, IProposalDefenseService proposalDefense, IRoomService designationService, IStudentGroupService studentGroupService)
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
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> StudentGroup()
        {
            var studentGroup =await _studentGroupService.GetAllAsync();
            var existingFypCommitee =await _fYPCommitteService.GetAllAsync();
            var user = studentGroup.FirstOrDefault();
            var allGroups =await _studentGroupService.GetAllAsync();
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
            var studentGroup =await _studentGroupService.GetAllAsync();
            var user = studentGroup.FirstOrDefault();
            var allGroups =await _studentGroupService.GetAllAsync();
            var userGroup = allGroups
                .Where(x => x.student1LID == user.student1LID || x.student2ID == user.student2ID || x.student3ID == user.student3ID)
                .FirstOrDefault();
            var supervisorId = userGroup.SupervisorID;
            var project = await _projectService.GetAllAsync();
            var projects = project.Where(x=>x.SupervsiorApproved == "Approved" && x.projectGroup == FetchUserDepartment);
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
                return RedirectToAction("Projects","Cordinator");
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

            var projects = _projectService.GetAllAsync().Result.Where(x=>x.batch == batch);
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
            return PartialView("projects",model);
        }

        public async Task<IActionResult> Evaluation(string Etype,int marks,string batch)
        {
            var group = await _studentGroupService.GetAllAsync();
            var distinctBatches = group.Select(x => x.Batch).Distinct().ToList();
            string date = DateTime.Now.ToString("dd-MM-yy"); // Example string date
            DateTime parsedDate = DateTime.ParseExact(date, "dd-MM-yy", CultureInfo.InvariantCulture);
            var model = new EvaluationViewModel()
                {
                    EvaluationName = Etype,
                    batches = distinctBatches,
                    LastDate = parsedDate,
                    Marks = marks,
                    
                };
                return PartialView(model);
            
        }
        [HttpGet]
        public async Task<IActionResult> GetEvaluationData(string batch, string evaluationName)
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
                    lastDate = evaluation.LastDate.ToString("dd/MM/yy")  // Formatting date
                });
            }
            else
            {
                return Json(new { success = false });
            }
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
                    existingEvaluation.EvaluationName = model.EvaluationName;
                    existingEvaluation.Marks = model.Marks;
                    existingEvaluation.PBatch = model.PBatch;
                    existingEvaluation.LastDate = model.LastDate;

                    _evaluationService.UpdateAsync(existingEvaluation);
                return RedirectToAction("Projects", "Cordinator");

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
                    return RedirectToAction("Projects", "Cordinator");

            }




        }


        //[HttpGet]
        //public async Task<IActionResult> Incharge()
        //{

        //}

    }
}
