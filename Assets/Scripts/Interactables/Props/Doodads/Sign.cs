using UnityEngine;
using System.Collections;

public class Sign : Interactable
{
    [Space(10)]
    [TextArea]
    public  string message;

    public override void Interact(GameObject go)
    {
        textMesh.text = message;

        PlayerCameraComponent cam = go.GetComponent<PlayerCameraComponent>();
        cam.zoom = 0.3f;
    }

    public override void Leave(GameObject go)
    {

        PlayerCameraComponent cam = go.GetComponent<PlayerCameraComponent>();
        cam.zoom = 0f;
    }
}
