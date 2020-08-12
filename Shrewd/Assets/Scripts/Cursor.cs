using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OutlinedPolygon))]
public class Cursor : MonoBehaviour
{
    public float size;
    public float borderThickness;

    private OutlinedPolygon polygon;

    private void Start()
    {
        polygon = GetComponent<OutlinedPolygon>();
        polygon.UpdateInnerSize(size);
        polygon.UpdateOuterSize(size + borderThickness);
    }

    void Update()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        transform.position = mousePosition;
    }
}
