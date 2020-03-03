using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
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
        public IActionResult Login(UserAccount ua)
        {
            if(ModelState.IsValid)
            {
                if (_service.Contains(ua))
                {
                    //check password
                    if (_service.Get(ua.UserName).Password == ua.Password)
                    {
                        HttpContext.Session.SetString("username", ua.UserName);
                    }
                    ModelState.AddModelError("Password", "Incorrect password");
                }
                else
                {
                    ModelState.AddModelError("UserName", "Username does not exist");
                }
            }
            

            return View();

        }

        //public IActionResult SignUp(string username, string password)
        public IActionResult SignUp(UserAccount ua)
        {
            if(ModelState.IsValid)
            {
                if (_service.Create(ua))
                {
                    HttpContext.Session.SetString("username", ua.UserName);
                }
                ModelState.AddModelError("UserName", "Username already taken!");
            }
            
            return View("Login");
        }

        public IActionResult CharacterCreator()
        {
            return View();
        }

        //This is testing code for embedding a pdf file into the cshtml
        [HttpPost]
        public ActionResult ViewPDF()
        {
            string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"500px\" height=\"300px\">";
            embed += "If you are unable to view file, you can download from <a href = \"{0}\">here</a>";
            embed += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
            embed += "</object>";
            TempData["Embed"] = string.Format(embed, VirtualPathUtility.ToAbsolute("~/public/DNDCharacterSheet.pdf"));

            return RedirectToAction("CharacterCreator");
        }
    }
}
