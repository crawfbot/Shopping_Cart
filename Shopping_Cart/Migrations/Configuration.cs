namespace Shopping_Cart.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Shopping_Cart.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Shopping_Cart.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //TODO: Add code to allow us to create a new role
            //Step 1: Spin up an instance of the Role Manager

            var RoleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            //Step 2: Look for an existing role with the name Admin and if one is not found create one

            if (!context.Roles.Any(r=> r.Name == "Admin"))
            {
                RoleManager.Create(new IdentityRole { Name = "Admin" });
            }


            //TODO: Add code to create a new User
            //Step 1: Spin up a new instance of the user Manager class

            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            //Step 2: Look for an existing user with associated email address = to email below
            //ApplicationUser user;
         
           
            if (!context.Users.Any(u => u.Email == "jacobcrawford1990@gmail.com"))
            {
                userManager.Create(
                new ApplicationUser
                {
                    UserName = "jacobcrawford1990@gmail.com",
                    Email = "jacobcrawford1990@gmail.com",
                    FirstName = "Jacob",
                    LastName = "Crawford",
                    DisplayName = "Jacob"

                }
                , ".CoderFoundry$");
            }

            //References Application Users table

            //Finds the record with email = jacobcrawford1990@gmail.com

            //Assigns the above record's ID to the User ID Variable

            var userID = userManager.FindByEmail("jacobcrawford1990@gmail.com").Id;

            // Assigns the user identified by the userid variable(jacobcrawford1990@gmail.com)
            // to the Admin role

            userManager.AddToRole(userID, "Admin");



            //TODO: Add Code to assign a user to a role
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            
            if (!context.Items.Any()) {
                context.Items.AddOrUpdate(x => x.Id,
                new Item()
                { Name = ""
                    ,
                    Price = 600.00M
                    ,
                    Type = "Bed Linen"
                    ,
                    Brand = "BBB"
                    ,
                    MediaUrl = "~/Content/images/pi3.jpg"
                    ,
                    Description = "The softest bed linens imaginable"         
                    ,
                    Created = DateTimeOffset.Now
                    ,
                    Updated = DateTimeOffset.Now

            });
                };
        }
    }
}
