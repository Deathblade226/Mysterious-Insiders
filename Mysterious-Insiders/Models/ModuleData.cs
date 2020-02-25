using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

/// <summary>
/// The necessary data to make a module. Each ModularSheet will have a collection
/// of these. When a ModularCharacter is created, it will use its ModularSheet's
/// collection to know what modules to put where, so that they can be filled in.
/// </summary>
public class ModuleData
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

    /// <summary>
    /// The type of module that will be created by this ModuleData.
    /// </summary>
    public moduleType ModuleType { get; set; }

    /// <summary>
    /// The module's id, for easy lookup. This lookup is used by derivative and roll
    /// modules for their logic.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The X coordinate of where this module goes on the ModularSheet. Every module
    /// will have "position: absolute;" in its CSS to ensure that it goes right where
    /// it is supposed to go within its parent.
    /// </summary>
    public int X { get; set; }

    /// <summary>
    /// The Y coordinate of where this module goes on the ModularSheet. Every module
    /// will have "position: absolute;" in its CSS to ensure that it goes right where
    /// it is supposed to go within its parent.
    /// </summary>
    public int Y { get; set; }

    /// <summary>
    /// The width of this module. This should match the width of its background image,
    /// if it has one, and should make the text wrap if it needs to.
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// The height of this module. This should match the height of its background image,
    /// if it has one, and should make the text wrap if it needs to.
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// Each sheet should have an indexed collection of images that are used for modules
    /// that the sheet contains. This is the index to use to look up this module's image.
    /// If the module doesn't have an image, this should be -1.
    /// </summary>
    public int BgImageIndex { get; set; } = -1;

    /// <summary>
    /// The color to display this module's text and numbers, if it has either of those.
    /// </summary>
    public Color TextColor { get; set; }

    /// <summary>
    /// A string representation of the logic used for how this module displays. Different
    /// types of modules use different logic. For a NONE module, this will just be its text.
    /// </summary>
    public string SerializedLogic { get; set; }

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
            string logic = "";
            logic += list[0].Id;
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
    public static string SerializeLogicTEXT(int maximumLength = int.MaxValue, int numberOfLines = 1)
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
    /// <returns></returns>
    public static string SerializeLogicTEXT(uint maximumLength = uint.MaxValue, uint numberOfLines = 1)
    {
        if (maximumLength < 1) throw new ArgumentException("The maximum length in TEXT ModuleData logic must be a positive integer.");
        if (numberOfLines < 1) throw new ArgumentException("The number of lines in TEXT ModuleData logic must be a positive integer.");
        if (maximumLength > int.MaxValue) throw new ArgumentException("The maximum length in TEXT ModuleData logic cannot be greater than 2147483647.");
        if (numberOfLines < 1) throw new ArgumentException("The number of lines in TEXT ModuleData logic cannot be greater than 2147483647.");
        return maximumLength + "," + numberOfLines;
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
