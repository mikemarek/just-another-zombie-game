/**
**/

using UnityEngine;
using System.Collections;

public class AIMovementComponent : MonoBehaviour
{
    [Header("Steering Behaviour Properties")]
    public  float       maximumVelocity = 5f;
    //public  float       maximumSteering = 2f;
    public  float       turningSpeed    = 0.2f;
    public  float       mass            = 1f;

    private Vector3     velocity        = Vector3.zero;
    private Vector3     steering        = Vector3.zero;

    private Rigidbody   rb;

    /**
    **/
    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    /**
    **/
    public void UpdateSteering()
    {
        velocity = rb.velocity;

        steering = Vector3.ClampMagnitude(steering, maximumVelocity);
        steering /= mass;

        velocity += steering;
        velocity = Vector3.ClampMagnitude(velocity, maximumVelocity);

        rb.velocity = velocity;
        steering = Vector3.zero;

        //Debug.Log(velocity.magnitude);

        //----------

        int mask = Physics.AllLayers;
        mask = mask & ~(LayerMask.NameToLayer("Environment"));
        mask = mask & ~(LayerMask.NameToLayer("Zombie"));
        mask = mask & ~(LayerMask.NameToLayer("Player"));
        mask = mask & ~(LayerMask.NameToLayer("Prop"));

        RaycastHit info;
        //bool hit = Physics.Raycast(transform.position, );

        Vector3 A = transform.position - transform.forward;
        Vector3 B = A + 2f * transform.right;

        Debug.DrawLine(A, B, Color.red);

        //----------

        if (velocity.magnitude > Mathf.Epsilon)
        {
            Vector3 normal = velocity.normalized;
            float angle = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
            //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turningSpeed);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, rotation, turningSpeed));
        }
    }

    /**
    **/
    public void Seek(Vector3 target, float speed, float slowingRadius=0f)
    {
        Vector3 desired = target - transform.position;
        float distance = desired.magnitude;
        desired.Normalize();

        if (distance <= slowingRadius)
            desired *= speed * (distance / slowingRadius);
        else
            desired *= speed;

        steering += desired - velocity;
    }

    /**
    **/
    public void Flee()
    {
    }

    /**
    **/
    public void Pursue()
    {
    }

    /**
    **/
    public void Evade()
    {
    }
}
