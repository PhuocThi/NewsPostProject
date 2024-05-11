using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using News.Web.Models.ViewModels;
using News.Web.Repositories;
using System.Reflection.Metadata.Ecma335;

namespace News.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IUserRepository userRepository;

        public AccountController(UserManager<IdentityUser> userManager, 
                                SignInManager<IdentityUser> signInManager,
                                IUserRepository userRepository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            var user = await userRepository.FindByEmail(registerViewModel.Email);
            var user2 = await userRepository.FindByUserName(registerViewModel.Username);

            if (user != null)
            {
                ModelState.AddModelError("Email", "Email này đã có người sử dụng");
            }

            if (user2 != null)
            {
                ModelState.AddModelError("Username", "Tên tài khoản này đã có người sử dụng");
            }

            if (ModelState.IsValid)
            {
                var identityUser = new IdentityUser
                {
                    UserName = registerViewModel.Username,
                    Email = registerViewModel.Email,

                };

                var identityResult = await userManager.CreateAsync(identityUser, registerViewModel.Password);

                if (identityResult.Succeeded)
                {
                    var roleIdentityResult = await userManager.AddToRoleAsync(identityUser, "User");

                    if (roleIdentityResult.Succeeded)
                    {
                        return RedirectToAction("Login");
                    }

                }
            }
            if(user==null && user2 == null)
            {
                ModelState.AddModelError("Password", "Mật khẩu phải có ít nhất 1 chữ hoa, 1 kí tự số và 1 kí tự đặc biệt");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            var model = new LoginViewModel { ReturnUrl = returnUrl };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await signInManager.PasswordSignInAsync(loginViewModel.Username,
                                                    loginViewModel.Password, false, false);

                if (signInResult != null && signInResult.Succeeded)
                {
                    if (!string.IsNullOrWhiteSpace(loginViewModel.ReturnUrl))
                    {
                        return RedirectToPage(loginViewModel.ReturnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("Password", "Không tìm thấy tài khoản");

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
