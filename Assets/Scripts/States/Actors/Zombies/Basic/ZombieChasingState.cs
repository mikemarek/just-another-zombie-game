using UnityEngine;
using System.Collections;

public class ZombieChasingState : ActorState
{
    private float               chasingSpeed    = 4.5f;   //how fast we chase the player
    private float               attackRadius    = 1f;   //distance from player before attacking

    private Transform           target;

    private AIVisionComponent   vision;
    private AIMovementComponent movement;

    public ZombieChasingState(Transform target)
    {
        this.target = target;
    }

    public override ActorState HandleInput(GameObject parent)
    {
        if (!vision.TargetVisible(target.position))
            return new ZombieWanderingState();

        if (Vector3.Distance(parent.transform.position, target.position) <= attackRadius)
            return new ZombieAttackingState(target);

        return null;
    }

    public override void Update(GameObject parent)
    {
        movement.Seek(target.position, chasingSpeed, 0f);
        movement.UpdateSteering();
    }

    public override void Initialize(GameObject parent)
    {
        vision = parent.GetComponent<AIVisionComponent>();
        movement = parent.GetComponent<AIMovementComponent>();
    }

    public override void OnEnter(GameObject parent)
    {
        if (target == null)
        {
            Exit();
            return;
        }
    }

    public override void OnExit(GameObject parent)
    {
    }
}
