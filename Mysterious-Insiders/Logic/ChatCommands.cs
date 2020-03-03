﻿using Mysterious_Insiders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mysterious_Insiders.Logic {
public class ChatCommands {

//To add a command, Create a new command and set its format to how you want the command to be entered. Numbers will be changed into one x. Ex, 123 = x.
//The method is an int that is passed through a switch to do the logic for the command. This makes it so the command is modular and we dont need an item for each method.
private static List<Command> Commands = new List<Command>() { 
    new Command() {Format = "/help",            Method = 0},
    new Command() {Format = "/?",               Method = 0},
    new Command() {Format = "/r xdx",           Method = 1},
    new Command() {Format = "/r xdx+x",         Method = 1},
    new Command() {Format = "/r (xdx)+x",       Method = 1},
    new Command() {Format = "/roll xdx",        Method = 1},
    new Command() {Format = "/roll xdx+x",      Method = 1},
    new Command() {Format = "/roll (xdx)+x",    Method = 1},
    new Command() {Format = "/r xdx-x",         Method = 2},
    new Command() {Format = "/r (xdx)-x",       Method = 2},
    new Command() {Format = "/roll xdx-x",      Method = 2},
    new Command() {Format = "/roll (xdx)-x",    Method = 2},
    new Command() {Format = "/rollstats",       Method = 3},
    new Command() {Format = "/basestats",       Method = 4},
    //new Command() {Format = "/clear",           Method = 5}, 
    new Command() {Format = "cake!",            Method = 6},
    new Command() {Format = "/wr",          Method = 7}
};

private static string username = "User";

public static bool CheckForCommand(string message, string name) { //Change this to a bool. Make it so if the person passes in a command return false then have this print out the message.
    username = name;
return checkCommands(message.ToLower()); }

private static bool checkCommands(string message) { 
    string altered = Regex.Replace(message, "[0-9]{1,6}", "x");
    if (message.Contains("/wr ") && altered.Length > 4 && altered.Length < 11) altered = "/wr";
    
    int method = -1;
    foreach(Command command in Commands) { if (altered == command.Format) { method = command.Method; break; } }

    switch (method) {
    case 0: HelpCommand(); break;
    case 1: RollDiceCommand(message); break; 
    case 2: RollDiceCommandNegMod(message); break; 
    case 3: RollStats(message); break;
    case 4: BaseStats(message); break;
    case 5: ClearChat(); break;
    case 6: Cake(message); break;
    case 7: WhisperRoll(message); break;
    }

return (method != -1);}

private static void HelpCommand() { 
    ChatWindow.Messages.Add(new UserMessage() { Name = "/r xdx", Message = "This rolls x number of dice with x number of sides. (Max:999999)" });
    ChatWindow.Messages.Add(new UserMessage() { Name = "/roll xdx", Message = "This rolls x number of dice with x number of sides. (Max:999999)"});
    ChatWindow.Messages.Add(new UserMessage() { Name = "/r xdx+x", Message = "This rolls x number of dice with x number of sides and adds x to the sum of all the rolls. (Max:999999)" });
    ChatWindow.Messages.Add(new UserMessage() { Name = "/roll xdx+x", Message = "This rolls x number of dice with x number of sides and adds x to the sum of all the rolls. (Max:999999)" });
    ChatWindow.Messages.Add(new UserMessage() { Name = "/r (xdx)+x", Message = "This rolls x number of dice with x number of sides and adds x to each of the rolls. (Max:999999)" });
    ChatWindow.Messages.Add(new UserMessage() { Name = "/roll (xdx)+x", Message = "This rolls x number of dice with x number of sides and adds x to each of the rolls. (Max:999999)" });
    ChatWindow.Messages.Add(new UserMessage() { Name = "/rollstats", Message = "Rolls 6 sets of 4d6 an gives you the sum of the 3 highes rolls from each set."});
    ChatWindow.Messages.Add(new UserMessage() { Name = "/basestats", Message = "Returns the default set of stats."});
    ChatWindow.Messages.Add(new UserMessage() { Name = "/help", Message = "Shows all function commands."});
    ChatWindow.Messages.Add(new UserMessage() { Name = "/?", Message = "Shows all function commands."});
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
    string output = $"{command}: ";
    for(int i = 0; i < rolls.Count(); i++) {

    if (i == rolls.Count()-1) { output += $"{rolls.ElementAt(i)} = {rolls.Sum()}";
    } else { output += $"{rolls.ElementAt(i)} + "; }

    }

    ChatWindow.Messages.Add(new UserMessage() { Name = username, Message = output});
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
    string output = $"{command}: ";
    for(int i = 0; i < rolls.Count(); i++) {

    if (i == rolls.Count()-1) { output += $"{rolls.ElementAt(i)} = {rolls.Sum()}";
    } else { output += $"{rolls.ElementAt(i)} + "; }

    }
    ChatWindow.Messages.Add(new UserMessage() { Name = username, Message = output});
}
private static void BaseStats(string command) { 
    ChatWindow.Messages.Add(new UserMessage() { Name = username, Message = $"{command}: 15, 14, 13, 12, 10, 8" });
}
private static void RollStats(string command) { 
    string output = $"{command}: ";

    for (int i = 0; i < 6; i++) { 

    IEnumerable<int> rolls = Dice.RollDice(4, 6);
    rolls = rolls.OrderByDescending(n => n);
    
    if (i == 0) output += $"{(rolls.Sum() - rolls.Last())}";
    else output += $", {(rolls.Sum() - rolls.Last())}";
    }
    ChatWindow.Messages.Add(new UserMessage() { Name = username, Message = output });
}
private static void ClearChat() { 
    ChatWindow.Messages.Clear(); 
}
private static void Cake(string command) { 
    ChatWindow.Messages.Add(new UserMessage() { Name = username, Message = $"{command} It's a lie!" });
}
private static void WhisperRoll(string command) { 

}

}

}
