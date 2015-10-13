using UnityEngine;
using System.Collections;

public static class ItemFactory 
{
    public static Item GetItem(Item.ItemType type, string subtype, string subtype2)
    {
        switch (type)
        {
            case Item.ItemType.Placeable:
                return Placeable.GetPlaceable(subtype, subtype2);

            case Item.ItemType.Tool:
                return Tool.GetTool(subtype);

            default:
                return null;
        }
    }
}
