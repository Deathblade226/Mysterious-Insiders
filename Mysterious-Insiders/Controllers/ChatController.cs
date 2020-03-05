using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Mysterious_Insiders.Models;
using Mysterious_Insiders.Logic;

namespace Mysterious_Insiders.Controllers {
public class ChatController : Controller {

private readonly IMessageDAL MessageDB;

public ChatController(IMessageDAL input) : base() {
    MessageDB = input;
}

//[Route("chat/Chattest")] 
public IActionResult Index() {
    string name = HttpContext.Session.GetString("username");
    if (name == "" || name == null) name = "User";
    ViewBag.Name = name;
    return View(MessageDB.GetMessages());
}

}

}