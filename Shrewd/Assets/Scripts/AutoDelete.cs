using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to destroy rocket's explosion collider after few frames
public class AutoDelete : MonoBehaviour
{
    public GameObject squareEffectPrefab;
    private int liveFrame = 0;

    private void Start()
    {
        Instantiate(squareEffectPrefab, transform.position, Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f)));
    }

    void Update()
    {
        liveFrame++;
        if (liveFrame > 3)
        {
            Destroy(gameObject);
        }
    }
}
