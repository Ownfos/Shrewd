using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// All objects that can be destroyed by an attack instance uses this component.
// Manages health, attack collision effect, and desctruction.
public class Destructible : MonoBehaviour
{
    public int health;
    public GameObject squareEffectPrefab;
    public GameObject debrisEffectPrefab;

    // Since enemy & shield's desctruction and player's destruction have different consequences,
    // I decided to handle this issue using a unity event.
    // Score increment, scene transition are registerd here.
    public UnityEvent onDestruction;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Attack")
        {
            // Prevent enemy's friendly fire while bullet safety lock timer is running
            // This will make sure that the bullet fired from an enemy will not immediately hit itself or a foe right ahead
            if (gameObject.tag == "Enemy" && collision.gameObject.GetComponent<AttackSafetyLock>().IsLocked()) return;

            health--;
            CreateParticle(squareEffectPrefab, 1, 0.0f);
            CreateParticle(debrisEffectPrefab, 20, 0.0f);

            if (health <= 0)
            {
                onDestruction.Invoke();
                Destroy(gameObject);
            }
        }
    }

    void CreateParticle(GameObject prefab, int number, float randomPositionRange)
    {
        for(int i=0;i<number;i++)
        {
            var randomOffset = new Vector3(
                Random.Range(-randomPositionRange, randomPositionRange),
                Random.Range(-randomPositionRange, randomPositionRange)
            );
            var randomRotation = Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f));
            Instantiate(prefab, transform.position + randomOffset, randomRotation);
        }
    }
}
