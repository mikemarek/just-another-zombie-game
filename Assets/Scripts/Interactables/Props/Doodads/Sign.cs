/**
* Sign.cs
* Created by Michael Marek (2016)
*
* Displays a message for an actor when interacted with.
**/

using UnityEngine;
using System.Collections;

public class Sign : Interactable
{
    [Space(10)]
    [TextArea]
    public string message;

    /**
    **/
    public override void Interact(GameObject go)
    {
        textMesh.text = message;

        InventoryComponent inventory = go.GetComponent<InventoryComponent>();
        PlayerCameraComponent cam = go.GetComponent<PlayerCameraComponent>();

        if (inventory != null)  inventory.allowUse = false;
        if (cam != null)        cam.zoom = 0.4f;
    }

    /**
    **/
    public override void Leave(GameObject go)
    {
        InventoryComponent inventory = go.GetComponent<InventoryComponent>();
        PlayerCameraComponent cam = go.GetComponent<PlayerCameraComponent>();

        if (inventory != null)  inventory.allowUse = true;
        if (cam != null)        cam.zoom = 0f;
    }
}
