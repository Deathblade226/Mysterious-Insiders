using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mysterious_Insiders.Logic {
public class Dice {

public static List<int> RollDice(int number = 1, int sides = 20, int mod = 0, bool ModOnAllRolls = true) { 
    Random rng = new Random();
    List<int> rolls = new List<int>();
    if (ModOnAllRolls) for (int i = 0; i < number; i++) { rolls.Add(rng.Next(1, sides + 1) + mod); }
    else for (int i = 0; i < number; i++) { rolls.Add(rng.Next(1, sides + 1)); }
return rolls;}

}

}
