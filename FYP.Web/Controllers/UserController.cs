using FYP.Entities;
using FYP.Services;
using FYP.Services.Implementation;
using FYP.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;

namespace FYP.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ISupervisorService _supervisorService;
        private readonly IProposalDefenseService _proposalDefenseService;
        private readonly IDesignationService _designationService;
        private readonly IStudentGroupService _studentGroupService;
        private readonly IStudentService _studentService;
        private readonly UserManager<AppUser> _userManager;

        public UserController(IStudentService studentService,IUserService userService, UserManager<AppUser> userManager, ISupervisorService supervisorService, IProposalDefenseService proposalDefense,IDesignationService designationService,IStudentGroupService studentGroupService)
        {
            _userService = userService;
            _userManager = userManager;
            _proposalDefenseService = proposalDefense;
            _supervisorService = supervisorService;
            _studentGroupService = studentGroupService;
            _designationService = designationService;
            _studentService = studentService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Profile()
        {
            var LoggedInUser = _userManager.GetUserId(User);
            var user =_userService.GetByIdAsync(LoggedInUser).Result;
            var model = new AppUserViewModel()
            {
                Id = user.Id,
                Name = user.Name,
                email = user.Email,
                Address = user.Address,
                Cnic = user.Cnic,
                Department = user.Department,
                Telephone = user.Telephone,
                Role = user.Role,
                Docs = user.Docs,
                Designation = user.Designation,
                };
            if (User.IsInRole("Supervisor"))
            {
                //var getSupervisor1stleadername = _studentGroupService.GetAllAsync().Result.Where(x => x.SupervisorID == LoggedInUser).Select(x => x.student1LID).First();
                //var getSupervisor2ndleadername = _studentGroupService.GetAllAsync().Result.Where(x => x.SupervisorID == LoggedInUser).Select(x => x.student1LID).Last();
                //var getSupervisor1groupname = _studentGroupService.GetAllAsync().Result.Where(x => x.SupervisorID == LoggedInUser).Select(x => x.Name).First();
                //var getSupervisor2groupname = _studentGroupService.GetAllAsync().Result.Where(x => x.SupervisorID == LoggedInUser).Select(x => x.Name).Last();
                //model.group1name = getSupervisor1groupname;
                //model.group2name = getSupervisor2groupname;
                //model.group1leader = _userManager.FindByIdAsync(getSupervisor1stleadername).Result.Email;
                //model.group2leader = _userManager.FindByIdAsync(getSupervisor2ndleadername).Result.Email;
            }
            if (User.IsInRole("Student"))
            {
                var getSupervisor = _studentGroupService.GetAllAsync().Result.Where(x => x.student1LID == LoggedInUser || x.student2ID == LoggedInUser || x.student3ID == LoggedInUser).FirstOrDefault().SupervisorID;
                var getGroup = _studentGroupService.GetAllAsync().Result.Where(x => x.student1LID == LoggedInUser || x.student2ID == LoggedInUser || x.student3ID == LoggedInUser).FirstOrDefault().ID;
                var getLeader = _studentGroupService.GetAllAsync().Result.Where(x => x.ID == getGroup).Select(x => x.student1LID).FirstOrDefault();
                model.Enrollment = _studentService.GetAllAsync().Result.Where(x => x.studentId == LoggedInUser).FirstOrDefault().ENo;
                model.regNo = _studentService.GetAllAsync().Result.Where(x => x.studentId == LoggedInUser).FirstOrDefault().RegNo;
                model.semester = _studentService.GetAllAsync().Result.Where(x => x.studentId == LoggedInUser).FirstOrDefault().Semester;
                model.supervisorStudent = _userManager.FindByIdAsync(getSupervisor).Result.Name;
                model.supervisorEmail = _userManager.FindByIdAsync(getSupervisor).Result.Email;
                model.groupname = _studentGroupService.GetAllAsync().Result.Where(x=>x.ID == getGroup).Select(x=>x.Name).FirstOrDefault();
                model.LeaderName = _userManager.FindByIdAsync(getLeader).Result.Name;
               }
            return PartialView(model);
        }
        public IActionResult Create()
        {
            var LoggedInUser = _userManager.GetUserId(User);
            var user = _userService.GetByIdAsync(LoggedInUser).Result;

            var model = new AppUserViewModel()
            {
                Id = user.Id,
                Name = user.Name,
                email = user.Email,
                Address = user.Address,
                Cnic = user.Cnic,
                Department = user.Department,
                Telephone = user.Telephone,
                Role = user.Role,
                Docs = user.Docs,
                Designation = user.Designation,
            };

            if (User.IsInRole("Student"))
            {
                // Fetch student group data once
                var studentGroups = _studentGroupService.GetAllAsync().Result
                    .Where(x => x.student1LID == LoggedInUser || x.student2ID == LoggedInUser || x.student3ID == LoggedInUser)
                    .FirstOrDefault();

                if (studentGroups != null)
                {
                    var getSupervisor = studentGroups.SupervisorID;
                    var getGroup = studentGroups.ID;
                    var getLeader = studentGroups.student1LID;

                    // Fetch student data once
                    var student = _studentService.GetAllAsync().Result
                        .Where(x => x.studentId == LoggedInUser)
                        .FirstOrDefault();

                    if (student != null)
                    {
                        model.Enrollment = student.ENo;
                        model.regNo = student.RegNo;
                        model.semester = student.Semester;
                    }

                    // Fetch supervisor and leader data once
                    var supervisor = _userManager.FindByIdAsync(getSupervisor).Result;
                    var leader = _userManager.FindByIdAsync(getLeader).Result;

                    if (supervisor != null)
                    {
                        model.supervisorStudent = supervisor.Name;
                        model.supervisorEmail = supervisor.Email;
                    }

                    if (leader != null)
                    {
                        model.LeaderName = leader.Name;
                    }

                    model.groupname = studentGroups.Name;
                }
            }

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] AppUserViewModel model)
        {
            // Fetch the existing user from the database
            var user = await _userManager.FindByIdAsync(model.Id);
            var supervisor = _supervisorService.GetAllAsync().Result.Where(x=>x.SupervisorID == model.Id).FirstOrDefault();
            if (user == null)
            {
                return Json(new { success = false, message = "User not found!" });
            }

            // Update the user's properties with the new values from the view model
            user.Email = model.email;
            user.UserName = model.email;
            user.Address = model.Address;
            user.Cnic = model.Cnic;
            user.Name = model.Name;
            user.Department = model.Department;
            user.Telephone = model.Telephone;
            user.Docs = model.Docs;
            if (User.IsInRole("Supervisor") || User.IsInRole("Cordinator"))
            {
                user.Designation = model.Designation;

            }
            if (User.IsInRole("Student"))
            {
            var student = _studentService.GetAllAsync().Result
                       .Where(x => x.studentId == user.Id)
                       .FirstOrDefault();

                student.studentId = user.Id;
                student.ENo = model.Enrollment;
                student.RegNo = model.regNo;
                student.Semester = model.semester;
            await _studentService.UpdateAsync(student);

            }
            //user.Role = model.Role;
            //supervisor.Designation = model.Designation;
            // Update the user in the database
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded )
            {
                return RedirectToAction("Profile", "User");
            }

            return Json(new { success = false, message = "Invalid data!" });
        }

        public IActionResult Message()
        {
            return PartialView();
        }
        
    }
}
