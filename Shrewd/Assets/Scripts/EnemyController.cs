using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{
    public float approachDistance;
    public float escapeDistance;
    public float bulletEvasionAngle;
    public float bulletEvasionDistance;
    public float motionDelay;
    public float attackDelay;
    public float movementSpeed;
    public float rotationLerpSpeed;

    public GameObject bulletPrefab;

    private float motionTimer = float.MaxValue;
    private float attackTimer = float.MaxValue;
    private float motionStartAngle;

    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // When attack motion starts, attack timer and motion timer resets to 0.
        // Motion timer indicates the duration of rotating attack motion's delay
        // and attack timer indicates the interval between two consecutive attacks.
        // Enemy cannot do any more action while motion timer is running,
        // but can freely move and evade while motion timer is done and
        // attack timer is still running.
        if(attackTimer < attackDelay)
        {
            attackTimer += Time.deltaTime;

            if (motionTimer < motionDelay)
            {
                motionTimer += Time.deltaTime;

                // Rotate 360 degrees relative to the angle where rotation started
                var deltaAngle = Math.Min(360.0f, 360.0f * ((float)Math.Log10(motionTimer * 30 + 5) - 0.5f));
                transform.rotation = Quaternion.Euler(0, 0, motionStartAngle + deltaAngle);

                return;
            }
        }

        // Try to evade incoming bullet
        var bullets = GameObject.FindGameObjectsWithTag("Attack");
        foreach(var bullet in bullets)
        {
            // A vector from bullet to self
            var bulletDisplacement = transform.position - bullet.transform.position;
            if (bulletDisplacement.sqrMagnitude < bulletEvasionDistance * bulletEvasionDistance)
            {
                // If bullet's relative position's direction is similar to its movement direction,
                // we can conclude that the bullet is about to collide
                var bulletDirection = bullet.transform.up;
                if(Vector3.Angle(bulletDirection, bulletDisplacement) < bulletEvasionAngle)
                {
                    // The opposite of bullet direction's orthogonal component
                    // with respect to displacement (bullet -> self)
                    var evasionDirection = -(bulletDirection - Vector3.Dot(bulletDirection, bulletDisplacement) * bulletDisplacement.normalized);

                    transform.position += evasionDirection.normalized * movementSpeed * Time.deltaTime;
                    RotateToward(bullet);

                    return;
                }
            }
        }

        // If there were no bullets about to collide, try to keep specified distance with player.
        // If the distance was between approachDistance and escapeDistance, try to attack.
        try
        {
            RotateToward(player);

            // A vector from self to the player
            var displacement = player.transform.position - transform.position;
            if(displacement.sqrMagnitude > approachDistance * approachDistance)
            {
                // Move towards the player
                transform.position += displacement.normalized * movementSpeed * Time.deltaTime;
            }
            else if(displacement.sqrMagnitude < escapeDistance * escapeDistance)
            {
                // Move away from the player
                transform.position -= displacement.normalized * movementSpeed * Time.deltaTime;
            }
            else
            {
                if(attackTimer >= attackDelay)
                {
                    // Make some randomness on attack delay
                    attackTimer = UnityEngine.Random.Range(0.0f, (attackDelay - motionDelay) * 0.25f);

                    // Start rotating motion
                    motionTimer = 0.0f;
                    motionStartAngle = transform.rotation.eulerAngles.z;

                    // Shoot a bullet towards the player
                    var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                    bullet.transform.up = displacement.normalized;
                }
            }
        }
        catch
        {
            // This clause is executed when player is dead
            gameObject.SetActive(false);
        }
    }

    // Smoothly rotate the facing direction towards target
    private void RotateToward(GameObject target)
    {
        // Remember current state
        var currentAngle = transform.rotation.eulerAngles.z;

        // Actually change the direction to the desired one
        var targetDirection = target.transform.position - transform.position;
        transform.up = targetDirection;

        // Remember the desired angle
        var targetAngle = transform.rotation.eulerAngles.z;

        // Interpolate current angle and desired angle
        var deltaAngle = Mathf.DeltaAngle(targetAngle, currentAngle);
        currentAngle -= deltaAngle * rotationLerpSpeed;

        transform.rotation = Quaternion.Euler(0, 0, currentAngle);
    }

    // Used by ShieldController to decide whether to
    // synchronize position with an owner's facing direction.
    // Note that shield shouldn't follow owner's forward direction
    // while the owner is under attack motion (rotating 360 degrees).
    public bool IsUnderMotion()
    {
        return motionTimer < motionDelay;
    }

    public void OnDestruction()
    {
        GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>().IncreaseScore(1);
    }
}
