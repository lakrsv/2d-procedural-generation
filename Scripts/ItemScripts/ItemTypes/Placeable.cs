using UnityEngine;
using System.Collections;

public abstract class Placeable : Item 
{
    public int tileID;
    public Placeable()
    {
        Stackable = true;
    }
    public static Item GetPlaceable(string subtype, string subtype2)
    {
        switch (subtype)
        {
            case "Structure":
                return Structure.GetStructure(subtype2);
            case "PlaceableTile":
                return PlaceableTile.GetPlaceableTile(subtype2);
            default:
                return null;
        }
    }
}
