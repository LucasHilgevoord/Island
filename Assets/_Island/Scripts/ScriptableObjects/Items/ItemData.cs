using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public InventoryItemData inventory;
    public PhysicalItemData physical;
}
