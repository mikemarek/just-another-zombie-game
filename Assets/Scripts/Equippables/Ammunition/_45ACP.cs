using UnityEngine;
using System.Collections;

public class _45ACP : Ammunition
{
    void Awake()
    {
        itemType     = ItemManager.ItemType._45ACP;
        stackSize    = 30;
        maxStackSize = 60;
        equippable   = false;
    }
}
