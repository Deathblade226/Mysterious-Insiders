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
    /// which are stored by their Ids for easy lookup. Also contains a collection of strings
    /// that are the URLs of images used by the sheet (for the backgrounds of modules) which should
    /// not be modified directly. Instead, use the AddImage and RemoveImage methods, which adjust
    /// the BgImageIndex properties of the contained ModuleData objects automatically.
    /// </summary>
    public class ModularSheet
    {
        [BsonElement]
        private List<ModuleData> modules;
        [BsonElement]
        private List<string> imageUrls;

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string DatabaseId { get; set; }

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
        /// The List of string urls of images that this ModularSheet will use. Since a url's index
        /// in this list is relevant for the sheet's ModuleData objects, it can't be modified directly.
        /// Use the AddImageUrl and RemoveImageUrl methods instead.
        /// </summary>
        [BsonIgnore]
        public IReadOnlyList<string> ImageUrls { get => imageUrls; }

        /// <summary>
        /// Creates a ModularSheet with an empty Dictionary of ModuleData and an empty List of ImageUrls.
        /// </summary>
        public ModularSheet()
        {
            modules = new List<ModuleData>();
            imageUrls = new List<string>();
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

        /// <summary>
        /// Adds the given string to this ModularSheet's list of image urls.
        /// </summary>
        /// <param name="url">The url string to add.</param>
        /// <exception cref="ArgumentNullException">url is null.</exception>
        /// <exception cref="ArgumentException">url is blank.</exception>
        public void AddImageUrl(string url)
        {
            if (url == null)
            {
                throw new ArgumentNullException("Cannot add a null string to a ModularSheet's list of image urls.");
            }
            if (url.Trim().Length == 0)
            {
                throw new ArgumentException("Cannot add a blank string to a ModularSheet's list of image urls.");
            }
            imageUrls.Add(url);
        }

        /// <summary>
        /// Removes the image url from the given index of this ModularSheet's list of image urls.
        /// </summary>
        /// <param name="index">The index of the url to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException">index is negative or too high.</exception>
        public void RemoveImageUrl(int index)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("Cannot remove a ModularSheet's image url with a negative index.");
            }
            if (index >= imageUrls.Count())
            {
                throw new ArgumentOutOfRangeException("This ModularSheet only has " + imageUrls.Count() + " image urls in its list. Cannot remove index " + index + ".");
            }
            foreach (var mod in modules)
            {
                if (mod.BgImageIndex == index)
                {
                    mod.BgImageIndex = -1;
                }
            }
            imageUrls.RemoveAt(index);
        }

        /// <summary>
        /// Removes the given string from this ModularSheet's list of image urls.
        /// </summary>
        /// <param name="url">The url to remove.</param>
        /// <exception cref="ArgumentNullException">url is null.</exception>
        /// <exception cref="ArgumentException">url is blank, or not found in the list.</exception>
        public void RemoveImageUrl(string url)
        {
            if (url == null)
            {
                throw new ArgumentNullException("Cannot remove a null string from a ModularSheet's list of image urls.");
            }
            if (url.Trim().Length == 0)
            {
                throw new ArgumentException("Cannot remove a blank string from a ModularSheet's list of image urls.");
            }
            int index = imageUrls.IndexOf(url);
            if (index == -1)
            {
                throw new ArgumentException("Url not found in this ModularSheet's list of image urls.");
            }
            RemoveImageUrl(index);
        }
    }
}
