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
        private List<ModuleBase> modules;
        private ModularSheet sheet;

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string DatabaseId { get; set; }

        public string Name { get; set; }

        public IReadOnlyDictionary<string, ModuleBase> Modules { get => GetModules<ModuleBase>(); }

        public ModularSheet Sheet { get => sheet; }

        public IReadOnlyDictionary<string, T> GetModules<T>() where T : ModuleBase
        {
            Dictionary<string, T> result = new Dictionary<string, T>();

            foreach(ModuleBase module in modules)
            {
                if (module is T) result.Add(module.Id, module as T);
            }

            return result;
        }

        public ModularCharacter(ModularSheet sheet)
        {
            this.sheet = sheet;
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
