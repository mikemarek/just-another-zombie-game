/**
* ImpactRemoval.cs
* Created by Michael Marek (2016)
*
* For projectile impact visuals that rely on the Unity particle system for producing particle
* emissions, we can use this script to remove them once the particle system has finished running.
**/

using UnityEngine;
using System.Collections;

public class ImpactRemoval : MonoBehaviour
{
    private ParticleSystem ps;

    /**
    **/
    void Start()
    {
        ps = gameObject.GetComponent<ParticleSystem>();
    }

    /**
    **/
    void Update()
    {
        if (!ps.IsAlive())
            Destroy(gameObject);
    }
}
