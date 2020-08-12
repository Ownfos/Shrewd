using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FilledPolygon))]
public class Effect3 : MonoBehaviour
{
    [Tooltip("Time in seconds until effect destruction")]
    public float maxLife;

    [Tooltip("Speed of effect size transition. Used in logarithmic function's input coefficient.")]
    public float speed;

    [Tooltip("Coefficient for the effect's size")]
    public float size;

    [Tooltip("Starting transparency of the effect")]
    public float initialAlpha;

    private float life = 0.0f;

    private FilledPolygon polygon;

    void Awake()
    {
        polygon = GetComponent<FilledPolygon>();
        polygon.UpdateSize(0.0f);
        polygon.UpdateAlpha(initialAlpha);
    }

    void Update()
    {
        life += Time.deltaTime;

        polygon.UpdateSize(size * (float)Math.Log10(1.0 + life * speed));
        polygon.UpdateAlpha((1 - life / maxLife) * initialAlpha);

        if (life >= maxLife)
        {
            Destroy(gameObject);
        }
    }
}
