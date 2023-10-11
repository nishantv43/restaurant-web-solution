using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Restaurant.Web.Models;
using Restaurant.Web.Service.IService;
using Restaurant.Web.Utility;

namespace Restaurant.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new();
            return View(loginRequestDto);
        }

        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text= SD.RoleAdmin, Value= SD.RoleAdmin},
                new SelectListItem{Text= SD.RoleCustomer, Value = SD.RoleCustomer}
            };
            ViewBag.RoleList = roleList;
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            return View();
        }

    }
}
