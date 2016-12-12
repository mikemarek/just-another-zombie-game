/**
* PlayerCameraComponent.cs
* Created by Michael Marek (2016)
*
* Manages the movement and various properties (such as zoom, position offet, and aiming bias when
* shouldering a weapon) of the camera that tracks this player. The component also provides several
* methods for affecting how the camera behaves, such as the ability to shake the camera, etc.
*
**/

using UnityEngine;
using System.Collections;

public class PlayerCameraComponent : MonoBehaviour
{
    [Header("Target Camera")]
    public  GameObject              cam;

    [Header("Toggle Functionality")]
    public  bool                    allowAimingBias = true;

    [Header("Tracking Properties")]
    public  float                   panningSpeed    = 0.2f;
    public  float                   aimingBias      = 0.75f;
    public  float                   zoom            = 0f;
    public  float                   shakeDecay      = 0.9f;
    [Space(10)]
    public  Vector3                 positionOffset  = Vector3.zero;
    public  Vector3                 rotationOffset  = Vector3.zero;

    private Vector3                 velocity;
    private Vector3                 shake;

    private PlayerInputComponent    input;


    /**
    * Obtain reference to the player's input component to calculate aiming bias. Set the inital
    * position of the camera.
    *
    * @param    null
    * @return   null
    **/
    void Start()
    {
        input = gameObject.GetComponent<PlayerInputComponent>();

        cam.transform.position = transform.position + positionOffset;
        cam.transform.rotation = Quaternion.Euler(rotationOffset.x, rotationOffset.y, rotationOffset.z);
    }


    /**
    * Position the camera based on the player it is tracking, camera offset, zoom, aiming bias, and
    * camera shake/recoil effects.
    *
    * @param    null
    * @return   null
    **/
    void LateUpdate()
    {
        Vector3 zooming = zoom * positionOffset;

        Vector3 aiming = allowAimingBias ? (Vector3)input.Aim * aimingBias : Vector3.zero;

        Vector3 target = transform.position + positionOffset + aiming + shake - zooming;
        target.z = positionOffset.z - zooming.z;

        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, target, ref velocity, panningSpeed);

        shake *= shakeDecay;
    }


    /**
    * Applies a recoil effect to the camera (swiftly moves the camera in a specific direction).
    *
    * @param    Vector3 directon of the recoil (non-normalized)
    * @return   null
    **/
    public void Recoil(Vector3 recoil)
    {
        shake += recoil * Time.deltaTime;
    }


    /**
    * Applies a shaking effect to the camera (jolts the camera in a random direction).
    *
    * @param    float   force multiplier for the shaking effect
    * @return   null
    **/
    public void Shake(float force)
    {
        shake += force * Random.onUnitSphere * Time.deltaTime;
    }
}
