using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mysterious_Insiders.Logic;
using Mysterious_Insiders.Models;
using Mysterious_Insiders.Services;
using Microsoft.AspNetCore.Http;


namespace Mysterious_Insiders.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserAccountService _service;

        public HomeController(ILogger<HomeController> logger, UserAccountService service)
        {
            _logger = logger;
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            //UserAccount ua = new UserAccount(username, password);
            UserAccount ua = new UserAccount();
            ua.UserName = username;
            ua.Password = password;
            //var redirect = RedirectToAction("Create", "UserAccount", ua);
            _service.Create(ua);
            //TempData["username"] = username;
            HttpContext.Session.SetString("username", username);
            return View();

        }

        public IActionResult SignUp()
        {
            return View();
        }
    }
}
