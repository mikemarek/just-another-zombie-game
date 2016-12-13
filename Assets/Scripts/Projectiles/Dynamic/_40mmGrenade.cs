/**
* _40mmGrenade.cs
* Created by Michael Marek (DATE)
*
* A projectile that explodes upon contact with a target or after timing out and damages actors and
* objects in the surrounding area.
**/

using UnityEngine;
using System.Collections;

public class _40mmGrenade : Projectile
{
    public float explosionRadius = 5f;

    /**
    **/
    protected override void OnImpact(GameObject hit)
    {
        Explode();
    }

    /**
    **/
    protected override void OnTimeout()
    {
        Explode();
    }

    /**
    **/
    private void Explode()
    {
        int mask = 0;
        mask |= 1 << LayerMask.NameToLayer("Environment");
        mask |= 1 << LayerMask.NameToLayer("Player");
        mask |= 1 << LayerMask.NameToLayer("Zombie");
        mask |= 1 << LayerMask.NameToLayer("Prop");

        Collider[] colliders = Physics.OverlapSphere(transform.position, 5f, mask);

        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
                rb.AddExplosionForce(impactForce, transform.position, explosionRadius);
        }

        Destroy(gameObject);
    }
}
