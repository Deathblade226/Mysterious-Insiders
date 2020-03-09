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
using Microsoft.AspNetCore;


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

        //This is testing code for PDF
        //Method 1
        public FileResult DisplayPDF()
        {
            return File("~/public/DNDCharacterSheet.pdf", "application/pdf");
        }

        //Method 2
        //public FileResult PDFDisplay()
        //{
        //    string filepath = Server.MapPath("/Temp.pdf");
        //    byte[] pdfByte = Helper.GetBytesFromFile(filepath);
        //    return File(pdfByte, "application/pdf");
        //}

        //Method 4
        public PartialViewResult PDFPartialView()
        {
            return PartialView();
        }
    }
}
