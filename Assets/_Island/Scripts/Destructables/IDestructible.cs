using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Island.Destructibles
{
    public interface IDestructible
    {
        float Health { get; set; }
        void TakeDamage(float amount);
        void DestroyObject();
    }
}
