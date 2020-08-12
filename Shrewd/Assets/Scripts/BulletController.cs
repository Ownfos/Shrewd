using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BulletController : MonoBehaviour
{
    public float speed;

    public GameObject reflectionEffectPrefab;

    private const float MAP_LIMIT = 20.0f;
    private bool reflected = false;

    private GameObject boundary;
    private GameObject player;
    private Material material;
    private AttackSafetyLock safetyLock;
    private ScoreManager scoreManager;

    void Start()
    {
        boundary = GameObject.FindGameObjectWithTag("Boundary");
        player = GameObject.FindGameObjectWithTag("Player");
        scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        material = GetComponent<MeshRenderer>().material;
        safetyLock = GetComponent<AttackSafetyLock>();

        material.color = Color.gray;
    }

    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;

        // Destroy when out of map boundary
        if(Vector3.Distance(transform.position, boundary.transform.position) > MAP_LIMIT)
        {
            Destroy(gameObject);
        }

        // Handle reflection
        if(material.color == Color.yellow && Input.GetMouseButtonDown(0))
        {
            Instantiate(reflectionEffectPrefab, transform.position, Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f)));
            transform.up = player.transform.up;
            reflected = true;
            speed *= 1.3f;
            safetyLock.Unlock();
        }
     }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }

        if(collision.gameObject.tag == "Enemy" && !safetyLock.IsLocked())
        {
            // Reward player for skill play
            scoreManager.IncreaseScore(1);
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "ReflectionArea")
        {
            material.color = Color.yellow;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "ReflectionArea")
        {
            material.color = Color.gray;
        }
    }

    // Used by rocket projectile to decide whether it should explode on contact.
    // Note that only reflected bullets can explode rockets.
    public bool IsReflected()
    {
        return reflected;
    }
}
