using UnityEngine;
using System.Collections;

public class TileTemplate : PlaceableTile
{
    public TileTemplate(string subtype)
    {
        ItemObject = Resources.Load<GameObject>("Prefabs/Tile");
        ItemIcon = Resources.Load<Sprite>("SingleSprites/" + subtype +"Tile");
        ItemName = subtype;
        switch (subtype)
        {
            case "Rock":
                tileID = 0;
                break;
            case "Dirt":
                tileID = 1;
                break;
            case "Iron":
                tileID = 2;
                break;
            case "Coal":
                tileID = 3;
                break;
            case "Gold":
                tileID = 4;
                break;
            case "Titanium":
                tileID = 5;
                break;
        }
    }
    public GameObject GetObject()
    {
        var tileScript = this.ItemObject.GetComponent<Tile>();
        tileScript.TileTypeID = tileID;
        return this.ItemObject;

    }
}
