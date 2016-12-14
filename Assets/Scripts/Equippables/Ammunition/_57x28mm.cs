/**
* _57x28mm.cs
* Created by Michael Marek (DATE)
*
* Inventory item for a pack of ammunition; for weapons firing 5.7x28mm "Five-Seven" cartridges.
**/

using UnityEngine;
using System.Collections;

public class _57x28mm : Ammunition
{
    /**
    **/
    public _57x28mm()
    {
        itemType   = ItemType._57x28mm;
        stackSize  = 30;
        maxStack   = 30;
        equippable = false;
    }
}
