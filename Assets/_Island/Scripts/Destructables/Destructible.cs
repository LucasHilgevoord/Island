using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour, IDestructible
{
    public float Health { get { return health; } set { health = value; } }
    [SerializeField] private float health = 100f;
    
    [SerializeField] private float damageCooldownInSec = 0.5f;
    private float damageCooldown = 0;

    private void Update()
    {
        if (damageCooldown > 0)
        {
            damageCooldown -= Time.deltaTime;
        }
    }

    public void TakeDamage(float amount)
    {
        // Only take damage if the cooldown has expired
        if (damageCooldown <= 0)
        {
            Debug.Log("HIt");
            health -= amount;
            if (health <= 0)
            {
                DestroyObject();
            }

            // Reset the cooldown
            damageCooldown = damageCooldownInSec;
        }
    }

    public void DestroyObject()
    {
        // Perform destruction effects, remove object from the game world, etc.
        Destroy(gameObject);
    }
}