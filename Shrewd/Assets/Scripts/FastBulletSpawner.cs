using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastBulletSpawner : MonoBehaviour
{
    public GameObject trajectoryEffectPrefab;
    public GameObject fastBulletPrefab;
    public float spawnDelay;
    private float timer = 0.0f;

    void Start()
    {
        Instantiate(trajectoryEffectPrefab, transform.position, transform.rotation);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer > spawnDelay)
        {
            Instantiate(fastBulletPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
