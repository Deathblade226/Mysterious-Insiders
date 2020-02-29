using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Mysterious_Insiders.Models;
using Mysterious_Insiders.Logic;

namespace Mysterious_Insiders.Hubs {
public class ChatHub : Hub {

public async Task SendMessage(string name, string message) {

    if (message != null && !ChatCommands.CheckForCommand(message, name)) { ChatWindow.Messages.Add(new UserMessage() { Name = name, Message = message }); }
   
    await Clients.All.SendAsync("ReceiveMessage", ChatWindow.Messages.Last().Name, ChatWindow.Messages.Last().Message, ChatWindow.MessageString());
}

}

}
