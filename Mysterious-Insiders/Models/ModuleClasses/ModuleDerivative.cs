using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mysterious_Insiders.Models
{
    /// <summary>
    /// A module that displays a number based on other modules. The displayed
    /// number is calculated using a series of DerivativeOperation objects that
    /// are stored by the module.
    /// </summary>
    public class ModuleDerivative : ModuleNumeric
    {
        /// <summary>
        /// These are used to make derivative modules work. Each one can modify the
        /// starting value, in turn, using its Calc method.
        /// </summary>
        public class DerivativeOperation
        {
            private ModuleBase reference;
            private char operation;
            private string logic;

            /// <summary>
            /// Creates a new derivative operation from a module and some logic.
            /// </summary>
            /// <param name="reference">The module to base the operation on, if any.</param>
            /// <param name="logic">The logic of the operation.</param>
            /// <exception cref="ArgumentException">logic is null, or the module is an invalid type.</exception>
            public DerivativeOperation(ModuleBase reference, string logic)
            {
                if (logic == null) throw new ArgumentException("Cannot make a DerivativeOperation based on null logic.");
                if (reference != null)
                {
                    switch (reference.ModuleType)
                    {
                        case ModuleData.moduleType.CHECK:
                        case ModuleData.moduleType.MENU:
                        case ModuleData.moduleType.NUMERIC:
                        case ModuleData.moduleType.DERIVATIVE:
                            break;
                        default:
                            throw new ArgumentException("Cannot make a DerivativeOperation based on a " + reference.ModuleType + " module.");
                    }
                    this.reference = reference;
                }
                this.logic = logic;
            }

            /// <summary>
            /// Creates a new derivative operation from some logic.
            /// </summary>
            /// <param name="logic">The logic of the operation.</param>
            /// <exception cref="ArgumentException">logic is null.</exception>
            public DerivativeOperation(string logic) : this(null, logic) {}

            /// <summary>
            /// Does this DerivativeOperation's calculation on a number.
            /// </summary>
            /// <param name="source">The original number.</param>
            /// <returns>The number after calculation.</returns>
            public double Calc(double source)
            {
                double val = source;

                if (reference == null)
                {
                    char oper = logic[0];
                    double mod = double.Parse(logic.Substring(1));
                    val = Operate(oper, source, mod);
                } else if (reference is ModuleCheck)
                {
                    ModuleCheck check = reference as ModuleCheck;
                    string[] logics = logic.Split(',');
                    if (check.IsChecked)
                    {
                        char oper = logics[0][0];
                        double mod = double.Parse(logics[0].Substring(1));
                        val = Operate(oper, source, mod);
                    } else
                    {
                        char oper = logics[1][0];
                        double mod = double.Parse(logics[1].Substring(1));
                        val = Operate(oper, source, mod);
                    }
                } else if (reference is ModuleMenu)
                {
                    ModuleMenu menu = reference as ModuleMenu;
                    string[] logics = logic.Split(',');
                    char oper = logics[menu.MenuIndex - 1][0];
                    double mod = double.Parse(logics[menu.MenuIndex - 1].Substring(1));
                    val = Operate(oper, source, mod);
                } else if (reference is ModuleNumeric)
                {
                    ModuleNumeric numeric = reference as ModuleNumeric;
                    char oper = logic[0];
                    double mod = numeric.Number;
                    val = Operate(oper, source, mod);
                } else
                {
                    throw new ArgumentException("Cannot make a DerivativeOperation based on a " + reference.ModuleType + " module.");
                }

                return val;
            }

            private double Operate(char oper, double source, double mod)
            {
                switch (oper)
                {
                    case '+':
                        return source + mod;
                    case '-':
                        return source - mod;
                    case '~':
                        return mod - source;
                    case '*':
                        return source * mod;
                    case '/':
                        return (mod == 0) ? 0 : source / mod;
                    case '|':
                        return (source == 0) ? mod / source : 0;
                    case '%':
                        return (mod == 0) ? 0 : source % mod;
                    case '^':
                        return Math.Pow(source, mod);
                    case '<':
                        return Math.Min(source, mod);
                    case '>':
                        return Math.Max(source, mod);
                    default:
                        return source;
                }
            }
        }

        private List<DerivativeOperation> operations = new List<DerivativeOperation>();
        private double startingValue;

        public ModuleDerivative(ModuleData data, ModularCharacter character) : base(data, character, ModuleNumeric.KindByChar(data.SerializedLogic[0]), double.MinValue, double.MaxValue)
        {
            if (data.ModuleType != ModuleData.moduleType.DERIVATIVE) throw new ArgumentException("Cannot create a ModuleNumeric object with ModuleData that has a type other than DERIVATIVE.");
            string[] logic = data.SerializedLogic.Split(';');
            startingValue = double.Parse(logic[0].Substring(1));
            for (int i = 1; i < logic.Length; i++)
            {

            }
        }

        private void Recalc()
        {

        }
    }
}
