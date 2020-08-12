using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSubprojectile : MonoBehaviour
{
    public float maxLife;
    public float speed;
    public float distance;
    public float randomOffsetRange;

    public GameObject smokeEffectPrefab;

    private float life = 0.0f;
    private Vector3 direction;
    private Vector3 initialPosition;


    void Awake()
    {
        initialPosition = transform.position;
        direction = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0.0f, 360.0f)) * Vector3.one;
    }

    void Update()
    {
        life += Time.deltaTime;

        transform.position = initialPosition + distance * (float)Math.Log10(1.0 + life * speed) * direction;

        var randomOffset = new Vector3(
            UnityEngine.Random.Range(-randomOffsetRange, randomOffsetRange),
            UnityEngine.Random.Range(-randomOffsetRange, randomOffsetRange)
        );
        var randomRotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0.0f, 360.0f));
        Instantiate(smokeEffectPrefab, transform.position + randomOffset, randomRotation);

        if(life >= maxLife)
        {
            Destroy(gameObject);
        }
    }
}
