using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mysterious_Insiders.Logic {
public class Dice {

/// <summary>
/// Generates random dice based on input data.
/// </summary>
/// <param name="number">The number of dice to roll</param>
/// <param name="sides">The number of sides the dice has</param>
/// <param name="mod">The mod that will be added to the roll</param>
/// <param name="ModOnAllRolls">Should the mod be added to all the rolls. The mod wont be used if false.</param>
/// <returns></returns>
public static List<int> RollDice(int number = 1, int sides = 20, int mod = 0, bool ModOnAllRolls = true) { 
    Random rng = new Random();
    List<int> rolls = new List<int>();
    if (ModOnAllRolls) for (int i = 0; i < number; i++) { rolls.Add(rng.Next(1, sides + 1) + mod); }
    else { for (int i = 0; i < number; i++) { rolls.Add(rng.Next(1, sides + 1)); } rolls.Add(mod); }
return rolls;}

}

}
