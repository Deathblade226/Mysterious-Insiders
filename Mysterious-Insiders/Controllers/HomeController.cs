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
        private readonly IMessageDAL MessageDB;

        public HomeController(ILogger<HomeController> logger, UserAccountService service, IMessageDAL input)
        {
            _logger = logger;
            _service = service;
            MessageDB = input;
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
        //[HttpPost]
        //public IActionResult DiceRoll(int total, int sides, int mod, int allRolls) {
        //string roll = (allRolls == 1) ? $"/r ({total}d{sides})+{mod}" : $"/r {total}d{sides}+{mod}";
        //roll = ChatCommands.CheckForCommand(roll, "Commands");
        //UserMessage message = new UserMessage() { Name = "Command", Message = roll };
        //MessageDB.AddMessage(message);
        //return ChatTest();
        //}
        [Route("/Chattest")]
        public IActionResult ChatTest() {
            string name = HttpContext.Session.GetString("username");
            if (TempData["username"] != null) name = TempData["username"].ToString();
            if (name == "" || name == null) name = "User";
            ViewBag.Name = name;
            return View(MessageDB.GetMessages());
        }
        [HttpPost][Route("/Chattest")]
        public IActionResult ChatTest(string msg) {
            string name = HttpContext.Session.GetString("username");
            if (TempData["username"] != null) name = TempData["username"].ToString();
            if (name == "" || name == null) name = "User";
            ViewBag.Name = name;

        if (msg != null && !ChatCommands.CheckForCommand(msg, name)) { 

            UserMessage message = new UserMessage() { Name = name, Message = msg };

            MessageDB.AddMessage(message);

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
