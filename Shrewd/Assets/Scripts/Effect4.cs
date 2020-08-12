using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FilledPolygon))]
public class Effect4 : MonoBehaviour
{
    [Tooltip("Time in seconds until effect destruction")]
    public float maxLife;

    [Tooltip("Time in seconds left before fading effect's start")]
    public float vanishLimit;

    [Tooltip("Speed of effect size transition. Used in logarithmic and exponential function's input coefficient.")]
    public float speed;

    [Tooltip("Starting transparency of the effect")]
    public float initialAlpha;

    [Tooltip("Starting size of the effect")]
    public float initialSize;

    [Tooltip("Final size is equal to initial size multiplied by this factor")]
    public float finalSizeRatio;

    private float life = 0.0f;

    private const float LENGTH = 100.0f;

    private FilledPolygon polygon;

    void Awake()
    {
        polygon = GetComponent<FilledPolygon>();
        polygon.UpdateAlpha(initialAlpha);

        transform.localScale = new Vector3(LENGTH, initialSize);
    }

    private void Update()
    {
        life += Time.deltaTime;

        transform.localScale = new Vector3(initialSize * (finalSizeRatio + (1.0f - finalSizeRatio) * (float)Math.Pow(0.9f, life * speed)), LENGTH);

        // Start fading out when the amount of life left is less then vanishLimit
        // Before this point, the polygon will not be transparent
        if (maxLife - life <= vanishLimit)
        {
            polygon.UpdateAlpha((maxLife - life) / vanishLimit * initialAlpha);

            if (life >= maxLife)
            {
                Destroy(gameObject);
            }
        }
    }
}
