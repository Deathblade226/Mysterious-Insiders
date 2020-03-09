using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Mysterious_Insiders.Models
{
    /// <summary>
    /// The data used to lay out a character sheet. Contains a Dictionary of ModuleData objects,
    /// which are stored by their Ids for easy lookup. 
    /// </summary>
    public class ModularSheet
    {
        [BsonElement]
        private List<ModuleData> modules;

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string DatabaseId { get; set; }

        /// <summary>
        /// If true, this sheet can be searched by users other than its creator. Other users can't edit
        /// it, though. Only the user has editing priveleges.
        /// </summary>
        public bool Public { get; set; }

        /// <summary>
        /// The name of the user that created this ModularSheet (and can edit it.)
        /// </summary>
        public string UserOwner { get; set; }

        /// <summary>
        /// The Name of this ModularSheet. This can be used to distinguish it from other ModularSheets.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The Date that this ModularSheet was created. It should set itself by default, but it can be
        /// modified directly if need be.
        /// </summary>
        [BsonElement] 
        public DateTime Date { get; set; }

        /// <summary>
        /// The Dictionary of ModuleData objects that are placed on this ModularSheet. Since its data
        /// must be stored by its Id, it can't be modified directly. Use the AddModuleData and
        /// RemoveModuleData methods instead.
        /// </summary>
        [BsonIgnore]
        public IReadOnlyDictionary<string, ModuleData> Modules { 
            get {
                Dictionary<string, ModuleData> mods = new Dictionary<string, ModuleData>();
                foreach(ModuleData mod in modules)
                {
                    mods.Add(mod.Id, mod);
                }
                return mods;
            } 
        }

        /// <summary>
        /// Creates a ModularSheet with an empty Dictionary of ModuleData.
        /// </summary>
        public ModularSheet()
        {
            modules = new List<ModuleData>();
            Date = DateTime.Now;
        }

        /// <summary>
        /// Adds the given ModuleData to this ModularSheet's Dictionary.
        /// </summary>
        /// <param name="module">The ModuleData to add.</param>
        /// <exception cref="ArgumentNullException">module is null.</exception>
        /// <exception cref="ArgumentException">module.Id is null, blank, or already used.</exception>
        public void AddModuleData(ModuleData module)
        {
            if (module == null)
            {
                throw new ArgumentNullException("Cannot add a null ModuleData to a ModularSheet.");
            }
            if (module.Id == null)
            {
                throw new ArgumentException("Cannot add a ModuleData with a null Id to a ModularSheet.");
            }
            if (module.Id.Trim().Length == 0)
            {
                throw new ArgumentException("Cannot add a ModuleData with a blank Id to a ModularSheet.");
            }
            if (modules.Where(m => m.Id == module.Id).Count() > 0)
            {
                throw new ArgumentException("The ModularSheet already has a Module with Id " + module.Id + ". Cannot add another.");
            }
            modules.Add(module);
        }

        /// <summary>
        /// Removes the ModuleData with the given Id from this ModularSheet's Dictionary.
        /// </summary>
        /// <param name="moduleId">The Id of the ModuleData to remove.</param>
        /// <exception cref="ArgumentNullException">moduleId is null.</exception>
        /// <exception cref="ArgumentException">moduleId is blank, or not found in the Dictionary.</exception>
        public void RemoveModuleData(string moduleId)
        {
            if (moduleId == null)
            {
                throw new ArgumentNullException("Cannot remove ModuleData from a ModularSheet based on a null string.");
            }
            if (moduleId.Trim().Length == 0)
            {
                throw new ArgumentException("Cannot remove a ModuleData from a ModularSheet based on a blank string.");
            }
            ModuleData module = modules.Find(m => m.Id == moduleId);
            if (module == null)
            {
                throw new ArgumentException("No ModuleData found with that Id. Cannot remove a ModuleData that doesn't exist in this ModularSheet.");
            }
            modules.Remove(module);
        }

        /// <summary>
        /// Removes the given ModuleData, based on its Id, from this ModularSheet's Dictionary.
        /// </summary>
        /// <param name="module">The ModuleData to remove.</param>
        /// <exception cref="ArgumentNullException">module is null.</exception>
        /// <exception cref="ArgumentException">module.Id is null, blank, or not found in the Dictionary.</exception>
        public void RemoveModuleData(ModuleData module)
        {
            if (module == null)
            {
                throw new ArgumentNullException("Cannot remove a null ModuleData from a ModularSheet.");
            }
            if (module.Id == null)
            {
                throw new ArgumentException("Cannot remove a ModuleData with a null Id from a ModularSheet.");
            }
            if (module.Id.Trim().Length == 0)
            {
                throw new ArgumentException("Cannot remove a ModuleData with a blank Id from a ModularSheet.");
            }
            RemoveModuleData(module.Id);
        }
    }
}
