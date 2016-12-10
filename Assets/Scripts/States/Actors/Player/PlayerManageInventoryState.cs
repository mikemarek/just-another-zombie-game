using UnityEngine;
using System.Collections;

public class PlayerManageInventoryState : ActorState
{
    private PlayerInputComponent        input;
    private PlayerMovementComponent     movement;
    private PlayerEquipmentComponent    equipment;
    private InventoryComponent          inventory;
    private PlayerCameraComponent       camera;

    /**
    **/
    public override ActorState HandleInput(GameObject parent)
    {
        if (!input.Inventory)
            Exit();

        return null;
    }

    /**
    **/
    public override void Update(GameObject parent)
    {
        //scroll current selection
        Position scroll;
        if (input._ScrollLeft)
            scroll = new Position(-1, 0);
        else if (input._ScrollRight)
            scroll = new Position(1, 0);
        else if (input._ScrollUp)
            scroll = new Position(0, 1);
        else if (input._ScrollDown)
            scroll = new Position(0, -1);
        else
            scroll = new Position(0, 0);
        equipment.selectedSlot = inventory.ValidPosition(equipment.selectedSlot + scroll);
        equipment.displayedItem = inventory.GetItem(equipment.selectedSlot);

        //equip a selected item
        if (input._Interact)
            if (equipment.selectedSlot != equipment.hotkeyedSlot)
                equipment.EquipItem(equipment.selectedSlot);
            else
                equipment.HotswapItems();

        //set hotkeyed item slot
        if (input._Hotswap)
            if (equipment.selectedSlot != equipment.equippedSlot)
                equipment.hotkeyedSlot = equipment.selectedSlot.Clone();

        //drop an item from the inventory
        if (input._Drop)
            inventory.DropItem(equipment.selectedSlot);
    }

    /**
    **/
    public override void Initialize(GameObject parent)
    {
        input = parent.GetComponent<PlayerInputComponent>();
        movement = parent.GetComponent<PlayerMovementComponent>();
        equipment = parent.GetComponent<PlayerEquipmentComponent>();
        inventory = parent.GetComponent<InventoryComponent>();
        camera = parent.GetComponent<PlayerCameraComponent>();
    }

    /**
    **/
    public override void OnEnter(GameObject parent)
    {
        movement.allowMovement = false;
        movement.allowAiming = false;

        equipment.allowItemUse = false;

        inventory.beingManaged = true;

        camera.allowAimingBias = false;
        camera.zoom = 0.5f;
    }

    /**
    **/
    public override void OnExit(GameObject parent)
    {
        inventory.beingManaged = false;

        camera.allowAimingBias = true;
        camera.zoom = 0f;
    }
}
