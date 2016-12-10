/**
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
    **/
    void Start()
    {
        input = gameObject.GetComponent<PlayerInputComponent>();
        controller = gameObject.GetComponent<ActorControllerComponent>();
        inventory = gameObject.GetComponent<InventoryComponent>();
    }

    /**
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
            if (equippedItem.allowMultiUse)
                equippedItem.MultiUse(new uint[2]{0, 1});
    }

    /**
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
    **/
    public void DestroyHeldItem()
    {
        inventory.DeleteItem(equippedItem);
        equippedItem = null;
        equippedSlot = new Position(-1, -1);
    }

    /**
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

        for (int i = 0; i < weapon.firingModes.Length; i++)
        {
            WeaponFiringMode mode = weapon.firingModes[i];

            if (controller.state is PlayerReloadState && !mode.continuousReload)
                continue;

            if (inventory.GetItems(mode.ammunitionType).Count > 0)
                if ((int)mode.clipSize - (int)mode.ammunition > 0)
                    return true;
        }

        return false;
    }

    /**
    **/
    public float ReloadTime()
    {
        if (equippedItem == null)
            return 0f;

        if (!(equippedItem is Weapon))
            return 0f;

        Weapon weapon = equippedItem as Weapon;

        float time = 0f;
        for (int i = 0; i < weapon.firingModes.Length; i++)
        {
            WeaponFiringMode mode = weapon.firingModes[i];

            if (inventory.GetItems(mode.ammunitionType).Count > 0)
                if (mode.clipSize - mode.ammunition > 0)
                    time += mode.reloadSpeed;
        }

        return time;
    }

    /**
    **/
    public void StartReload()
    {
        Weapon weapon = equippedItem as Weapon;

        for (uint i = 0; i < weapon.firingModes.Length; i++)
        {
            if (!(weapon.firingModes[i].currentState is WeaponInactiveState))
                if ((int)weapon.firingModes[i].clipSize - (int)weapon.firingModes[i].ammunition > 0)
                    weapon.GotoState(i, new WeaponReloadState());
        }
    }

    /**
    **/
    public void CancelReload()
    {
        Weapon weapon = equippedItem as Weapon;

        for (uint i = 0; i < weapon.firingModes.Length; i++)
            if (!(weapon.firingModes[i].currentState is WeaponInactiveState))
                if (weapon.firingModes[i].currentState is WeaponReloadState)
                    weapon.GotoState(i, new WeaponActiveState());
    }

    /**
    **/
    public void ReloadWeapon()
    {
        if (equippedItem == null)
            return;

        Weapon weapon = equippedItem as Weapon;

        for (uint i = 0; i < weapon.firingModes.Length; i++)
        {
            if (!(weapon.firingModes[i].currentState is WeaponReloadState))
                continue;

            weapon.GotoState(i, new WeaponActiveState());

            List<Item> ammo = inventory.GetItems(weapon.firingModes[i].ammunitionType);
            if (ammo.Count == 0)
                continue;

            int totalAmmo = 0;
            for (int a = 0; a < ammo.Count; a++)
                totalAmmo += (int)ammo[a].stackSize;

            int usedAmmo = weapon.Reload(i, totalAmmo);

            while (usedAmmo > 0)
            {
                if (ammo[0].stackSize > usedAmmo)
                {
                    ammo[0].stackSize -= (uint)usedAmmo;
                    break;
                }
                else if (ammo[0].stackSize <= usedAmmo)
                {
                    usedAmmo -= (int)ammo[0].stackSize;
                    inventory.DeleteItem(ammo[0]);
                    ammo.RemoveAt(0);

                    if (usedAmmo <= 0)
                        break;
                }
            }
        }
    }

    /**
    **/
    public Item equipped { get { return equippedItem; } }
}
