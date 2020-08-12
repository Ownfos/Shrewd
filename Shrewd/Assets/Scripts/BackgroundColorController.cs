using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Smoothly translates background color to the desired one
public class BackgroundColorController : MonoBehaviour
{
    public float lerpSpeed;
    public Color targetColor;

    void Update()
    {
        var camera = Camera.main;
        var currentColor = camera.backgroundColor;
        var newColor = Color.Lerp(currentColor, targetColor, lerpSpeed);

        camera.backgroundColor = newColor;
    }
}
