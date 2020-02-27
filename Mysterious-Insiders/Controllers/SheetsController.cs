using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mysterious_Insiders.Services;
using Mysterious_Insiders.Models;

namespace Mysterious_Insiders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SheetsController : ControllerBase
    {
        private readonly SheetService sheetService;

        public SheetsController(SheetService service)
        {
            sheetService = service;
        }

        [HttpGet]
        public ActionResult<List<ModularSheet>> Get()
        {
            return sheetService.Get();
        }

        [HttpGet("{id:length(24)}", Name = "GetSheet")]
        public ActionResult<ModularSheet> Get(string id)
        {
            var sheet = sheetService.Get(id);

            if(sheet == null)
            {
                return NotFound();
            }

            return sheet;
        }

        [HttpPost]
        public ActionResult<ModularSheet> Create(ModularSheet sheet)
        {
            sheetService.Create(sheet);

            return CreatedAtRoute("GetSheet", new { id = sheet.DatabaseId.ToString() }, sheet);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, ModularSheet sheetIn)
        {
            var sheet = sheetService.Get(id);

            if (sheet == null)
            {
                return NotFound();
            }

            sheetService.Update(id, sheetIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var sheet = sheetService.Get(id);

            if (sheet == null)
            {
                return NotFound();
            }

            sheetService.Remove(sheet.DatabaseId);

            return NoContent();
        }

    }
}