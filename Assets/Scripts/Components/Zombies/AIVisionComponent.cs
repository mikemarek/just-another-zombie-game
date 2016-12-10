/**
**/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIVisionComponent : MonoBehaviour
{
    public  float               visionRange         = 5f;
    public  float               fieldOfView         = 45f;
    public  float               instantSightRange   = 3f;
    private List<GameObject>    targets             = new List<GameObject>();

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
        if (collider.gameObject.tag == "Player")
            if (!targets.Contains(collider.gameObject))
                targets.Add(collider.gameObject);
    }

    /**
    **/
    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
            if (targets.Contains(collider.gameObject))
                targets.Remove(collider.gameObject);
    }

    /**
    **/
    public bool TargetPresent()
    {
        return targets.Count > 0;
    }

    /**
    **/
    public bool TargetVisible(Vector3 target)
    {
        Vector3 direction = target - transform.position;

        if (Vector3.Distance(transform.position, target) < instantSightRange)
            return true;

        if (direction.magnitude > visionRange)
            return false;

        if (Vector3.Angle(direction, transform.right) > fieldOfView)
            return false;

        int mask = Physics.AllLayers;
        mask = mask & ~(LayerMask.NameToLayer("Environment"));
        mask = mask & ~(LayerMask.NameToLayer("Prop"));

        Vector3 start = transform.position - transform.forward;
        Vector3 end = target - transform.forward;

        RaycastHit info;
        bool hit = Physics.Linecast(start, end, out info, mask);

        return info.collider.tag == "Player";
    }

    /**
    **/
    public GameObject ClosestTarget()
    {
        while (targets.Contains(null))
            targets.Remove(null);

        GameObject  closest  = null;
        float       distance = Mathf.Infinity;

        for (int i = 0; i < targets.Count; i++)
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
