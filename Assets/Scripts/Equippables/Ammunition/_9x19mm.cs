/**
* _9x19mm.cs
* Created by Michael Marek (DATE)
*
* Inventory item for a pack of ammunition; for weapons firing 9x19mm Parabellum cartridges.
**/

using UnityEngine;
using System.Collections;

public class _9x19mm : Ammunition
{
    /**
    **/
    void Awake()
    {
        itemType   = ItemType._9x19mm;
        stackSize  = 30;
        maxStack   = 90;
        equippable = false;
    }
}
