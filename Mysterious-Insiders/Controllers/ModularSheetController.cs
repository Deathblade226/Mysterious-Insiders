using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mysterious_Insiders.Models;
using Mysterious_Insiders.Services;
using Microsoft.AspNetCore.Http;


namespace Mysterious_Insiders.Controllers
{

	public class ModularSheetController : Controller
	{
		private static ModularSheet currentSheet;

		private readonly SheetService sheetService;

		public ModularSheetController(SheetService service) { sheetService = service; }

		public IActionResult DisplaySheetWithID(string id)
		{
			string username = HttpContext.Session.GetString("username");
			if (username != null && username != "")
			{
			//sheetService.CreateDnDSheet(username, "PlaceHolder");
			ModularSheet sheet = sheetService.Get(id);
				currentSheet = sheet;
			return View("DisplaySheet", sheet);
			}
			return RedirectToAction("Login", "Home");
		}

		public IActionResult DisplaySheet()
		{
			string username = HttpContext.Session.GetString("username");
			if (username != null && username != "")
			{
				//sheetService.CreateDnDSheet(username, "PlaceHolder");
			return View(currentSheet);
			}

			return RedirectToAction("Login", "Home");

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
		public IActionResult AddSheet()
		{
			string username = HttpContext.Session.GetString("username");
			if (username != null && username != "")
			{
				
			sheetService.CreateDnDSheet(username, "New Sheet");
			return RedirectToAction("Index", "ModularSheet");
			}

			return RedirectToAction("Login", "Home");			

		}

		public IActionResult Update(string id) {
			string username = HttpContext.Session.GetString("username");
			if (username != null && username != "")
			{
				return View(model:id);
			}

			return RedirectToAction("Login", "Home");
		}

		public IActionResult Rename(string id, string newName) { 

		var holder = sheetService.Get(id);
		holder.Name = newName;
		sheetService.Update(id, holder);
		return RedirectToAction("Index", "ModularSheet");

		}

		public IActionResult Delete(string id) { 
			string username = HttpContext.Session.GetString("username");
			if (username != null && username != "")
			{
				
			sheetService.Remove(sheetService.Get(id));
			return RedirectToAction("Index", "ModularSheet");
			}

			return RedirectToAction("Login", "Home");	
		}

		public IActionResult EditModule(string moduleID, string data)
		{
			ModuleData module = null;
			

			if(currentSheet.Modules.TryGetValue(moduleID, out module))
			{
				module.Data = data;
				sheetService.Update(currentSheet.DatabaseId, currentSheet);
			}
		return RedirectToAction("DisplaySheet");
		}
	}
}