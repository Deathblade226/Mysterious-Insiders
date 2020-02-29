using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Mysterious_Insiders.Models
{
    /// <summary>
    /// A module that displays a number based on other modules. The displayed
    /// number is calculated using a series of DerivativeOperation objects that
    /// are stored by the module. Most DerivativeOperation objects use a module
    /// in their calculations, and that module must ALREADY EXIST in the
    /// ModularCharacter to be used in one of these derivative operations.
    /// Order matters!
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
            /// Creates a new derivative operation from an operator and a double.
            /// </summary>
            /// <param name="oper">The operator.</param>
            /// <param name="val">The double.</param>
            public DerivativeOperation(char oper, double val) : this(null, oper + "" + val) { }

            /// <summary>
            /// Creates a new derivative operation from a check module, an operator, and a double.
            /// The operation will only happen when the module is checked.
            /// </summary>
            /// <param name="reference">The check module.</param>
            /// <param name="oper1">The operator.</param>
            /// <param name="val1">The double.</param>
            public DerivativeOperation(ModuleCheck reference, char oper1, double val1)
            {
                this.reference = reference;
                this.logic = oper1 + "" + val1;
            }

            /// <summary>
            /// Creates a new derivative operation from a check module and two operator/double pairs.
            /// The first operator and double will happen when the module is checked. The second
            /// operator and double will happen when it isn't.
            /// </summary>
            /// <param name="reference">The check module.</param>
            /// <param name="oper1">The first operator.</param>
            /// <param name="val1">The first double.</param>
            /// <param name="oper2">The second operator.</param>
            /// <param name="val2">The second double.</param>
            public DerivativeOperation(ModuleCheck reference, char oper1, double val1, char oper2, double val2)
            {
                this.reference = reference;
                this.logic = oper1 + "" + val1 + "," + oper2 + "" + val2;
            }

            /// <summary>
            /// Creates a new derivative operation from a menu module and one operator/double KeyValuePair for
            /// every option in the menu. The operator and double used will be based on which menu option is
            /// picked.
            /// </summary>
            /// <param name="reference">The menu module.</param>
            /// <param name="operations">The KeyValuePairs of operators and doubles.</param>
            public DerivativeOperation(ModuleMenu reference, params KeyValuePair<char, double>[] operations)
            {
                if (reference.MenuOptions.Length != operations.Length) throw new ArgumentException("Can only make a derivative operation based on a MenuModule if there's a KeyValuePair for every option.");
                this.reference = reference;
                logic = operations[0].Key + "" + operations[0].Value;
                for (int i = 1; i < operations.Length; i++)
                {
                    logic += "," + operations[i].Key + "" + operations[i].Value;
                }
            }

            /// <summary>
            /// Creates a new derivative operation from a numeric or derivative module and an operator.
            /// The operator will be used with the module's number value, instead of a built-in value.
            /// </summary>
            /// <param name="reference">The module.</param>
            /// <param name="oper">The operator.</param>
            public DerivativeOperation(ModuleNumeric reference, char oper)
            {
                if (reference.ModuleType != ModuleData.moduleType.NUMERIC && reference.ModuleType != ModuleData.moduleType.DERIVATIVE)
                    throw new ArgumentException("This subclass of ModuleNumeric can't be made into a derivative operation this way. It only works with NUMERIC and DERIVATIVE.");
                this.reference = reference;
                logic = oper + "";
            }

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
                    } else if (logics.Length > 1)
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
                    throw new ArgumentException("Cannot calculate a DerivativeOperation based on a " + reference.ModuleType + " module.");
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

            public override string ToString()
            {
                return reference.Id + ";" + logic;
            }
        }

        private List<DerivativeOperation> operations = new List<DerivativeOperation>();
        private double startingValue;
        private double currentValue;

        /// <summary>
        /// The number that is calculated by this module. It can't be modified directly.
        /// </summary>
        public override double Number { get => currentValue; }

        /// <summary>
        /// Creates a new module using a ModuleData and the ModularCharacter that it's for.
        /// </summary>
        /// <param name="data">The ModuleData to use to make this module.</param>
        /// <param name="character">The ModularCharacter that this module is for.</param>
        /// <exception cref="ArgumentNullException">data or character is null.</exception>
        /// <exception cref="ArgumentException">data's type property isn't DERIVATIVE, or one of the DerivativeProperty objects can't be created.</exception>
        public ModuleDerivative(ModuleData data, ModularCharacter character) : base(data, character, KindByChar(data.SerializedLogic[0]), double.MinValue, double.MaxValue)
        {
            if (data.ModuleType != ModuleData.moduleType.DERIVATIVE) throw new ArgumentException("Cannot create a ModuleNumeric object with ModuleData that has a type other than DERIVATIVE.");
            string[] logic = data.SerializedLogic.Split(';');
            startingValue = double.Parse(logic[0].Substring(1));
            for (int i = 1; i < logic.Length; i+=2)
            {
                ModuleBase module = (logic[i] == "") ? null : character.GetModules<ModuleBase>().Where(m => m.Key == logic[i]).First().Value;
                module.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(SourceModified);
                operations.Add(new DerivativeOperation(module, logic[i + 1]));
            }
            Recalc();
        }

        private void SourceModified(object sender, PropertyChangedEventArgs e)
        {
            Recalc();
        }

        private void Recalc()
        {
            currentValue = startingValue;
            for (int i = 0; i < operations.Count; i++)
            {
                currentValue = operations[i].Calc(currentValue);
            }
            FieldChanged("Number");
            FieldChanged("Text");
        }
    }
}
