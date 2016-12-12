/**
* AIMovementComponent.cs
* Created by Michael Marek
*
* A modified version of the player's movement component that allows artificial agents to move about
* the game world. Instead of moving the actor calculated from controller input, we use explicit methods
* that can be called from an external script and have the actor move based on that.
*
* The movement component is based off of a concept called "steering behaviours". To move the actor
* around the game world, you may call one or more of the "steering behaviour" methods to set the
* actors steering vector to your desired position, then call the Update() method to have the actor
* follow through with the motion.
*
* It should be noted that you should update the steering vector of the actor every frame by calling
* these behaviour methods, as it gets reset to zero after every frame.
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
    * Obtain a reference to the actor's rigid body so we can move the actor.
    *
    * @param    null
    * @return   null
    **/
    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }


    /**
    * Update the velocity and rotation of the actor based on the actor's steering vector.
    *
    * @param    null
    * @return   null
    **/
    void FixedUpdate()
    {
        velocity = rb.velocity;

        steering = Vector3.ClampMagnitude(steering, maximumVelocity);
        steering /= mass;

        velocity += steering;
        velocity = Vector3.ClampMagnitude(velocity, maximumVelocity);

        rb.velocity = velocity;
        steering = Vector3.zero;

        if (velocity.magnitude > Mathf.Epsilon)
        {
            Vector3 normal = velocity.normalized;
            float angle = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, rotation, turningSpeed));
        }

        //draw a line in the Unity scene editor denoting actor direction
        /*Vector3 A = transform.position - transform.forward;
        Vector3 B = A + 2f * transform.right;
        Debug.DrawLine(A, B, Color.red);*/
    }


    /**
    * Move and rotate the actor towards a designated target location.
    *
    * @param    Vector3 the target location the actor will move to
    * @param    float   the velocity at which to travel towards the target
    * @param    float   the actor will start to slow when within this distance from the target
    * @return   null
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
