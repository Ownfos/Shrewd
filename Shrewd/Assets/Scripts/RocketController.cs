using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class RocketController : MonoBehaviour
{
    public float speed;

    private const float MAP_LIMIT = 20.0f;

    private GameObject boundary;

    public GameObject rocketTrailPrefab;
    public GameObject explosionSubprojectilePrefab;
    public GameObject explosionColliderPrefab;
    private Material material;
    private AttackSafetyLock safetyLock;
    private ScoreManager scoreManager;
    private float effectSpawnTimer = 0.0f;
    private float randomPositionRange = 0.1f;

    void Start()
    {
        boundary = GameObject.FindGameObjectWithTag("Boundary");
        scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        material = GetComponent<MeshRenderer>().material;
        safetyLock = GetComponent<AttackSafetyLock>();

        material.color = Color.gray;
    }

    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;

        // Create rocket trail smoke effect
        effectSpawnTimer += Time.deltaTime;
        if(effectSpawnTimer > 0.02f)
        {
            effectSpawnTimer = 0.0f;
            var randomOffset = new Vector3(
                Random.Range(-randomPositionRange, randomPositionRange),
                Random.Range(-randomPositionRange, randomPositionRange)
            );
            var randomRotation = Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f));
            Instantiate(rocketTrailPrefab, transform.position + randomOffset, randomRotation);
        }

        // Destroy when it gets outside map boundary
        if (Vector3.Distance(transform.position, boundary.transform.position) > MAP_LIMIT)
        {
            Destroy(gameObject);
        }

        // Explode on reflection attempt
        if (material.color == Color.yellow && Input.GetMouseButtonDown(0))
        {
            Explode();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // A rocket explodes when collided with player, a reflected bullet, a rocket explosion, or an enemy.
        if (collision.gameObject.tag == "Player" ||
           collision.gameObject.tag == "Attack" ||
           (collision.gameObject.tag == "Enemy" && !safetyLock.IsLocked()))
        {
            if(collision.gameObject.tag == "Attack")
            {
                var bc = collision.gameObject.GetComponent<BulletController>();
                if (bc != null && !bc.IsReflected())
                {
                    // If collided bullet was not a reflected one, ignore collision and do not explode
                    return;
                }
                else
                {
                    // If player succeeds to destroy rocket with reflected bullet,
                    // increase score as reward for skill play and destroy the bullet
                    Destroy(collision.gameObject);
                    scoreManager.IncreaseScore(1);
                }
            }

            Explode();
        }

        if (collision.gameObject.tag == "ReflectionArea")
        {
            material.color = Color.yellow;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ReflectionArea")
        {
            material.color = Color.gray;
        }
    }

    public void Explode()
    {
        Instantiate(explosionColliderPrefab, transform.position, Quaternion.identity);
        for (int i = 0; i < 5; i++)
        {
            var randomOffset = new Vector3(
                Random.Range(-randomPositionRange, randomPositionRange),
                Random.Range(-randomPositionRange, randomPositionRange)
            );
            var randomRotation = Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f));
            Instantiate(explosionSubprojectilePrefab, transform.position + randomOffset, randomRotation);
        }
        Destroy(gameObject);
    }
}
