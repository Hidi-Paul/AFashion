using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using OCS.MVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace OCS.MVC.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private static string ServerAddr => ConfigurationManager.AppSettings["base-url"];

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = SignIn(model.Email, model.Password, model.RememberMe);
            if (!result)
            {
                ModelState.AddModelError("LoginResult", "Username or Password incorrect");
                return View(model);
            }


            Guid userSessionId = Guid.NewGuid();
            string userRouteData = "g-" + userSessionId.ToString();

            //Add To Request to we can access it in the MultiTennantCookieManager
            var context = HttpContext.GetOwinContext().Request.Set<string>("userSessionGuid", userRouteData);

            //For the weird cases where /some/ SessionID is in the url before login
            //We remove it so it doesn't get dupplicated
            if (returnUrl != null)
            {
                int a = returnUrl.IndexOf("/g-");
                if (a != -1)
                {
                    int b = returnUrl.Substring(a + 1).IndexOf("/");

                    var prefix = returnUrl.Substring(0, a);
                    var suffix = returnUrl.Substring(b + 1, returnUrl.Length - b - 1);
                    returnUrl = prefix + suffix;
                    returnUrl = "/" + userRouteData + returnUrl;
                }
            }
            else
            {
                returnUrl = "/" + userRouteData;
            }
            

            return Redirect(returnUrl);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("PostProduct")]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var response = GetRegister(ServerAddr, model.Email, model.Password, model.ConfirmPassword);

            var signInResult = SignIn(model.Email, model.Password);
            if (signInResult)
            {
                return RedirectToAction("Index", "Product");
            }
            ModelState.AddModelError("RegisterResult", "Failed to sign in");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            SignOut();
            return Redirect("/Account/Login");
        }

        #region Helpers
        private bool SignIn(string userName, string password, bool isPersistent = false)
        {
            //Token Request
            var response = GetToken(ServerAddr, userName, password);
            if (response == null)
            {
                return false;
            }

            var accessString = response.Content.ReadAsStringAsync().Result;
            Token token = JsonConvert.DeserializeObject<Token>(accessString);

            if (token.AccessToken == null)
            {
                return false;
            }


            //Identity config
            var claims = new List<Claim>(){
                new Claim(ClaimTypes.Name, token.Username),
                new Claim("AccessToken", token.AccessToken),
            };
            if (token.Role != null)
            {
                claims.Add(new Claim(ClaimTypes.Role, token.Role));
            }
            

            var identity = new ClaimsIdentity(claims.ToArray(), DefaultAuthenticationTypes.ApplicationCookie);
            
            //Authorization

            AuthenticationProperties authOptions = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = isPersistent,
                ExpiresUtc = DateTime.Now.AddSeconds(token.ExpiresIn)
            };
            Request.GetOwinContext().Authentication.SignIn(authOptions, identity);
            return true;
        }
        private void SignOut()
        {
            Request.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }

        private static HttpResponseMessage GetToken(string url, string userName, string password)
        {
            var pairs = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>( "grant_type", "password" ),
                        new KeyValuePair<string, string>( "username", userName ),
                        new KeyValuePair<string, string> ( "Password", password )
                    };
            var content = new FormUrlEncodedContent(pairs);
            using (var client = new HttpClient())
            {
                var response = client.PostAsync(url + "Token", content).Result;
                return response;
            }
        }

        private static HttpResponseMessage GetRegister(string url, string email, string password, string confirmPassword)
        {
            var pairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>( "Email", email ),
                new KeyValuePair<string, string>( "Password", password ),
                new KeyValuePair<string, string>( "ConfirmPassword", confirmPassword )
            };
            var content = new FormUrlEncodedContent(pairs);
            using (var client = new HttpClient())
            {
                var response = client.PostAsync(url + "Account/Register", content).Result;
                return response;
            }
        }


        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        #endregion Helpers
    }
}