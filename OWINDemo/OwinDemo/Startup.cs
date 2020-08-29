﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartup(typeof(OwinDemo.Startup))]

namespace OwinDemo
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888

            const string connectionString = @"Data Source=(LocalDb)\MSSQLLocalDB;Database=module3;trusted_connection=yes;";
            app.CreatePerOwinContext(() => new ExtendedUserDbContext(connectionString));
            app.CreatePerOwinContext<UserStore<ExtendedUser>>((opt, cont) => new UserStore<ExtendedUser> (cont.Get<ExtendedUserDbContext>()));
            app.CreatePerOwinContext<UserManager<IdentityUser>>(
               (opt, cont) =>
                    //new UserManager<ExtendedUser>(cont.Get<UserStore<ExtendedUser>>()));
                    {
                        var usermanager = new UserManager<IdentityUser>(cont.Get<UserStore<IdentityUser>>());
                        usermanager.RegisterTwoFactorProvider("SMS", new PhoneNumberTokenProvider<IdentityUser>{ MessageFormat = "Token: {0}" });
                        usermanager.SmsService = new SmsService();
                        return usermanager;
                    });
            app.CreatePerOwinContext<SignInManager<ExtendedUser, string>>(
                (opt, cont) => new SignInManager<ExtendedUser, string>(cont.Get<UserManager<ExtendedUser>>(), cont.Authentication));                

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie
            });

            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);
        }
    }
}