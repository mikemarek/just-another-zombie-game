/**
* PlayerEquipmentComponent.cs
* Created by Michael Marek (2016)
*
* Keeps track of the player's currently equipped items within their inventory. We keep track of two
* items in the player's inventory - their primary (held) item and their 'hotswap' (secondary) item
* which can be quickly equipped back and forth. Methods for determining if a weapon requires
* reloading and initializing the reload procedure are also found here.
**/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerEquipmentComponent : MonoBehaviour
{
    [Header("Toggle Functionality")]
    public  bool                        allowItemUse    = true;
    public  float                       swapItemTime    = 0.5f;

    [Header("Inventory Slot Selections")]
    public  Position                    selectedSlot    = new Position( 0,  0);
    public  Position                    equippedSlot    = new Position(-1, -1);
    public  Position                    hotkeyedSlot    = new Position(-1, -1);

    [Header("HUD Item Display")]
    public  Item                        displayedItem;

    private Item                        equippedItem;

    private PlayerInputComponent        input;
    private ActorControllerComponent    controller;
    private InventoryComponent          inventory;


    /**
    * Initialize references to other player components.
    *
    * @param    null
    * @return   null
    **/
    void Start()
    {
        input = gameObject.GetComponent<PlayerInputComponent>();
        controller = gameObject.GetComponent<ActorControllerComponent>();
        inventory = gameObject.GetComponent<InventoryComponent>();
    }


    /**
    * Activate the primary or secondary uses of held items, or swap out primary and hotswap items.
    *
    * @param    null
    * @return   null
    **/
    void Update()
    {
        if (!allowItemUse)
            return;

        if (equippedItem == null)
            return;

        if (input._Use && !input.AltUse)
            equippedItem.StartUse(0);
        if (input.Use_)
            equippedItem.StopUse(0);

        if (input._AltUse && !input.Use)
            equippedItem.StartUse(1);
        if (input.AltUse_)
            equippedItem.StopUse(1);

        if ((input.Use && input._AltUse) || (input.AltUse && input.Use))
            if (equippedItem.multiUse)
                equippedItem.MultiUse(new uint[2]{0, 1});
    }


    /**
    * Equip an inventory item at the designated slot as the player's primary item.
    *
    * @param    Position    position of the item in the player's inventory
    * @return   bool        returns true if the item was equipped; false otherwise
    **/
    public bool EquipItem(Position slot)
    {
        Item item = inventory.GetItem(slot);
        if (item == null)
            return false;
        if (!item.equippable)
            return false;

        if (equippedItem != null)
            equippedItem.OnUnequip();
        equippedItem = item;
        equippedItem.OnEquip();
        equippedSlot = slot;
        displayedItem = item;
        return true;
    }


    /**
    * Delete the player's primary item.
    *
    * @param    null
    * @return   null
    **/
    public void DestroyHeldItem()
    {
        inventory.DeleteItem(equippedSlot);
        equippedItem = null;
        equippedSlot = new Position(-1, -1);
    }


    /**
    * Exchange the player's primary item with their secondary item.
    *
    * @param    null
    * @return   null
    **/
    public void HotswapItems()
    {
        if (equippedSlot.x < 0 || equippedSlot.y < 0)
            return;
        if (hotkeyedSlot.x < 0 || hotkeyedSlot.y < 0)
            return;

        Position t = equippedSlot;
        if (EquipItem(hotkeyedSlot))
            hotkeyedSlot = t;
    }


    /**
    * Determine if a weapon's primary and/or secondary firing modes need reloading - magazines are
    * not at full capacity and the appropriate ammunution can be found in the player's inventory.
    *
    * @param    null
    * @return   bool    do we need to reload?
    **/
    public bool CanReload()
    {
        if (equippedItem == null)
            return false;
        if (!(equippedItem is Weapon))
            return false;

        Weapon weapon = equippedItem as Weapon;

        if (ReloadTime() == 0f)
            return false;

        for (int i = 0; i < weapon.attackModes.Length; i++)
        {
            WeaponAttackMode mode = weapon.attackModes[i];

            if (controller.state is PlayerReloadState && !mode.continuousReload)
                continue;

            if (inventory.GetItems(mode.ammunitionType).Count > 0)
                if ((int)mode.clipSize - (int)mode.ammunition > 0)
                    return true;
        }

        return false;
    }


    /**
    * Calculate the reload time for the weapon (only primary, only secondary, or primary+secondary
    * firing modes may need reloading).
    *
    * @param    null
    * @return   float   the time needed to fully reload the weapon (sec)
    **/
    public float ReloadTime()
    {
        if (equippedItem == null)
            return 0f;

        if (!(equippedItem is Weapon))
            return 0f;

        Weapon weapon = equippedItem as Weapon;
        float time = 0f;

        for (int i = 0; i < weapon.attackModes.Length; i++)
        {
            WeaponAttackMode mode = weapon.attackModes[i];

            if (inventory.GetItems(mode.ammunitionType).Count > 0)
                if (mode.clipSize - mode.ammunition > 0)
                    time += mode.reloadSpeed;
        }

        return time;
    }


    /**
    * Initiate the reloading sequence for the player's currently held weapon.
    *
    * @param    null
    * @return   null
    **/
    public void StartReload()
    {
        if (equippedItem == null)
            return;
        //if (!(equippedItem is Weapon))
        //    return;

        Weapon weapon = equippedItem as Weapon;

        for (uint i = 0; i < weapon.attackModes.Length; i++)
        {
            if (!(weapon.attackModes[i].currentState is WeaponInactiveState))
                if ((int)weapon.attackModes[i].clipSize - (int)weapon.attackModes[i].ammunition > 0)
                    weapon.GotoState(i, new WeaponReloadState());
        }
    }


    /**
    * Cancel the reloading sequence for the player's currently held weapon.
    *
    * @param    null
    * @return   null
    **/
    public void CancelReload()
    {
        Weapon weapon = equippedItem as Weapon;

        for (uint i = 0; i < weapon.attackModes.Length; i++)
            if (!(weapon.attackModes[i].currentState is WeaponInactiveState))
                if (weapon.attackModes[i].currentState is WeaponReloadState)
                    weapon.GotoState(i, new WeaponActiveState());
    }


    /**
    * Check for the appropriate ammunition in the player's inventory and reload the weapon.
    *
    * @param    null
    * @return   null
    **/
    public void ReloadWeapon()
    {
        if (equippedItem == null)
            return;

        Weapon weapon = equippedItem as Weapon;

        for (uint i = 0; i < weapon.attackModes.Length; i++)
        {
            if (!(weapon.attackModes[i].currentState is WeaponReloadState))
                continue;

            int availableAmmo;

            do
            {
                //get first instance of a compatible ammunition pack to reload with
                Position slot = inventory.GetItemSlot(weapon.attackModes[i].ammunitionType);
                Item ammo = inventory.GetItem(slot);

                //no more ammo available in inventory; stop reloading
                if (ammo == null)
                    break;

                availableAmmo = weapon.Reload(i, (int)ammo.stackSize);

                //this ammo pack more has enough to top off the clip
                if (ammo.stackSize > availableAmmo)
                {
                    ammo.stackSize -= (uint)availableAmmo;
                    break;
                }
                else if (ammo.stackSize <= availableAmmo)
                {
                    //use up the rest of the ammo in the stack and delete the item
                    availableAmmo -= (int)ammo.stackSize;
                    inventory.DeleteItem(slot);
                }
            } while (availableAmmo > 0);

            weapon.GotoState(i, new WeaponActiveState());
        }
    }


    public Item equipped { get { return equippedItem; } }
}
