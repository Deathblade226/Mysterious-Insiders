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
        DERIVATIVE
    }

    public moduleType ModuleType { get; set; } = moduleType.NONE;

    public ModuleData()
    {

    }
}
