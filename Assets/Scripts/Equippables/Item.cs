/**
* Item.cs
* Created by Michael Marek (2016)
*
* A template class for items found in the game. Items are interactable objects in the environment
* that can be picked up and used by an actor or stored in an inventory.
*
* An item, if usable, can have several functionalities depending on how the player uses it - these
* different modes are called "usage modes". An example of this would be for a weapon, where mode=0
* is the weapon's primary fire and mode=1 is the weapon's alternate fire. This mode is specified
* when activating or deactivating the item, and can be checked for using a simple switch()
* statement in the overrided methods StartUse() and StopUse().
**/

using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour
{
    [Header("Item Display Information")]
    public  ItemType    itemType        = ItemType.None;

    public  uint        stackSize   = 0;        //how many of the item are grouped together
    public  uint        maxStack    = 0;        //max allowed amount of the item (0=not stackable)
    public  bool        equippable  = true;     //can an actor equip this item?
    public  bool        multiUse    = false;    //can we activate multiple usage modes?


    /**
    * Start using the item.
    *
    * @param    uint    which usage mode to activate on the item
    * @return   null
    **/
    public virtual void StartUse(uint mode)
    {
    }


    /**
    * Stop using the item.
    *
    * @param    uint    which usage mode to deactivate on the item
    * @return   null
    **/
    public virtual void StopUse(uint mode)
    {
    }


    /**
    * Use multiple functionalities on the item. The item must first be flagged for multi-use.
    *
    * @param    uint[]  array of all usage modes to be activated
    * @return   null
    **/
    public virtual void MultiUse(uint[] modes)
    {
    }


    /**
    * Called when the item is equipped by an actor.
    *
    * @param    null
    * @return   null
    **/
    public virtual void OnEquip()
    {
    }


    /**
    * Called when the item is unequipped by an actor.
    *
    * @param    null
    * @return   null
    **/
    public virtual void OnUnequip()
    {
    }


    /**
    * Called when the item is added to an inventory system.
    *
    * @param    null
    * @return   null
    **/
    public virtual void OnPickup()
    {
    }


    /**
    * Called when the item is removed from an inventory system.
    *
    * @param    null
    * @return   null
    **/
    public virtual void OnDrop()
    {
    }
}
