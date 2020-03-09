using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public class ModuleBase : INotifyPropertyChanged
    {
        protected ModuleData mdata;
        private ModularCharacter character;
        private string text;
        private string id;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The type of module.
        /// </summary>
        [BsonElement] 
        public ModuleData.moduleType ModuleType { get => mdata.ModuleType; }

        /// <summary>
        /// The module's id, for easy lookup. This lookup is used by derivative and roll
        /// modules for their logic, and it should be serialized.
        /// </summary>
        [BsonElement] 
        public string Id { get => id; }

        /// <summary>
        /// The X coordinate of where this module goes on the ModularCharacter. Every module
        /// will have "position: absolute;" in its CSS to ensure that it goes right where
        /// it is supposed to go within its parent.
        /// </summary>
        [BsonIgnore]
        public int X { get => mdata.X; }

        /// <summary>
        /// The Y coordinate of where this module goes on the ModularCharacter. Every module
        /// will have "position: absolute;" in its CSS to ensure that it goes right where
        /// it is supposed to go within its parent.
        /// </summary>
        [BsonIgnore] 
        public int Y { get => mdata.Y; }

        /// <summary>
        /// The width of this module. This should match the width of its background image,
        /// if it has one, and should make the text wrap if it needs to.
        /// </summary>
        [BsonIgnore] 
        public int Width { get => mdata.Width; }

        /// <summary>
        /// The height of this module. This should match the height of its background image,
        /// if it has one, and should make the text wrap if it needs to.
        /// </summary>
        [BsonIgnore] 
        public int Height { get => mdata.Height; }

        /// <summary>
        /// The text to display in this module.
        /// </summary>
        [BsonElement]
        public virtual string Text {
            get => text;
            set
            {
                text = value;
                FieldChanged();
            }
        }

        /// <summary>
        /// The color to display this module's text and numbers, if it has either of those.
        /// </summary>
        [BsonIgnore] 
        public Color TextColor { get => mdata.TextColor; }

        /// <summary>
        /// The size of this module's text, if it has any.
        /// </summary>
        [BsonIgnore]
        public int FontSize { get => mdata.FontSize; }

        /// <summary>
        /// The url of this module's background image.
        /// </summary>
        public string BgImageUrl { get => mdata.BgImageUrl; }

        /// <summary>
        /// The character that this is a module for.
        /// </summary>
        [BsonIgnore] 
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
            Text = text;
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
        public ModuleBase(string id, ModularCharacter character) : this(character.Sheet.Modules[id], character) { }

        protected void FieldChanged([CallerMemberName] string field = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(field));
        }
    }
}
