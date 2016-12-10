using UnityEngine;
using System.Collections;

public class Vehicle : MonoBehaviour
{
    [Space(10)]
    public  bool        smoothSteering          = true;
    public  bool        safeSteering            = true;
    [Space(10)]
    public  float       mass                    = 1200f;        //vehicle mass
    public  float       inertiaScale            = 1f;           //mass and inertia multiplier
    [Space(10)]
    public  Transform   centerOfMass            = null;         //center of mass for vehicle
    public  Transform   frontAxle               = null;         //front wheel axle position
    public  Transform   rearAxle                = null;         //rear wheel axle position
    //public  Transform   frontBumper             = null;         //front bumper position
    //public  Transform   rearBumper              = null;         //rear bumper position
    [Space(10)]
    public  float       wheelRadius             = 0.3f;         //tire radius
    [Space(10)]
    public  float       wheelGrip               = 2.0f;         //how much grip tires have
    public  float       lockingGrip             = 0.7f;         //amount of grip available when wheels are locked
    public  float       weightTransfer          = 0.2f;         //amount of weight transferred during acceleration/braking
    [Space(10)]
    public  float       topSpeed                = 10f;          //maximum vehicle velocity
    public  float       engineForce             = 8000f;        //engine power
    public  float       brakingForce            = 12000f;       //braking force
    public  float       eBrakeForce             = 6000f;        //e-brake force
    [Space(10)]
    public  float       maxSteering             = 0.6f;         //maximum steering angle [in radians]
    public  float       frontCornerStiffness    = 5f;           //front axle corning stiffness
    public  float       rearCornerStiffness     = 6f;           //rear axle cornering stiffness
    [Space(10)]
    public  float       airResistance           = 5f;           //amount of drag acting on the vehicle
    public  float       rollingResistance       = 30f;          //rolling friction produced by the wheels
    public  float       idleResistance          = 0.99f;        //idle friction produced when there is no throttle/brake
    [Space(10)]
    public  float       runOverSpeed            = 3f;           //how fast we must be going before we start crushing stuff
    public  float       smooshMultiplier        = 10f;          //damage multiplier applied to velocity when running stuff over

    private float       steeringWheel           = 0f;           //input - steering [-1...+1]
    private float       gasPedal                = 0f;           //input - throttle [0...+1]
    private float       brakePedal              = 0f;           //input - braking [0...+1]
    private float       eBrakePedal             = 0f;           //input - e-brake [0...+1]

    private float       bearing                 = 0f;           //vehicle direction [in radians]
    private float       cgToFrontAxle           = 0f;           //distance from center of mass to front axle
    private float       cgToRearAxle            = 0f;           //distance from center of mass to rear axle
    private Vector2     velocity                = Vector2.zero; //vehicle velocity in world coordinates
    private Vector2     localVelocity           = Vector2.zero; //vehicle velocity in local frame
    private Vector2     acceleration            = Vector2.zero; //vehicle acceleration in world coordinates
    private Vector2     localAcceleration       = Vector2.zero; //vehicle acceleration in local frame
    private float       yawRate                 = 0f;           //vehicle rotation rate
    private float       steeringAngle           = 0f;           //direction of the steering wheel

    private bool        gear                    = true;         //forward/reverse gear
    private float       inertia                 = 0f;           //vehicle inertia
    private float       wheelBase               = 0f;           //distance between axles
    private float       frontAxleWeightRatio    = 0f;           //weight ratio applied to front axle
    private float       rearAxleWeightRatio     = 0f;           //weight ratio applied to rear axle

    private float       epsilon                 = 0.1f;         //reduce linear/angular velocity of below threshold

    private Rigidbody   rb;

    private float maxSpeed = 0f;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        cgToFrontAxle = Vector2.Distance(centerOfMass.position, frontAxle.position);
        cgToRearAxle = Vector2.Distance(centerOfMass.position, rearAxle.position);

