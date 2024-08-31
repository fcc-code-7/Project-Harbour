using FYP.Entities;
using FYP.Services;
using FYP.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FYP.Web.Controllers
{
    public class CordinatorController : Controller
    {
        private readonly IUserService _userService;
        private readonly ISupervisorService _supervisorService;
        private readonly IProposalDefenseService _proposalDefenseService;
        private readonly IDesignationService _designationService;
        private readonly IStudentGroupService _studentGroupService;
        private readonly IProjectService _projectService;
        private readonly ICompanyService _companyService;
        private readonly IEvaluationService _evaluationService;
        private readonly UserManager<AppUser> _userManager;

        public CordinatorController(IEvaluationService evaluationService, ICompanyService companyService, IProjectService projectService, IUserService userService, UserManager<AppUser> userManager, ISupervisorService supervisorService, IProposalDefenseService proposalDefense, IDesignationService designationService, IStudentGroupService studentGroupService)
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
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult StudentGroup()
        {
            var studentGroup = _studentGroupService.GetAllAsync().Result;
            var user = studentGroup.FirstOrDefault();
            var allGroups = _studentGroupService.GetAllAsync().Result;
            var userGroup = allGroups
                .Where(x => x.student1LID == user.student1LID || x.student2ID == user.student2ID || x.student3ID == user.student3ID)
                .FirstOrDefault();
            var supervisorId = userGroup.SupervisorID;
            var distinctBatches = studentGroup.Select(x => x.Batch).Distinct().ToList();
            var distinctYears = studentGroup.Select(x => x.Year).Distinct().ToList();

            var model = new StudentGroupViewModel()
            {
                StudentGroups = studentGroup.Select(x => new StudentGroupViewModel
                {
                    Id =x.ID,
                    Name = x.Name,
                    Batch = x.Batch,
                    student1LID = x.student1LID,
                    student2ID = x.student2ID,
                    student3ID = x.student3ID,
                    CoSupervisorID = x.CoSupervisorID,
                    SupervisorID = x.SupervisorID,
                    supervisorname = _userManager.FindByIdAsync(supervisorId).Result?.Name,
                    LeaderName = _userManager.FindByIdAsync(x.student1LID).Result?.Name,
                    member1 = _userManager.FindByIdAsync(x.student2ID).Result?.Name,
                    Member2 = _userManager.FindByIdAsync(x.student3ID).Result?.Name,
                    
                }).ToList(),
                
                Batches = distinctBatches,
                Years = distinctYears

            };
            return PartialView(model);
        }
        public IActionResult Projects()
        {
            var studentGroup = _studentGroupService.GetAllAsync().Result;
            var user = studentGroup.FirstOrDefault();
            var allGroups = _studentGroupService.GetAllAsync().Result;
            var userGroup = allGroups
                .Where(x => x.student1LID == user.student1LID || x.student2ID == user.student2ID || x.student3ID == user.student3ID)
                .FirstOrDefault();
            var supervisorId = userGroup.SupervisorID;

            var projects = _projectService.GetAllAsync().Result;
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
                }).ToList()
            };
            return View(model);
        }
        public async Task<IActionResult> StudentGroupbyBatch(string batch)
        {
            var studentGroups = (await _studentGroupService.GetAllAsync())
                                .Where(x => x.Batch == batch);
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

            var projects = _projectService.GetAllAsync().Result;
            var model = new ProjectViewModel
            {
                projects = projects.Where(x=>x.batch == batch).Select(x => new ProjectViewModel
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
                }).ToList()
            };
            return View(model);
        }
    }
}
