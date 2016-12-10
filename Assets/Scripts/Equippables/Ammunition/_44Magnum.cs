using UnityEngine;
using System.Collections;

public class _44Magnum : Ammunition
{
    void Awake()
    {
        itemType     = ItemManager.ItemType._44Magnum;
        stackSize    = 12;
        maxStackSize = 24;
        equippable   = false;
    }
}
