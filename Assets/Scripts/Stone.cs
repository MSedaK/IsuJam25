using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    public int damage = 10;
    public string targetTag;
    public GameObject impactVFX; 
    private Collider stoneCollider; 

    void Start()
    {
        stoneCollider = GetComponent<Collider>();
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            if (stoneCollider != null)
            {
                stoneCollider.enabled = false;
            }

            if (impactVFX != null)
            {
                Instantiate(impactVFX, transform.position, Quaternion.identity);
            }
        }
    }
}
