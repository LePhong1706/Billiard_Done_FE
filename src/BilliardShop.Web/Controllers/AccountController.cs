using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using BilliardShop.Application.Interfaces.Services;
using BilliardShop.Application.Common.DTOs;
using BilliardShop.Web.Models;

namespace BilliardShop.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly INguoiDungService _nguoiDungService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            INguoiDungService nguoiDungService,
            ILogger<AccountController> logger)
        {
            _nguoiDungService = nguoiDungService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _nguoiDungService.ValidateLoginAsync(model.UsernameOrEmail, model.Password);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage ?? result.Errors.FirstOrDefault() ?? "Đăng nhập thất bại");
                return View(model);
            }

            var user = result.Data!;

            // Tạo claims cho user
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.TenDangNhap),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, $"{user.Ho} {user.Ten}".Trim()),
                new Claim(ClaimTypes.Role, user.MaVaiTro.ToString()),
                new Claim("TenVaiTro", user.TenVaiTro ?? "KhachHang")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(30) : DateTimeOffset.UtcNow.AddHours(2)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // Cập nhật thời gian đăng nhập cuối
            await _nguoiDungService.UpdateLastLoginAsync(user.Id, DateTime.Now);

            _logger.LogInformation("User {Username} logged in at {Time}", user.TenDangNhap, DateTime.UtcNow);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Kiểm tra email đã tồn tại
            if (!await _nguoiDungService.IsEmailAvailableAsync(model.Email))
            {
                ModelState.AddModelError("Email", "Email đã được sử dụng");
                return View(model);
            }

            // Kiểm tra username đã tồn tại
            if (!await _nguoiDungService.IsUsernameAvailableAsync(model.TenDangNhap))
            {
                ModelState.AddModelError("TenDangNhap", "Tên đăng nhập đã được sử dụng");
                return View(model);
            }

            var createDto = new CreateNguoiDungDto
            {
                TenDangNhap = model.TenDangNhap,
                Email = model.Email,
                MatKhau = model.MatKhau,
                Ho = model.Ho,
                Ten = model.Ten,
                SoDienThoai = model.SoDienThoai,
                NgaySinh = model.NgaySinh,
                GioiTinh = model.GioiTinh,
                MaVaiTro = 4 // KhachHang
            };

            var result = await _nguoiDungService.CreateAsync(createDto);

            if (!result.IsSuccess)
            {
                if (!string.IsNullOrEmpty(result.ErrorMessage))
                {
                    ModelState.AddModelError(string.Empty, result.ErrorMessage);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
                return View(model);
            }

            TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
