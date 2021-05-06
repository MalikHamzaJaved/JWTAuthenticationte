using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ShopOnClick.Models;

namespace ShopOnClick.Areas.Auth.Controllers
{
    [Area("Auth")]
    public class Security : Controller
    {
        private dynamic expObj;
        public Security()
        {
           
            expObj = new ExpandoObject();
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> verify(LoginModel postedData)
        {
            var status = false;
            if (postedData != null)
            {
                var client = new HttpClient();
             
                var data = JsonConvert.SerializeObject(postedData);
                var stringContent = new StringContent(data, UnicodeEncoding.UTF8, "application/json");
                var response = await client.PostAsync("http://localhost:61955/api/authenticate/login", stringContent);

                if(response.ReasonPhrase == "Unauthorized")
                {
                    status = false;
                    return Json(new { status = status });
                }

                var stringJson = await response.Content.ReadAsStringAsync();
                var tokenObject = JsonConvert.DeserializeObject<Token>(stringJson);

                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(tokenObject.token);

                var userClaims = new List<Claim>() {
                    new Claim(ClaimTypes.Name,""),
                    new Claim(ClaimTypes.NameIdentifier,tokenObject.token),
                    new Claim(ClaimTypes.Email,""),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var userIdentity = new ClaimsIdentity(userClaims, "User Identity");
                var userPrincipal = new ClaimsPrincipal(new[] { userIdentity });

                HttpContext.SignInAsync(userPrincipal);
                status = userPrincipal.Identity.IsAuthenticated;
            }

            return Json(new { status = status });
        }


        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();

            return RedirectToAction("Login");
        }

    }
}
