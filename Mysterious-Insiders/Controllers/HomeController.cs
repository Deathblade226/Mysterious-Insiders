using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mysterious_Insiders.Logic;
using Mysterious_Insiders.Models;

namespace Mysterious_Insiders.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMessageDAL LibraryDB;

    public HomeController(ILogger<HomeController> logger, IMessageDAL input) : base()
        {
            _logger = logger;
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
    }
}
