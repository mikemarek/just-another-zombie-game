/**
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
    **/
    void Start()
    {
        input = gameObject.GetComponent<PlayerInputComponent>();

        cam.transform.position = transform.position + positionOffset;
        cam.transform.rotation = Quaternion.Euler(rotationOffset.x, rotationOffset.y, rotationOffset.z);
    }

    /**
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
    **/
    public void Recoil(Vector3 recoil)
    {
        shake += recoil * Time.deltaTime;
    }

    /**
    **/
    public void Shake(float amount)
    {
        shake += amount * Time.deltaTime * Random.onUnitSphere;
    }
}
