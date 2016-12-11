/**
* CameraManager.cs
* Created by Michael Marek (2016)
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

    private float   targetAspectRatio;
    private float   windowAspectRatio;
    private float   scaleHeight;
    private float   scaleWidth;
    private bool    landscape;

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
    **/
    void Awake()
    {
    }


    /**
    **/
    void Update()
    {
    }


    /**
    **/
    public void Initialize(Transform[] players)
    {
        cameras = new List<GameObject>();

        if (players.Length == 0)
        {
            Debug.Log("Please ensure at least one controller is connected!");
            return;
        }

        //add cameras+link them to players; add player UI elements; set splitscreen graphic
        switch (players.Length)
        {
            case 1:
                AddCameraToPlayer(VIEWPORT_FULLSCREEN, ref players[0], ref UIContainers[0]);
                splitscreenImage.sprite = emptyDivision;
            break;

            case 2:
                if (splitscreenPreference == SplitscreenPreference.Vertical)
                {
                    AddCameraToPlayer(VIEWPORT_LEFT,  ref players[0], ref UIContainers[0]);
                    AddCameraToPlayer(VIEWPORT_RIGHT, ref players[1], ref UIContainers[1]);
                    splitscreenImage.sprite = twoPlayerVerticalDivision;
                }
                else //SplitscreenPreference.Horizontal
                {
                    AddCameraToPlayer(VIEWPORT_TOP,    ref players[0], ref UIContainers[0]);
                    AddCameraToPlayer(VIEWPORT_BOTTOM, ref players[1], ref UIContainers[1]);
                    splitscreenImage.sprite = twoPlayerHorizontalDivision;
                }
            break;

            case 3:
                if (splitscreenPreference == SplitscreenPreference.Vertical)
                {
                    AddCameraToPlayer(VIEWPORT_LEFT,        ref players[0], ref UIContainers[0]);
                    AddCameraToPlayer(VIEWPORT_TOPRIGHT,    ref players[1], ref UIContainers[1]);
                    AddCameraToPlayer(VIEWPORT_BOTTOMRIGHT, ref players[2], ref UIContainers[2]);
                    splitscreenImage.sprite = threePlayerVerticalDivision;
                }
                else //SplitscreenPreference.Horizontal
                {
                    AddCameraToPlayer(VIEWPORT_TOP,         ref players[0], ref UIContainers[0]);
                    AddCameraToPlayer(VIEWPORT_BOTTOMLEFT,  ref players[1], ref UIContainers[1]);
                    AddCameraToPlayer(VIEWPORT_BOTTOMRIGHT, ref players[2], ref UIContainers[2]);
                    splitscreenImage.sprite = threePlayerHorizontalDivision;
                }
            break;

            case 4:
                AddCameraToPlayer(VIEWPORT_TOPLEFT,     ref players[0], ref UIContainers[0]);
                AddCameraToPlayer(VIEWPORT_TOPRIGHT,    ref players[1], ref UIContainers[1]);
                AddCameraToPlayer(VIEWPORT_BOTTOMLEFT,  ref players[2], ref UIContainers[2]);
                AddCameraToPlayer(VIEWPORT_BOTTOMRIGHT, ref players[3], ref UIContainers[3]);
                splitscreenImage.sprite = splitscreenDivision;
            break;
        }
    }


    /**
    **/
    private void AddCameraToPlayer(Rect view, ref Transform player, ref RectTransform container)
    {
        GameObject cam = AddCamera(view);
        cameras.Add(cam);

        AddHUD(view, ref player, ref container);
        AddInventory(view, ref player, ref container);

        PlayerCameraComponent pcc = player.gameObject.GetComponent<PlayerCameraComponent>();
        pcc.cam = cam;
    }


    /**
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
    **/
    private void AddHUD(Rect view, ref Transform player, ref RectTransform container)
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
    **/
    private void AddInventory(Rect view, ref Transform player, ref RectTransform container)
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
