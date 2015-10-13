using UnityEngine;
using System.Collections;

public class MouseHandler : MonoBehaviour
{
    public bool MouseinRange;
    public int relativePositionX;
    public int relativePositionY;

    public float MaxReach;
    public Vector2 NewMousePos;
    private RaycastHit2D MouseHit;

    public GameObject MouseCursor;

    public float MiningSpeed;

    private bool ReadyToHit = true;
    public bool Blocked;

    private PlayerInventoryManager _playerInventoryMan;

    // Update is called once per frame
    void Start()
    {
        _playerInventoryMan = GetComponent<PlayerInventoryManager>();
        MouseCursor = new GameObject();
        MouseCursor.name = "MouseCursor";
        Blocked = false;
    }
    void Update()
    {
        MouseClick();
        MousePos();
        MouseActivate();
    }
    void MousePos()
    {
        Vector3 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MousePos.z = 0;
        if (MousePos.x > 0)
        {
            MousePos.x = (int)MousePos.x + 0.5f;
        }
        else
        {
            MousePos.x = (int)MousePos.x - 0.5f;
        }
        if (MousePos.y > 0)
        {
            MousePos.y = (int)MousePos.y + 0.5f;
        }
        else
        {
            MousePos.y = (int)MousePos.y - 0.5f;
        }
        NewMousePos = MousePos;
        MouseCursor.transform.position = new Vector2(NewMousePos.x - 0.5f, NewMousePos.y - 0.5f);

        relativePositionX = Mathf.Abs((int)(gameObject.transform.position.x - NewMousePos.x));
        relativePositionY = Mathf.Abs((int)(gameObject.transform.position.y - NewMousePos.y));
        MouseHit = Physics2D.Raycast(NewMousePos, Vector2.zero);
    }
    void MouseActivate()
    {
        if (relativePositionX < MaxReach && relativePositionY < MaxReach)
        {
            MouseinRange = true;
        }
        else
        {
            MouseinRange = false;
        }
    }
    void MouseClick()
    {
        if (Input.GetMouseButton(0))
        {
            if (MouseinRange)
            {
                if (MouseHit.transform != null)
                {
                    Blocked = true;
                    if (MouseHit.transform.name.Contains("Tile"))
                    {
                        if (!_playerInventoryMan.HeldObjectRecently)
                        {
                            if (ReadyToHit)
                            {
                                ReadyToHit = false;
                                StartCoroutine(HitDelay());
                                var tileScript = MouseHit.transform.GetComponent<Tile>();
                                tileScript.TakeDamage(1);
                            }
                        }
                    }
                }
                else
                {
                    Blocked = false;
                }
            }
        }
    }
    IEnumerator HitDelay()
    {
        yield return new WaitForSeconds(MiningSpeed);
        ReadyToHit = true;

    }
}
