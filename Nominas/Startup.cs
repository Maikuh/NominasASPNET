using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Nominas.Models;
using Owin;

[assembly: OwinStartupAttribute(typeof(Nominas.Startup))]
namespace Nominas
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateDefaultRolesAndUsers();
        }

        private void CreateDefaultRolesAndUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!roleManager.RoleExists("Admin"))
            {

                // Create Admin role  
                var role = new IdentityRole { Name = "Admin" };
                roleManager.Create(role);

                // Create default User with Admin role               
                var user = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@nominas.com"
                };

                string userPWD = "Asdf_1234";

                var checkUser = UserManager.Create(user, userPWD);

                // Add default User to Admin role if user creation succeeded
                if (checkUser.Succeeded)
                {
                    var result = UserManager.AddToRole(user.Id, "Admin");
                }
            }

            if (!roleManager.RoleExists("Contable"))
            {
                // Create Contable role
                var role = new IdentityRole { Name = "Contable"};
                roleManager.Create(role);

                // Create default User with Contable role               
                var user = new ApplicationUser
                {
                    UserName = "contable",
                    Email = "contable@nominas.com"
                };

                string userPWD = "Asdf_1234";

                var checkUser = UserManager.Create(user, userPWD);

                // Add default User to Contable role if user creation succeeded
                if (checkUser.Succeeded)
                {
                    var result = UserManager.AddToRole(user.Id, "Contable");
                }
            }
        }
    }
}
