using FYP.Entities;
using FYP.Services;
using FYP.Web.Models;
using FYP.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FYP.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

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
        private readonly ICompanyService _companyService;

        private readonly IEvaluationCriteriaService _evaluationCriteriaService;

        public HomeController(ICompanyService companyService, IEvaluationCriteriaService evaluationCriteriaService, IRoomAllotmentService roomAllotmentService, IEvaluationService evaluationService, IRoomInChargeService roomInChargeService, IRoomService roomService, IFYPCommitteService fYPCommitteService, IChangeSupervisorFormService changeSupervisorFormService, IUserService userService, IRoomAllotmentService supervisorService, IStudentService studentService, IStudentGroupService studentGroupService, UserManager<AppUser> userManager, IProjectService projectService, IProposalDefenseService proposalDefense)
        {
            _studentGroupService = studentGroupService;
            _userManager = userManager;
            _companyService = companyService;

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
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [Route("criteriaNPolicies")]
        public IActionResult CriteriaNPolicies()
        {
            return View("criteriaNPolicies");
        }

        [Route("fypCommittee")]
        public async Task<IActionResult> FypCommittee()
        {
            var fetchAllSupervisor = await _userManager.GetUsersInRoleAsync("Supervisor");

            var model = new AppUserViewModel
            {
                ListAppUser = fetchAllSupervisor.Select(x => new AppUserViewModel
                {
                    Name = x.Name,
                    email = x.Email,
                    areaofintrest = x.areaofintrest,
                    Docs = x.Docs
                }).ToList()
            };

            return View(model);
        }

        [Route("home")]
        public IActionResult Home()
        {
            return View();
        }

        [Route("notifications")]
        public IActionResult Notifications()
        {
            return View();
        }

        [Route("objectives")]
        public IActionResult Objectives()
        {
            return View();
        }

        [Route("recentProjects")]
        public IActionResult RecentProjects()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
