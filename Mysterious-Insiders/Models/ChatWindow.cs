using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mysterious_Insiders.Models {
public class ChatWindow : IMessageDAL {

public static List<UserMessage> Messages = new List<UserMessage>();
public void AddMessage(UserMessage message) { Messages.Add(message); }
public void RemoveMessage(UserMessage message) { Messages.Add(message); }
public IEnumerable<UserMessage> GetMessages() { return Messages; }

public static string MessageString() { 
    string messages = "";
    foreach(UserMessage message in Messages) { 
    messages += $"{message.ToString()}\n";        
    }
return messages;}

}

}
