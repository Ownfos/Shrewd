using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OutlinedPolygon))]
public class Effect1 : MonoBehaviour
{
    [Tooltip("Time in seconds untli effect destruction")]
    public float maxLife;

    [Tooltip("Time in seconds left before fading effect's start")]
    public float vanishLimit;

    [Tooltip("Speed of effect size transition. Used in logarithmic and exponential function's input coefficient.")]
    public float speed;

    [Tooltip("Coefficient for the effect's size")]
    public float size;

    [Tooltip("Maximum outer radius")]
    public float maxSize;

    private float life = 0.0f;

    private OutlinedPolygon polygon;

    void Awake()
    {
        polygon = GetComponent<OutlinedPolygon>();
    }

    void Update()
    {
        life += Time.deltaTime;

        var outerSize = Math.Min(maxSize, size * (float)Math.Log10(life * speed + 1.0));
        polygon.UpdateOuterSize(outerSize);
        polygon.UpdateInnerSize(outerSize * (1.0f - (float)Math.Pow(0.8, life * speed)));

        // Start fading out when the amount of life left is less then vanishLimit
        // Before this point, the polygon will not be transparent
        if (maxLife - life <= vanishLimit)
        {
            polygon.UpdateAlpha((maxLife - life) / vanishLimit);

            if (life >= maxLife)
            {
                Destroy(gameObject);
            }
        }
    }
}
