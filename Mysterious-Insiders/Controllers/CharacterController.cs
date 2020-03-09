using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Mysterious_Insiders.Services;

namespace Mysterious_Insiders.Controllers
{
    public class CharacterController : Controller
    {
        private readonly SheetService sheetService;
        public CharacterController(SheetService sheets)
        {
            sheetService = sheets;
        }

        public IActionResult Index()
        {
            string username = HttpContext.Session.GetString("username");

            return View(sheetService.FilterByUser(username));
        }

        [HttpPost]
        public IActionResult SelectSheet(string name)
        {
            string username = HttpContext.Session.GetString("username");
            var first = sheetService.FilterByUser(username);
            var second = first.Where(s => s.Name == name).ToList().First();



            return RedirectToAction("DisplaySheet", "ModularSheet", second);
        }
    }
}