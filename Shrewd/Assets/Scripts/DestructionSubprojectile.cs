using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionSubprojectile : MonoBehaviour
{
    public float maxLife;
    public float speed;
    public float distance;

    public GameObject octagonEffectPrefab;

    private float life = 0.0f;
    private Vector3 direction;
    private Vector3 initialPosition;

    void Awake()
    {
        initialPosition = transform.position;
        direction = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0.0f, 360.0f)) * Vector3.one;
        distance *= UnityEngine.Random.Range(0.2f, 1.0f);
        maxLife *= UnityEngine.Random.Range(0.7f, 1.0f);
    }

    void Update()
    {
        life += Time.deltaTime;

        transform.position = initialPosition + distance * (float)Math.Log10(1.0 + life * speed) * direction;

        if (life >= maxLife)
        {
            var randomRotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0.0f, 360.0f));
            Instantiate(octagonEffectPrefab, transform.position, randomRotation);

            Destroy(gameObject);
        }
    }
}
