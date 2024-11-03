using FYP.Databases;
using FYP.Entities;
using FYP.Models;
using FYP.Repositories;
using FYP.Repositories.Implementation;
using FYP.Services;
using FYP.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text.RegularExpressions;

namespace FYP.Web.Controllers
{
    public class SupervisorController : Controller
    {
        private readonly IStudentGroupService _studentGroupService;
        private readonly IProjectService _projectService;
        private readonly IProposalDefenseService _proposalDefenseService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ISupervisorService _supervisorService;
        private readonly IUserService _userService;
        private readonly IChangeSupervisorFormService _changeSupervisorFormService;
        private readonly IStudentService _studentService;

        public SupervisorController(IChangeSupervisorFormService changeSupervisorFormService, IUserService userService, ISupervisorService supervisorService, IStudentService studentService, IStudentGroupService studentGroupService, UserManager<AppUser> userManager, IProjectService projectService, IProposalDefenseService proposalDefense)
        {
            _studentGroupService = studentGroupService;
            _userManager = userManager;
            _projectService = projectService;
            _proposalDefenseService = proposalDefense;
            _studentService = studentService;
            _supervisorService = supervisorService;
            _changeSupervisorFormService = changeSupervisorFormService;
            _userService = userService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ProjectGroupbyBatch(string batch)
        {
            var userid = _userManager.GetUserId(User);
            var studentGroup = _studentGroupService.GetAllAsync().Result.Where(x => x.SupervisorID == userid);
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

        public async Task<IActionResult> Create()
        {
            var phoneticAlphabet = new List<string> { "Alpha", "Bravo", "Charlie", "Delta" };
            var random = new Random();
            string uniqueGroupName;
            bool isUnique;

            do
            {
                var baseName = phoneticAlphabet[random.Next(phoneticAlphabet.Count)];
                var uniqueSuffix = random.Next(1000, 9999); // Random 4-digit suffix
                uniqueGroupName = $"{baseName}-{uniqueSuffix}";

                // Check if the generated name already exists
                var group = await _studentGroupService.GetAllAsync();
                isUnique = !group.Any(x => x.Name == uniqueGroupName);

            } while (!isUnique);

            var model = new StudentGroupViewModel
            {
                Name = uniqueGroupName
            };

            return PartialView(model);
        }


        public async Task<IActionResult> Projects()
        {
            var userId = _userManager.GetUserId(User);

            // Fetch all projects and student groups
            var projects = _projectService.GetAllAsync().Result;
            var groupList = _studentGroupService.GetAllAsync().Result
                .Where(x => x.SupervisorID == userId)
                .Select(x => x.ID)
                .ToList();
            var distinctBatches = projects.Select(x => x.batch).Distinct().ToList();

            // Initialize the project list
            var model = new ProjectViewModel
            {
                projects = new List<ProjectViewModel>() // Initialize as a list
            };

            // Iterate through each group and add the related projects to the list
            foreach (var groupId in groupList)
            {
                var groupProjects = projects.Where(x => x.groupId == groupId.ToString()).Select(x => new ProjectViewModel
                {
                    code = x.code,
                    Title = x.Title,
                    ProjectCategory = x.ProjectCategory,
                    projectGroup = x.projectGroup,
                    Others = x.Others,
                    groupId = x.groupId,
                    Specialization = x.Specialization,
                    Tools = x.Tools,
                    companyID = x.companyID,
                    Summary = x.Summary,
                    objectives = x.objectives,
                    ExpectedResults = x.ExpectedResults,
                    commiteeId = x.commiteeId,
                    Status = x.Status,
                    SupervsiorApproved = x.SupervsiorApproved
                }).ToList();

                // Add the group projects to the model's project list
                model.projects.AddRange(groupProjects);
                model.batches = distinctBatches;
            }

            return PartialView(model);
        }


    }
}
