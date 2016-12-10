using UnityEngine;
using System.Collections;

public class ControlPanel : Interactable
{
    [Space(10)]
    public  BigFuckingDoor  door;

    public override void Interact(GameObject go)
    {
        if (door.open)
            door.Close();
        else
            door.Open();
    }
}
