/**
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
    **/
    void Start()
    {
    }

    /**
    **/
    void Update()
    {
    }

    /**
    **/
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Item" || collider.gameObject.tag == "Prop")
            if (!reachables.Contains(collider.gameObject))
                reachables.Add(collider.gameObject);
    }

    /**
    **/
    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Item" || collider.gameObject.tag == "Prop")
            if (reachables.Contains(collider.gameObject))
                reachables.Remove(collider.gameObject);
    }

    /**
    **/
    public bool WithinReach()
    {
        return reachables.Count > 0;
    }

    /**
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
