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
        private readonly Services.IRoomAllotmentService _supervisorService;
        private readonly IProposalDefenseService _proposalDefenseService;
        private readonly IRoomService _designationService;
        private readonly IStudentGroupService _studentGroupService;
        private readonly IStudentService _studentService;
        private readonly UserManager<AppUser> _userManager;

        public UserController(IStudentService studentService, IUserService userService, UserManager<AppUser> userManager, Services.IRoomAllotmentService supervisorService, IProposalDefenseService proposalDefense, IRoomService designationService, IStudentGroupService studentGroupService)
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
        public IActionResult Profile(string toastrnotification)
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
                areaofintrest = user.areaofintrest,  // Trim to remove unwanted spaces
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
                model.groupname = _studentGroupService.GetAllAsync().Result.Where(x => x.ID == getGroup).Select(x => x.Name).FirstOrDefault();
                model.LeaderName = _userManager.FindByIdAsync(getLeader).Result.Name;
                model.supervisorAddress = _userManager.FindByIdAsync(getSupervisor).Result.Address;
                if (User.IsInRole("Supervisor"))
                {
                    model.group2name = _studentGroupService.GetAllAsync().Result.Where(x => x.SupervisorID == LoggedInUser).ToList();

                }

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
                areaofintrest = user.areaofintrest,
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
            if (user == null)
            {
                return RedirectToAction("Login", "Account", new { toastrnotification = "InvalidLogin" });
            }
            bool hasChanges = false;

            if (user.Email != model.email)
            {
                user.Email = model.email;
                hasChanges = true;
            }

            if (user.UserName != model.email)
            {
                user.UserName = model.email;
                hasChanges = true;
            }

            if (user.Address != model.Address)
            {
                user.Address = model.Address;
                hasChanges = true;
            }



            if (user.Cnic != model.Cnic)
            {
                user.Cnic = model.Cnic;
                hasChanges = true;
            }

            if (user.Name != model.Name)
            {
                user.Name = model.Name;
                hasChanges = true;
            }

            if (user.Department != model.Department)
            {
                user.Department = model.Department;
                hasChanges = true;
            }

            if (user.Telephone != model.Telephone)
            {
                user.Telephone = model.Telephone;
                hasChanges = true;
            }

            if (user.Docs != model.Docs)
            {
                user.Docs = model.Docs;
                hasChanges = true;
            }

            if (user.areaofintrest != model.areaofintrest)
            {
                user.areaofintrest = model.areaofintrest;
                hasChanges = true;
            }

            if (User.IsInRole("Supervisor") || User.IsInRole("Cordinator"))
            {
                user.Designation = model.Designation;
                hasChanges = true;


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
            if (hasChanges)
            {
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Profile", "User", new { toastrnotification = "Updated" });
                }
                else
                {
                    return RedirectToAction("Profile", "User", new { toastrnotification = "Failed" });
                }
            }

            return RedirectToAction("Profile", "User", new { toastrnotification = "No Changes" });

        }

        public IActionResult Message()
        {
            return PartialView();
        }

        public async Task<IActionResult> ChangePassword()
        {
            var LoggedInUser = _userManager.GetUserId(User);
            var model = new RegisterViewModel()
            {
                Id = LoggedInUser,
                Name = _userManager.FindByIdAsync(LoggedInUser).Result.Name,
            };
            return PartialView(model);
        }
        [HttpPost]
public async Task<IActionResult> ChangePassword([FromBody] RegisterViewModel model)
{
    // Check if the model is valid
   

    // Get the currently logged-in user
    var loggedInUserId = _userManager.GetUserId(User);
    var user = await _userManager.FindByIdAsync(loggedInUserId);

    if (user == null)
    {
    return Json(new { success = false, message = "User Not Found."  });
    }

    // Ensure that Password and ConfirmPassword match
    if (model.Password != model.ConfirmPassword)
    {
        ModelState.AddModelError(string.Empty, "Password and Confirm Password do not match.");
        return BadRequest(ModelState);
    }

    // Attempt to change the password
    var changePasswordResult = await _userManager.ChangePasswordAsync(user,model.oldPassword , model.Password);

    if (!changePasswordResult.Succeeded)
    {
        foreach (var error in changePasswordResult.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
        return BadRequest(ModelState);
    }

    // Password change successful, return success response
    return Json(new { success = true, message = "Password successfully changed."  });
}

    }

}
