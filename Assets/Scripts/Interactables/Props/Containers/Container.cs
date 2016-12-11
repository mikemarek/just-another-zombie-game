/**
**/

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class Container : Interactable
{
    [Header("Container Display Prefab")]
    public  GameObject          displayPrefab;
    public  Transform           displayLocation;

    [Header("Container Inventory")]
    public  InventoryComponent  inventory;

    [Header("Initial Inventory Items")]
    public  MonoScript[]        startingItems;

    private GameObject          display;

    private Dictionary<GameObject, Position> registeredPlayers;

    void Start()
    {
        registeredPlayers = new Dictionary<GameObject, Position>();

        GameObject go = GameObject.Find("Scene Manager");
        SceneManager sm = go.GetComponent<SceneManager>();

        display = Instantiate(displayPrefab) as GameObject;
        display.transform.SetParent(sm.canvasContainer);
        display.transform.position = displayLocation.position;

        ContainerInventoryDisplay container = display.GetComponent<ContainerInventoryDisplay>();
        container.inventory = inventory;
        container.location = displayLocation;

        for (int i = 0; i < startingItems.Length; i++)
            if (startingItems[i] != null)
                inventory.AddItem(startingItems[i].GetClass());
    }

    void OnDestroy()
    {
        Destroy(display);
    }

    public override void Interact(GameObject go)
    {
        ActorControllerComponent controller = go.GetComponent<ActorControllerComponent>();

        controller.GotoState(new PlayerManageContainerState(this, inventory));
    }

    public bool Register(GameObject player)
    {
        Position openSlot = OpenInventorySlot();
        registeredPlayers.Add(player, openSlot);
        inventory.beingManaged = true;
        StartCoroutine(FadeOutText());
        return true;
    }

    public void Unregister(GameObject player)
    {
        if (registeredPlayers.ContainsKey(player))
            registeredPlayers.Remove(player);
        if (registeredPlayers.Count == 0)
        {
            inventory.beingManaged = false;
            StartCoroutine(FadeInText());
        }
    }

    public void SetPlayerPosition(GameObject player, Position position)
    {
        if (registeredPlayers.ContainsKey(player))
            registeredPlayers[player] = position;
    }

    public Position GetPlayerPosition(GameObject player)
    {
        if (registeredPlayers.ContainsKey(player))
            return registeredPlayers[player];
        return new Position(-1, -1);
    }

    private Position OpenInventorySlot()
    {
        for (int y = 0; y < inventory.size.y; y++)
            for (int x = 0; x < inventory.size.x; x++)
                if (registeredPlayers.ContainsValue(new Position(x, y)))
                    continue;
                else
                    return new Position(x, y);
        return new Position(0, 0);
    }

    public Dictionary<GameObject, Position> players { get { return registeredPlayers; } }
}
