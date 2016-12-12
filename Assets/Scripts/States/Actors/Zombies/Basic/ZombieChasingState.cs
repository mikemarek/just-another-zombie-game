/**
* ZombieChasingState.cs
* Created by Michael Marek (2016)
*
* When a zombie finds a targets, we can attempt to chase it down in order to get close enough to
* attack it. However, if we lose sight of the target while chasing it, we can keep track of the
* target's last-seen location, and travel there. If we find a new target along the way, great. If
* not, just return back to wandering the wasteland.
*
**/

using UnityEngine;
using System.Collections;

public class ZombieChasingState : ActorState
{
    private float               chasingSpeed    = 4.5f;     //how fast we chase the target
    private float               attackRadius    = 1f;       //distance from target before attacking

    private bool                lostSight;                  //have we lost sight of the target?
    private Vector3             lastSeen;                   //last-known location of the target
    private Transform           target;

    private AIVisionComponent   vision;
    private AIMovementComponent movement;

    /**
    **/
    public ZombieChasingState(Transform target)
    {
        this.target = target;
    }

    /**
    **/
    public override ActorState HandleInput(GameObject parent)
    {
        //attack that sumbitch if we get close enough
        if (Vector3.Distance(parent.transform.position, target.position) <= attackRadius)
            return new ZombieAttackingState(target);

        //keep track of the target's last-seen position until we lose 'em
        if (!vision.TargetVisible(target))
            lostSight = true;
        else
            lastSeen = target.position;

        //we've lost sight of our target, but reached their last-known position
        if (lostSight && Vector3.Distance(parent.transform.position, lastSeen) < attackRadius)
        {
            Transform potential = vision.GetTarget();

            //if we find a new target, give chase; otherwise start wandering again
            if (potential != null)
                target = potential;
            else
                return new ZombieWanderingState();

            lostSight = false;
        }
        else
        {
            Transform potential = vision.GetTarget();

            //we might fight a more optimal target while chasing our current one
            if (potential != null)
                target = potential;
        }

        return null;
    }

    /**
    **/
    public override void Update(GameObject parent)
    {
        movement.Seek(lastSeen, chasingSpeed, 0f);
    }

    /**/
    public override void Initialize(GameObject parent)
    {
        vision = parent.GetComponent<AIVisionComponent>();
        movement = parent.GetComponent<AIMovementComponent>();
    }

    /**
    **/
    public override void OnEnter(GameObject parent)
    {
        lostSight = false;

        if (target == null)
            Exit();
    }

    /**
    **/
    public override void OnExit(GameObject parent)
    {
    }
}
