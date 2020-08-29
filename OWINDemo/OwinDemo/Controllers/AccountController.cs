using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace OwinDemo.Controllers
{
    public class AccountController : Controller
    {
        public UserManager<IdentityUser> UserManager => HttpContext.GetOwinContext().Get<UserManager<IdentityUser>>();
        public SignInManager<IdentityUser, string> SignInManager => HttpContext.GetOwinContext().Get<SignInManager<IdentityUser, string>>();

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            var user = await UserManager.FindByNameAsync(model.Username);

            if (user != null)
            {
                var token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var resetUrl = Url.Action("PasswordReset", "Account", new {  userid = user.Id, token = token }, Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Password Reset", $"Use link to reset password: {resetUrl}");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model) 
        { 
            var signInStatus = await SignInManager.PasswordSignInAsync(model.Username, model.Password, true, true);

            switch (signInStatus)
            {
                case SignInStatus.Success:
                    return RedirectToAction("Index", "Home");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("ChooseProvider");
                default:
                    ModelState.AddModelError("", "invalid Credentials");
                    return View(model);
            }
        }
        public async Task<ActionResult> ChooseProvider()
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            var providers = await UserManager.GetValidTwoFactorProvidersAsync(userId);

            return View(new ChooseProviderModel { Providers = providers.ToList()});
        }

        [HttpPost]
        public async Task<ActionResult> ChooseProvider(ChooseProviderModel model)
        {
            await SignInManager.SendTwoFactorCodeAsync(model.ChosenProvider);
            return RedirectToAction("TwoFactor", new {provider = model.ChosenProvider });
        }

        public ActionResult TwoFactor(string provider)
        {
            return View(new TwoFactorModel { Provider = provider });
        }

        [HttpPost]
        public async Task<ActionResult> TwoFactor(TwoFactorModel model)
        {
            var signInStatus = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, true, model.RememberBrowser);
            switch (signInStatus)
            {
                case SignInStatus.Success:
                    return RedirectToAction("Index", "Home");
                default:
                    ModelState.AddModelError("", "Invalid Credentials");
                    return View(model);   
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            var identityUser = await UserManager.FindByNameAsync(model.Username);
            if (identityUser != null)
            {
                return RedirectToAction("Index", "Home");
            }
            //var user = new ExtendedUser
            //{
            //    UserName = model.Username,
            //    FullName = model.FullName
            //};
            //user.Addresses.Add(new Address { AddressLine = model.AddressLine, Country = model.Country, UserId = user.Id});
            var user = new IdentityUser(model.Username) { Email = model.Username};
            var identityResult = await UserManager.CreateAsync(user, model.Password);

            if (identityResult.Succeeded)
            {
                var token = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                var confirmUrl = Url.Action("ConfirEmail", "Account", new { userid = user.Id, token = token }, Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Email Confirmation", $"Use link to confirm email: {confirmUrl}");

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", identityResult.Errors.FirstOrDefault());

            return View(model);
        }

        public async Task<ActionResult> ConfirmEmail(string userid, string token)
        {
            var identityResult = await UserManager.ConfirmEmailAsync(userid, token);
            if (!identityResult.Succeeded)
            {
                return View("Error");
            }

            return RedirectToAction("Index", "Home");
        }
        public ActionResult PasswordReset(string userid, string token)
        {
            return View(new PasswordResetModel { UserId = userid, Token = token});
        }

        [HttpPost]
        public async Task<ActionResult> PasswordReset(PasswordResetModel model)
        {
            var identityResult = await UserManager.ResetPasswordAsync(model.UserId, model.Token, model.Password);
            if (!identityResult.Succeeded)
            {
                return View("Error");
            }

            return RedirectToAction("Index", "Home");
        }
    }

    public class PasswordResetModel
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
    }
    public class ForgotPasswordModel
    {
        public string Username { get; set; }
    }
    public class TwoFactorModel
    {
        public string Provider { get; set; }
        public string Code { get; set; }
        public bool RememberBrowser { get; set; }
    }
    public class ChooseProviderModel
    {
        public List<string> Providers { get; set; }
        public string ChosenProvider { get; set; }
    }
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RegisterModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string AddressLine { get; set;}
        public string Country { get; set; }
    }

}