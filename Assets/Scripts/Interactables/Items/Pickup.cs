/**
**/

using UnityEngine;
using System.Collections;

public class Pickup : Interactable
{
    [Header("Pickup Item")]
    public Item item;

    void Start()
    {
        Collider collider = null;
        foreach (Collider col in gameObject.GetComponents<Collider>())
        {
            if (col.isTrigger)
                continue;
            collider = col;
            break;
        }

        if (collider == null)
            return;

        GameManager gm = GameObject.Find("Game Manager").GetComponent<GameManager>();

        foreach (Transform player in gm.playerList)
        {
            Collider col = player.gameObject.GetComponent<Collider>();
            Physics.IgnoreCollision(collider, col, true);
        }
    }

    public override void Interact(GameObject go)
    {
        InventoryComponent inventory = go.GetComponent<InventoryComponent>();
        PlayerEquipmentComponent equipped = go.GetComponent<PlayerEquipmentComponent>();

        bool added = inventory.AddItem(item);

        if (!added)
        {
            StopInteracting();
            return;
        }

        Position slot = inventory.GetItemSlot(item.GetType());

        if (equipped.equipped == null)
            equipped.EquipItem(slot);
        else if (equipped.hotkeyedSlot.x < 0 || equipped.hotkeyedSlot.y < 0)
            equipped.hotkeyedSlot = slot;

        StopInteracting();

        if (added)
            Destroy(gameObject);
    }
}
