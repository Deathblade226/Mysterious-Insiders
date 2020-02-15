using System;

public class ModuleData
{
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

    private moduleType m_moduleType;

    public moduleType ModuleType { get { return m_moduleType; } }
    public string Id { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int BgImageIndex { get; set; }
    //public Color TextColor { get; set; }

    protected ModuleData(moduleType ModuleType)
    {
        m_moduleType = ModuleType;
    }

    public ModuleData() : this(moduleType.NONE) {}
}
