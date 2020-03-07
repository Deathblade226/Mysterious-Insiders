using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mysterious_Insiders.Models;
using Mysterious_Insiders.Services;

namespace Mysterious_Insiders.Controllers {

public class ModularSheetController : Controller {

private readonly SheetService sheetService;

public ModularSheetController(SheetService service) { sheetService = service; }

public IActionResult Index(string id = "5e572d68083f8b4924a2411f") {
    string user = HttpContext.Session.GetString("username");
    //sheetService.CreateDnDSheet("andrew", "This is a test");
return View(sheetService.Get(id)); }

}

}