using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed;
    public float rotationLerpSpeed;
    public float teleportDelay;
    public float maxTeleportDistance;
    public float timeControlDuration;
    public float timeControlRatio;
    public float timeControlRechargeDelay;
    public float timeControlLerpSpeed;
    public Color playerColor;

    public GameObject teleportEffectPrefab;

    private float teleportTimer;
    private float timeControlTimer;
    private float timeControlRechargeTimer = 0.0f;

    private BoundaryCircle boundary;
    private GameObject cursor;
    private GameObject reflectionArea;
    private CameraShake cameraShake;
    private Slider teleportGauge;
    private Slider timeControlGauge;
    private Text lifeText;
    private Destructible destructible;

    void Start()
    {
        boundary = GameObject.FindGameObjectWithTag("Boundary").GetComponent<BoundaryCircle>();
        cursor = GameObject.FindGameObjectWithTag("Cursor");
        reflectionArea = GameObject.FindGameObjectWithTag("ReflectionArea");
        cameraShake = Camera.main.GetComponent<CameraShake>();
        teleportGauge = GameObject.FindGameObjectWithTag("TeleportGauge").GetComponent<Slider>();
        timeControlGauge = GameObject.FindGameObjectWithTag("TimeControlGauge").GetComponent<Slider>();
        lifeText = GameObject.FindGameObjectWithTag("LifeText").GetComponent<Text>();
        destructible = GetComponent<Destructible>();

        GetComponent<MeshRenderer>().material.color = playerColor;

        teleportTimer = teleportDelay;
        timeControlTimer = timeControlDuration;
    }

    void Update()
    {
        // Move the player and reflection area
        transform.position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * movementSpeed * Time.deltaTime;
        reflectionArea.transform.position = transform.position + transform.up * transform.localScale.y * 1.414f;
        reflectionArea.transform.rotation = transform.rotation;

        // Smoothly rotate the character towards mouse cursor.
        // The logic used below is exactly the same as EnemyController::RotateToward().
        var currentAngle = transform.rotation.eulerAngles.z;
        var targetDirection = cursor.transform.position - transform.position;
        transform.up = targetDirection;
        var targetAngle = transform.rotation.eulerAngles.z;
        currentAngle -= Mathf.DeltaAngle(targetAngle, currentAngle) * rotationLerpSpeed;
        transform.rotation = Quaternion.Euler(0, 0, currentAngle);


        // Keep the player inside boundary circle
        var displacement = transform.position - boundary.transform.position;
        if(displacement.magnitude > boundary.currentRadius)
        {
            transform.position = boundary.transform.position + displacement.normalized * boundary.currentRadius;
        }

        // Handle teleport
        if(Input.GetKeyDown(KeyCode.Space) && teleportTimer >= teleportDelay)
        {
            Instantiate(teleportEffectPrefab, transform.position, Quaternion.identity);

            teleportTimer = 0.0f;
            transform.position += transform.up * maxTeleportDistance;
        }
        else if(teleportTimer < teleportDelay)
        {
            teleportTimer += Time.deltaTime;
        }
        teleportGauge.value = teleportTimer / teleportDelay;

        // Handle time control
        if (Input.GetMouseButton(1) && timeControlTimer > 0)
        {
            Time.timeScale += (timeControlRatio - Time.timeScale) * timeControlLerpSpeed;
            timeControlTimer -= Time.deltaTime / Time.timeScale; // make this timer deplete as fast as 1x time scale
            timeControlRechargeTimer = 0.0f;
        }
        else
        {
            Time.timeScale += (1.0f - Time.timeScale) * timeControlLerpSpeed;
            if(timeControlRechargeTimer < timeControlRechargeDelay)
            {
                timeControlRechargeTimer += Time.deltaTime;
            }
            else if (timeControlTimer < timeControlDuration)
            {
                timeControlTimer += Time.deltaTime;
            }
        }
        timeControlGauge.value = timeControlTimer / timeControlDuration;

        // Remaining life UI
        lifeText.text = $"{destructible.health}";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Attack")
        {
            Camera.main.backgroundColor = Color.red;
            Time.timeScale = 0.01f;
            cameraShake.AddShake(0.1f, 5.0f);
        }
    }
}
