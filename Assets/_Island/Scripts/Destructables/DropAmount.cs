
namespace Island.Items
{
    using System;

    [Serializable]
    public class DropAmount
    {
        public DropAmount(int amount, float probability)
        {
            this.amount = amount;
            this.probability = probability;
        }
        public int amount;
        public float probability;
    }
}