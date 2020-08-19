using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(OWIN.Startup))]

namespace OWIN
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888

            const string connectionString = @"Data Source=(LocalDb)\MSSQLLocalDB;Database=module3;trusted_connection=yes;";
            app.CreatePerOwinContext(()=> new IdentityDbContext(connectionString));
            app.CreatePerOwinContext<UserStore<IdentityUser>>((opt, cont) => new UserStore<IdentityUser>(cont.Get<IdentityDbContext>()));
            app.CreatePerOwinContext<UserManager<IdentityUser>>((opt, cont) => new UserManager<IdentityUser>(cont.Get<UserStore<IdentityUser>>()));

        }
    }
}
