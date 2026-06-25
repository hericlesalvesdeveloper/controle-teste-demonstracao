using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ControleTeste.Context;
using ControleTeste.Models;

namespace ControleTeste.Data
{
    public static class SeedData
    {
        public static async Task EnsureAdminAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ControleTesteContext>();
            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            var username = config["DefaultAdmin:Username"] ?? "admin";
            var password = config["DefaultAdmin:Password"] ?? "Admin@123";

            // check existing
            var exists = await context.Usuarios.FindAsync(1);
            // try find by username
            if (exists == null)
            {
                var userByName = await context.Usuarios.FirstOrDefaultAsync(u => u.Username == username);
                if (userByName == null)
                {
                    var admin = new Usuario
                    {
                        Username = username,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                        IsAdmin = true,
                        CreatedAt = DateTime.UtcNow
                    };
                    context.Usuarios.Add(admin);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
