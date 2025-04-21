using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace API.Data
{
    public static class Seed
    {
        ////public static async Task ClearConnections(DataContext context)
        ////{
        ////    context.Connections.RemoveRange(context.Connections);
        ////    await context.SaveChangesAsync();
        ////}

        public static async Task SeedUsersAsync(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var users = JsonSerializer.Deserialize<List<User>>(userData, options);

            var roles = new List<Role>
            {
                new Role {Name = "Member"},
                new Role {Name = "Admin"},
                new Role {Name = "Moderator"}
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            if (users is null) return;

            foreach (var user in users)
            {
                user.UserName = user.UserName.ToLower();
                user.CreatedAt = DateTime.SpecifyKind(user.CreatedAt.DateTime, DateTimeKind.Utc);
                user.LastActive = DateTime.SpecifyKind(user.LastActive.DateTime, DateTimeKind.Utc);

                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, "Member");
            }

            var admin = new User
            {
                UserName = "admin",
                KnownAs = "admin",
                Gender = "irrelevant",
                City = "irrelevant",
                Country = "irrelevant"
            };

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });

            Console.WriteLine("Seeded users and roles successfully.");
        }
    }
}