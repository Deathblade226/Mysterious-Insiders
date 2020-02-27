using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mysterious_Insiders.Models
{
    /// <summary>
    /// A module with a dropdown menu that the user can pick an option from.
    /// This is a subclass of ModuleNumeric, and its kind is always INTEGER,
    /// so it can be easily used by derivative modules. Its values start at
    /// 1, since users aren't always computer scientists. The value is based
    /// on which menu option is currently selected.
    /// </summary>
    public class ModuleMenu : ModuleNumeric
    {
        private string[] menuOptions;
        private int menuIndex;

        /// <summary>
        /// This module's menu options, for the user to choose from. This
        /// returns a copy, to make sure the options aren't edited.
        /// </summary>
        public string[] MenuOptions { get => (string[])menuOptions.Clone(); }

        /// <summary>
        /// The text to display in this module. This is actually the text of the
        /// currently selected menu option. If you try to set it, it tries to set
        /// the menu to an option that contains what you set it to (not case-sensitive.)
        /// </summary>
        public override string Text { 
            get
            {
                return menuOptions[menuIndex - 1];
            } 
            set
            {
                for (int i = 0; i < menuOptions.Length; i++)
                {
                    if (menuOptions[i].ToLower().Contains(value.ToLower()))
                    {
                        menuIndex = i + 1;
                        return;
                    }
                }
            } 
        }

        /// <summary>
        /// The number that is stored in this module. Since this is a menu, the number is the index of which menu option
        /// is currently selected.
        /// </summary>
        public override double Number { get => menuIndex; set => menuIndex = Math.Clamp((int)value, 1, menuOptions.Length); }

        /// <summary>
        /// The index of the menu option that the user has selected. Basically the same thing as Number, except as an int.
        /// </summary>
        public int MenuIndex { get => menuIndex; set => menuIndex = Math.Clamp((int)value, 1, menuOptions.Length); }

        /// <summary>
        /// Creates a new module using a ModuleData and the ModularCharacter that it's for.
        /// </summary>
        /// <param name="data">The ModuleData to use to make this module.</param>
        /// <param name="character">The ModularCharacter that this module is for.</param>
        /// <exception cref="ArgumentNullException">data or character is null.</exception>
        /// <exception cref="ArgumentException">data's type property isn't MENU.</exception>
        public ModuleMenu(ModuleData data, ModularCharacter character) : base(data, character, KindOfNumber.INTEGER, 1, data.SerializedLogic.Where(c => c == ',').Count() + 1)
        {
            if (data.ModuleType != ModuleData.moduleType.MENU) throw new ArgumentException("Cannot create a ModuleMenu object with ModuleData that has a type other than MENU.");
            menuOptions = data.SerializedLogic.Split(',');
            if (menuOptions.Length < 2) throw new ArgumentException("Cannot create a ModuleMenu object with an invalid logic string.");
        }
    }
}