        bearing = transform.eulerAngles.z * Mathf.Deg2Rad;
        inertia = mass * inertiaScale;
        wheelBase = cgToFrontAxle + cgToRearAxle;
        frontAxleWeightRatio = cgToFrontAxle / wheelBase;
        rearAxleWeightRatio = cgToRearAxle / wheelBase;

        Vector3 com = transform.InverseTransformPoint(centerOfMass.position);
        rb.centerOfMass = com;
        //mass = rb.mass;
    }

    /**
    **/
    void FixedUpdate()
    {
        //forward gear
        if (gasPedal > 0f && brakePedal == 0f)
            gear = true;
        //reverse gear
        else if (gasPedal == 0f && brakePedal > 0f)
            gear = false;

        bearing = transform.eulerAngles.z * Mathf.Deg2Rad;
        steeringAngle = steeringWheel * maxSteering;

        //vehicle velocity in local coordinates
        localVelocity.x = Mathf.Cos(bearing) * velocity.x + Mathf.Sin(bearing) * velocity.y;
        localVelocity.y = Mathf.Cos(bearing) * velocity.y - Mathf.Sin(bearing) * velocity.x;

        //weight on axles based on center of gravity and weight shift due to forward/reverse acceleration
        float frontAxleWeight = mass * (frontAxleWeightRatio * Physics.gravity.z - weightTransfer * localAcceleration.x * Mathf.Abs(centerOfMass.position.z) / wheelBase);
        float rearAxleWeight = mass * (rearAxleWeightRatio * Physics.gravity.z + weightTransfer * localAcceleration.x * Mathf.Abs(centerOfMass.position.z) / wheelBase);

        //velocity of the wheels as a result of the yaw of the vehicle
        float frontYaw = cgToFrontAxle * yawRate;
        float rearYaw = -cgToRearAxle * yawRate;

        //slipping angles for front and rear wheels
        float frontSlippingAngle = Mathf.Atan2(localVelocity.y + frontYaw, Mathf.Abs(localVelocity.x)) - Mathf.Sign(localVelocity.x) * steeringAngle;
        float rearSlippingAngle = Mathf.Atan2(localVelocity.y + rearYaw, Mathf.Abs(localVelocity.x));

        //
        float frontWheelGrip = wheelGrip;
        float rearWheelGrip = wheelGrip * (1f - eBrakePedal * (1f - lockingGrip));

        //
        float frontWheelFriction = Mathf.Clamp(-frontCornerStiffness * frontSlippingAngle, -frontWheelGrip, frontWheelGrip) * frontAxleWeight;
        float rearWheelFriction = Mathf.Clamp(-rearCornerStiffness * rearSlippingAngle, -rearWheelGrip, rearWheelGrip) * rearAxleWeight;

        //calculate braking and throttle forces
        float brake;
        float throttle;
        if (gear)
        {
            brake = Mathf.Min(brakePedal * brakingForce + eBrakePedal * eBrakeForce, brakingForce);
            throttle = gasPedal * engineForce;
        }
        else
        {
            brake = -Mathf.Min(gasPedal * brakingForce + eBrakePedal * eBrakeForce, brakingForce);
            throttle = brakePedal * -0.4f * engineForce;
        }

        //local force applied to vehicle (RWD)
        /*Vector2 tractionForce = new Vector2(
            throttle - brake * Mathf.Sign(localVelocity.x),
            0f
        );*/

        //local force applied to vehicle (RWD)
        Vector2 tractionForce = new Vector2(
            throttle - brake * Mathf.Sign(localVelocity.x),
            0f
        );

        //drag force applied to the vehicle
        Vector2 dragForce = new Vector2(
            -rollingResistance * localVelocity.x - airResistance * localVelocity.x * Mathf.Abs(localVelocity.x),
            -rollingResistance * localVelocity.y - airResistance * localVelocity.y * Mathf.Abs(localVelocity.y)
        );

        //total force applied to the vehicle
        Vector2 totalForce = new Vector2(
            dragForce.x + tractionForce.x,
            dragForce.y + tractionForce.y + Mathf.Cos(steeringAngle) * frontWheelFriction + rearWheelFriction
        );

        //acceleration along vehicle axis
        localAcceleration = totalForce / mass;

        //acceleration in world coordinates
        acceleration.x = Mathf.Cos(bearing) * localAcceleration.x - Mathf.Sin(bearing) * localAcceleration.y;
        acceleration.y = Mathf.Sin(bearing) * localAcceleration.x + Mathf.Cos(bearing) * localAcceleration.y;

        //update vehicle velocity
        velocity = rb.velocity;
        velocity += acceleration * Time.deltaTime;

        float torque = (frontWheelFriction + tractionForce.y) * cgToFrontAxle - rearWheelFriction * cgToRearAxle;
        float angularAcceleration = torque / inertia;

        if (velocity.magnitude < 1f && throttle == 0f)
        {
            velocity = Mathf.Lerp(velocity.magnitude, 0f, 0.1f) * velocity.normalized;
            torque = Mathf.Lerp(torque, 0f, 0.1f);
            yawRate = Mathf.Lerp(yawRate, 0f, 0.1f);
        }

        if (gasPedal == 0f && brakePedal == 0f && eBrakePedal == 0f)
            rb.velocity *= idleResistance;

        /*if (velocity.magnitude > topSpeed)
            velocity = topSpeed * velocity.normalized;*/

        yawRate += angularAcceleration * Time.deltaTime;
        bearing += yawRate * Time.deltaTime;

        if (localAcceleration.magnitude < Mathf.Epsilon)
            localAcceleration = Vector2.zero;
        if (Mathf.Abs(yawRate) < Mathf.Epsilon)
            yawRate = 0f;

        rb.AddRelativeForce(localAcceleration * mass);
        rb.angularVelocity = new Vector3(0f, 0f, yawRate);
    }

    /**
    **/
    public virtual void ApplyGas(float amount)
    {
        gasPedal = Mathf.Clamp(amount, 0f, 1f);

        if (rb.velocity.magnitude > maxSpeed)
            maxSpeed = rb.velocity.magnitude;
        //Debug.Log(maxSpeed);
    }

    /**
    **/
    public virtual void ApplyBrakes(float amount)
    {
        brakePedal = Mathf.Clamp(amount, 0f, 1f);
    }

    /**
    **/
    public virtual void ApplyEBrakes(float amount)
    {
        eBrakePedal = Mathf.Clamp(amount, 0f, 1f);
    }

    /**
    **/
    public virtual void Steer(float amount)
    {
        steeringWheel = -Mathf.Clamp(amount, -1f, 1f);

        if (smoothSteering)
            steeringWheel = SmoothSteering(steeringWheel);
        if (safeSteering)
            steeringWheel = SafeSteering(steeringWheel);
    }

    /**
    **/
    private float SmoothSteering(float steeringInput)
    {
        float steering = 0f;

        if (Mathf.Abs(steeringInput) > 0f)
        {
            steering = Mathf.Clamp(steeringWheel + steeringInput * 2f * Time.deltaTime, -1f, 1f);
        }
        else
        {
            if (steeringWheel > 0f)
                steering = Mathf.Max(steeringWheel - Time.deltaTime, 0f);
            else if (steeringWheel < 0f)
                steering = Mathf.Max(steeringWheel + Time.deltaTime, 0f);
        }

        return steering;
    }

    /**
    **/
    private float SafeSteering(float steeringInput)
    {
        float avel = Mathf.Min(velocity.magnitude, 250f);
        return steeringInput * (1f - (avel / 280f));
    }

    /**
    **/
    void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag != "Zombie") // && collision.collider.tag != "Player")
            return;

        if (rb.velocity.magnitude < runOverSpeed)
            return;

        float massRatio = rb.mass / collision.rigidbody.mass;
        float damage = /*massRatio * */rb.velocity.magnitude;
        Debug.Log(damage);

        HealthComponent health = collision.gameObject.GetComponent<HealthComponent>();
        health.Damage(damage, collision.contacts[0].point);
    }
}
