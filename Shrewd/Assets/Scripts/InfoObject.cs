using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to pass variable to a different scene
public class InfoObject : MonoBehaviour
{
    public int score;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
