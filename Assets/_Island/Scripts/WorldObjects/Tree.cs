namespace Island.WorldObjects
{
    using Island.Destructibles;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Tree : Destructible
    {
        public override void OnHit()
        {
            Debug.Log("Tree hit! Leaves rustle.");
        }

        public override void DestroyObject()
        {
            Debug.Log("Tree destroyed! Falling down.");
            
            base.DestroyObject();
        }
    }
}