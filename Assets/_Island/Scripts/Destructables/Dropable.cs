
namespace Island.Items
{
    using System.Collections.Generic;
    using System;

    [Serializable]
    public class Dropable
    {
        public ItemData item;
        public List<DropAmount> dropAmount = new List<DropAmount>() { new DropAmount(1, 100) };
        public float dropChance = 100;
    }
}