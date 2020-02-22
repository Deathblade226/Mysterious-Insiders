using Mysterious_Insiders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mysterious_Insiders.Logic {
public class ChatCommands {

private static List<Command> Commands = new List<Command>() {
    new Command() {Format = "/r xdx", Method = 1},
    new Command() {Format = "/r xdx+x", Method = 1},
    new Command() {Format = "/r xdx-x", Method = 2},
    new Command() {Format = "/r (xdx)+x", Method = 1},
    new Command() {Format = "/r (xdx)-x", Method = 2},
    new Command() {Format = "/roll xdx", Method = 1},
    new Command() {Format = "/roll xdx+x", Method = 1},
    new Command() {Format = "/roll xdx-x", Method = 2},
    new Command() {Format = "/roll (xdx)+x", Method = 1},
    new Command() {Format = "/roll (xdx)-x", Method = 2},
    new Command() {Format = "/help", Method = 0}

};

private static string output = $" : ";

public static string CheckForCommand(string message) {
    output = " : ";
    if (message == null || message.Trim() == "") return message;
return (checkCommands(message.ToLower()) ? $"{message}{output}" : message);}

private static bool checkCommands(string message) { 
    string altered = Regex.Replace(message, "[0-9]{1,4}", "x");

    int method = -1;
    foreach(Command command in Commands) { if (altered == command.Format) { method = command.Method; break; } }

    switch (method) {
    case 0: HelpCommand(); break;
    case 1: RollDiceCommand(message); break; 
    case 2: RollDiceCommandNegMod(message); break; 
    }

return (method != -1);}

private static void HelpCommand() { 
    output = "[Help]: ";
    ChatWindow.Messages.Add(new UserMessage() { Name = "/help", Message = "This command shows all other commands."});
    ChatWindow.Messages.Add(new UserMessage() { Name = "/r xdx", Message = "This rolls x number of dice with x number of sides."});
    ChatWindow.Messages.Add(new UserMessage() { Name = "/roll xdx", Message = "This rolls x number of dice with x number of sides."});
    ChatWindow.Messages.Add(new UserMessage() { Name = "/r xdx+x", Message = "This rolls x number of dice with x number of sides and adds x to the sum of all the rolls."});
    ChatWindow.Messages.Add(new UserMessage() { Name = "/roll xdx+x", Message = "This rolls x number of dice with x number of sides and adds x to the sum of all the rolls."});
    ChatWindow.Messages.Add(new UserMessage() { Name = "/r (xdx)+x", Message = "This rolls x number of dice with x number of sides and adds x to each of the rolls."});
    ChatWindow.Messages.Add(new UserMessage() { Name = "/roll (xdx)+x", Message = "This rolls x number of dice with x number of sides and adds x to each of the rolls."});
}

private static void RollDiceCommand(string command) { 
    bool modAll = (command.Contains("/r (") && command.Contains(")+"));
    string[] numbers = Regex.Split(command, @"\D+");
    List<int> ints = new List<int>();

    foreach(string number in numbers) {
    if (number != "") { 
    int num = 0;
    int.TryParse(number, out num);
    ints.Add(num);
    }
    }

    List<int> rolls = (ints.Count == 2) ? Dice.RollDice(ints.ElementAt(0), ints.ElementAt(1), 0, modAll) : Dice.RollDice(ints.ElementAt(0), ints.ElementAt(1), ints.ElementAt(2), modAll);   
    output = " [Rolls]: ";
    for(int i = 0; i < rolls.Count(); i++) {

    if (i == rolls.Count()-1) { output += $"{rolls.ElementAt(i)} = {rolls.Sum()}";
    } else { output += $"{rolls.ElementAt(i)} + "; }

    }
}
private static void RollDiceCommandNegMod(string command) { 
    bool modAll = (command.Contains("/r (") && command.Contains(")+"));
    string[] numbers = Regex.Split(command, @"\D+");
    List<int> ints = new List<int>();

    foreach(string number in numbers) {
    if (number != "") { 
    int num = 0;
    int.TryParse(number, out num);
    ints.Add(num);
    }
    }

    List<int> rolls = (ints.Count == 2) ? Dice.RollDice(ints.ElementAt(0), ints.ElementAt(1), 0, modAll) : Dice.RollDice(ints.ElementAt(0), ints.ElementAt(1), -ints.ElementAt(2), modAll);   
    output = " [Rolls]: ";
    for(int i = 0; i < rolls.Count(); i++) {

    if (i == rolls.Count()-1) { output += $"{rolls.ElementAt(i)} = {rolls.Sum()}";
    } else { output += $"{rolls.ElementAt(i)} + "; }

    }
}

}

}
