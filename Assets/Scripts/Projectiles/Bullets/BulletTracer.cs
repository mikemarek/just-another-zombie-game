/**
* BulletTracer.cs
* Created by Michael Marek (2016)
*
* An aesthetic tracer visual for hitscan projectiles. Since the hitscan projectile collides with
* its target instantaneously, we need something to visually represent projectile during the attack.
**/

using UnityEngine;
using System.Collections;

public class BulletTracer : MonoBehaviour
{
    public  float           velocity            = 20f;

    private Vector3         target              = Vector3.zero;
    private float           distanceTravelled   = 0f;
    private float           totalDistance       = 0f;

    private SpriteRenderer  sprite;

    /**
    **/
    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
    }

    /**
    **/
    void FixedUpdate()
    {
        //move the projectile through the game world
        Vector3 position = transform.position;
        position += (target - position).normalized * velocity * Time.deltaTime;

        distanceTravelled += velocity * Time.deltaTime;

        if (distanceTravelled >= totalDistance)
        {
            transform.position = target;
            if (sprite != null)
                Destroy(sprite);
            return;
        }

        transform.position = position;
    }

    /**
    **/
    public void SetTarget(Vector3 target)
    {
        this.target = target;
        distanceTravelled = 0f;
        totalDistance = Vector3.Distance(transform.position, target);
    }
}
