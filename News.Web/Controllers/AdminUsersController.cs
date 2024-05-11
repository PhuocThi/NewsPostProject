using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using News.Web.Models.ViewModels;
using News.Web.Repositories;

namespace News.Web.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminUsersController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly UserManager<IdentityUser> userManager;

        public AdminUsersController(IUserRepository userRepository,
                                    UserManager<IdentityUser> userManager)
        {
            this.userRepository = userRepository;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var users = await userRepository.GetAll();

            var usersViewModel = new UserViewModel();
            usersViewModel.Users = new List<User>();

            foreach (var user in users)
            {
                usersViewModel.Users.Add(new User
                {
                    Id = Guid.Parse(user.Id),
                    Username = user.UserName,
                    EmailAddress =user.Email
                });
            }

            return View(usersViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> List(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var identityUser = new IdentityUser
                {
                    UserName = userViewModel.Username,
                    Email = userViewModel.Email,
                };

                var identityResult = await userManager.CreateAsync(identityUser, userViewModel.Password);

                if (identityResult != null)
                {
                    if (identityResult.Succeeded)
                    {
                        var roles = new List<string> { "User" };

                        if (userViewModel.AdminRoleCheckbox)
                        {
                            roles.Add("Admin");
                        }

                        identityResult = await userManager.AddToRolesAsync(identityUser, roles);

                        if (identityResult != null && identityResult.Succeeded)
                        {
                            return RedirectToAction("List", "AdminUsers");
                        }
                    }

                }

            }
                return RedirectToAction("List", "AdminUsers");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            if(user != null)
            {
                var identityResult = await userManager.DeleteAsync(user);

                if(identityResult!= null && identityResult.Succeeded)
                {
                    return RedirectToAction("List", "AdminUsers");
                }
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddUserRequest request)
        {
            var user = await userRepository.FindByEmail(request.Email);
            var user2 = await userRepository.FindByUserName(request.Username);

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
                    UserName = request.Username,
                    Email = request.Email,
                };

                var identityResult = await userManager.CreateAsync(identityUser, request.Password);

                if (identityResult != null)
                {
                    if (identityResult.Succeeded)
                    {
                        var roles = new List<string> { "User" };

                        if (request.AdminRoleCheckbox)
                        {
                            roles.Add("Admin");
                        }

                        identityResult = await userManager.AddToRolesAsync(identityUser, roles);

                        if (identityResult != null && identityResult.Succeeded)
                        {
                            return RedirectToAction("List", "AdminUsers");
                        }
                    }

                }

            }

            if (user == null && user2 == null)
            {
                ModelState.AddModelError("Password", "Mật khẩu phải có ít nhất 6 kí tự, 1 chữ hoa, 1 kí tự số và 1 kí tự đặc biệt");
            }

            return View();
        }
    }
}
