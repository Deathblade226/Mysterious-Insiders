using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mysterious_Insiders.Models;
using Mysterious_Insiders.Services;
using Microsoft.AspNetCore.Http;


namespace Mysterious_Insiders.Controllers
{

	public class ModularSheetController : Controller
	{

		private readonly SheetService sheetService;

		public ModularSheetController(SheetService service) { sheetService = service; }

		public IActionResult DisplaySheetWithID(string id)
		{
			sheetService.CreateDnDSheet();
			ModularSheet sheet = sheetService.Get(id);
			return View("DisplaySheet", sheet);
		}

		public IActionResult DisplaySheet(ModularSheet sheet)
		{
			sheetService.CreateDnDSheet();
			return View(sheet);
		}

		public IActionResult Index()
		{
			string username = HttpContext.Session.GetString("username");
			if(username != null && username != "")
			{
				return View(sheetService.FilterByUser(username));
			}

			return RedirectToAction("Login", "Home");

		}

	}

	

}