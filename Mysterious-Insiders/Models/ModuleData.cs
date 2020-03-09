using Mysterious_Insiders.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;
using System.Runtime.CompilerServices;

/// <summary>
/// The necessary data to make a module. Each ModularSheet will have a collection
/// of these. When a ModularCharacter is created, it will use its ModularSheet's
/// collection to know what modules to put where, so that they can be filled in.
/// </summary>
public class ModuleData : INotifyPropertyChanged
{
    /// <summary>
    /// Represents what type of module this is. Here's a summary:
    /// NONE - This module can's have anything inputted into it. Good for background
    /// decorations, and the like. It can have text, but that text isn't editable.
    /// CHECK - A checkbox or radio button. Has a list of other check modules that
    /// will be deactivated if this one is activated, to make it work like a radio
    /// button. This can be left empty to keep it as an ordinary checkbox.
    /// TEXT - An editable text box, which can have a maximum length and a number
    /// of lines.
    /// MENU - A dropdown menu with a series of text options. Behind the scenes, an
    /// actual menu module's value in an int, representing which menu option was
    /// picked. Since the users won't always be computer scientists, the first menu
    /// option will have its value be 1, not 0.
    /// NUMERIC - A number input. Can have a minimum and a maximum, and can have its
    /// input limited to integers, percentages, or decimals, but it will always be
    /// a double behind the scenes.
    /// DERIVATIVE - Updates its display based on logic. This logic can use check,
    /// menu, numeric, and other derivative modules, and the user can only change the
    /// logic for the sheet (no changing the logic on a character-by-character basis.)
    /// By default it displays a number, but we might be able to make these do more
    /// if we have time.
    /// ROLL - Kind of like a derivative, but instead of a simple number it has a
    /// number of dice, sides per die, and a bonus or penalty. We might be able to
    /// make the actual modules clickable, and have the click cause the roll happen
    /// in the relevant chat.
    /// </summary>
    public enum moduleType
    {
        NONE,
        CHECK,
        TEXT,
        MENU,
        NUMERIC,
        DERIVATIVE,
        ROLL
    }

    private int x, y, width, height, fontsize = 12;
    private string imageUrl;
    private string logic;

    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// The type of module that will be created by this ModuleData.
    /// </summary>
    [BsonElement] 
    public moduleType ModuleType { get; set; }    

    /// <summary>
    /// The module's id, for easy lookup. This lookup is used by derivative and roll
    /// modules for their logic.
    /// </summary>
    [BsonElement]
    public string Id { get; set; }
    
    [BsonElement]
    public string Data { get; set; }
    
    /// <summary>
    /// The X coordinate of where this module goes on the ModularSheet. Every module
    /// will have "position: absolute;" in its CSS to ensure that it goes right where
    /// it is supposed to go within its parent.
    /// </summary>
    [BsonElement] 
    public int X { get => x;
        set
        {
            x = value;
            FieldChanged();
        }
    }

    /// <summary>
    /// The Y coordinate of where this module goes on the ModularSheet. Every module
    /// will have "position: absolute;" in its CSS to ensure that it goes right where
    /// it is supposed to go within its parent.
    /// </summary>
    [BsonElement] 
    public int Y
    {
        get => y;
        set
        {
            y = value;
            FieldChanged();
        }
    }

    /// <summary>
    /// The width of this module. This should match the width of its background image,
    /// if it has one, and should make the text wrap if it needs to.
    /// </summary>
    [BsonElement] 
    public int Width
    {
        get => width;
        set
        {
            width = value;
            FieldChanged();
        }
    }

    /// <summary>
    /// The height of this module. This should match the height of its background image,
    /// if it has one, and should make the text wrap if it needs to.
    /// </summary>
    [BsonElement] 
    public int Height
    {
        get => height;
        set
        {
            height = value;
            FieldChanged();
        }
    }

    /// <summary>
    /// The url of this module's background image.
    /// </summary>
    [BsonIgnoreIfDefault]
    public string BgImageUrl
    {
        get => imageUrl;
        set
        {
            imageUrl = value;
            FieldChanged();
        }
    }

    [BsonElement]
    private int r;
    [BsonElement]
    private int g;
    [BsonElement]
    private int b;

    /// <summary>
    /// The color to display this module's text and numbers, if it has either of those.
    /// </summary>
    [BsonIgnore]
    public Color TextColor { 
        get { return Color.FromArgb(r, g, b); } 
        set 
        {
            r = value.R;
            g = value.G;
            b = value.B;
            FieldChanged();
        } 
    }

    /// <summary>
    /// The size of this module's text, if it has any.
    /// </summary>
    [BsonIgnoreIfDefault]
    public int FontSize
    {
        get => fontsize;
        set
        {
            fontsize = value;
            FieldChanged();
        }
    }

    /// <summary>
    /// A string representation of the logic used for how this module displays. Different
    /// types of modules use different logic. For a NONE module, this will just be its text.
    /// </summary>
    [BsonElement] 
    public string SerializedLogic
    {
        get => logic;
        set
        {
            logic = value;
            FieldChanged();
        }
    }

