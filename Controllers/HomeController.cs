using GerenciamentoIdentity.Models;
using GerenciamentoIdentity.Seeds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;

namespace GerenciamentoIdentity.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IHttpContextAccessor _accessor;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public HomeController(IHttpContextAccessor accessor, ILogger<HomeController> logger, UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager)
        {
            _accessor = accessor;
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public string LocalIPAddr { get; private set; }

        public async Task <IActionResult> Index()
        {
            ViewData["UsuarioLogado"] = User.Identity?.Name;
            ViewData["HostName"] = System.Net.Dns.GetHostName();
            ViewBag.ListaIps = await System.Net.Dns.GetHostAddressesAsync(System.Net.Dns.GetHostName());
            CookieManager();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public void CookieManager()
        {
            var roles = ((ClaimsIdentity)User.Identity).Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value).ToList();

            Response.Cookies.Delete("Permission");
            Response.Cookies.Append("Permission", string.Join(",", roles));
        }
    }
}