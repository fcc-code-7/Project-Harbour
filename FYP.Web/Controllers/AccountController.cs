using FYP.Databases;
using FYP.Db;
using FYP.Entities;
using FYP.Services;
using FYP.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FYP.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IRoomAllotmentService _supervisorService;
        public AccountController(IRoomAllotmentService supervisorService, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _supervisorService = supervisorService;
        }

        public IActionResult Login(string toastrnotification)
        {
            if (toastrnotification != null)
            {
                ViewBag.success = toastrnotification;
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Contains("Supervisor"))
                    {
                        return RedirectToAction("Index", "Supervisor");
                    }
                    else if (roles.Contains("Cordinator"))
                    {
                        return RedirectToAction("Index", "Cordinator");
                    }
                    else if (roles.Contains("Student"))
                    {
                        return RedirectToAction("Index", "Student");
                    }
                    else if (roles.Contains("External"))
                    {
                        return RedirectToAction("Index", "Student");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid user role.");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Login Attempt");
                    return RedirectToAction("Login", "Account", new { toastrnotification = "InvalidAccountDetails" });

                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid Login Attempt");
                return RedirectToAction("Login", "Account" , new { toastrnotification = "NoAccount"});
            }
            return View(model);
        }


        public IActionResult Register()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var toastrnotification = "Added";
            AppUser user = new()
            {
                Email = model.Email,
                UserName = model.Email,
                Name = model.Name,
                Department = model.Department,
                DateofCreation = DateOnly.FromDateTime(DateTime.Now),
                ActiveState = "Active",


            };
            if (model.Role == "Supervisor")
            {
                user.Role = "Supervisor";
                user.Designation = "Supervisor";
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Roles.Supervisor.ToString());
                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Login", "Account", new { toastrnotification});
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            else if (model.Role == "Cordinator")
            {
                user.Role = "Cordinator";
                user.Designation = "Cordinator";
                user.areaofintrest = "N/A";
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Roles.Cordinator.ToString());
                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Login", "Account", new { toastrnotification });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            else
            {
                model.Role = "External";
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Roles.External.ToString());

                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login","Account");
        }
    }



}
