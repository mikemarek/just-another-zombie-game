/**
* ZombieAttackingState.cs
* Created by Michael Marek
*
* Zombie melle attack. They're gonna smack their target a bit, do some damage, then take a quick
* pause before giving chase again.
**/

using UnityEngine;
using System.Collections;

public class ZombieAttackingState : ActorState
{
    private float               chasingSpeed    = 4.5f;
    private float               slowdownRadius  = 1f;
    private float               damage          = Random.Range(15, 25);
    private float               attackCooldown  = 0.25f;

    private Transform           target;
    private Vector3             attackSpot;

    private AIMovementComponent movement;

    /**
    **/
    public ZombieAttackingState(Transform target)
    {
        this.target = target;
    }

    /**
    **/
    public override ActorState HandleInput(GameObject parent)
    {
        if (attackCooldown <= 0f)
            Exit();

        return null;
    }

    /**
    **/
    public override void Update(GameObject parent)
    {
        attackCooldown -= Time.deltaTime;

        movement.Seek(attackSpot, chasingSpeed, slowdownRadius);
    }

    /**
    **/
    public override void Initialize(GameObject parent)
    {
        movement = parent.GetComponent<AIMovementComponent>();
    }

    /**
    **/
    public override void OnEnter(GameObject parent)
    {
        HealthComponent health = target.gameObject.GetComponent<HealthComponent>();
        health.Damage(damage, target.position - Vector3.forward);

        attackSpot = target.position;
    }

    /**
    **/
    public override void OnExit(GameObject parent)
    {
    }
}
