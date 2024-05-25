using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItemData
{
    public Sprite icon;
    public bool isStackable = false;

    [ShowIf("isStackable")]
    public int maxStackSize;
}
