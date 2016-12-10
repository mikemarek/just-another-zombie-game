/**
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
        totalDistance = Vector3.Distance(transform.position, target);
    }
}
