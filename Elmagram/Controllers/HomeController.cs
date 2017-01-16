using System.Collections.Generic;
using System.Threading.Tasks;
using Elmagram.Models;
using Elmagram.Models.HomeViewModels;
using Elmagram.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Elmagram.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        private readonly UserManager<ApplicationUser> _userManager;
        public HomeController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();
            var chatModel = new ChatModel();
            if (user != null)
            {
                chatModel.Users.AddRange(ElmaManager.Instance.GetElmaUsers("admin", ""));
            }
            
            if (_userManager != null)
            {
                if (user != null)
                {
                    chatModel.CurrentUserId = user.Id;
                }
            }
            return View(chatModel);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
