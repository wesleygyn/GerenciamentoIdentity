using GerenciamentoIdentity.Constants;
using GerenciamentoIdentity.Models;
using GerenciamentoIdentity.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection;
using System.Security.Claims;

namespace GerenciamentoIdentity.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class IdentityController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly Dictionary<string, string> _roles;
        private readonly Dictionary<string, string> _claimTypes;

        public IdentityController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;

            _roles = roleManager.Roles.ToDictionary(r => r.Id, r => r.Name);
            var fldInfo = typeof(ClaimTypes).GetFields(BindingFlags.Static | BindingFlags.Public);
            _claimTypes = fldInfo.ToDictionary(i => i.Name, i => (string)i.GetValue(null));
        }

        public async Task<ActionResult> Index()
        {
            return View(await _userManager.Users.ToListAsync());
        }

        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserManagerViewModel userManager)
        {
            var user = new IdentityUser() 
            { 
                Email = userManager.Email, 
                UserName = userManager.UserName,
                PhoneNumber = userManager.PhoneNumber,
                EmailConfirmed = userManager.EmailConfirmed,
                PhoneNumberConfirmed = userManager.PhoneNumberConfirmed,
                LockoutEnabled = false,
            };

            var userCad = await _userManager.FindByEmailAsync(userManager.Email);

            if (await _roleManager.RoleExistsAsync("Basic") == false)
            {
                await _roleManager.CreateAsync(new IdentityRole("Basic"));
            }

            if (userCad == null)
            {
                var result = await _userManager.CreateAsync(user, userManager.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Basic");

                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }

        public async Task<IActionResult> UpdateUser(string id)
        {
            var userCad = await _userManager.FindByIdAsync(id);
            if (userCad != null)
            {
                return View(userCad);
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UpdateUser(IdentityUser identityUser)
        {
            var user = await _userManager.FindByIdAsync(identityUser.Id);
            if (user == null)
                return View();

            user.UserName = identityUser.UserName;
            user.Email = identityUser.Email;
            user.LockoutEnabled = identityUser.LockoutEnabled;
            user.EmailConfirmed = identityUser.EmailConfirmed;
            user.PhoneNumberConfirmed = identityUser.PhoneNumberConfirmed;
            user.PhoneNumber = identityUser.PhoneNumber;

            if(identityUser.LockoutEnabled == true)
            {
                user.LockoutEnd = identityUser.LockoutEnd > DateTime.Now ? identityUser.LockoutEnd : DateTimeOffset.MaxValue;
            }
            else
            {
                user.LockoutEnd = DateTime.MinValue;
            }

            var result = await _userManager.UpdateAsync(user);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ResetPassword(string id)
        {
            ResetPasswordViewModel resetPassword = new ResetPasswordViewModel()
            { 
                Id = id 
            };
            var userCad = await _userManager.FindByIdAsync(id);
            if (userCad != null)
            {
                return View(resetPassword);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPassword)
        {
            try
            {
                if (resetPassword.Password != resetPassword.Verify)
                    return View();

                var user = await _userManager.FindByIdAsync(resetPassword.Id);
                if (user == null)
                    return View();

                if (await _userManager.HasPasswordAsync(user))
                    await _userManager.RemovePasswordAsync(user);

                var result = await _userManager.AddPasswordAsync(user, resetPassword.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                    return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public async Task<ActionResult> UserInRole(string id)
        {
            var viewModel = new List<UserRolesViewModel>();
            var usuario = await _userManager.FindByIdAsync(id);
            foreach (var role in _roleManager.Roles.ToList())
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleName = role.Name
                };

                if (await _userManager.IsInRoleAsync(usuario, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }
                viewModel.Add(userRolesViewModel);
            }
            var model = new ManageUserRolesViewModel()
            {
                UserId = id,
                UserRoles = viewModel
            };

            ViewData["UsuarioSelecionado"] = usuario;
            return View(model);
        }

        public async Task<IActionResult> UpdateUserInRole(string id, ManageUserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            result = await _userManager.AddToRolesAsync(user, model.UserRoles.Where(x => x.Selected).Select(y => y.RoleName));
            var currentUser = await _userManager.GetUserAsync(User);
            await _signInManager.RefreshSignInAsync(currentUser);
            await Seeds.DefaultUsers.SeedSuperAdminAsync(_userManager, _roleManager);

            return RedirectToAction("UserInRole", "Identity", new { id = id });
        }

        public async Task<ActionResult> DeleteUser(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                    return View();

                var roles = await _userManager.GetRolesAsync(user);
                var removeRoles = await _userManager.RemoveFromRolesAsync(user, roles);

                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                    return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }
    }
}