    /// <summary>
    /// Empty ModuleData. Use serialization to fill in its properties.
    /// </summary>
    public ModuleData() { }

    /// <summary>
    /// ModuleData with an initial type.
    /// </summary>
    /// <param name="ModuleType">The type of module that this ModuleData is for.</param>
    public ModuleData(moduleType ModuleType)
    {
        this.ModuleType = ModuleType;
    }

    protected void FieldChanged([CallerMemberName] string field = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(field));
    }

    /// <summary>
    /// Converts one or more CHECK ModuleData objects into a logic string for a CHECK ModuleData.
    /// These are the objects whose modules should be unchecked if the one that holds this logic
    /// string is checked.
    /// </summary>
    /// <param name="list">The ModuleData objects to serialize into a logic string.</param>
    /// <returns>The logic string</returns>
    /// <exception cref="ArgumentException">One of the ModuleData objects isn't a CHECK module.</exception>
    public static string SerializeLogicCHECK(params ModuleData[] list)
    {
        if (list == null || list.Length == 0) return "";
        else
        {
            foreach (ModuleData module in list)
            {
                if (module.ModuleType != moduleType.CHECK) throw new ArgumentException("Only CHECK ModuleDatas can be serialized into CHECK ModuleData logic.");
            }
            string logic = list[0].Id;
            for (int i = 1; i < list.Length; i++)
            {
                logic += "," + list[i].Id;
            }
            return logic;
        }
    }

    /// <summary>
    /// Converts one or more CHECK ModuleData objects into a logic string for a CHECK ModuleData.
    /// These are the objects whose modules should be unchecked if the one that holds this logic
    /// string is checked.
    /// </summary>
    /// <param name="list">The ModuleData objects to serialize into a logic string.</param>
    /// <returns>The logic string</returns>
    /// <exception cref="ArgumentException">One of the ModuleData objects isn't a CHECK module.</exception>
    public static string SerializeLogicCHECK(ICollection<ModuleData> list)
    {
        return SerializeLogicCHECK(list.ToArray());
    }

    /// <summary>
    /// Creates a TEXT ModuleData's logic string, which can have a maximum length and a number of lines.
    /// It must have a maximum length set in order to set the number of lines.
    /// </summary>
    /// <param name="maximumLength">The maximum length, in terms of number of characters.</param>
    /// <param name="numberOfLines">The number of lines the text can wrap across.</param>
    /// <exception cref="ArgumentException">The maximum length or number of lines is less than 1.</exception>
    /// <returns></returns>
    public static string SerializeLogicTEXT(int maximumLength, int numberOfLines = 1)
    {
        if (maximumLength < 1) throw new ArgumentException("The maximum length in TEXT ModuleData logic must be a positive integer.");
        if (numberOfLines < 1) throw new ArgumentException("The number of lines in TEXT ModuleData logic must be a positive integer.");
        return maximumLength + "," + numberOfLines;
    }

    /// <summary>
    /// Creates a TEXT ModuleData's logic string, which can have a maximum length and a number of lines.
    /// It must have a maximum length set in order to set the number of lines.
    /// </summary>
    /// <param name="maximumLength">The maximum length, in terms of number of characters.</param>
    /// <param name="numberOfLines">The number of lines the text can wrap across.</param>
    /// <exception cref="ArgumentException">The maximum length or number of lines is less than 1.</exception>
    /// <returns>The logic string</returns>
    public static string SerializeLogicTEXT(uint maximumLength, uint numberOfLines = 1)
    {
        if (maximumLength < 1) throw new ArgumentException("The maximum length in TEXT ModuleData logic must be a positive integer.");
        if (numberOfLines < 1) throw new ArgumentException("The number of lines in TEXT ModuleData logic must be a positive integer.");
        if (maximumLength > int.MaxValue) throw new ArgumentException("The maximum length in TEXT ModuleData logic cannot be greater than 2147483647.");
        if (numberOfLines < 1) throw new ArgumentException("The number of lines in TEXT ModuleData logic cannot be greater than 2147483647.");
        return maximumLength + "," + numberOfLines;
    }

    /// <summary>
    /// Creates a TEXT ModuleData's logic string.
    /// </summary>
    /// <returns>The logic string</returns>
    public static string SerializeLogicTEXT()
    {
        return int.MaxValue + ",1";
    }

    /// <summary>
    /// Creates a NUMERIC ModuleData's logic string, which has a kind of number and can have a minimum
    /// and a maximum.
    /// </summary>
    /// <param name="kind">The kind of number that the module will contain (INTEGER, PERCENT, or DECIMAL.)</param>
    /// <param name="min">The number's minimum.</param>
    /// <param name="max">The number's maximum.</param>
    /// <returns>The logic string</returns>
    public static string SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber kind, double min = double.MinValue, double max = double.MaxValue)
    {
        string logic;
        switch (kind)
        {
            case ModuleNumeric.KindOfNumber.INTEGER:
                logic = "I";
                break;
            case ModuleNumeric.KindOfNumber.PERCENT:
                logic = "P";
                break;
            default:
                logic = "D";
                break;
        }
        if (min > double.MinValue) logic += "," + min;
        if (max < double.MaxValue) logic += "," + max;
        return logic;
    }

    /// <summary>
    /// Creates a MENU ModuleData's logic string from at least 2 menu option strings.
    /// </summary>
    /// <param name="options">The menu options.</param>
    /// <returns>The logic string</returns>
    /// <exception cref="ArgumentException">The number of options is less than 2.</exception>
    public static string SerializeLogicMENU(params string[] options)
    {
        if (options.Length < 2) throw new ArgumentException("MENU ModuleData logic must contain at least 2 menu options.");
        string logic = options[0];
        for (int i = 1; i < options.Length; i++)
        {
            logic += "," + options[i];
        }
        return logic;
    }

    /// <summary>
    /// Creates a DERIVATIVE ModuleData's logic string from a kind of number, a starting value, and
    /// a series of DerivativeOperations. The logic for a DerivativeOperation is in the ModuleDerivative
    /// class, and they can be constructed in a wide variety of ways. Most of its constructors take a
    /// subclass of ModuleBase and whatever operators or doubles are needed to build that subclass's logic.
    /// </summary>
    /// <param name="kind">The kind of number that the module will contain (INTEGER, PERCENT, or DECIMAL.)</param>
    /// <param name="startingValue">The value that the module uses to start its calculations.</param>
    /// <param name="operations">The derivative operations, used one by one to derive the module's number.</param>
    /// <returns>The logic string.</returns>
    public static string SerializeLogicDERIVATIVE(ModuleNumeric.KindOfNumber kind, double startingValue, params ModuleDerivative.DerivativeOperation[] operations)
    {
        string logic;
        switch (kind)
        {
            case ModuleNumeric.KindOfNumber.INTEGER:
                logic = "I";
                break;
            case ModuleNumeric.KindOfNumber.PERCENT:
                logic = "P";
                break;
            default:
                logic = "D";
                break;
        }
        logic += startingValue;
        for (int i = 0; i < operations.Length; i++)
        {
            logic += ";" + operations[i].ToString();
        }
        return logic;
    }

    /// <summary>
    /// Creates a ROLL ModuleData's logic string from three NUMERIC ModuleData objects whose kinds
    /// are INTEGER. The first one will be the source of the number of dice, the second will be the
    /// source of the sides each die will have, and the third will be the source of the bonus. The
    /// bonus can also be flagged to apply to each die, instead of to the total.
    /// </summary>
    /// <param name="diceCountSource">The source of the number of dice.</param>
    /// <param name="diceSidesSource">The source of the number of sides each die has.</param>
    /// <param name="bonusSource">The source of the bonus.</param>
    /// <param name="bonusPerDie">True if the bonus should apply to each die.</param>
    /// <returns>The logic string.</returns>
    /// <exception cref="ArgumentNullException">Any of the sources are null.</exception>
    /// <exception cref="ArgumentException">Any of the sources aren't NUMERIC or don't have the INTEGER kind.</exception>
    public static string SerializeLogicROLL(ModuleData diceCountSource, ModuleData diceSidesSource, ModuleData bonusSource, bool bonusPerDie = false)
    {
        if (diceCountSource == null || diceSidesSource == null || bonusSource == null) throw new ArgumentNullException("Cannot serialize ROLL ModuleData logic with null sources.");
        if (diceCountSource.ModuleType != moduleType.NUMERIC || diceSidesSource.ModuleType != moduleType.NUMERIC || bonusSource.ModuleType != moduleType.NUMERIC)
            throw new ArgumentException("Cannot serialize ROLL ModuleData logic with non-NUMERIC sources.");
        if (diceCountSource.SerializedLogic[0] != 'I' || diceSidesSource.SerializedLogic[0] != 'I' || bonusSource.SerializedLogic[0] != 'I')
            throw new ArgumentException("Cannot serialize ROLL ModuleData logic with NUMERIC sources whose kinds aren't INTEGER.");
        return diceCountSource.Id + "," + diceSidesSource.Id + "," + bonusSource.Id + "," + (bonusPerDie ? "per" : "flat");
    }

    /*
    /// <summary>
    /// Fix this later, once there's an actual CheckModule class.
    /// </summary>
    /// <param name="logic"></param>
    /// <param name="allModules"></param>
    /// <returns></returns>
    public static ModuleData[] DeserializeLogicCHECK(string logic, ModuleData[] allModules)
    {
        if (logic.Length == 0) return new ModuleData[0];
        else
        {
            List<ModuleData> foundModules = new List<ModuleData>();
            string[] ids = logic.Split(',');
            foreach(string id in ids)
            {
                ModuleData foundModule = allModules.Where(m => m.Id == id && m.ModuleType == moduleType.CHECK).First();
                if (foundModule != null) foundModules.Add(foundModule);
            }
            return foundModules.ToArray();
        }
    }
    */
}