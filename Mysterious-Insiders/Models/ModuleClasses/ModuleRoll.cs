using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Mysterious_Insiders.Models.ModuleClasses
{
    /// <summary>
    /// A module that can be clicked to automatically roll dice in a chat window.
    /// It figures out which chat command to use and what values to use for it based
    /// on three numeric modules: one for the number of dice, one for the sides per
    /// die, and one for the bonus to add. The bonus can either be added to each die,
    /// or to the total.
    /// If you don't want the character input to affect one of the values, make a
    /// derivative module that doesn't have any derivative operations. That way, its
    /// number will be constant.
    /// </summary>
    public class ModuleRoll : ModuleBase
    {
        private ModuleNumeric diceCountSource, diceSidesSource, bonusSource;
        private bool bonusPerDie;

        /// <summary>
        /// The text to display in this module. Represents the roll that will occur.
        /// </summary>
        public override string Text { 
            get 
            {
                if (bonusPerDie)
                {
                    return diceCountSource.Text + "d(" + diceSidesSource.Text + " + " + bonusSource.Text + ")";
                } else
                {
                    return diceCountSource.Text + "d" + diceSidesSource.Text + " (+ " + bonusSource.Text + ")";
                }
            }
        }

        /// <summary>
        /// Creates a new module using a ModuleData and the ModularCharacter that it's for.
        /// </summary>
        /// <param name="data">The ModuleData to use to make this module.</param>
        /// <param name="character">The ModularCharacter that this module is for.</param>
        /// <exception cref="ArgumentNullException">data or character is null.</exception>
        /// <exception cref="ArgumentException">data's type property isn't ROLL, the logic is invalid, or one of the sources is null or non-integer.</exception>
        public ModuleRoll(ModuleData data, ModularCharacter character) : base(data, character, "")
        {
            if (data.ModuleType != ModuleData.moduleType.ROLL) throw new ArgumentException("Cannot create a ModuleRoll object with ModuleData that has a type other than ROLL.");
            string[] logic = data.SerializedLogic.Split(',');
            if (logic.Length != 4) throw new ArgumentException("Cannot create a ModuleRoll object with invalid logic.");
            diceCountSource = character.GetModules<ModuleNumeric>().Where(m => m.Key == logic[0]).First().Value;
            diceSidesSource = character.GetModules<ModuleNumeric>().Where(m => m.Key == logic[1]).First().Value;
            bonusSource = character.GetModules<ModuleNumeric>().Where(m => m.Key == logic[2]).First().Value;
            if (diceCountSource == null || diceSidesSource == null || bonusSource == null) throw new ArgumentException("Cannot create a ModuleRoll object with nonexistent sources.");
            if (diceCountSource.Kind != ModuleNumeric.KindOfNumber.INTEGER || diceSidesSource.Kind != ModuleNumeric.KindOfNumber.INTEGER || bonusSource.Kind != ModuleNumeric.KindOfNumber.INTEGER) 
                throw new ArgumentException("Cannot create a ModuleRoll object with non-Integer sources.");
            bonusPerDie = logic[3] == "per";
            diceCountSource.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(SourceModified);
            diceSidesSource.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(SourceModified);
            bonusSource.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(SourceModified);
        }

        /// <summary>
        /// Gets the chat command that should be entered when this module is clicked.
        /// </summary>
        /// <returns></returns>
        public string GetRollCommand()
        {
            if (bonusPerDie)
            {
                return "/r " + diceCountSource.Text + "d" + diceSidesSource.Text + "-" + bonusSource.Text;
            } else return "/r " + diceCountSource.Text + "d" + diceSidesSource.Text + "+" + bonusSource.Text;
        }

        private void SourceModified(object sender, PropertyChangedEventArgs e)
        {
            FieldChanged("Text");
        }
    }
}
