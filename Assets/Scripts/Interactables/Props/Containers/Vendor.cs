/**
* Vendor.cs
* Created by Michael Marek (2016)
*
* A vendor simply stores a collection of the same item type and dispenses them one at a time.
**/

using UnityEngine;
using System.Collections;

public class Vendor : Interactable
{
    [Space(10)]
    [Range(1, 99)]
    public  int         itemCount;          //how many items are available
    public  GameObject  vendorItem;         //the item that is dispensed
    public  Transform   dispensingPoint;    //where the item is dispensed from

    /**
    **/
    public override void Interact(GameObject go)
    {
        if (itemCount <= 0)
            return;

        SceneManager scene = GameObject.Find("Scene Manager").GetComponent<SceneManager>();
        GameObject dropped = Instantiate(vendorItem) as GameObject;
        Rigidbody rb = dropped.GetComponent<Rigidbody>();

        dropped.transform.SetParent(scene.propContainer);
        dropped.transform.position = dispensingPoint.position;
        dropped.transform.rotation = UnityEngine.Random.rotation;

        //apply a force to the item to make it shoot out from the vendor
        Vector3 velocity = -transform.up;
        rb.velocity = velocity;

        //apply some angular momentum to make it go "woosh"
        Vector3 angular = UnityEngine.Random.insideUnitSphere;
        rb.angularVelocity = angular;

        itemCount--;
    }
}
