using UnityEngine;
using System.Collections;

public abstract class Tool : Item
{
    public bool Stackable;
    public static Item GetTool(string subtype)
    {
        switch (subtype)
        {
            case "DrillMK1":
                return new DrillMKI();
            case "DrillMK2":
                return new DrillMKII();
            default:
                return null;
        }
    }
}
