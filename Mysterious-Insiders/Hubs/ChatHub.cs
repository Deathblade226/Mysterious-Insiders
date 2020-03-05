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
    int msgCountB = ChatWindow.Messages.Count();
    if (message != null && !ChatCommands.CheckForCommand(message, name)) { ChatWindow.Messages.Add(new UserMessage() { Name = name, Message = message }); }
    
    int msgDiff = ChatWindow.Messages.Count() - msgCountB;
    
    if (msgCountB == 0 && message != "/?" && message != "/help") { 
    await Clients.All.SendAsync("ReceiveMessage", ChatWindow.Messages.Last().Name, ChatWindow.Messages.Last().Message, ChatWindow.MessageString(), (message.Contains("/wr")));
    } else { 
    for (int i = 0; i < msgDiff; i++) { 
    await Clients.All.SendAsync("ReceiveMessage", ChatWindow.Messages.ElementAt((msgCountB) + i).Name, ChatWindow.Messages.ElementAt((msgCountB) + i).Message, ChatWindow.MessageString());
    }
    }

}
//public static async Task StaticSendMessage(string name, string message) {

//}

}

}
