using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerInventoryManager : MonoBehaviour
{
    private GUIManager _GUIMan;
    private MouseHandler _mouseHan;
    [SerializeField]
    private Item[] _toolBarItems = new Item[8];
    [Range(0, 8)]
    private int _toolBarItemCount;
    private Dictionary<string, int> _itemCount;
    private int[] _stackAmount;
    private int _tempButtonID;

    private bool _holdingObject, _relocatingItem;
    public bool HeldObjectRecently;

    //Temporary values
    private GameObject _instantiatedObject;
    private BoxCollider2D _instantiatedCollider;
    private SpriteRenderer _instantiatedRenderer;

    // Use this for initialization
    void Start()
    {

        _holdingObject = false;
        _GUIMan = GameObject.FindGameObjectWithTag("Canvas").GetComponent<GUIManager>();
        _mouseHan = GetComponent<MouseHandler>();
        _itemCount = new Dictionary<string, int>();
        _stackAmount = new int[8];
        _tempButtonID = -1;
    }

    //Testing
    void Update()
    {
        PlaceItem();
        HandleAlphaNumeric();
    }

    public void AddItem(Item.ItemType type, string subtype, string subtype2)
    {
        _toolBarItemCount++;
        var item = ItemFactory.GetItem(type, subtype, subtype2);
        int firstEmpty = System.Array.IndexOf(_toolBarItems, null);

        if (subtype2 != null)
        {
            if (_itemCount.ContainsKey(subtype2))
            {
                _itemCount[subtype2]++;
                if (item.Stackable)
                {
                    for (int i = 0; i < _toolBarItems.Length; i++)
                    {
                        if (_toolBarItems[i] != null)
                        {
                            if (_toolBarItems[i].ItemName != null)
                            {
                                if (_toolBarItems[i].ItemName == subtype2)
                                {
                                    _stackAmount[i]++;
                                    _GUIMan.UpdateButton(i);
                                }
                            }
                        }
                    }
                }
                else
                {
                    _itemCount.Add(subtype2, 1);
                    _toolBarItems[firstEmpty] = (Item)item;
                }
            }

            else
            {
                _itemCount.Add(subtype2, 1);
                _stackAmount[firstEmpty]++;
                _toolBarItems[firstEmpty] = (Item)item;
                _GUIMan.PopulateButton(firstEmpty, item.ItemIcon);
                _GUIMan.UpdateButton(firstEmpty);
            }
        }
        else if (subtype != null)
        {
            if (_itemCount.ContainsKey(subtype))
            {
                _itemCount[subtype]++;
                if (item.Stackable)
                {
                    for (int i = 0; i < _toolBarItems.Length; i++)
                    {
                        if (_toolBarItems[i] != null)
                        {
                            if (_toolBarItems[i].ItemName != null)
                            {
                                if (_toolBarItems[i].ItemName == subtype)
                                {
                                    _stackAmount[i]++;
                                    _GUIMan.UpdateButton(i);
                                }
                            }
                        }
                    }
                }
                else
                {
                    _itemCount.Add(subtype, 1);
                    _toolBarItems[firstEmpty] = (Item)item;
                }
            }
            else
            {
                _itemCount.Add(subtype, 1);
                _stackAmount[firstEmpty]++;
                _toolBarItems[firstEmpty] = (Item)item;
                _GUIMan.PopulateButton(firstEmpty, item.ItemIcon);
                _GUIMan.UpdateButton(firstEmpty);
            }
        }
        else
        {
            //ERROR; Bad If-Nest, Subtype will not be recognized due to how the nest works, 
            //fix this when you add subtype1-dependent items.
            //(Add item to main inventory)
        }
        _GUIMan._itemStackAmount = _stackAmount;
    }
    public void UseItem(int buttonid)
    {

        if (_toolBarItems[buttonid] is Item)
        {
            _tempButtonID = buttonid;
            if (_toolBarItems[buttonid] is Placeable)
            {
                GhostItem(buttonid);
            }
            else if (_toolBarItems[buttonid] is Tool)
            {

            }
        }
    }
    void GhostItem(int buttonid)
    {
        if (!_holdingObject)
        {
            _holdingObject = true;
            HeldObjectRecently = false;
            var item = (Item)_toolBarItems[buttonid];
            TileTemplate tiletemp = (TileTemplate)item;

            var instObject = Instantiate(tiletemp.GetObject());

            var objectCollider = instObject.GetComponent<BoxCollider2D>();
            var objectRenderer = instObject.GetComponent<SpriteRenderer>();

            objectCollider.enabled = false;
            var mouseCursor = _mouseHan.MouseCursor;

            instObject.transform.SetParent(mouseCursor.transform);
            instObject.transform.localPosition = Vector2.zero;

            //Temporary values
            _instantiatedObject = instObject;
            _instantiatedCollider = objectCollider;
            _instantiatedRenderer = objectRenderer;
            //End of Temporary values

        }

    }
    //Item relocation causes object reference exception in PlaceItem(), Line 186, if spammed.
    void PlaceItem()
    {
        if (_holdingObject)
        {
            ItemEffects();
            if (Input.GetMouseButtonDown(0))
            {
                if (!_mouseHan.Blocked)
                {
                    if (_mouseHan.MouseinRange)
                    {
                        _stackAmount[_tempButtonID]--;
                        var specificItemAmount = _itemCount[_toolBarItems[_tempButtonID].ItemName]--;
                        var specificItemName = _toolBarItems[_tempButtonID].ItemName;
                        if (specificItemAmount == 1)
                        {
                            _GUIMan._itemStackAmount[_tempButtonID] = 0;
                            _toolBarItems[_tempButtonID] = null;
                            _itemCount.Remove(specificItemName);
                        }
                        _instantiatedRenderer.color = new Color(1, 1, 1, 1);
                        StartCoroutine(HeldObjectDelay());
                        _mouseHan.MouseCursor.transform.DetachChildren();
                        _instantiatedCollider.enabled = true;
                        _holdingObject = false;
                        _GUIMan.UpdateButton(_tempButtonID);
                        _tempButtonID = -1;
                    }
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                Destroy(_mouseHan.MouseCursor.transform.GetChild(0).gameObject);
                _holdingObject = false;
                HeldObjectRecently = false;
                _tempButtonID = -1;
            }
        }
    }
    void ItemEffects()
    {
        if (_mouseHan.MouseinRange)
        {
            _instantiatedRenderer.color = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            _instantiatedRenderer.color = new Color(1, 1, 1, 0.25f);
        }
    }

    IEnumerator HeldObjectDelay()
    {
        HeldObjectRecently = true;
        yield return new WaitForSeconds(0.25f);
        HeldObjectRecently = false;
    }
    void HandleAlphaNumeric()
    {
        for (int i = 0; i < 8; i++)
        {
            KeyCode Key = (KeyCode)Enum.Parse(typeof(KeyCode), "Alpha" + i);
            if (Input.GetKeyDown(Key))
            {
                UseItem(i - 1);
            }
        }
    }
    public void RelocateItem(int newbuttonid)
    {
        //Not working properly
        _relocatingItem = true;
        if (_tempButtonID != -1 && _tempButtonID != newbuttonid)
        {
            var tempNewItem = (Item)_toolBarItems[newbuttonid];
            var tempNewStackAmount = _stackAmount[newbuttonid];
            var tempOldItem = (Item)_toolBarItems[_tempButtonID];
            var tempOldStackAmount = _stackAmount[_tempButtonID];

            _stackAmount[newbuttonid] = _stackAmount[_tempButtonID];
            _stackAmount[_tempButtonID] = tempNewStackAmount;

            if (tempOldItem != null && tempNewItem != null)
            {
                _toolBarItems[newbuttonid] = tempOldItem;
                _toolBarItems[_tempButtonID] = tempNewItem;
                _stackAmount[newbuttonid] = tempOldStackAmount;
                _stackAmount[_tempButtonID] = tempNewStackAmount;
                _GUIMan.UpdateButton(newbuttonid);
                _GUIMan.UpdateButton(_tempButtonID);
                _GUIMan.PopulateButton(newbuttonid, _toolBarItems[newbuttonid].ItemIcon);
                _GUIMan.PopulateButton(_tempButtonID, _toolBarItems[_tempButtonID].ItemIcon);
                _relocatingItem = false;
                return;
            }

            if (tempOldItem != null)
            {
                _toolBarItems[newbuttonid] = tempOldItem;
                _GUIMan.UpdateButton(newbuttonid);
                _GUIMan.PopulateButton(newbuttonid, tempOldItem.ItemIcon);
            }
            else
            {
                _toolBarItems[newbuttonid] = null;
                _stackAmount[newbuttonid] = 0;
                _GUIMan.UpdateButton(newbuttonid);
                _GUIMan.PopulateButton(newbuttonid, null);
                _GUIMan._itemStackAmount[newbuttonid] = 0;
            }
            if (tempNewItem != null)
            {
                _toolBarItems[_tempButtonID] = tempNewItem;
                _GUIMan.UpdateButton(_tempButtonID);
                _GUIMan.PopulateButton(_tempButtonID, tempNewItem.ItemIcon);
            }
            else
            {
                _toolBarItems[_tempButtonID] = null;
                _stackAmount[_tempButtonID] = 0;
                _GUIMan.UpdateButton(_tempButtonID);
                _GUIMan.PopulateButton(_tempButtonID, null);
                _GUIMan._itemStackAmount[_tempButtonID] = 0;
            }
            _relocatingItem = false;


        }
    }
}
