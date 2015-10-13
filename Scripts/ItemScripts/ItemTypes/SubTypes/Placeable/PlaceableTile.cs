using UnityEngine;
using System.Collections;

public abstract class PlaceableTile : Placeable {

    public static Item GetPlaceableTile(string subtype2)
    {
        return new TileTemplate(subtype2);
    }
}
