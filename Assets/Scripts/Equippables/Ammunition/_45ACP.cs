/**
* _45ACP.cs
* Created by Michael Marek (DATE)
*
* Inventory item for a pack of ammunition; for weapons firing .45 ACP cartridges.
**/

using UnityEngine;
using System.Collections;

public class _45ACP : Ammunition
{
    /**
    **/
    void Awake()
    {
        itemType   = ItemType._45ACP;
        stackSize  = 30;
        maxStack   = 60;
        equippable = false;
    }
}
