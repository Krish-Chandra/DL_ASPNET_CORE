using DL_Core_WebAPP_Release.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DL_Core_WebAPP_Release.Migrations
{
    public class DatabaseSeeder
    {

        public async static Task SeedTheDatabase(IServiceProvider serviceProvider, bool createRolesAndUsers = true)
        {
            var context = (DLContext)serviceProvider.GetService(typeof(DLContext));
            {
                if (!context.Authors.Any())
                {
                    Author auth1 = new Author { Name = "Krish Chandrasekaran", Address = "1 Main Street", City = "Chennai", State = "TN", PinCode = "600053", Email = "kc@example.com", Phone = "123456" };
                    Author auth2 = new Author { Name = "John Doe", Address = "1 Main Street", City = "Chennai", State = "TN", PinCode = "600053", Email = "kc@example.com", Phone = "123456" };

                    context.Authors.Add(auth1);
                    context.Authors.Add(auth2);

                    Category cat1 = new Category { Name = "ASP.NET Core", Description = "The new open-source web framework from Microsoft" };
                    Category cat2 = new Category { Name = "PHP", Description = "Very popular server-side programming language for web development" };
                    context.Categories.Add(cat1);
                    context.Categories.Add(cat2);

                    Publisher pub1 = new Publisher { Name = "Microsoft Press", Address="1 Washington Blvd", City="Houston", State="TX", Email="mspress@example.com", Phone="123-123-1234", PinCode="123456" };
                    Publisher pub2 = new Publisher { Name = "APress", Address = "1 Washington Blvd", City = "Houston", State = "TX", Email = "mspress@example.com", Phone = "123-123-1234", PinCode = "123456" };
                    Publisher pub3 = new Publisher { Name = "Packt Publisher", Address = "1 Washington Blvd", City = "Houston", State = "TX", Email = "mspress@example.com", Phone = "123-123-1234", PinCode = "123456" };
                    context.Publishers.Add(pub1);
                    context.Publishers.Add(pub2);
                    context.Publishers.Add(pub3);

                    context.Books.AddRange(
                        new Book { Title = "Getting Strted with ASP.NET Core 1.0", Author = auth1, Category = cat1, Publisher = pub1, ISBN = "X-12-B-12345", TotalCopies = 10, AvailableCopies = 10 },
                        new Book { Title = "ASP.NET Core MVC Unleashed", Author = auth2, Category = cat1, Publisher = pub3, ISBN = "Y-123-A98765", TotalCopies = 5, AvailableCopies = 5 },
                        new Book { Title = "Learning Yii 2.0!", Author = auth2, Category = cat2, Publisher = pub3, ISBN = "Z-XYP-A98765", TotalCopies = 5, AvailableCopies = 5 }
                    );
                    if (createRolesAndUsers)
                    {
                        await CreateRolesAndUsers(serviceProvider);
                    }

                    context.SaveChanges();
                }
            }
        }
        #region Step21
        private static async Task CreateRolesAndUsers(IServiceProvider serviceProvider)
        {

            const string adminRole = "Administrator";
            const string librarianRole = "Librarian";
            const string memberRole = "Member";

            const string adminEmailId = "admin@example.com";
            const string librarianEmailId = "librarian@example.com";
            const string memberEmailId = "guest@example.com";

            UserManager<IdentityUser> _userManager = serviceProvider.GetService(typeof(UserManager<IdentityUser>)) as UserManager<IdentityUser>;
            RoleManager<IdentityRole> _roleManager = serviceProvider.GetService(typeof(RoleManager<IdentityRole>)) as RoleManager<IdentityRole>;
            if (!await _roleManager.RoleExistsAsync(adminRole))
            {
                IdentityRole adminIdentityRole = new IdentityRole(adminRole);

                var idenResult = await _roleManager.CreateAsync(adminIdentityRole);
                if (idenResult.Succeeded)
                {
                    #region Claim
                    await _roleManager.AddClaimAsync(adminIdentityRole, new Claim("AdminUsers", "Allowed"));
                    //await _roleManager.AddClaimAsync(adminIdentityRole, new Claim("Members", "Allowed"));
                    await _roleManager.AddClaimAsync(adminIdentityRole, new Claim("Authors", "Allowed"));
                    await _roleManager.AddClaimAsync(adminIdentityRole, new Claim("Books", "Allowed"));
                    await _roleManager.AddClaimAsync(adminIdentityRole, new Claim("Categories", "Allowed"));
                    await _roleManager.AddClaimAsync(adminIdentityRole, new Claim("Publishers", "Allowed"));
                    await _roleManager.AddClaimAsync(adminIdentityRole, new Claim("Requests", "Allowed"));
                    await _roleManager.AddClaimAsync(adminIdentityRole, new Claim("Issues", "Allowed"));
                    await _roleManager.AddClaimAsync(adminIdentityRole, new Claim("Roles", "Allowed"));
                    #endregion

                    var admin = await _userManager.FindByNameAsync(adminEmailId);
                    if (admin == null)
                    {
                        var adminUser = new IdentityUser {  UserName = adminEmailId, Email = adminEmailId };
                        await _userManager.CreateAsync(adminUser, "Password+1");
                        await _userManager.AddToRoleAsync(adminUser, adminRole);
                    }
                }
            }
            if (!await _roleManager.RoleExistsAsync(librarianRole))
            {
                IdentityRole librarianIdentityRole = new IdentityRole(librarianRole);

                var idenResult = await _roleManager.CreateAsync(librarianIdentityRole);
                if (idenResult.Succeeded)
                {
                    #region Claim
                    await _roleManager.AddClaimAsync(librarianIdentityRole, new Claim("Authors", "Allowed"));
                    await _roleManager.AddClaimAsync(librarianIdentityRole, new Claim("Books", "Allowed"));
                    await _roleManager.AddClaimAsync(librarianIdentityRole, new Claim("Categories", "Allowed"));
                    await _roleManager.AddClaimAsync(librarianIdentityRole, new Claim("Publishers", "Allowed"));
                    await _roleManager.AddClaimAsync(librarianIdentityRole, new Claim("Requests", "Allowed"));
                    await _roleManager.AddClaimAsync(librarianIdentityRole, new Claim("Issues", "Allowed"));
                    #endregion

                    var librarian = await _userManager.FindByNameAsync(librarianEmailId);
                    if (librarian == null)
                    {
                        var librarianUser = new IdentityUser { UserName = librarianEmailId, Email = librarianEmailId };
                        await _userManager.CreateAsync(librarianUser, "Password+1");
                        await _userManager.AddToRoleAsync(librarianUser, librarianRole);
                    }
                }
            }

            if (!await _roleManager.RoleExistsAsync(memberRole))
            {
                var memberIdentityRole = new IdentityRole(memberRole);
                var result = await _roleManager.CreateAsync(memberIdentityRole);
                if (result.Succeeded)
                {
                    var member = await _userManager.FindByNameAsync(memberEmailId);
                    if (member == null)
                    {
                        var memberUser = new IdentityUser { UserName = memberEmailId, Email = memberEmailId};
                        await _userManager.CreateAsync(memberUser, "Password+1");
                        await _userManager.AddToRoleAsync(memberUser, memberRole);
                    }
                }
            }
        }
        #endregion Step21
    }
}
