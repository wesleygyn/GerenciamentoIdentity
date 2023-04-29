using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using GerenciamentoIdentity.Constants;
using GerenciamentoIdentity.Models;

namespace GerenciamentoIdentity.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (await roleManager.RoleExistsAsync("SuperAdmin") == false)
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
            }
            if (await roleManager.RoleExistsAsync("Admin") == false)
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            }
            if (await roleManager.RoleExistsAsync("Basic") == false)
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Basic.ToString()));
            }
        }
    }
}