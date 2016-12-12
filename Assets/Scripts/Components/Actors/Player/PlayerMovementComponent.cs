/**
* PlayerMovementComponent.cs
* Created by Michael Marek (2016)
*
* Using input recieved from the PlayerInputComponent, allows the player to move about the game
* world. We can perform several types of movement, including walking (when using an item or
* reloading a weapon), running, sprinting (with stamina), or performing a strafe while shouldering
* a weapon. We also modify the player's movement speed depending if they are currently shooting or
* reloading their weapon.
**/

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInputComponent))]
public class PlayerMovementComponent : MonoBehaviour
{
    [Header("Toggle Functionality")]
    public  bool                    allowMovement       = true;
    public  bool                    allowAiming         = true;
    [Space(8)]
    public  bool                    sprinting           = false;
    public  bool                    reloading           = false;
    public  bool                    shooting            = false;

    [Header("Movement Properties")]
    public  float                   walkingSpeed        = 2.0f;
    public  float                   joggingSpeed        = 4.5f;
    public  float                   sprintingSpeed      = 8f;
    [Space(8)]
    public  float                   maximumSpeed        = 10f;
    public  float                   dynamicFriction     = 0.7f;

    [Header("Aiming Properties")]
    public  float                   aimingSpeed         = 0.15f;
    public  float                   joggingTurnSpeed    = 0.15f;
    public  float                   sprintingTurnSpeed  = 0.05f;

    [Header("Sprinting Stamina")]
    public  float                   staminaDecay        = 1f;
    public  float                   staminaRegeneration = 1f;

    public  float                   shootingSpeed       { get; set; }
    public  float                   reloadingSpeed      { get; set; }

    private Vector3                 velocity            = Vector3.zero;
    private Quaternion              rotation            = Quaternion.identity;
    private float                   stamina             = 1f;
    private float                   epsilon             = 0.1f;

    private PlayerInputComponent    input;
    private Rigidbody               rb;
    private LineRenderer            line;


    /**
    * Initialize references to other player components.
    *
    * @param    null
    * @return   null
    **/
    void Start()
    {
        input = gameObject.GetComponent<PlayerInputComponent>();
        rb = gameObject.GetComponent<Rigidbody>();
        line = gameObject.GetComponent<LineRenderer>();
    }


    /**
    * Move the player about the game world based on inputs from the player's input component.
    *
    * @param    null
    * @return   null
    **/
    void FixedUpdate()
    {
        velocity = rb.velocity;
        rotation = gameObject.transform.rotation;

        //handle sprinting stamina decay and regeneration when sprinting/resting
        if (sprinting)
        {
            if (stamina > 0f)
            {
                stamina -= (1f / staminaDecay) * Time.deltaTime;
            }
            else if (stamina <= 0f)
            {
                stamina = 0f;
                sprinting = false;
            }
        }
        else
        {
            if (stamina < 1f)
                stamina += (1f / staminaRegeneration) * Time.deltaTime;
            else if (stamina > 1f)
                stamina = 1f;
        }

        //handle player movement whether they are sprinting, jogging, walking, or reloading
        if (allowMovement && input.Move.magnitude > 0f)
        {
            //sprinting
            if (sprinting)
            {
                float angle = Mathf.Atan2(input.Move.y, input.Move.x) * Mathf.Rad2Deg;
                TurnTowards(angle, sprintingTurnSpeed);

                angle = rotation.eulerAngles.z * Mathf.Deg2Rad;
                velocity.x = sprintingSpeed * Mathf.Cos(angle);
                velocity.y = sprintingSpeed * Mathf.Sin(angle);
                velocity.z = 0f;
            }
            else
            {
                //walking (aiming/shooting)
                if (allowAiming && input.Aim.magnitude > 0f)
                {
                    float angle = Mathf.Atan2(input.Aim.y, input.Aim.x) * Mathf.Rad2Deg;
                    TurnTowards(angle, aimingSpeed);

                    float speed = (reloading ? reloadingSpeed : shooting ? shootingSpeed : 1f);
                    velocity = (Vector3)(speed * walkingSpeed * input.Move.normalized);
                }
                //jogging
                else
                {
                    float angle = Mathf.Atan2(input.Move.y, input.Move.x) * Mathf.Rad2Deg;
                    TurnTowards(angle, joggingTurnSpeed);

                    float speed = (reloading ? reloadingSpeed : shooting ? shootingSpeed : 1f);
                    velocity = (Vector3)(speed * joggingSpeed * input.Move.normalized);
                }
            }
        }
        //idle - no movement
        else
        {
            if (allowAiming && input.Aim.magnitude > 0f)
            {
                float angle = Mathf.Atan2(input.Aim.y, input.Aim.x) * Mathf.Rad2Deg;
                TurnTowards(angle, aimingSpeed);
            }

            velocity = velocity * dynamicFriction;
        }

        //limit player velocity (maximum and minimum movement speeds)
        if (velocity.magnitude > maximumSpeed)
            velocity = maximumSpeed * velocity.normalized;
        else if (velocity.magnitude < epsilon)
            velocity = Vector3.zero;

        rb.velocity = velocity;
        rb.MoveRotation(rotation);

        /*if (input.Aim.magnitude > Mathf.Epsilon)
            LaserAiming();
        else
            line.SetPosition(1, transform.position - transform.forward);*/
    }


    /**
    * Rotate the player towards an angle at a specific speed. This method rotates the player by a
    * delta every frame, so it must be called continuously.
    *
    * @param    float   angle in which to rotate towards (Euler; 0...360 degrees)
    * @param    float   the speed at which to rotate towards the angle (0...1)
    * @return   null
    **/
    public void TurnTowards(float angle, float speed)
    {
        Quaternion direction = Quaternion.Euler(0f, 0f, angle);
        rotation = Quaternion.Slerp(rotation, direction, speed);
    }


    /**
    **/
    public void LaserAiming()
    {
        float laserRange = 50f;

        Vector3 start = transform.position - transform.forward;
        Vector3 end;

        int mask = Physics.AllLayers;
        mask = mask & ~(LayerMask.NameToLayer("Environment"));
        mask = mask & ~(LayerMask.NameToLayer("Player"));
        mask = mask & ~(LayerMask.NameToLayer("Zombie"));
        mask = mask & ~(LayerMask.NameToLayer("Prop"));

        RaycastHit info;
        bool hit = Physics.Raycast(start, transform.right, out info, laserRange, mask);

        end = hit ? info.point : start + laserRange * transform.right;

        line.SetPosition(1, end);
    }


    public Vector2      currentVelocity     { get { return rb.velocity;                     } }
    public Quaternion   currentRotation     { get { return gameObject.transform.rotation;   } }
    public float        sprintingStamina    { get { return stamina;                         } }
}
