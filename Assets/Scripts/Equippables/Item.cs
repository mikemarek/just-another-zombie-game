/**
**/

using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour
{
    [Header("Item Display Information")]
    public  ItemManager.ItemType    itemType        = ItemManager.ItemType.None;

    public  uint                    stackSize       = 0;
    public  uint                    maxStackSize    = 250;
    public  bool                    equippable      = true;
    public  bool                    allowMultiUse   = false;

    //-------------------------------------------------------------------------

    /**
    **/
    public virtual void StartUse(uint mode)
    {
    }

    /**
    **/
    public virtual void StopUse(uint mode)
    {
    }

    /**
    **/
    public virtual void MultiUse(uint[] modes)
    {
    }

    //-------------------------------------------------------------------------

    /**
    **/
    public virtual void OnEquip()
    {
    }

    /**
    **/
    public virtual void OnUnequip()
    {
    }

    /**
    **/
    public virtual void OnPickup()
    {
    }

    /**
    **/
    public virtual void OnDrop()
    {
    }
}
