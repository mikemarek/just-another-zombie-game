using UnityEngine;
using System.Collections;

public class _9x19mm : Ammunition
{
    void Awake()
    {
        itemType     = ItemManager.ItemType._9x19mm;
        stackSize    = 30;
        maxStackSize = 90;
        equippable   = false;
    }
}
