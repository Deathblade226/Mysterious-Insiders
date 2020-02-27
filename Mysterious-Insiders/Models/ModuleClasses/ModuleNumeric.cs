using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mysterious_Insiders.Models
{
    /// <summary>
    /// A module that the user can type a number into. Can have a minimum and
    /// a maximum, and can be constrained as an integer, percentage, or decimal.
    /// </summary>
    public class ModuleNumeric : ModuleBase
    {
        /// <summary>
        /// Represents a kind of number that can be stored in a Numeric module.
        /// Behind the scenes, it's always a double. Integers have their decimal
        /// places trimmed off, and percentages will be multiplied by 100 before
        /// being displayed (so that 1.0 is 100%.)
        /// </summary>
        public enum KindOfNumber
        {
            INTEGER,
            PERCENT,
            DECIMAL
        }

        private double min = double.MinValue;
        private double max = double.MaxValue;
        private double number;
        private KindOfNumber kind = KindOfNumber.DECIMAL;

        /// <summary>
        /// The number that is stored in this module.
        /// </summary>
        public virtual double Number { get => number; 
            set
            {
                if (kind == KindOfNumber.INTEGER)
                {
                    number = (int)Math.Clamp(value, min, max);
                    if (number < min) number++;
                }
                else number = Math.Clamp(value, min, max);
            }
        }

        /// <summary>
        /// The text to display in this module. This can be edited by the user, and it
        /// converts to the number on its own, where possible.
        /// </summary>
        public override string Text
        {
            get 
            {
                if (kind == KindOfNumber.INTEGER)
                {
                    return ((int)number).ToString();
                }
                else if (kind == KindOfNumber.PERCENT)
                {
                    return (number * 100) + "%";
                }
                else return number.ToString();
            }
            set
            {
                if (kind == KindOfNumber.INTEGER)
                {
                    if (int.TryParse(value, out int result))
                    {
                        Number = result;
                    }
                }
                else if (kind == KindOfNumber.PERCENT)
                {
                    if (value.IndexOf('%') > 0)
                    {
                        if (double.TryParse(value.Substring(0, value.IndexOf('%')), out double result))
                        {
                            Number = result / 100;
                        }
                    } else
                    {
                        if (double.TryParse(value, out double result))
                        {
                            Number = result / 100;
                        }
                    }
                    
                }
                else if (double.TryParse(value, out double result))
                {
                    Number = result;
                }
            }
        }

        /// <summary>
        /// The kind of number that can be stored in this module.
        /// </summary>
        public KindOfNumber Kind { get => kind; }

        /// <summary>
        /// Creates a new module using a ModuleData and the ModularCharacter that it's for.
        /// </summary>
        /// <param name="data">The ModuleData to use to make this module.</param>
        /// <param name="character">The ModularCharacter that this module is for.</param>
        /// <exception cref="ArgumentNullException">data or character is null.</exception>
        /// <exception cref="ArgumentException">data's type property isn't NUMERIC, or the logic string is invalid.</exception>
        public ModuleNumeric(ModuleData data, ModularCharacter character) : base(data, character, "")
        {
            if (data.ModuleType != ModuleData.moduleType.NUMERIC) throw new ArgumentException("Cannot create a ModuleNumeric object with ModuleData that has a type other than NUMERIC.");
            string[] logic = data.SerializedLogic.Split(',');
            kind = KindByChar(logic[0][0]);
            if (logic.Length > 1) double.TryParse(logic[1], out min);
            if (logic.Length > 2) double.TryParse(logic[2], out max);
        }

        /// <summary>
        /// For internal use only, to let subclasses bypass the deserialization.
        /// </summary>
        protected ModuleNumeric(ModuleData data, ModularCharacter character, KindOfNumber kind, double min, double max) : base(data, character, "")
        {
            this.kind = kind;
            this.min = min;
            this.max = max;
        }

        protected static KindOfNumber KindByChar(char c)
        {
            switch (c)
            {
                case 'I':
                    return KindOfNumber.INTEGER;
                case 'P':
                    return KindOfNumber.PERCENT;
                case 'D':
                    return KindOfNumber.DECIMAL;
                default:
                    throw new ArgumentException("Cannot create a ModuleNumeric object with an invalid logic string.");
            }
        }
    }
}
