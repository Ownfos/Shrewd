using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages enemy spawn and boundary circle radius.
public class Spawner : MonoBehaviour
{
    public float spawnOffset;

    // Since there are only three types of enemies(slow, fast, rocket),
    // the remaining portion of spawn ratio becomes the ratio of rocket enemy.
    public float slowEnemySpawnRatio;
    public float fastEnemySpawnRatio;

    // These three are the level information.
    // They decide how long a level lasts,
    // how frequently enemies spawn,
    // and how small the boundary becomes.
    public List<float> leveldurations;
    public List<float> spawnDelays;
    public List<float> boundaryRadius;

    public GameObject fastEnemyPrefab;
    public GameObject slowEnemyPrefab;
    public GameObject rocketEnemyPrefab;
    public GameObject shieldPrefab;

    private int level = 0;
    private float duration = 0.0f;
    private float spawnTimer = 0.0f;
    private BoundaryCircle boundaryCircle;

    void Start()
    {
        boundaryCircle = GameObject.FindGameObjectWithTag("Boundary").GetComponent<BoundaryCircle>();
        boundaryCircle.targetRadius = boundaryRadius[level];
    }

    void Update()
    {
        // Handle level transition
        duration += Time.deltaTime;
        if(level < leveldurations.Count - 1)
        {
            if(duration > leveldurations[level])
            {
                level++;
                duration = 0.0f;
                boundaryCircle.targetRadius = boundaryRadius[level];
            }
        }

        // Handle enemy spawn
        spawnTimer += Time.deltaTime;
        if(spawnTimer > spawnDelays[level])
        {
            spawnTimer = 0.0f;

            // 0 ~ slowEnemySpawnRatio                      : slow enemy
            // slowEnemySpawnRatio ~ fastEnemySpawnRatio    : fast enemy
            // fastEnemySpawnRatio ~ 1                      : rocket enemy
            var randomValue = Random.Range(0.0f, 1.0f);
            if(randomValue < slowEnemySpawnRatio)
            {
                SpawnSlowEnemy(RandomSpawnPosition());
            }
            else if(randomValue < slowEnemySpawnRatio + fastEnemySpawnRatio)
            {
                Instantiate(fastEnemyPrefab, RandomSpawnPosition(), Quaternion.identity);
            }
            else
            {
                Instantiate(rocketEnemyPrefab, RandomSpawnPosition(), Quaternion.identity);
            }
        }
    }

    public void SpawnSlowEnemy(Vector3 position)
    {
        var enemy = Instantiate(slowEnemyPrefab, position, Quaternion.identity);
        var shield = Instantiate(shieldPrefab, position, Quaternion.identity);
        shield.GetComponent<ShieldController>().owner = enemy;
    }

    private Vector3 RandomSpawnPosition()
    {
        var direction = Random.Range(0, 4);
        var x = 0.0f;
        var y = 0.0f;
        var randomValue = Random.Range(-spawnOffset, spawnOffset);
        switch (direction)
        {
            case 0:
                x = -spawnOffset;
                y = randomValue;
                break;
            case 1:
                x = spawnOffset;
                y = randomValue;
                break;
            case 2:
                x = randomValue;
                y = -spawnOffset;
                break;
            case 3:
                x = randomValue;
                y = spawnOffset;
                break;
        }

        return new Vector3(x, y);
    }
}
