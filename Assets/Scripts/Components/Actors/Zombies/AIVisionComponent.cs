/**
* AIVisionComponent.cs
* Created by Michael Marek
*
* Provides a means for artificial agents to aquire targets in their environment. The vision
* component create both a field-of-view and "instant detection radius" for the actor, allowing
* external sources to quickly see what targets are available for the actor.
**/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIVisionComponent : MonoBehaviour
{
    public  float               visionRange         = 5f;
    public  float               fieldOfView         = 45f;
    public  float               instantDetectRange  = 3f;

    private List<GameObject>    potentials          = new List<GameObject>();


    /**
    * Adds a target to our list of potential targets when one gets close.
    *
    * @param    null
    * @return   null
    **/
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
            if (!potentials.Contains(collider.gameObject))
                potentials.Add(collider.gameObject);
    }


    /**
    * Removes a target from our list of potential targets when one gets too far away.
    *
    * @param    null
    * @return   null
    **/
    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
            if (potentials.Contains(collider.gameObject))
                potentials.Remove(collider.gameObject);
    }


    /**
    * Returns the most optimal (closest) detected target.
    *
    * @param    null
    * @return   Transform   target located; null if none
    **/
    public Transform GetTarget()
    {
        if (PotentialTargets())
        {
            GameObject target = ClosestTarget( VisibleTargets() );

            if (target != null)
                return target.transform;
        }

        return null;
    }


    /**
    * Check and see if there are any potential targets around us.
    *
    * @param    null
    * @return   bool    are there any targets?
    **/
    public bool PotentialTargets()
    {
        return potentials.Count > 0;
    }


    /**
    * Find out which of our potential targets are visible to the actor.
    *
    * @param    null
    * @return   GameObject[]    a list of all targets that the actor can see
    **/
    public GameObject[] VisibleTargets()
    {
        //remove redundant references from our targets list
        while (potentials.Contains(null))
            potentials.Remove(null);

        List<GameObject> visible = new List<GameObject>();

        for (int i = 0; i < potentials.Count; i++)
            if (TargetVisible(potentials[i].transform))
                visible.Add(potentials[i]);

        return visible.ToArray();
    }


    /**
    * Determine if a target is visible to the actor.
    *
    * @param    Transform   the target we want to check for visibility
    * @return   bool        is the target visible?
    **/
    public bool TargetVisible(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;

        //close enough to the actor to be instantly detected
        if (Vector3.Distance(transform.position, target.position) < instantDetectRange)
            return true;

        //too far away to be seen
        if (Vector3.Distance(transform.position, target.position) > visionRange)
            return false;

        //outside of our field-of-view
        if (Vector3.Angle(direction, transform.right) > fieldOfView)
            return false;

        //check out FOV to see if the target is visible
        int mask = Physics.AllLayers;
        mask = mask & ~(LayerMask.NameToLayer("Environment"));
        mask = mask & ~(LayerMask.NameToLayer("Prop"));

        Vector3 start = transform.position - transform.forward;
        Vector3 end = target.position - transform.forward;

        RaycastHit info;
        bool hit = Physics.Linecast(start, end, out info, mask);

        return info.collider.tag == "Player";
    }


    /**
    * Return the target that is closest to the actor out of a list.
    *
    * @param    GameObject[]    list of actors to check for distance to
    * @return   GameObject      target that is closest to the actor
    **/
    public GameObject ClosestTarget(GameObject[] targets)
    {
        GameObject  closest  = null;
        float       distance = Mathf.Infinity;

        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i] == null)
                continue;

            Transform t = targets[i].GetComponent<Transform>();
            float check = Vector3.Distance(transform.position, t.position);

            if (check < distance)
            {
                closest = targets[i];
                distance = check;
            }
        }

        return closest;
    }
}
