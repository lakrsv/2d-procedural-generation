using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    public bool ZoomIn, Recalculate;
    public int PixelsToUnits;
    public float Zoom;
    private Transform _playerTransform;
    public float Smooth;
    // Use this for initialization
    void Start()
    {
        SetOrthoSize();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
        if (Recalculate)
        {
            SetOrthoSize();
        }
    }
    private void SetOrthoSize()
    {
        int h = Screen.height;// / PixelsToUnits * Zoom;
        //camera.pixelRect = new Rect (0, 0, Screen.width, h);

        if (ZoomIn)
        {
            GetComponent<UnityEngine.Camera>().orthographicSize = (h / (2.0f * PixelsToUnits)) / Zoom;
        }
        else
        {
            GetComponent<UnityEngine.Camera>().orthographicSize = (h / (2.0f / PixelsToUnits)) * Zoom;
        }
        Recalculate = false;
    }
    private void FollowPlayer()
    {
        var tempPlayerPos = new Vector3(_playerTransform.position.x, _playerTransform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, tempPlayerPos, Smooth * Time.deltaTime);
    }
}
