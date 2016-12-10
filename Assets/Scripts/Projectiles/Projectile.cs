/*
*/

using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(Transform))]
//[RequireComponent(typeof(Rigidbody))]
//[RequireComponent(typeof(LineRenderer))]
public class Projectile : MonoBehaviour
{
    public  float           muzzleVelocity  = 1f;
    public  float           maximumRange    = 10f;
    public  float           impactDamage    = 1f;
    public  float           impactForce     = 0f;

    private GameObject      shotBy          = null;
    private Vector3         shotDirection   = Vector3.zero;
    private float           travelDistance  = 0f;

    private Rigidbody       rb;

    /**
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
    **/
    void FixedUpdate()
    {
        travelDistance += muzzleVelocity * Time.deltaTime;
        if (travelDistance > maximumRange)
            OnTimeout();
    }

    /**
    **/
    protected virtual void Initialize()
    {
    }

    /**
    **/
    protected virtual void OnImpact(GameObject hit)
    {
        Destroy(gameObject);
    }

    /**
    **/
    protected virtual void OnTimeout()
    {
        Destroy(gameObject);
    }

    /**
    **/
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == shotBy)
            return;

        OnImpact(collider.gameObject);
    }

    /**
    **/
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == shotBy)
            return;

        OnImpact(GetComponent<Collider>().gameObject);
    }

    /**
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
