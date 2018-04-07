using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using GoogleAuthentication.Models;
using Microsoft.Owin.Security;

namespace GoogleAuthentication
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                LoginPath = new PathString("/Account/Index"),
                SlidingExpiration = true
            });
            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "1075921257588-retbeq31ev09f45m0447nlgvnvs45nni.apps.googleusercontent.com",
                ClientSecret = "XB-iUs2_r4qW-p2gbsEV37OT",
                 CallbackPath = new PathString("/GoogleLoginCallback")
            });
        }
    }
}