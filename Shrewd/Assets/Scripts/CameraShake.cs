using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class CameraShake : MonoBehaviour
{
    public float strengthDecaySpeed;

    private float currentTranslationStrength;
    private float currentRotationStrength;

    private float initialAngle;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
        initialAngle = transform.rotation.eulerAngles.z;
    }

    void Update()
    {
        currentTranslationStrength *= strengthDecaySpeed;
        currentRotationStrength *= strengthDecaySpeed;

        var randomOffset = new Vector3(
            Random.Range(-currentTranslationStrength, currentTranslationStrength),
            Random.Range(-currentTranslationStrength, currentTranslationStrength)
        );
        var randomAngle = Random.Range(-currentRotationStrength, currentRotationStrength);

        transform.position = initialPosition + randomOffset;
        transform.rotation = Quaternion.Euler(0, 0, initialAngle + randomAngle);
    }

    public void AddShake(float translationStrength, float rotationStrength)
    {
        currentTranslationStrength = translationStrength;
        currentRotationStrength = rotationStrength;
    }
}
