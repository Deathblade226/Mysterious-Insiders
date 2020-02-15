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

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
        public IActionResult DiceRoll(int total = 1, int sides = 20, int mod = 0, bool allRolls = true) { 
        return View(Dice.RollDice(total, sides, mod, allRolls));
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
