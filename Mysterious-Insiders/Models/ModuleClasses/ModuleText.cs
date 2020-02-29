using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mysterious_Insiders.Models
{
    /// <summary>
    /// A module that the user can type text into. Can have a maximum length and
    /// a limited number of lines for word wrapping. By default it will have no
    /// maximum length but only 1 line.
    /// </summary>
    public class ModuleText : ModuleBase
    {
        private int maximumLength;
        private int numberOfLines;

        /// <summary>
        /// The text to display in this module. This can be edited by the user.
        /// </summary>
        public override string Text
        {
            get => base.Text;
            set
            {
                if (value.Length > MaximumLength)
                {
                    base.Text = value.Substring(0, MaximumLength);
                }
                else base.Text = value;
                FieldChanged();
            }
        }

        /// <summary>
        /// The maximum character length of this module's text.
        /// </summary>
        public int MaximumLength { get => maximumLength; }

        /// <summary>
        /// The maximum number of lines that this module's text will word wrap into.
        /// </summary>
        public int NumberOfLines { get => numberOfLines; }

        /// <summary>
        /// Creates a new module using a ModuleData and the ModularCharacter that it's for.
        /// </summary>
        /// <param name="data">The ModuleData to use to make this module.</param>
        /// <param name="character">The ModularCharacter that this module is for.</param>
        /// <exception cref="ArgumentNullException">data or character is null.</exception>
        /// <exception cref="ArgumentException">data's type property isn't CHECK.</exception>
        public ModuleText(ModuleData data, ModularCharacter character) : base(data, character, "")
        {
            if (data.ModuleType != ModuleData.moduleType.TEXT) throw new ArgumentException("Cannot create a ModuleCheck object with ModuleData that has a type other than TEXT.");
            string[] logic = data.SerializedLogic.Split(',');
            if (logic.Length != 2) throw new ArgumentException("Cannot create a ModuleText object with an invalid logic string.");
            int.TryParse(logic[0], out maximumLength);
            int.TryParse(logic[1], out numberOfLines);
        }
    }
}
