using AutoMapper;
using GerenciamentoIdentity.Constants;
using GerenciamentoIdentity.Data;
using GerenciamentoIdentity.Models;
using GerenciamentoIdentity.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection;
using System.Security.Claims;
using MySqlConnector;
using Dapper;

namespace GerenciamentoIdentity.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class IdentityController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly Dictionary<string, string> _roles;
        private readonly Dictionary<string, string> _claimTypes;
        private readonly string _connectionString;

        public IdentityController(ApplicationDbContext context, IMapper mapper, UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;

            _connectionString = configuration.GetConnectionString("DefaultConnection");

            _roles = roleManager.Roles.ToDictionary(r => r.Id, r => r.Name);
            var fldInfo = typeof(ClaimTypes).GetFields(BindingFlags.Static | BindingFlags.Public);
            _claimTypes = fldInfo.ToDictionary(i => i.Name, i => (string)i.GetValue(null));
        }

        public async Task<ActionResult> Index()
        {
            var result = await _userManager.Users.ToListAsync();

            return View(result);
        }

        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserManagerViewModel userManager)
        {
            DateTime max = DateTime.Now.AddYears(5000);

            if (userManager.UsuarioSistema == true)
            {
                userManager.LockoutEnabled = false;
            }
            else
            {
                userManager.EmailConfirmed = false;
                userManager.LockoutEnabled = true;
                userManager.LockoutEnd = max;
                userManager.Password = Utils.GeneratePassword();
            }

            var user = new Usuario()
            {
                Email = userManager.Email,
                UserName = userManager.Email,
                PhoneNumber = userManager.PhoneNumber,
                EmailConfirmed = userManager.EmailConfirmed,
                PhoneNumberConfirmed = userManager.PhoneNumberConfirmed,
                LockoutEnabled = false,
                LockoutEnd = userManager.LockoutEnd,
                Matricula = userManager.Matricula,
                Admissao = userManager.Admissao,
                Nascimento = userManager.Nascimento,
                Documento = userManager.Documento,
                Ativo = userManager.Ativo,
                UsuarioSistema = userManager.UsuarioSistema,
                Nome = userManager.Nome,
                Sexo = userManager.Sexo,
                Telefone = userManager.Telefone,
                Cep = userManager.Cep,
                Ibge = userManager.Ibge,
                Logradouro = userManager.Logradouro,
                Complemento = userManager.Complemento,
                Numero = userManager.Numero,
                Bairro = userManager.Bairro,
                Municipio = userManager.Municipio,
                Uf = userManager.Uf
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

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    if(userManager.UsuarioSistema)
                    {
                        var teste = ConfirmEmail(user.Id, code);
                    }

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
        public async Task<IActionResult> UpdateUser(Usuario usuario)
        {
            if(usuario.Ativo && usuario.UsuarioSistema)
            {
                usuario.UsuarioSistema = true;
                usuario.EmailConfirmed = true;
            }

            if(!usuario.Ativo || !usuario.UsuarioSistema)
            {
                usuario.UsuarioSistema = false;
                usuario.EmailConfirmed = false;
                usuario.LockoutEnabled = true;
            }

            if (usuario.LockoutEnabled == true)
            {
                usuario.LockoutEnd = usuario.LockoutEnd > DateTime.Now ? usuario.LockoutEnd : DateTimeOffset.MaxValue;
            }

            usuario.UserName = usuario.Email;

            var parameters = new
            {
                usuario.Id,
                usuario.Matricula,
                usuario.Admissao,
                usuario.Nascimento,
                usuario.Documento,
                usuario.Ativo,
                usuario.UsuarioSistema,
                usuario.Nome,
                usuario.Sexo,
                usuario.Telefone,
                usuario.Cep,
                usuario.Ibge,
                usuario.Logradouro,
                usuario.Complemento,
                usuario.Numero,
                usuario.Bairro,
                usuario.Municipio,
                usuario.Uf,
                usuario.UserName,
                usuario.Email,
                usuario.LockoutEnabled,
                usuario.LockoutEnd,
                usuario.EmailConfirmed,
                usuario.PhoneNumberConfirmed,
                usuario.PhoneNumber,
            };

            if (UserExists(usuario.Id))
            {
                using (var sqlConnection = new MySqlConnection(_connectionString))
                {
                    const string sql = "UPDATE aspnetusers SET Matricula = @Matricula, Admissao = @Admissao, Nascimento = @Nascimento, Documento = @Documento, Ativo = @Ativo, UsuarioSistema = @UsuarioSistema, Nome = @Nome, Sexo = @Sexo, Telefone = @Telefone, Cep = @Cep, Ibge = @Ibge, Logradouro = @Logradouro, Complemento = @Complemento, Numero = @Numero, Bairro = @Bairro, Municipio = @Municipio, Uf = @Uf, UserName = @UserName, LockoutEnabled = @LockoutEnabled, LockoutEnd = @LockoutEnd, EmailConfirmed = @EmailConfirmed, Email = @Email, PhoneNumberConfirmed = @PhoneNumberConfirmed, PhoneNumber = @PhoneNumber WHERE Id = @Id";

                    await sqlConnection.ExecuteAsync(sql, parameters);

                    TempData["mensagemResultSucces"] = $"O cadastro de {usuario.Nome} foi editado com sucesso!";

                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                TempData["mensagemResultError"] = $"Não existe cadastro com o Id {usuario.Id}.";
                return NotFound();
            }
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

        public async Task<IActionResult> ConfirmEmail(string? userid, string? code)
        {
            if (userid is null || code is null)
            {
                return View("Error");
            }

            var user = await _userManager.FindByIdAsync(userId: userid);
            if (user is null)
                return View("Error");

            var result = await _userManager.ConfirmEmailAsync(user, code);

            return result.Succeeded ? View("ConfirmEmail") : View("Error");
        }

        // GET
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            returnUrl ??= Url.Content("~/");

            if (!ModelState.IsValid) return View(model);

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password,
                isPersistent: model.RememberMe, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }

            if (result.IsLockedOut)
                return View("Lockout");

            ModelState.AddModelError(string.Empty, "Invalid login attempt");

            return View(model);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
        }

        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Index), "Home");
        }

        private bool UserExists(string id)
        {
            var usuario = _userManager.FindByIdAsync(id);

            if (usuario is null)
            {
                return false;
            }

            return true;
        }
    }
}