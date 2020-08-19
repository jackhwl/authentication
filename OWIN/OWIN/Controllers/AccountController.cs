using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace OWIN.Controllers
{
    public class AccountController : Controller
    {
        public UserManager<IdentityUser> UserManager => HttpContext.GetOwinContext().Get<UserManager<IdentityUser>>();

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            var identityResult = await UserManager.CreateAsync(new IdentityUser(model.Username), model.Password);

            if (identityResult.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", identityResult.Errors.FirstOrDefault());

            return View(model);
        }
    }

    public class RegisterModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}