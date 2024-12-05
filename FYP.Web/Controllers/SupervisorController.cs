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
        private readonly IRoomAllotmentService _supervisorService;
        private readonly IUserService _userService;
        private readonly IChangeSupervisorFormService _changeSupervisorFormService;
        private readonly IStudentService _studentService;

        public SupervisorController(IChangeSupervisorFormService changeSupervisorFormService, IUserService userService, IRoomAllotmentService supervisorService, IStudentService studentService, IStudentGroupService studentGroupService, UserManager<AppUser> userManager, IProjectService projectService, IProposalDefenseService proposalDefense)
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

        public async Task<IActionResult> Create(string registeredMessage,string ViewValue, string groupName, string stu1, string stu2, string stu3, string batch)
        {
            if (ViewValue != null)
            {
                // Set the success message in ViewBag
                ViewBag.success = ViewValue;

                // Ensure the registered message is initialized properly
                var registeredParts = new List<string>();

                // Check if the email is registered (not null) and build the message
                if (string.IsNullOrEmpty(stu1))
                    registeredParts.Add($"Leader");
                if (string.IsNullOrEmpty(stu2))
                    registeredParts.Add($"Member 1");
                if (string.IsNullOrEmpty(stu3))
                    registeredParts.Add($"Member 2");

                // Join the parts into a single message
                ViewBag.registeredMessage = string.Join(", ", registeredParts);

                // Initialize the model
                var modelNew = new StudentGroupViewModel
                {
                    Name = groupName,
                    Batch = batch,
                    student1LEmail = stu1,
                    student2Email = stu2,
                    student3Email = stu3
                };

                // Clear the appropriate email field if it's null (unregistered)
                if (stu1 == null)
                    modelNew.student1LEmail = ""; // Clear if null
                if (stu2 == null)
                    modelNew.student2Email = ""; // Clear if null
                if (stu3 == null)
                    modelNew.student3Email = ""; // Clear if null

                // Return the partial view with the model
                return PartialView(modelNew);
            }
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
            var projects = await _projectService.GetAllAsync();
            var groupList = await _studentGroupService.GetAllAsync();

            if (projects == null || !projects.Any() || groupList == null || !groupList.Any())
            {
                // Handle the case where no projects or groups are available
                return PartialView(new ProjectViewModel());
            }
            var supervisorGroups = new List<StudentGroup>();
            if (User.IsInRole("Supervisor"))
            {
                // Filter groups based on the supervisor's ID
                 supervisorGroups = groupList.Where(x => x.SupervisorID == userId).ToList();

                if (!supervisorGroups.Any())
                {
                    // Handle the case where no groups are assigned to the supervisor
                    return PartialView(new ProjectViewModel());
                }
            }
            if (User.IsInRole("Cordinator"))
            {
                supervisorGroups = groupList.ToList();
            }
          

            // Get the distinct batch values for the supervisor's groups
            var distinctBatches = supervisorGroups.Select(x => x.Batch).Distinct().ToList();

            // Fetch the last project batch
            var fetchLastProjectBatch = projects.OrderByDescending(x => x.batch).FirstOrDefault();

            // Initialize the project list
            var model = new ProjectViewModel
            {
                projects = new List<ProjectViewModel>(),
                batches = distinctBatches,
                batch = fetchLastProjectBatch?.batch // Handle null condition for fetchLastProjectBatch
            };
            var userServices = await _userService.GetAllAsync();
            // Iterate through each group and add the related projects to the list
            foreach (var groupId in supervisorGroups.Select(x => x.ID))
            {
                var groupProjects = projects
                    .Where(x => x.groupId == groupId.ToString() && x.batch == fetchLastProjectBatch.batch)
                    .Select(x => new ProjectViewModel
                    {
                        Id = x.ID,
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
                        SupervsiorApproved = x.SupervsiorApproved,
                        groupname = groupList.Where(x=>x.ID == groupId).FirstOrDefault().Name,
                        MidPPT = x.MidPPT,
                        MidReport = x.MidReport,
                        PropPPT = x.PropPPT,
                        PropReport = x.PropReport,
                        FinalDocs = x.FinalDocs,
                        TotalMarks = x.TotalMarks,
                        
                    })
                    .ToList();

                // Add the group projects to the model's project list
                model.projects.AddRange(groupProjects);
            }

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> FilterProjects(string ProjectWord, string Batch)
        {
            // Fetch all groups and projects
            var groups = await _studentGroupService.GetAllAsync();
            var projects = await _projectService.GetAllAsync();
            var filteredProjects = new List<Project>();
            var groupList = new List<string>();

            if (User.IsInRole("Supervisor"))
            {
                var userId = _userManager.GetUserId(User);

                // Get the IDs of groups supervised by the current user
                 groupList = groups
                    .Where(x => x.SupervisorID == userId) // Filter groups by SupervisorID
                    .Select(x => x.ID.ToString()) // Select group IDs
                    .ToList();

                // Filter projects where groupId matches the supervised group IDs
                 filteredProjects = projects
                    .Where(p => groupList.Contains(p.groupId)) // Match project groupId with supervised group IDs
                    .Where(p => string.IsNullOrWhiteSpace(ProjectWord) || p.Title.Contains(ProjectWord, StringComparison.OrdinalIgnoreCase)) // Filter by ProjectWord
                    .Where(p => string.IsNullOrEmpty(Batch) || p.batch == Batch) // Filter by Batch
                    .ToList();
            }
            if (User.IsInRole("Cordinator"))
            {
                // Get the IDs of groups supervised by the current user
                groupList = groups
                  
                   .Select(x => x.ID.ToString()) // Select group IDs
                   .ToList();

                // Filter projects where groupId matches the supervised group IDs
                filteredProjects = projects
                   .Where(p => groupList.Contains(p.groupId)) // Match project groupId with supervised group IDs
                   .Where(p => string.IsNullOrWhiteSpace(ProjectWord) || p.Title.Contains(ProjectWord, StringComparison.OrdinalIgnoreCase)) // Filter by ProjectWord
                   .Where(p => string.IsNullOrEmpty(Batch) || p.batch == Batch) // Filter by Batch
                   .ToList();
            }
            // Get the current user's ID
            

            // Create a list of view models for the filtered projects
            var projectViewModels = filteredProjects.Select(project => new ProjectViewModel
            {
                code = project.code,
                Title = project.Title,
                ProjectCategory = project.ProjectCategory,
                projectGroup = project.projectGroup,
                Others = project.Others,
                groupId = project.groupId,
                Specialization = project.Specialization,
                Tools = project.Tools,
                companyID = project.companyID,
                batch = project.batch,
                Summary = project.Summary,
                objectives = project.objectives,
                ExpectedResults = project.ExpectedResults,
                SupervsiorApproved = project.SupervsiorApproved,
                Status = project.Status,
                PropPPT = project.PropPPT,
                PropReport = project.PropReport,
                MidPPT = project.MidPPT,
                MidReport = project.MidReport,
                FinalDocs = project.FinalDocs,
                TotalMarks = project.TotalMarks,
                Id = project.ID,
                groupname = groups.Where(x => x.ID.ToString() == project.groupId).FirstOrDefault().Name
            }).ToList();

            // Return the filtered projects as JSON
            return Json(projectViewModels);
        }


    }
}
