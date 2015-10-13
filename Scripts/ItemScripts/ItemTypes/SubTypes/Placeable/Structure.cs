using UnityEngine;
using System.Collections;

public abstract class Structure : Placeable
{
    public static Item GetStructure(string subtype2)
    {
        switch (subtype2)
        {
            case "LampMK1":
                return new LampMKI();

            case "FurnaceMK1":
                return new FurnaceMKI();

            default:
                return null;
        }
    }
}
