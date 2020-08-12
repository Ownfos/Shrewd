using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Smoothly change the boundary circle's radius to the desired value
[RequireComponent(typeof(OutlinedPolygon))]
public class BoundaryCircle : MonoBehaviour
{
    public float targetRadius;
    public float borderThickness;
    public float lerpSpeed;
    public float currentRadius = 0.0f;

    private OutlinedPolygon polygon;


    void Start()
    {
        // Note that setting the size of polygon to zero will result in zero vectors.
        // Since the polygon class's resizing logic uses vertices' normalized vectors,
        // setting it to zero will permanetly disable future resizing.
        polygon = GetComponent<OutlinedPolygon>();
        polygon.UpdateInnerSize(0.0001f);
        polygon.UpdateOuterSize(0.0001f);
    }

    void Update()
    {
        currentRadius += (targetRadius - currentRadius) * lerpSpeed;
        polygon.UpdateOuterSize(currentRadius + borderThickness);
        polygon.UpdateInnerSize(currentRadius);
    }
}
