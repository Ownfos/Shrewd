using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public GameObject owner;
    private EnemyController controller;

    void Start()
    {
        controller = owner.GetComponent<EnemyController>();
    }

    void Update()
    {
        try
        {
            if(owner==null)
            {
                throw new MissingReferenceException();
            }

            // Maintain the forward position of the owner.
            if(!controller.IsUnderMotion())
            {
                transform.position = owner.transform.position + owner.transform.up;
                transform.rotation = owner.transform.rotation;
            }
        }
        catch
        {
            // Destroy self when the owner is dead
            Destroy(gameObject);
        }
    }
}
