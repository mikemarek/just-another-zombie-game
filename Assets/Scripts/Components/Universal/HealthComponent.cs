/**
* HealthComponent.cs
* Created by Michael Marek (2016)
*
* Manages an object's health and destroy the object if it falls below zero.
**/

using UnityEngine;
using System.Collections;

public class HealthComponent : MonoBehaviour
{
    public  GameObject  impact      = null;         //doodad spawned when damage is taken
    public  Color       colour      = Color.red;    //colour of displayed health bar (if any)
    public  float       health      = 100;
    public  float       maxHealth   = 100;


    /**
    * Heal the object for a specified amount.
    *
    * @param    float   amount to heal by
    * @return   null
    **/
    public void Heal(float amount)
    {
        health += amount;
        if (health > maxHealth)
            health = maxHealth;
    }


    /**
    * Damage the object.
    *
    * @param    float   amount to damage by
    * @param    Vector3 location of damage (for spawning damage graphic)
    * @return   null
    **/
    public void Damage(float amount, Vector3 point)
    {
        health -= amount;

        SceneManager scene = GameObject.Find("Scene Manager").GetComponent<SceneManager>();
        GameObject go = Instantiate(impact, point, Quaternion.identity) as GameObject;
        go.transform.parent = scene.projectileContainer;

        if (health <= 0f)
        {
            health = 0f;
            Kill();
        }
    }


    /**
    * Kill the object and perform any cessation-of-existance-related actions.
    *
    * @param    null
    * @return   null
    **/
    public virtual void Kill()
    {
        Destroy(gameObject);
    }


    /**
    * High velocity impact splatter.
    *
    * @param    Collision   object containing information about the collision that just happened
    * @return   null
    **/
    /**
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag != "Prop" && collision.collider.tag != "Projectile")
            return;

        if (collision.relativeVelocity.magnitude > 5f)

        Rigidbody rb = gameObject.GetComponent<Rigidbody>();

        float massRatio = collision.rigidbody.mass / rb.mass;
        float damage = massRatio * collision.rigidbody.velocity.magnitude;
        //Debug.Log(damage);
    }
    **/
}
