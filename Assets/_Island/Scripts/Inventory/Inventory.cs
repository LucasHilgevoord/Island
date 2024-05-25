using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{
    public List<ItemData> items = new List<ItemData>();
    public int space = 20;

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public bool Add(ItemData item)
    {
        if (items.Count >= space)
        {
            Debug.Log("Not enough room.");
            return false;
        }
        items.Add(item);

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }

        return true;
    }

    public void Remove(ItemData item)
    {
        items.Remove(item);

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }
}
