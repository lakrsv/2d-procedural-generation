using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _buttons;
    [SerializeField]
    private Image[] _buttonImages;
    [SerializeField]
    private Text[] _stackText;
    public int[] _itemStackAmount;

    // Use this for initialization
    void Start()
    {
        GetButtons();
    }

    void GetButtons()
    {
        _buttons = new GameObject[transform.GetChild(0).childCount];
        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i] = transform.GetChild(0).GetChild(i).gameObject;
                
        }
        GetButtonComponentsInChildren();
    }
    void GetButtonComponentsInChildren()
    {
        _buttonImages = new Image[_buttons.Length];
        _stackText = new Text[_buttons.Length];
        _itemStackAmount = new int[_buttons.Length];
        var i = 0;
        foreach (GameObject button in _buttons)
        {
            _buttonImages[i] = button.GetComponentInChildren<Image>();
            _stackText[i] = button.GetComponentInChildren<Text>();
            i++;
        }
    }
    public void PopulateButton(int buttonid, Sprite buttonimage)
    {
        var button = _buttons[buttonid];
        _buttonImages[buttonid].sprite = buttonimage;
        if (_itemStackAmount[buttonid] < 1)
        {
            //This "workaround" sometimes displays 2 items presents when there is only one.
            _itemStackAmount[buttonid] = 1;
        }
    }
    public void UpdateButton(int buttonid)
    {
        _stackText[buttonid].text = _itemStackAmount[buttonid].ToString();
        if (_itemStackAmount[buttonid] < 1)
        {
            _buttonImages[buttonid].sprite = null;
        }
    }
}
