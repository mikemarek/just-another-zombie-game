using UnityEngine;
using System.Collections;

public class _57x28mm : Ammunition
{
    void Awake()
    {
        itemType     = ItemManager.ItemType._57x28mm;
        stackSize    = 30;
        maxStackSize = 30;
        equippable   = false;
    }
}
