using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GerenciamentoIdentity.Constants;
using GerenciamentoIdentity.Models;

namespace GerenciamentoIdentity.Seeds
{
    public static class DefaultUsers
    {
        public static async Task SeedSuperAdminAsync(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager)
        {
            var defaultUser = new Usuario
            {
                UserName = "superadmin@superadmin.com",
                Email = "superadmin@superadmin.com",
                PhoneNumber = "(99) 99999-9999",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                LockoutEnabled = false,
                Matricula = "DEV01",
                Admissao = DateTime.Now,
                Nascimento = DateTime.Now,
                Documento = "11111111111",
                Ativo =true,
                UsuarioSistema = true,
                Nome = "Super Administrador",
                Sexo = true,
                Telefone = "(11) 11111-1111",
                Cep = "01001-001",
                Ibge = "1111111",
                Logradouro = "Rua teste",
                Complemento = "Próximo ao ponto de teste",
                Numero = "111",
                Bairro = "Teste",
                Municipio = "Teste",
                Uf = "AA"
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.SuperAdmin.ToString());
                }
            }
        }

    }
}