using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Restaurant.Web.Models;
using Restaurant.Web.Service.IService;
using Restaurant.Web.Utility;

namespace Restaurant.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        //private readonly ITokenProvider _tokenProvider;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
            //_tokenProvider = tokenProvider;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new();
            return View(loginRequestDto);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto obj)
        {
            ResponseDto responseDto = await _authService.LoginAsync(obj);

            if (responseDto != null && responseDto.IsSuccess)
            {
                LoginResponseDto loginResponseDto =
                    JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result));

                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = responseDto.Message;
                return View(obj);
            }
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

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto registrationRequestDto)
        {
            var response = await _authService.RegisterAsync(registrationRequestDto);
            ResponseDto assingRole;

            if (response != null && response.IsSuccess)
            {
                if (string.IsNullOrEmpty(registrationRequestDto.Role))
                {
                    registrationRequestDto.Role = SD.RoleCustomer;
                }
                assingRole = await _authService.AssignRoleAsync(registrationRequestDto);
                if (assingRole != null && assingRole.IsSuccess)
                {
                    TempData["success"] = "Registration Successful";
                    return RedirectToAction(nameof(Login));
                }
            }
            else
            {
                TempData["error"] = response.Message;
            }

            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text=SD.RoleAdmin,Value=SD.RoleAdmin},
                new SelectListItem{Text=SD.RoleCustomer,Value=SD.RoleCustomer},
            };

            ViewBag.RoleList = roleList;
            return View(registrationRequestDto);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            return View();
        }

    }
}
