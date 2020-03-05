using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mysterious_Insiders.Models
{
    /// <summary>
    /// A character, made using a sheet, that the user can fill in with text and numbers.
    /// </summary>
    public class ModularCharacter
    {
        [BsonElement]
        private List<ModuleBase> modules;

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string DatabaseId { get; set; }

        /// <summary>
        /// The name of this ModularCharacter. This can be used to distinguish it from other ModularCharacters.
        /// You can think of this like the name of a saved file, whereas the character's actual name would be
        /// typed into one of its text modules.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The modules that are in this ModularCharacter. These are created based on the ModularSheet that
        /// is used to create this ModularCharacter, and they can't be edited directly.
        /// </summary>
        [BsonIgnore]
        public IReadOnlyDictionary<string, ModuleBase> Modules { get => GetModules<ModuleBase>(); }

        /// <summary>
        /// The ModularSheet that was used to create this ModularCharacter. It can't be modified.
        /// </summary>
        [BsonIgnore]
        public ModularSheet Sheet { get; }

        /// <summary>
        /// The id of this ModularCharacter's ModularSheet in the database, just in case it needs
        /// to be re-looked up for some reason.
        /// </summary>
        [BsonElement]
        public string SheetId { get => Sheet.DatabaseId; }

        /// <summary>
        /// Gets all of this ModularCharacter's modules of a particular subclass of ModuleBase in
        /// a dictionary based on their Ids, for easy lookup.
        /// </summary>
        /// <typeparam name="T">The class of modules to get. Must be a subclass of ModuleBase.</typeparam>
        /// <returns>A dictionary of modules.</returns>
        public IReadOnlyDictionary<string, T> GetModules<T>() where T : ModuleBase
        {
            Dictionary<string, T> result = new Dictionary<string, T>();

            foreach(ModuleBase module in modules)
            {
                if (module is T) result.Add(module.Id, module as T);
            }

            return result;
        }

        /// <summary>
        /// Creates a ModularCharacter based on the DatabaseId of a particular ModularSheet and a collection of 
        /// ModularSheets to look it up in. The sheet's ModuleData objects will be automatically read and used 
        /// to make actual modules for the character. Unlike ModuleData, modules can be filled in by the user.
        /// </summary>
        /// <param name="sheetid">The DatabaseId of the ModularSheet to look up.</param>
        /// <param name="sheets">The collection of sheets to look up the desired ModularSheet in.</param>
        public ModularCharacter(string sheetid, IEnumerable<ModularSheet> sheets) : this (sheets.Where(s => s.DatabaseId == sheetid).First()) {}

        /// <summary>
        /// Creates a ModularCharacter based on a ModularSheet. The sheet's ModuleData objects will be automatically
        /// read and used to make actual modules for the character. Unlike ModuleData, modules can be filled in by
        /// the user.
        /// </summary>
        /// <param name="sheet">The ModularSheet to base this ModularCharacter on.</param>
        public ModularCharacter(ModularSheet sheet)
        {
            if (sheet == null) throw new ArgumentNullException("Cannot create a ModularCharacter from a null ModularSheet.");
            this.Sheet = sheet;
            modules = new List<ModuleBase>();
            List<ModuleData> derivatives = new List<ModuleData>();
            List<ModuleData> rolls = new List<ModuleData>();
            foreach(var data in sheet.Modules)
            {
                switch (data.Value.ModuleType)
                {
                    case ModuleData.moduleType.NONE:
                        modules.Add(new ModuleBase(data.Value, this));
                        break;
                    case ModuleData.moduleType.CHECK:
                        modules.Add(new ModuleCheck(data.Value, this));
                        break;
                    case ModuleData.moduleType.TEXT:
                        modules.Add(new ModuleText(data.Value, this));
                        break;
                    case ModuleData.moduleType.MENU:
                        modules.Add(new ModuleMenu(data.Value, this));
                        break;
                    case ModuleData.moduleType.NUMERIC:
                        modules.Add(new ModuleNumeric(data.Value, this));
                        break;
                    case ModuleData.moduleType.DERIVATIVE:
                        derivatives.Add(data.Value);
                        break;
                    case ModuleData.moduleType.ROLL:
                        rolls.Add(data.Value);
                        break;
                }
            }
            foreach (var data in derivatives)
            {
                modules.Add(new ModuleDerivative(data, this));
            }
            foreach (var data in rolls)
            {
                modules.Add(new ModuleDerivative(data, this));
            }
        }
    }
}
