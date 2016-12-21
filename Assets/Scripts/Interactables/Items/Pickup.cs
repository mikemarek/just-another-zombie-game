/**
* Pickup.cs
* Created by Michael Marek (2016)
*
* A pickup is an interactable object containing a single item that an actor can pick up and store
* in their inventory system.
**/

using UnityEngine;
using System.Collections;

public class Pickup : Interactable
{
    [Header("Pickup Item")]
    public Item item;

    /**
    * Check to see if the pickup item has an atteched physics collider. If it does, loop through
    * all of the actors currently present in the scene and have their colliders ignore this one.
    * This is so that as an actor approaches the item to pick it up, they don't continuously push
    * it away from them.
    *
    * @param    null
    * @return   null
    **/
    void Start()
    {
        Collider collider = null;

        foreach (Collider col in gameObject.GetComponents<Collider>())
        {
            if (col.isTrigger)
                continue;
            collider = col;
            break;
        }

        if (collider == null)
            return;

        SceneManager scene = GameObject.Find("Scene Manager").GetComponent<SceneManager>();

        foreach (Transform player in scene.playerList)
        {
            Collider col = player.gameObject.GetComponent<Collider>();
            Physics.IgnoreCollision(collider, col, true);
        }
    }

    /**
    * Attempt to add the attached pickup item to the interacting actor's inventory.
    *
    * @param    GameObject  the actor that interacted with this object
    * @return   null
    **/
    public override void Interact(GameObject go)
    {
        InventoryComponent inventory = go.GetComponent<InventoryComponent>();

        bool added = inventory.AddItem(item);

        if (!added)
        {
            StopInteracting();
            return;
        }

        StopInteracting();
        Destroy(gameObject);
    }
}
