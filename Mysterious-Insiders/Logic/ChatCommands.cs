using Mysterious_Insiders.Models;
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
    new Command() {Format = "cake!",            Method = 6}
};

private static string output = $" : ";

public static string CheckForCommand(string message) {
    output = " : ";
    if (message == null || message.Trim() == "") return message;
return (checkCommands(message.ToLower()) ? $"{message}{output}" : message);}

private static bool checkCommands(string message) { 
    string altered = Regex.Replace(message, "[0-9]{1,6}", "x");

    int method = -1;
    foreach(Command command in Commands) { if (altered == command.Format) { method = command.Method; break; } }

    switch (method) {
    case 0: HelpCommand(); break;
    case 1: RollDiceCommand(message); break; 
    case 2: RollDiceCommandNegMod(message); break; 
    case 3: RollStats(); break;
    case 4: BaseStats(); break;
    case 5: ClearChat(); break;
    case 6: Cake(); break;
    }

return (method != -1);}

private static void HelpCommand() { 
    output = " - This command shows all other commands.";
    ChatWindow.Messages.Add(new UserMessage() { Name = "/r xdx", Message = "This rolls x number of dice with x number of sides. (Max:999999)" });
    ChatWindow.Messages.Add(new UserMessage() { Name = "/roll xdx", Message = "This rolls x number of dice with x number of sides. (Max:999999)"});
    ChatWindow.Messages.Add(new UserMessage() { Name = "/r xdx+x", Message = "This rolls x number of dice with x number of sides and adds x to the sum of all the rolls. (Max:999999)" });
    ChatWindow.Messages.Add(new UserMessage() { Name = "/roll xdx+x", Message = "This rolls x number of dice with x number of sides and adds x to the sum of all the rolls. (Max:999999)" });
    ChatWindow.Messages.Add(new UserMessage() { Name = "/r (xdx)+x", Message = "This rolls x number of dice with x number of sides and adds x to each of the rolls. (Max:999999)" });
    ChatWindow.Messages.Add(new UserMessage() { Name = "/roll (xdx)+x", Message = "This rolls x number of dice with x number of sides and adds x to each of the rolls. (Max:999999)" });
    ChatWindow.Messages.Add(new UserMessage() { Name = "/rollstats", Message = "Rolls 6 sets of 4d6 an gives you the sum of the 3 highes rolls from each set."});
    ChatWindow.Messages.Add(new UserMessage() { Name = "/basestats", Message = "Returns the default set of stats."});
    //ChatWindow.Messages.Add(new UserMessage() { Name = "/clear", Message = "Clears the chat window."});
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

private static void BaseStats() { output = " [Base Stats]: 15, 14, 13, 12, 10, 8"; }

private static void RollStats() { 
    output = " [Stats]: ";

    for (int i = 0; i < 6; i++) { 

    IEnumerable<int> rolls = Dice.RollDice(4, 6);
    rolls = rolls.OrderByDescending(n => n);
    
    if (i == 0) output += $"{(rolls.Sum() - rolls.Last())}";
    else output += $", {(rolls.Sum() - rolls.Last())}";
            }

}

private static void ClearChat() { output = " : [Chat Cleared]"; ChatWindow.Messages.Clear(); }
private static void Cake() { output = " : It's a lie!"; }

}

}
