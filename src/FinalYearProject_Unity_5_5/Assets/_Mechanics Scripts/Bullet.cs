using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public LayerMask _collisionMask;

    void OnCollisionEnter(Collision collision)
    {
        if((_collisionMask.value & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
        {
            // Destroy bullet
            Destroy(transform.parent.gameObject);
            Debug.Log("Bullet / Target destroyed.");
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if ((_collisionMask.value & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
        {
            // Dstroy bullet
            Destroy(transform.parent.gameObject);
            Debug.Log("Bullet destroyed.");
        }
    }
}
