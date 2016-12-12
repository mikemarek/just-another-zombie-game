/**
* CameraManager.cs
* Created by Michael Marek (2016)
*
* The Camera manager links, well, cameras to the player. When initialized, it takes into account
* the number of players that will be playing locally, and renders each camera to a different
* quadrant of the screen (depending player splitscreen preferences). It also attaches the player
* HUD and inventory UI elements and links them to their respective components.
**/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour
{
    public enum SplitscreenPreference {
        Vertical,
        Horizontal
    };

    [Header("Camera Properties")]
    public  GameObject              cameraPrefab;
    public  Transform               cameraContainer;

    [Header("HUD and Inventory Display Prefabs")]
    public  GameObject              HUDDisplay;
    public  GameObject              inventoryDisplay;
    public  Vector2                 UISpacing;
    public  RectTransform[]         UIContainers;

    [Header("Splitscreen Preferences")]
    public  Image                   splitscreenImage;
    public  SplitscreenPreference   splitscreenPreference;

    [Header("Splitscreen Graphics")]
    public  Sprite                  emptyDivision;
    public  Sprite                  twoPlayerVerticalDivision;
    public  Sprite                  twoPlayerHorizontalDivision;
    public  Sprite                  threePlayerVerticalDivision;
    public  Sprite                  threePlayerHorizontalDivision;
    public  Sprite                  splitscreenDivision;

    private List<GameObject>        cameras;
    private List<GameObject>        HUDs;
    private List<GameObject>        inventories;

    private Rect    VIEWPORT_FULLSCREEN     = new Rect(0f, 0f, 1f, 1f);
    private Rect    VIEWPORT_TOP            = new Rect(0f, 0.5f, 1f, 0.5f);
    private Rect    VIEWPORT_BOTTOM         = new Rect(0f, 0f, 1f, 0.5f);
    private Rect    VIEWPORT_LEFT           = new Rect(0f, 0f, 0.5f, 1f);
    private Rect    VIEWPORT_RIGHT          = new Rect(0.5f, 0f, 0.5f, 1f);
    private Rect    VIEWPORT_TOPLEFT        = new Rect(0f, 0.5f, 0.5f, 0.5f);
    private Rect    VIEWPORT_TOPRIGHT       = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
    private Rect    VIEWPORT_BOTTOMLEFT     = new Rect(0f, 0f, 0.5f, 0.5f);
    private Rect    VIEWPORT_BOTTOMRIGHT    = new Rect(0.5f, 0f, 0.5f, 0.5f);


    /**
    * Attaches cameras and UI elements (HUD and inventory display) to each player spawned in the
    * game. Each camera is also mapped to a specific region on the screen depending on number of
    * players present and splitscreen preferences.
    *
    * @param    Transform[] list of current players in the game
    * @return   null
    **/
    public void Initialize(Transform[] players)
    {
        cameras = new List<GameObject>();

        if (players.Length == 0)
        {
            Debug.Log("Please ensure at least one controller is connected!");
            return;
        }

        switch (players.Length)
        {
            case 1:
                AddCameraToPlayer(VIEWPORT_FULLSCREEN, players[0], UIContainers[0]);
                splitscreenImage.sprite = emptyDivision;
            break;

            case 2:
                if (splitscreenPreference == SplitscreenPreference.Vertical)
                {
                    AddCameraToPlayer(VIEWPORT_LEFT,  players[0], UIContainers[0]);
                    AddCameraToPlayer(VIEWPORT_RIGHT, players[1], UIContainers[1]);
                    splitscreenImage.sprite = twoPlayerVerticalDivision;
                }
                else //SplitscreenPreference.Horizontal
                {
                    AddCameraToPlayer(VIEWPORT_TOP,    players[0], UIContainers[0]);
                    AddCameraToPlayer(VIEWPORT_BOTTOM, players[1], UIContainers[1]);
                    splitscreenImage.sprite = twoPlayerHorizontalDivision;
                }
            break;

            case 3:
                if (splitscreenPreference == SplitscreenPreference.Vertical)
                {
                    AddCameraToPlayer(VIEWPORT_LEFT,        players[0], UIContainers[0]);
                    AddCameraToPlayer(VIEWPORT_TOPRIGHT,    players[1], UIContainers[1]);
                    AddCameraToPlayer(VIEWPORT_BOTTOMRIGHT, players[2], UIContainers[2]);
                    splitscreenImage.sprite = threePlayerVerticalDivision;
                }
                else //SplitscreenPreference.Horizontal
                {
                    AddCameraToPlayer(VIEWPORT_TOP,         players[0], UIContainers[0]);
                    AddCameraToPlayer(VIEWPORT_BOTTOMLEFT,  players[1], UIContainers[1]);
                    AddCameraToPlayer(VIEWPORT_BOTTOMRIGHT, players[2], UIContainers[2]);
                    splitscreenImage.sprite = threePlayerHorizontalDivision;
                }
            break;

            case 4:
                AddCameraToPlayer(VIEWPORT_TOPLEFT,     players[0], UIContainers[0]);
                AddCameraToPlayer(VIEWPORT_TOPRIGHT,    players[1], UIContainers[1]);
                AddCameraToPlayer(VIEWPORT_BOTTOMLEFT,  players[2], UIContainers[2]);
                AddCameraToPlayer(VIEWPORT_BOTTOMRIGHT, players[3], UIContainers[3]);
                splitscreenImage.sprite = splitscreenDivision;
            break;
        }
    }


    /**
    * Add a camera and UI elements to a specific player.
    *
    * @param    Rect            region of the screen that the camera will render to
    * @param    Transform       the player the camera and UI elements will be linked to
    * @param    RectTransform   container Game Object for holding player UI elements
    * @return   null
    **/
    private void AddCameraToPlayer(Rect view, Transform player, RectTransform container)
    {
        GameObject cam = AddCamera(view);
        cameras.Add(cam);

        AddHUD(view, player, container);
        AddInventory(view, player, container);

        //link player camera controller to camera'[;0]
        PlayerCameraComponent pcc = player.gameObject.GetComponent<PlayerCameraComponent>();
        pcc.cam = cam;
    }


    /**
    * Creates and returns a new instance of the camera prefab.
    *
    * @param    Rect    screen region that the camera will render to
    * @return   null
    **/
    private GameObject AddCamera(Rect view)
    {
        GameObject go = Instantiate(cameraPrefab);
        go.transform.parent = cameraContainer.transform;

        Camera cam = go.GetComponent<Camera>();
        cam.rect = view;

        return go;
    }


    /**
    * Add a HUD UI element and link it to a specific player and their components.
    *
    * @param    Rect            player camera viewport dimensions (so we know where to place stuff)
    * @param    Transform       the player the HUD UI element will be linked to
    * @param    RectTransform   container Game Object for holding the HUD UI element
    * @return   null
    **/
    private void AddHUD(Rect view, Transform player, RectTransform container)
    {
        GameObject hud = Instantiate(HUDDisplay);
        RectTransform transform = hud.GetComponent<RectTransform>();

        //link the HUD UI to the palyer's various components
        PlayerHeadsUpDisplay pud = hud.GetComponent<PlayerHeadsUpDisplay>();
        pud.inventory = player.GetComponent<InventoryComponent>();
        pud.equipment = player.GetComponent<PlayerEquipmentComponent>();
        pud.health = player.GetComponent<HealthComponent>();
        pud.progress = player.GetComponent<ProgressComponent>();

        //match the size of the UI to that of the camera viewport size
        container.anchorMin = new Vector2(view.x, view.y);
        container.anchorMax = new Vector2(view.x + view.width, view.y + view.height);
        container.offsetMin = Vector2.zero;
        container.offsetMax = Vector2.zero;

        //get the corner coordinates for the screen section in which to draw the camera and UI
        //corners[0] = bottom-left; clockwise-rotation for subsequent elements
        Vector3[] corners = new Vector3[4];
        (container.parent as RectTransform).GetLocalCorners(corners);

        //size of the inventory UI element
        Vector2 size = transform.rect.size;

        //set position of the inventory UI relative to the viewport window
        transform.SetParent(container, false);
        transform.localPosition = new Vector3(
            corners[0].x * view.width  + (size.x / 2f) + UISpacing.x,
            corners[0].y * view.height + (size.y / 2f) + UISpacing.y,
            0f);

        pud.openPosition = transform.localPosition;
        pud.closedPosition = new Vector3(
            transform.localPosition.x,
            corners[0].y * view.height - (size.y / 2f) - UISpacing.y,
            0f);
    }


    /**
    * Add an inventory UI element and link it to a specific player and their components.
    *
    * @param    Rect            player camera viewport dimensions (so we know where to place stuff)
    * @param    Transform       the player the inventory UI element will be linked to
    * @param    RectTransform   container Game Object for holding the inventory UI element
    * @return   null
    **/
    private void AddInventory(Rect view, Transform player, RectTransform container)
    {
        GameObject inventory = Instantiate(inventoryDisplay);
        RectTransform transform = inventory.GetComponent<RectTransform>();

        //link the inventory UI to the player's inventory component
        PlayerInventoryDisplay pid = inventory.GetComponent<PlayerInventoryDisplay>();
        pid.inventory = player.GetComponent<InventoryComponent>();

        //get the corner coordinates for the screen section in which to draw the camera and UI
        //corners[0] = bottom-left; clockwise-rotation for subsequent elements
        Vector3[] corners = new Vector3[4];
        (container.parent as RectTransform).GetLocalCorners(corners);

        //match the size of the UI to that of the camera viewport size
        container.anchorMin = new Vector2(view.x, view.y);
        container.anchorMax = new Vector2(view.x + view.width, view.y + view.height);
        container.offsetMin = Vector2.zero;
        container.offsetMax = Vector2.zero;

        //size of the inventory UI element
        Vector2 size = transform.rect.size;

        //set position of the inventory UI relative to the viewport window
        transform.SetParent(container, false);
        transform.localPosition = new Vector3(
            corners[0].x * view.width  + (size.x / 2f) + UISpacing.x,
            corners[0].y * view.height - (size.y / 2f) - UISpacing.y,
            0f);

        pid.openPosition = new Vector3(
            transform.localPosition.x,
            corners[0].y * view.height + (size.y / 2f) + UISpacing.y,
            0f);
        pid.closedPosition = transform.localPosition;
    }
}
