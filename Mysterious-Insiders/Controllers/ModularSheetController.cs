using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mysterious_Insiders.Models;
using Mysterious_Insiders.Services;

namespace Mysterious_Insiders.Controllers {

public class ModularSheetController : Controller {

private readonly SheetService sheetService;

public ModularSheetController(SheetService service) { sheetService = service; }

public IActionResult Index(string id = "5e572d68083f8b4924a2411f") {
    sheetService.CreateDnDSheet();
return View(sheetService.Get(id)); }

}

}