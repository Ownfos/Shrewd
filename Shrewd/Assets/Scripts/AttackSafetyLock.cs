using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Prevent's enemy friendly fire for a specified period.
// This was required due to the nature of bullet's spawn location.
// Since all enemies gather around player within close proximity,
// any bullet spawned will directly hit another enemy infront.
public class AttackSafetyLock : MonoBehaviour
{
    [Tooltip("Time in seconds required for attack <-> enemy collision to be enabled")]
    public float lockReleaseTime;

    private float life = 0.0f;

    void Update()
    {
        if(IsLocked())
            life += Time.deltaTime;
    }

    public bool IsLocked()
    {
        return life < lockReleaseTime;
    }

    // Invoked when player reflects a bullet
    public void Unlock()
    {
        life = lockReleaseTime;
    }
}
