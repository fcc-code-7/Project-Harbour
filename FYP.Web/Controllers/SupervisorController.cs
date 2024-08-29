using FYP.Databases;
using FYP.Entities;
using FYP.Repositories;
using FYP.Repositories.Implementation;
using FYP.Services;
using FYP.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace FYP.Web.Controllers
{
    public class SupervisorController : Controller
    {
        private readonly IStudentGroupService _studentGroupService;
        private readonly IProjectService _projectService;
        private readonly IProposalDefenseService _proposalDefenseService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IStudentService _studentService;

        public SupervisorController(IStudentService studentService,IStudentGroupService studentGroupService,UserManager<AppUser> userManager,IProjectService projectService, IProposalDefenseService proposalDefense)
        {
            _studentGroupService = studentGroupService;
            _userManager = userManager;
            _projectService = projectService;
            _proposalDefenseService = proposalDefense;
            _studentService = studentService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> StudentGroups()
        {
            var studentGroups = await _studentGroupService.GetAllAsync();
            var user = _userManager.GetUserId(User);
            var model = new StudentGroupViewModel()
            {
                StudentGroups = studentGroups.Where(x=>x.SupervisorID == user).Select(x=> new StudentGroupViewModel
                {
                    companyID=x.companyID,
                    Batch=x.Batch,
                    CordinatorID=x.CordinatorID,
                    CoSupervisorID=x.CoSupervisorID,
                    Id=x.ID,
                    Name=x.Name,
                    student1LID = x.student1LID,
                    student2ID = x.student2ID,
                    student3ID = x.student3ID,
                    SupervisorID = x.SupervisorID,
                    Year = x.Year,
                    projectname = _projectService.GetAllAsync().Result.Where(y=>y.projectGroup==x.Name).Select(y=>y.Title).FirstOrDefault()
                }
                ).ToList()};

            return PartialView(model);
        }
        public IActionResult Create()
        {
            return PartialView();

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
                             companyID= null,
                             CordinatorID = null,
                             CoSupervisorID = null,
                             
                             Batch ="spring",
                            Year = "2019" ,
                           
                        };
                        await _studentService.AddAsync(studentData1);
                        await _studentService.AddAsync(studentData2);
                        await _studentService.AddAsync(studentData3);
                         await _studentGroupService.AddAsync(usergroup);
                        return Json(new { success = true, message = "Data saved successfully!" });
                    }

                }
            }

            // Return an error response
            return Json(new { success = false, message = "Invalid data!" });
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
                    Status = x.Status
                }).ToList();

                // Add the group projects to the model's project list
                model.projects.AddRange(groupProjects);
            }

            return PartialView(model);
        }

    }
}
