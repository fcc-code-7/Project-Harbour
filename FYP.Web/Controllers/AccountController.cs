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
        private readonly ISupervisorService _supervisorService;
        public AccountController(ISupervisorService supervisorService, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _supervisorService = supervisorService;
        }

        public IActionResult Login()
        {
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
                    else
                    {
                        ModelState.AddModelError("", "Invalid user role.");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Login Attempt");
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid Login Attempt");
            }
            return View(model);
        }


        public IActionResult Register()
        {
            var model = new RegisterViewModel()
            {
                DesignationList = _context.Designations.Select(x => new DesignationViewModel
                {
                    Id = x.ID.ToString(),
                    Name = x.Name
                }).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            AppUser user = new()
            {
                Email = model.Email,
                UserName = model.Email,
                Name = model.Name,
                Department = model.Department,
                Docs = "",
                Designation = ""
            };
            if (model.Role == "Supervisor")
            {
                model.Designation = "Supervisor";
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Roles.Supervisor.ToString());

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Supervisor");
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
                model.Role = "Cordinator";
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Roles.Cordinator.ToString());

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Cordinator");
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
                model.Role = "Internal/External";
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Roles.Internal.ToString());

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Internal/External");
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
