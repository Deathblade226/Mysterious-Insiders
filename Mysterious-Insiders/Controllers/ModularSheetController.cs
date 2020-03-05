using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mysterious_Insiders.Services;

namespace Mysterious_Insiders.Controllers {

public class ModularSheetController : Controller {

private readonly SheetService sheetService;

public ModularSheetController(SheetService service) { sheetService = service; }

public IActionResult Index() {
return View(sheetService.Get().First()); }

}

}