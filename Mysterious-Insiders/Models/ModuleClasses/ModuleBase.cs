using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Mysterious_Insiders.Models
{
    /// <summary>
    /// A module. Each ModularCharacter will have a collection of these, each
    /// derived from on of the ModuleData objects in the ModularCharacter's
    /// ModularSheet. Most modules will be a subclass of this, but there can
    /// still be objects of this class. Most of a module's properties will be
    /// directly determined by its ModuleData.
    /// </summary>
    public class ModuleBase
    {
        protected ModuleData mdata;
        private ModularCharacter character;
        private string imageUrl;
        private string text;
        private string id;

        /// <summary>
        /// The type of module.
        /// </summary>
        public ModuleData.moduleType ModuleType { get => mdata.ModuleType; }

        /// <summary>
        /// The module's id, for easy lookup. This lookup is used by derivative and roll
        /// modules for their logic, and it should be serialized.
        /// </summary>
        public string Id { get => id; }

        /// <summary>
        /// The X coordinate of where this module goes on the ModularCharacter. Every module
        /// will have "position: absolute;" in its CSS to ensure that it goes right where
        /// it is supposed to go within its parent.
        /// </summary>
        public int X { get => mdata.X; }

        /// <summary>
        /// The Y coordinate of where this module goes on the ModularCharacter. Every module
        /// will have "position: absolute;" in its CSS to ensure that it goes right where
        /// it is supposed to go within its parent.
        /// </summary>
        public int Y { get => mdata.Y; }

        /// <summary>
        /// The width of this module. This should match the width of its background image,
        /// if it has one, and should make the text wrap if it needs to.
        /// </summary>
        public int Width { get => mdata.Width; }

        /// <summary>
        /// The height of this module. This should match the height of its background image,
        /// if it has one, and should make the text wrap if it needs to.
        /// </summary>
        public int Height { get => mdata.Height; }

        /// <summary>
        /// The text to display in this module.
        /// </summary>
        public string Text { get => text; }

        /// <summary>
        /// The color to display this module's text and numbers, if it has either of those.
        /// </summary>
        public Color TextColor { get => mdata.TextColor; }

        /// <summary>
        /// The url of this module's background image.
        /// </summary>
        public string BgImageUrl { get => imageUrl; }

        /// <summary>
        /// The character that this is a module for.
        /// </summary>
        public ModularCharacter Character { get => character; }

        /// <summary>
        /// For internal use only, to allow subclasses to bypass the error check.
        /// </summary>
        protected ModuleBase(ModuleData data, ModularCharacter character, string text)
        {
            if (data == null) throw new ArgumentNullException("Cannot create a ModuleBase object with null ModuleData.");
            if (character == null) throw new ArgumentNullException("Cannot create a ModuleBase object with a null ModularCharacter.");
            mdata = data;
            this.character = character;
            id = data.Id;
            imageUrl = character.Sheet.ImageUrls[data.BgImageIndex];
            this.text = text;
        }

        /// <summary>
        /// Creates a new module using a ModuleData and the ModularCharacter that it's for.
        /// </summary>
        /// <param name="data">The ModuleData to use to make this module.</param>
        /// <param name="character">The ModularCharacter that this module is for.</param>
        /// <exception cref="ArgumentNullException">data or character is null.</exception>
        /// <exception cref="ArgumentException">data's type property isn't NONE.</exception>
        public ModuleBase(ModuleData data, ModularCharacter character) : this(data, character, data.SerializedLogic)
        {
            if (data.ModuleType != ModuleData.moduleType.NONE) throw new ArgumentException("Cannot create a base-class ModuleBase object with ModuleData that has a type other than NONE. Use a subclass.");
        }

        /// <summary>
        /// Creates a new module using an id and the ModularCharacter that it's for. Looks
        /// up the corresponding ModuleData from the ModularCharacter's Sheet.
        /// </summary>
        /// <param name="id">The id of the ModuleData to use to make this module.</param>
        /// <param name="character">The ModularCharacter to look up the id in.</param>
        /// <exception cref="ArgumentNullException">id doesn't map to ModuleData, or character is null.</exception>
        /// <exception cref="ArgumentException">The ModuleData's type property isn't NONE.</exception>
        public ModuleBase(string id, ModularCharacter character) : this(character.Sheet.Modules[id], character) {}
    }
}
