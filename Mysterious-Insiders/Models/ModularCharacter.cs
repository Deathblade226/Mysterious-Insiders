using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mysterious_Insiders.Models
{
    public class ModularCharacter
    {
        private ModularSheet sheet;
        private List<ModuleBase> modules;

        public ModularSheet Sheet { get => sheet; 
            set 
            { 
                sheet = value; 
            } 
        }

        public IReadOnlyDictionary<string, ModuleBase> Modules { get => GetModules<ModuleBase>(); }

        public IReadOnlyDictionary<string, T> GetModules<T>() where T : ModuleBase
        {
            Dictionary<string, T> result = new Dictionary<string, T>();

            foreach(ModuleBase module in modules)
            {
                if (module is T) result.Add(module.Id, module as T);
            }

            return result;
        }
    }
}
