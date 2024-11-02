using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomOnScroll : MonoBehaviour
{

    [SerializeField][Range(1, 50)]
    private float zoomMax;
    [SerializeField][Range(1, 20)]
    private float zoomMin;
    [SerializeField][Range(0, 1)]
    private float zoomSpeed;
    [SerializeField]
    private bool flipZoom;

    void Update()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + (zoomSpeed*(flipZoom ? 1 : -1)), zoomMin, zoomMax);
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - (zoomSpeed*(flipZoom ? 1 : -1)), zoomMin, zoomMax);
        }
    }

}
