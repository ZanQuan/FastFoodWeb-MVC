using FastFoodWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace FastFoodWeb.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    // GET: /Account/Register
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register() => View(new RegisterViewModel());

    // POST: /Account/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
        {
            ModelState.AddModelError("Email", "Email này đã được đăng ký.");
            return View(model);
        }

        var user = new IdentityUser
        {
            UserName = model.Email,
            Email = model.Email
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {

            try
            {
                await _userManager.AddToRoleAsync(user, "User");
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", "Lỗi khi thêm role người dùng.");
                return View(model);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        return View(model);
    }

    // GET: /Account/Login
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login() => View(new LoginViewModel());

    // POST: /Account/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var email = model.Email?.Trim() ?? "";

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            ModelState.AddModelError("", "Tài khoản không tồn tại.");
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(
            user.UserName!,
            model.Password,
            model.RememberMe,
            lockoutOnFailure: true);

        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Home");
        }

        if (result.IsLockedOut)
        {
            ModelState.AddModelError("", "Tài khoản bị khóa do nhập sai mật khẩu quá nhiều lần.");
            return View(model);
        }

        ModelState.AddModelError("", "Email hoặc mật khẩu không đúng.");
        return View(model);
    }

    // POST: /Account/Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    // GET: /Account/AccessDenied
    [HttpGet]
    public IActionResult AccessDenied() => View();

    // FORGOT PASSWORD –  Nhập email
    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Không tìm thấy tài khoản với email này.");
            return View(model);
        }

        return RedirectToAction("ResetPassword", new { email = model.Email });
    }

    // RESET PASSWORD –  Nhập mật khẩu mới
    [HttpGet]
    public IActionResult ResetPassword(string email)
    {
        if (string.IsNullOrEmpty(email))
            return RedirectToAction("ForgotPassword");

        var model = new ResetPasswordViewModel { Email = email };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            TempData["Error"] = "Tài khoản không tồn tại.";
            return RedirectToAction("ForgotPassword");
        }

        // Đặt lại mật khẩu mà không cần token 
        var removeResult = await _userManager.RemovePasswordAsync(user);
        if (removeResult.Succeeded)
        {
            var addResult = await _userManager.AddPasswordAsync(user, model.NewPassword);
            if (addResult.Succeeded)
            {
                TempData["Success"] = "Đặt lại mật khẩu thành công! Vui lòng đăng nhập.";
                return RedirectToAction("Login");
            }
            foreach (var err in addResult.Errors)
                ModelState.AddModelError(string.Empty, err.Description);
        }
        else
        {
            foreach (var err in removeResult.Errors)
                ModelState.AddModelError(string.Empty, err.Description);
        }

        return View(model);
    }
    // GET: /Account/ChangePassword
    [HttpGet]
    [Authorize]
    public IActionResult ChangePassword()
    {
        return View(new ChangePasswordViewModel());
    }

    // POST: /Account/ChangePassword
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return RedirectToAction("Login");

        var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        
        if (result.Succeeded)
        {
            await _signInManager.RefreshSignInAsync(user); // Giữ phiên đăng nhập
            TempData["Success"] = "Đổi mật khẩu thành công!";
            return RedirectToAction("ChangePassword");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }
}