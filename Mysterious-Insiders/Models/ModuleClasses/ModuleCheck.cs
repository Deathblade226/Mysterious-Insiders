using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mysterious_Insiders.Models
{
    /// <summary>
    /// A module that contains a checkbox or a radio button. Has a list of other check modules that
    /// will be deactivated if this one is activated, to make it work like a radio button. If this
    /// list is empty, it's a regular check box.
    /// </summary>
    public class ModuleCheck : ModuleBase
    {
        private List<string> exclusives;
        private bool isChecked = false;

        /// <summary>
        /// Whether this module is checked or not. If this module is a radio button, setting this
        /// to true will set the other radio button's it's grouped with to false.
        /// </summary>
        public bool IsChecked { get => isChecked; 
            set
            {
                isChecked = value;
                if (isChecked && IsRadioButton)
                {
                    IReadOnlyDictionary<string, ModuleCheck> modules = this.Character.GetModules<ModuleCheck>();
                    foreach(string exclusive in exclusives)
                    {
                        if (modules.TryGetValue(exclusive, out ModuleCheck module))
                        {
                            module.IsChecked = false;
                        }
                    }
                }
                FieldChanged();
            }
        }

        /// <summary>
        /// Whether any other ModuleCheck objects should uncheck themselves when this one is checked.
        /// </summary>
        public bool IsRadioButton { get => exclusives.Count > 0; }

        /// <summary>
        /// Creates a new module using a ModuleData and the ModularCharacter that it's for.
        /// </summary>
        /// <param name="data">The ModuleData to use to make this module.</param>
        /// <param name="character">The ModularCharacter that this module is for.</param>
        /// <exception cref="ArgumentNullException">data or character is null.</exception>
        /// <exception cref="ArgumentException">data's type property isn't CHECK.</exception>
        public ModuleCheck(ModuleData data, ModularCharacter character) : base(data, character, "")
        {
            if (data.ModuleType != ModuleData.moduleType.CHECK) throw new ArgumentException("Cannot create a ModuleCheck object with ModuleData that has a type other than CHECK.");
            if (data.SerializedLogic.Length > 0) exclusives = new List<string>(data.SerializedLogic.Split(','));
        }


    }
}
