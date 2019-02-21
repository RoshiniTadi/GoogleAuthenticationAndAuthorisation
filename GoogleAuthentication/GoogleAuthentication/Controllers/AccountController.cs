using GoogleAuthentication.Models;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace GoogleAuthentication.Controllers
{
    public class AccountController : Controller
    {/// <summary>
    /// This is the logoff action from the google account
    /// </summary>
    /// <returns></returns>
        public ActionResult LogOff()
        { 
            HttpContext.GetOwinContext().Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return RedirectToAction("Index", "Home");
        }
    
        /// <summary>
        /// Login action into the google account
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }
       
            public void SignIn(string ReturnUrl = "/", string type = "")
            {
                if (!Request.IsAuthenticated)
                {
                    if (type == "Google")
                    {
                        HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = "Account/GoogleLoginCallback" }, "Google");
                    }
                }
            }
        /// <summary>
        /// The google logi call back method which will be redirected when we login into the google account
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult GoogleLoginCallback()
        {
            var claimsPrincipal = HttpContext.User.Identity as ClaimsIdentity;

            var loginInfo = GoogleLoginViewModel.GetLoginInfo(claimsPrincipal);
            if (loginInfo == null)
            {
                return RedirectToAction("Index");
            }
            
            webEntities2 db = new webEntities2();
            Role Obj = new Role();
            var user = db.UserAccounts.FirstOrDefault(x => x.Email == loginInfo.emailaddress);
            
            if (user == null)
            {
                user = new UserAccount
                {
                    Email = loginInfo.emailaddress,
                    GivenName = loginInfo.givenname,
                    Identifier = loginInfo.nameidentifier,
                    Name = loginInfo.name,
                    SurName = loginInfo.surname,
                    IsActive = true
                };
                db.UserAccounts.Add(user);
          if(user.Email.Contains(".com"))
                {
                    user.Role = 1;
                    Obj = db.Roles.FirstOrDefault(x=>x.Roleid==user.Role);
                 
                }
                else
                {
                    user.Role = 2;
                    Obj = db.Roles.FirstOrDefault(x => x.Roleid == user.Role);
                }
                user.Roles.Add(Obj);
                db.SaveChanges();
            }
           else
            {
                Obj = db.Roles.FirstOrDefault(x => x.Roleid == user.Role);

            }
            var ident = new ClaimsIdentity(
                    new[] { 
            	            new Claim(ClaimTypes.NameIdentifier, user.Email),
                            new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),
                            new Claim(ClaimTypes.Name, user.Name),
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim(ClaimTypes.Role, Obj.RoleName)
                    },
                    CookieAuthenticationDefaults.AuthenticationType);


            HttpContext.GetOwinContext().Authentication.SignIn(
                        new AuthenticationProperties { IsPersistent = false }, ident);
           

            return RedirectToAction("Index","Users");

        }
    }
}
