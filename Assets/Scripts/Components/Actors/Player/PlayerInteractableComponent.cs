/**
* PlayerInteractableComponent.cs
* Created by Michael Marek (2016)
*
* Allows the player to interact with various objects in the environment. We keep track of any Game
* Objects that are flagged as "interactable" and are within reach of the player. Once the player
* attempts to interact with an object, we simply select the closest one and perform an interaction
* on it.
**/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInteractableComponent : MonoBehaviour
{
    [Header("Toggle Functionality")]
    public  bool                allowInteraction    = true;

    private List<GameObject>    reachables          = new List<GameObject>();


    /**
    * Adds an interactle object to our list of "potential interactables" when we get close to one.
    *
    * @param    Collider    the collider of the object that we hit
    * @return   null
    **/
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Item" || collider.gameObject.tag == "Prop")
            if (!reachables.Contains(collider.gameObject))
                reachables.Add(collider.gameObject);
    }


    /**
    * Remove an interactle object to our list of "potential interactables" when we get too far.
    *
    * @param    Collider    the collider of the object that we hit
    * @return   null
    **/
    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Item" || collider.gameObject.tag == "Prop")
            if (reachables.Contains(collider.gameObject))
                reachables.Remove(collider.gameObject);
    }


    /**
    * Check to see if we are able to interact with anything (something is within reach to use).
    *
    * @param    null
    * @return   bool    is there anything nearby to interact with?
    **/
    public bool WithinReach()
    {
        return reachables.Count > 0;
    }


    /**
    * Determine the interactable object that is closest to the player.
    *
    * @param    null
    * @return   Interactable    a reference to the interactable that is closest to the player
    **/
    public Interactable ClosestInteraction()
    {
        while (reachables.Contains(null))
            reachables.Remove(null);

        Interactable closest  = null;
        float        distance = Mathf.Infinity;

        for (int i = 0; i < reachables.Count; i++)
        {
            if (reachables[i] == null)
                continue;

            Interactable interact = reachables[i].GetComponent<Interactable>();

            if (interact.GetType() == typeof(Interactable))
                continue;

            if (interact != null && interact.Usable())
            {
                Transform t = reachables[i].GetComponent<Transform>();
                float check = Vector3.Distance(transform.position, t.position);

                if (check < distance)
                {
                    closest = interact;
                    distance = check;
                }
            }
        }

        return closest;
    }
}
