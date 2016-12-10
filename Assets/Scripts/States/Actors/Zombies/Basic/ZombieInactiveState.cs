using UnityEngine;
using System.Collections;

public class ZombieInactiveState : ActorState
{
    private AIVisionComponent   vision;

    public override ActorState HandleInput(GameObject parent)
    {
        return null;
    }

    public override void Update(GameObject parent)
    {
        if (vision.TargetPresent())
        {
            GameObject target = vision.ClosestTarget();
            vision.TargetVisible(target.transform.position);
        }
    }

    public override void Initialize(GameObject parent)
    {
        vision = parent.GetComponent<AIVisionComponent>();
    }

    public override void OnEnter(GameObject parent)
    {
    }

    public override void OnExit(GameObject parent)
    {
    }
}
