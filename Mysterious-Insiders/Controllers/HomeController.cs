﻿using System;
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

namespace Mysterious_Insiders.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserAccountService _service;
        private readonly IMessageDAL LibraryDB;

        public HomeController(ILogger<HomeController> logger, UserAccountService service, IMessageDAL input)
        {
            _logger = logger;
            _service = service;
            LibraryDB = input;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// This is a demo method to show that my rolling logic works. Im using routing to take in the data.
        /// </summary>
        /// <param name="total">Number of dice</param>
        /// <param name="sides">Number of sides</param>
        /// <param name="mod">Mod on rolls</param>
        /// <param name="allRolls">Is the mod added to all rolls</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DiceRoll(int total, int sides, int mod, int allRolls) {
        string roll = (allRolls == 1) ? $"/r ({total}d{sides})+{mod}" : $"/r {total}d{sides}+{mod}";
        roll = ChatCommands.CheckForCommand(roll);
        UserMessage message = new UserMessage() { Name = "Command", Message = roll };
        LibraryDB.AddMessage(message);
        return ChatTest();
        }
        [Route("/Chattest")]
        public IActionResult ChatTest(string name = "") {
            if (name == "" || name == null) name = "User";
            ViewBag.Name = name;
            return View(LibraryDB.GetMessages());
        }
        [HttpPost][Route("/Chattest")]
        public IActionResult ChatTest(string name, string msg) {
            if (name == "" || name == null) name = "User";
            ViewBag.Name = name;

        if (msg != null) { 
                
            msg = ChatCommands.CheckForCommand(msg);

            UserMessage message = new UserMessage() { Name = name, Message = msg };

            LibraryDB.AddMessage(message);

            } 
            return RedirectToAction(actionName:"ChatTest", routeValues:name);
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
            TempData["username"] = username;
            return View();

        }

        public IActionResult SignUp()
        {
            return View();
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
