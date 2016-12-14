/**
* Container.cs
* Created by Michael Marek (2016)
*
* A container is an entity that holds an inventory component, much like the player. The container
* uses this inventory to store a collection of items, either as spawned loot or placed there by an
* actor for later use. The container also keeps track of actors currently interacting with it and
* managing the inventory system.
**/

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Container : Interactable
{
    [Header("Container Inventory Display")]
    public  GameObject          displayPrefab;      //prefab for displaying container contents
    public  Transform           displayLocation;    //spawn location of the inventory display

    [Header("Container Inventory Component")]
    public  InventoryComponent  inventory;          //inventory component attached to container

    [Header("Initial Inventory Items")]
    public  ItemType[]          startingItems;      //items present in container upon creation

    private GameObject          display;            //reference to spawned inventory display prefab

    //list of players currently accessing the container inventory and their current selected slot
    private Dictionary<GameObject, Position> registeredActors;


    /**
    * Add a display to the container so interacting actors can view its contents. Add the initial
    * list of items to the contnainer.
    *
    * @param    null
    * @return   null
    **/
    void Start()
    {
        GameObject go;

        go = GameObject.Find("Scene Manager");
        SceneManager scene = go.GetComponent<SceneManager>();

        go = GameObject.Find("Item Manager");
        ItemManager items = go.GetComponent<ItemManager>();

        registeredActors = new Dictionary<GameObject, Position>();

        display = Instantiate(displayPrefab) as GameObject;
        display.transform.SetParent(scene.canvasContainer);
        display.transform.position = displayLocation.position;

        ContainerInventoryDisplay container = display.GetComponent<ContainerInventoryDisplay>();
        container.inventory = inventory;
        container.location = displayLocation;

        for (int i = 0; i < startingItems.Length; i++)
        {
            if ( !inventory.AddItem( Type.GetType( items.classes[ (int)startingItems[i] ] ) ) )
                break;
        }
    }


    /**
    * Perform any cleanup when we destroy the container.
    *
    * @param    null
    * @return   null
    **/
    void OnDestroy()
    {
        Destroy(display);
    }


    /**
    * Have an actor start managing the container inventory system when interacted with.
    *
    * @param    GameObject  the actor that has interacted with the container
    * @return   null
    **/
    public override void Interact(GameObject go)
    {
        ActorControllerComponent controller = go.GetComponent<ActorControllerComponent>();

        controller.GotoState(new PlayerManageContainerState(this));
    }


    /**
    * Register an actor for management of the container inventory system.
    *
    * @param    GameObject  the actor to register
    * @return   bool        true if actor wasd registered successfully; false otherwise
    **/
    public bool Register(GameObject actor)
    {
        Position openSlot = OpenInventorySlot();

        if (!openSlot.Valid())
            return false;

        registeredActors.Add(actor, openSlot);
        inventory.beingManaged = true;
        StartCoroutine(FadeOutText());
        return true;
    }


    /**
    * Unregister an actor from management of the container inventory system.
    *
    * @param    GameObject  the actor to unregister
    * @return   null
    **/
    public void Unregister(GameObject actor)
    {
        if (registeredActors.ContainsKey(actor))
            registeredActors.Remove(actor);

        if (registeredActors.Count == 0)
        {
            inventory.beingManaged = false;
            StartCoroutine(FadeInText());
        }
    }


    /**
    * Set which inventory slot an actor is currently selecting within the inventory system.
    *
    * @param    GameObject  the actor whose selection to register
    * @param    Position    the position of the item they're selecting in the inventory system
    * @return   null
    **/
    public void SetActorPosition(GameObject actor, Position position)
    {
        if (registeredActors.ContainsKey(actor))
            registeredActors[actor] = position;
    }


    /**
    * Get the inventory slot an actor is currently selecting.
    *
    * @param    GameObject  the actor whose position in the inventory system we wish to determine
    * @return   null
    **/
    public Position GetActorPosition(GameObject actor)
    {
        if (registeredActors.ContainsKey(actor))
            return registeredActors[actor];
        return Position.invalid;
    }


    /**
    * Determine the first available (not selected by any other actor) slot in the inventory system.
    *
    * @param    null
    * @return   Position    first available inventory slot; invalid position (-1, -1) if none found
    **/
    private Position OpenInventorySlot()
    {
        for (int y = 0; y < inventory.size.y; y++)
            for (int x = 0; x < inventory.size.x; x++)
                if (registeredActors.ContainsValue(new Position(x, y)))
                    continue;
                else
                    return new Position(x, y);
        return Position.invalid;
    }


    public Dictionary<GameObject, Position> players { get { return registeredActors; } }
}
