namespace Island.Destructibles
{
    using DG.Tweening;
    using Island.Items;
    using UnityEngine;

    public class Destructible : MonoBehaviour, IDestructible
    {
        public string identifier;

        public float Health { get { return health; } set { health = value; } }
        [SerializeField] private float health = 100f;
        [SerializeField] private float damageCooldownInSec = 0.2f;
        private float damageCooldown = 0;
        
        [SerializeField] private Dropable[] dropables;

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

                health -= amount;
                if (health <= 0)
                {
                    DestroyObject();
                }
                else
                {
                    OnHit();
                }

                // Reset the cooldown
                damageCooldown = damageCooldownInSec;
            }
        }

        public virtual void OnHit() { }

        public virtual void DestroyObject()
        {
            // Perform destruction effects, remove object from the game world, etc.
            Destroy(gameObject);
        }
    }
}
