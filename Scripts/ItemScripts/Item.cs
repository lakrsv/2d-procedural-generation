using UnityEngine;
using System.Collections;

public abstract class Item{
    public bool Stackable;
    public enum ItemType
    {
        Tool,
        Placeable,
        Consumable
    }
    public string ItemName, ItemDesc;
    public int ItemID;
    public Sprite ItemIcon;
    public GameObject ItemObject;
    public ItemType Type;

}
