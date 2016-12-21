/**
* PlayerManageContainerState.cs
* Created by Michael Marek (2016)
*
* Handles the control scheme for the player as they interact with an external inventory system. The
* player can toggle between controlling their own inventory and the external inventory to either
* take or place items between inventories.
**/

using UnityEngine;
using System.Collections;

public class PlayerManageContainerState : ActorState
{
    public  float                       exitDistance;       //exit the inventory if we move too far away

    private Container                   container;          //Container storing the external inventory
    private InventoryComponent          otherInventory;     //reference to the external inventory

    private Vector3                     standingPosition;   //position of player when inventory opened
    private bool                        managedInventory;   //true = own; false = external
    private Position                    lastSelectedSlot;   //highlighted slot in other inventory
    private Item                        displayedItem;      //currently highlighted item in inventory

    private PlayerInputComponent        input;
    private PlayerMovementComponent     movement;
    private PlayerEquipmentComponent    equipment;
    private InventoryComponent          inventory;
    private PlayerCameraComponent       camera;

    /**
    **/
    public PlayerManageContainerState(Container container)
    {
        this.container = container;
        this.otherInventory = container.GetComponent<InventoryComponent>();
    }

    /**
    **/
    public override ActorState HandleInput(GameObject parent)
    {
        if (input._Drop)
            Exit();

        if (Vector3.Distance(parent.transform.position, standingPosition) > exitDistance)
            Exit();

        return null;
    }

    /**
    **/
    public override void Update(GameObject parent)
    {
        //swap management of inventories
        if (input._Hotswap)
        {
            managedInventory = !managedInventory;

            if (managedInventory)
            {
                equipment.selectedSlot = lastSelectedSlot;
                lastSelectedSlot = container.GetActorPosition(parent);
                container.SetActorPosition(parent, new Position(-1, -1));
            }
            else
            {
                container.SetActorPosition(parent, lastSelectedSlot);
                lastSelectedSlot = equipment.selectedSlot;
                equipment.selectedSlot = new Position(-1, -1);
            }
        }

        //scroll current selection within inventory
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

        //manage personal inventory
        if (managedInventory)
        {
            equipment.selectedSlot = inventory.ValidPosition(equipment.selectedSlot + scroll);
            equipment.displayedItem = inventory.GetItem(equipment.selectedSlot);

            if (input._Interact)
                inventory.TransferItem(equipment.selectedSlot, otherInventory);
        }
        //manage external inventory
        else
        {
            container.SetActorPosition(parent, otherInventory.ValidPosition(container.GetActorPosition(parent) + scroll));
            equipment.displayedItem = otherInventory.GetItem(container.GetActorPosition(parent));

            if (input._Interact)
                otherInventory.TransferItem(container.GetActorPosition(parent), inventory);
        }
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
        //register use with the container
        if (!container.Register(parent))
        {
            Exit();
            return;
        }

        exitDistance = 0.5f;

        movement.allowMovement = true; //false;
        movement.allowAiming = true; //false;

        equipment.allowItemUse = false;

        inventory.beingManaged = true;

        otherInventory.beingManaged = true;

        managedInventory = false; //manage external inventory
        lastSelectedSlot = equipment.selectedSlot;
        equipment.selectedSlot = new Position(-1, -1);

        camera.allowAimingBias = false;
        camera.zoom = 0.3f;

        standingPosition = parent.transform.position;

        displayedItem = equipment.displayedItem;
    }

    /**
    **/
    public override void OnExit(GameObject parent)
    {
        inventory.beingManaged = false;

        container.Unregister(parent);

        if (!managedInventory)
            equipment.selectedSlot = lastSelectedSlot;

        camera.allowAimingBias = true;
        camera.zoom = 0f;

        equipment.displayedItem = displayedItem;
    }
}
