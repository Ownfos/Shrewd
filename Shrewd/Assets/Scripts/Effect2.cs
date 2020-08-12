using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FilledPolygon))]
public class Effect2 : MonoBehaviour
{
    [Tooltip("Time in seconds untli effect destruction")]
    public float maxLife;

    [Tooltip("Starting size of the effect. It deceases down to 20% of the initial size")]
    public float initialSize;

    private float life = 0.0f;

    private FilledPolygon polygon;

    void Start()
    {
        polygon = GetComponent<FilledPolygon>();
        polygon.UpdateSize(initialSize);
    }

    void Update()
    {
        life += Time.deltaTime;

        var remainingLifeRatio = (maxLife - life) / maxLife;
        polygon.UpdateSize(initialSize * (0.2f + 0.8f * remainingLifeRatio));
        polygon.UpdateAlpha(remainingLifeRatio);

        if(life >= maxLife)
        {
            Destroy(gameObject);
        }
    }
}
