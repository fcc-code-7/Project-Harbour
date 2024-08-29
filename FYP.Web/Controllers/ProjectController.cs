using FYP.Entities;
using FYP.Services;
using FYP.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FYP.Web.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IUserService _userService;
        private readonly ISupervisorService _supervisorService;
        private readonly IProposalDefenseService _proposalDefenseService;
        private readonly IDesignationService _designationService;
        private readonly IStudentGroupService _studentGroupService;
        private readonly IProjectService _projectService;
        private readonly ICompanyService _companyService;
        private readonly UserManager<AppUser> _userManager;

        public ProjectController(ICompanyService companyService,IProjectService projectService,IUserService userService, UserManager<AppUser> userManager, ISupervisorService supervisorService, IProposalDefenseService proposalDefense, IDesignationService designationService, IStudentGroupService studentGroupService)
        {
            _userService = userService;
            _userManager = userManager;
            _proposalDefenseService = proposalDefense;
            _supervisorService = supervisorService;
            _studentGroupService = studentGroupService;
            _designationService = designationService;
            _projectService = projectService;
            _companyService = companyService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Projects()
        {
            var loggedInUser = _userManager.GetUserId(User);
            var allGroups = _studentGroupService.GetAllAsync().Result;
            var userGroup = allGroups
                .Where(x => x.student1LID == loggedInUser || x.student2ID == loggedInUser || x.student3ID == loggedInUser)
                .FirstOrDefault();

            if (userGroup == null)
            {
                return PartialView(new ProjectViewModel()); // Handle no group case
            }

            var projects = _projectService.GetAllAsync().Result
                .Where(x => x.groupId == userGroup.ID.ToString())
                .ToList();
            var supervisorId = userGroup.SupervisorID;
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

            if (User.IsInRole("Student"))
            {
                supervisorId = userGroup.SupervisorID;
                var groupLeader = userGroup.student1LID;

                model.supervisorname = _userManager.FindByIdAsync(supervisorId).Result?.Name;
                model.groupname = userGroup.Name;
            }

            return PartialView(model);
        }

        public async  Task<IActionResult> Create()
        {
            var project = _projectService.GetAllAsync().Result;
            var group = _studentGroupService.GetAllAsync().Result;
            var LoggedInUser = _userManager.GetUserId(User);
            
            var model = new ProjectViewModel()
            {
                projects = project.Select(x => new ProjectViewModel
                {
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
                    
                }).ToList(),
            };
            if (User.IsInRole("Student"))
            {
                var getSupervisor =  _studentGroupService.GetAllAsync().Result.Where(x => x.student1LID == LoggedInUser || x.student2ID == LoggedInUser || x.student3ID == LoggedInUser).FirstOrDefault().SupervisorID;
                var getGroup = _studentGroupService.GetAllAsync().Result.Where(x => x.student1LID == LoggedInUser || x.student2ID == LoggedInUser || x.student3ID == LoggedInUser).FirstOrDefault().ID;
                var getLeader = _studentGroupService.GetAllAsync().Result.Where(x => x.ID == getGroup).Select(x => x.student1LID).FirstOrDefault();
                model.supervisorname = _userManager.FindByIdAsync(getSupervisor).Result.Name;
                model.groupname = _studentGroupService.GetAllAsync().Result.Where(x => x.ID == getGroup).Select(x => x.Name).FirstOrDefault();
            }
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            var batch = "";

            // Determine batch based on the current month
            if (month > 2 && month < 6)
            {
                batch = "Fall" + (year + 1).ToString();
            }
            else
            {
                batch = "Spring" + year.ToString();
            }

            // Get the batch code (e.g., "FAL", "SPR")
            var batchCode = batch.Substring(0, 3).ToUpper();

            // Fetch the last project code from the database that starts with "FYP + batchCode"
            var lastProjectCode =  _projectService.GetAllAsync().Result
                                    .OrderByDescending(p => p.code)
                                    .Select(p => p.code)
                                    .FirstOrDefault();

            int lastNumber = 0;

            if (!string.IsNullOrEmpty(lastProjectCode))
            {
                // Extract the numeric part after the batch prefix
                var numberPart = lastProjectCode.Substring(("FYP-" + batchCode + "-").Length);
                int.TryParse(numberPart, out lastNumber);
            }

            // Increment the last number by 1
            lastNumber += 1;

            // Format the new number as 5 digits
            var newNumber = lastNumber.ToString("D5");

            // Generate the new project code
            var newProjectCode = "FYP-" + batchCode + "-" + newNumber;

            model.code = newProjectCode;

            return PartialView(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProjectViewModel model)
        {
            var company = new Company()
            {
                 Name = model.companyName,
                 MentorEmail = model.mentoremail,
                 MentorName = model.companyMentor,
                 MentorTelephone = model.mentorcontact
            };
            
            await _companyService.AddAsync(company);
           
            var companyId = _companyService.GetAllAsync().Result.Where(x=>x.Name == model.companyName).FirstOrDefault().ID;
            var group = _studentGroupService.GetAllAsync().Result.Where(x=>x.Name == model.groupname).FirstOrDefault().ID;
            
            var project = new Project()
            {
                code = model.code,
                companyID = companyId.ToString(),
                ExpectedResults = model.ExpectedResults,
                objectives = model.objectives,
                ProjectCategory = model.ProjectCategory,
                Others = model.Others,
                projectGroup = model.projectGroup,
                Specialization = model.Specialization,
                Tools = model.Tools,
                Summary = model.Summary,
                Status = false,
                Title = model.Title,
                groupId = group.ToString(),
                batch = model.batch.ToString(),
                
                
            };
            if (model.Others == "")
            {
                project.Others = "N/A";
            }
            else
            {
                project.Others = model.Others;
            }
            var result = _projectService.AddAsync(project);
            if (result != null)
            {

                return Json(new { success = true, message = "Data saved successfully!" });
            }
            return Json(new { success = false, message = "Invalid data!" });
            

        }
    }
}
