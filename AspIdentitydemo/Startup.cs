using AspIdentitydemo.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System.Collections.Generic;

[assembly: OwinStartupAttribute(typeof(AspIdentitydemo.Startup))]
namespace AspIdentitydemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesAndUsers();
        }

        private void createRolesAndUsers() {

            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            if (!roleManager.RoleExists("Admin")) {

                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);


                var user = new ApplicationUser();
                user.UserName = "admin@gmail.com";
                user.Email = "admin@gmail.com";
                user.EmailConfirmed = true;

                string UserPwd = "Admin@123";


                var chkUser = userManager.Create(user, UserPwd);
                if (chkUser.Succeeded) {

                    var result1 = userManager.AddToRole(user.Id, "Admin");

                }





            }

            if (!roleManager.RoleExists("Sales"))
            {

                var role = new IdentityRole();
                role.Name = "Sales";
                roleManager.Create(role);

               
            }

            if (!roleManager.RoleExists("Manager"))
            {

                var role = new IdentityRole();
                role.Name = "Manager";
                roleManager.Create(role);


            }

            if (!roleManager.RoleExists("Accounts"))
            {

                var role = new IdentityRole();
                role.Name = "Accounts";
                roleManager.Create(role);


            }




        }
    }
}
