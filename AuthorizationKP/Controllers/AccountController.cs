using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using AuthorizationKP.Domain.Interfaces;
using AuthorizationKP.Domain.Entity;
using AuthorizationKP.Models;
using AuthorizationKP.Domain.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using AuthorizationKP.Domain.ViewModels.Users;
using AuthorizationKP.Domain.Response;
using System.Net.Http;

namespace AuthorizationKP.Controllers
{
    public class AccountController : Controller
    {

        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _userService.DeleteUser(id);
            if (response.StatusCode == Domain.Enum.StatusCode.Success)
            {
                return RedirectToAction("GetUsers");
            }
            return RedirectToAction("Error");

        }


        [HttpGet]
        public ActionResult Register() => View();

        [HttpPost]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {
                var response = await _userService.Register(model);
                if (response.StatusCode == Domain.Enum.StatusCode.Success)
                {

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new System.Security.Claims.ClaimsPrincipal(response.Data));
                    //CookieOptions options = new CookieOptions();
                    //options.Expires = DateTime.Now.AddMinutes(2);
                    //Response.Cookies.Append("name", model["LoginTFA"])
                    var systemConfirmCode = response.Description;
                    HttpContext.Response.Cookies.Append("LoginTFA", model.Login, new CookieOptions
                    {
                        Expires = DateTime.Now.AddMinutes(3)
                    });
                    HttpContext.Response.Cookies.Append("systemConfirmCode", response.Description , new CookieOptions
                    {
                        Expires = DateTime.Now.AddMinutes(3)
                    });
                    return RedirectToAction("TwoFactAuthenticate");
                }
                ModelState.AddModelError("", response.Description);
            }
           return View(model);
        }


        [HttpGet]
        public ActionResult TwoFactAuthenticate() => View();

        [HttpPost]
        public async Task<ActionResult> TwoFactAuthenticate(TwoFactAuthenticationViewModel model1)
        {
            
            if (model1.userConfirmCode == null)
                {
                    return RedirectToAction("Index", "Home");
                }
            if (ModelState.IsValid)
            {
                var tempLogin = HttpContext.Request.Cookies["LoginTFA"];
                var systemConfirmCode = HttpContext.Request.Cookies["systemConfirmCode"];
                var response = await _userService.TwoFactAuthenticate(model1,tempLogin,systemConfirmCode);
                if (response.StatusCode == Domain.Enum.StatusCode.Success)
                {
                    return RedirectToAction("Index","Home");
                }
                ModelState.AddModelError("", response.Description);
            }
            return View("Login");
        }
        

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login (LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _userService.Login(model);
                if (response.StatusCode == Domain.Enum.StatusCode.Success)
                {

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new System.Security.Claims.ClaimsPrincipal(response.Data));

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", response.Description);
            }
            return View(model);
        }

        //[ValidateAntiForgeryToken]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult ChangePassword() => View();

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _userService.ChangePassword(model);
                if (response.StatusCode == Domain.Enum.StatusCode.Success)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
