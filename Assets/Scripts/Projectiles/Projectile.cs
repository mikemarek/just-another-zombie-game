/**
* Projectile.cs
* Created by Michael Marek (2016)
*
* Dynamic projectile base class for weapons performing projectile attacks.
**/

using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public  float           muzzleVelocity  = 1f;           //initial velocity of the projectile
    public  float           maximumRange    = 10f;          //maximum range of the projectile
    public  float           impactDamage    = 1f;           //how much damage the projectile does
    public  float           impactForce     = 0f;           //magnitude of force applied to target

    private GameObject      shotBy          = null;         //who shot this projectile?
    private Vector3         shotDirection   = Vector3.zero; //direction in which to shoot
    private float           travelDistance  = 0f;           //total distance travelled so far

    private Rigidbody       rb;


    /**
    * Set initial projectile properties.
    *
    * @param    null
    * @return   null
    **/
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        shotDirection = new Vector2(
            Mathf.Cos(gameObject.transform.eulerAngles.z * Mathf.Deg2Rad),
            Mathf.Sin(gameObject.transform.eulerAngles.z * Mathf.Deg2Rad)
        );

        rb.freezeRotation = true;
        rb.velocity = muzzleVelocity * shotDirection;
    }


    /**
    * Move the projectile through the game world.
    *
    * @param    null
    * @return   null
    **/
    void FixedUpdate()
    {
        travelDistance += muzzleVelocity * Time.deltaTime;
        if (travelDistance > maximumRange)
            OnTimeout();
    }


    /**
    * If an inherited class needs to initialize any properties, they can do it here.
    *
    * @param    null
    * @return   null
    **/
    protected virtual void Initialize()
    {
    }


    /**
    * What to do when the projectile collides with a target.
    *
    * @param    GameObject  the target that we collided with
    * @return   null
    **/
    protected virtual void OnImpact(GameObject hit)
    {
        Destroy(gameObject);
    }


    /**
    * What to do when the projectile travels past its maximum range.
    *
    * @param    null
    * @return   null
    **/
    protected virtual void OnTimeout()
    {
        Destroy(gameObject);
    }


    /**
    * Impact with the target when their Game Object collides with ours.
    *
    * @param    Collider    the collider of the target that we hit
    * @return   null
    **/
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == shotBy)
            return;

        OnImpact(collider.gameObject);
    }


    /**
    * Impact with the target when their Game Object collides with ours.
    *
    * @param    Collision   an object with information about the collision that just happened
    * @return   null
    **/
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == shotBy)
            return;

        OnImpact(collision.gameObject);
    }


    /**
    * Set the owner of the projectile. The projectile will not collide with its owner, so we can
    * use this method to avoid collisions with the actor that initially spawns the projectile.
    *
    * @param    GameObject  the actor that spawned this projectile
    * @return   null
    **/
    public void SetOwner(GameObject owner)
    {
        Collider self = gameObject.GetComponent<Collider>();
        Collider other = owner.GetComponent<Collider>();

        Physics.IgnoreCollision(self, other, true);

        if (shotBy != null)
        {
            Collider previous = shotBy.GetComponent<Collider>();
            Physics.IgnoreCollision(self, previous, false);
        }

        shotBy = owner;
    }
}
