/**
**/

using UnityEngine;
using System.Collections;

public class HealthComponent : MonoBehaviour
{
    public  GameObject  impact      = null;
    public  Color       colour      = Color.red;
    public  float       health      = 100;
    public  float       maxHealth   = 100;

    /**
    **/
    public void Heal(float amount)
    {
        health += amount;
        if (health > maxHealth)
            health = maxHealth;
    }

    /**
    **/
    public void Damage(float amount, Vector3 point)
    {
        health -= amount;

        GameManager gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        GameObject go = Instantiate(impact, point, Quaternion.identity) as GameObject;
        go.transform.parent = gm.projectileContainer;

        if (health <= 0f)
        {
            health = 0f;
            Kill();
        }
    }

    /**
    **/
    public virtual void Kill()
    {
        Destroy(gameObject);
    }

    /**
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag != "Prop" && collision.collider.tag != "Projectile")
            return;

        if (collision.relativeVelocity.magnitude > 5f)

        Rigidbody rb = gameObject.GetComponent<Rigidbody>();

        float massRatio = collision.rigidbody.mass / rb.mass;
        float damage = massRatio * collision.rigidbody.velocity.magnitude;
        Debug.Log(damage);
    }
    **/
}
