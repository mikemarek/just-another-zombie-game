/**
* _44Magnum.cs
* Created by Michael Marek (DATE)
*
* Inventory item for a pack of ammunition; for weapons firing .44 magnum cartridges.
**/

using UnityEngine;
using System.Collections;

public class _44Magnum : Ammunition
{
    /**
    **/
    public _44Magnum()
    {
        itemType   = ItemType._44Magnum;
        stackSize  = 12;
        maxStack   = 24;
        equippable = false;
    }
}
