using UnityEngine;
using System.Collections;

public class ZombieWanderingState : ActorState
{
    private float               wanderSpeed     = 1f;   //speed zombie wanders around
    private float               wanderRadius    = 3f;   //new wander spot selection radius
    private float               slowdownRadius  = 1f;   //distance from spot before slowing down

    private float               wanderSkip      = 7f;   //time between selecting random spots
    private float               wanderTime      = 0f;   //timer for counting how long we stand

    private Vector3             target;

    private Transform           transform;
    private AIVisionComponent   vision;
    private AIMovementComponent movement;

    public override ActorState HandleInput(GameObject parent)
    {
        if (vision.TargetPresent())
        {
            GameObject target = vision.ClosestTarget();
            if (vision.TargetVisible(target.transform.position))
                return new ZombieChasingState(target.transform);
        }

        return null;
    }

    public override void Update(GameObject parent)
    {
        movement.Seek(target, wanderSpeed, slowdownRadius);
        movement.UpdateSteering();

        wanderTime += (1f / wanderSkip) * Time.deltaTime;
        if (wanderTime >= 1f)
        {
            Wander();
            wanderTime = 0f;
        }
    }

    public override void Initialize(GameObject parent)
    {
        transform = parent.transform;
        vision = parent.GetComponent<AIVisionComponent>();
        movement = parent.GetComponent<AIMovementComponent>();
    }

    public override void OnEnter(GameObject parent)
    {
        wanderTime = 0f;

        target = transform.position;

        Wander();
    }

    public override void OnExit(GameObject parent)
    {
    }

    private void Wander()
    {
        Vector3 center = transform.position;
        Vector3 circle = wanderRadius * Random.insideUnitCircle;

        target = center + circle;
    }
}
