/**
**/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour
{
    public  enum SplitscreenPreference {
        Vertical,
        Horizontal
    };

    [Header("Camera Properties")]
    public  GameObject              cameraPrefab;
    public  Transform               cameraContainer;

    [Header("HUD and Inventory Displays")]
    public  GameObject              HUDPrefab;
    public  GameObject              inventoryPrefab;
    public  RectTransform[]         inventoryContainers;
    public  Vector2                 edgeSpacing;

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

    void Awake()
    {
        /*targetAspectRatio       = 16f / 9f;
        windowAspectRatio       = (float)Screen.width / (float)Screen.height;
        scaleHeight             = windowAspectRatio / targetAspectRatio;
        scaleWidth              = 1f / scaleHeight;
        landscape               = scaleHeight < 1f;

        if (scaleWidth >= 1f)
        {
            VIEWPORT_FULLSCREEN     = new Rect(0f, (1f - scaleHeight) / 2f, 1f, scaleHeight);
            //VIEWPORT_FULLSCREEN     = new Rect(0f, 0f, 1f, 1f);
            VIEWPORT_TOP            = new Rect(0f, 0f, 1f, 0.5f);
            VIEWPORT_BOTTOM         = new Rect(0f, 0.5f, 1f, 0.5f);
            VIEWPORT_LEFT           = new Rect(0f, 0f, 0.5f, 1f);
            VIEWPORT_RIGHT          = new Rect(0.5f, 0f, 0.5f, 1f);
            VIEWPORT_TOPLEFT        = new Rect(0f, 0f, 0.5f, 0.5f);
            VIEWPORT_TOPRIGHT       = new Rect(0.5f, 0f, 0.5f, 0.5f);
            VIEWPORT_BOTTOMLEFT     = new Rect(0f, 0.5f, 0.5f, 0.5f);
            VIEWPORT_BOTTOMRIGHT    = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
        }
        else
        {
            VIEWPORT_FULLSCREEN     = new Rect((1f - scaleWidth) / 2f, 0f, scaleWidth, 1f);
            //VIEWPORT_FULLSCREEN     = new Rect(0f, 0f, 1f, 1f);
            VIEWPORT_TOP            = new Rect(0f, 0f, 1f, 0.5f);
            VIEWPORT_BOTTOM         = new Rect(0f, 0.5f, 1f, 0.5f);
            VIEWPORT_LEFT           = new Rect(0f, 0f, 0.5f, 1f);
            VIEWPORT_RIGHT          = new Rect(0.5f, 0f, 0.5f, 1f);
            VIEWPORT_TOPLEFT        = new Rect(0f, 0f, 0.5f, 0.5f);
            VIEWPORT_TOPRIGHT       = new Rect(0.5f, 0f, 0.5f, 0.5f);
            VIEWPORT_BOTTOMLEFT     = new Rect(0f, 0.5f, 0.5f, 0.5f);
            VIEWPORT_BOTTOMRIGHT    = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
        }*/
    }


    void Update()
    {
        /*float t = 16f / 9f;
        float w = (float)Screen.width / (float)Screen.height;
        float y = w / t;
        float x = 1f / y;

        Debug.Log(System.String.Format("{0}, {1}, {2}, {3}", t, w, y, x));*/
    }


    /**
    **/
    public void Initialize(List<Transform> players)
    {
        cameras = new List<GameObject>();

        if (players.Count == 0)
        {
            Debug.Log("Please ensure at least one controller is connected!");
            return;
        }

        //set splitscreen image
        if (players.Count == 2)
        {
            if (splitscreenPreference == SplitscreenPreference.Vertical)
                splitscreenImage.sprite = twoPlayerVerticalDivision;
            else //splitscreenPreference == SplitscreenPreference.Horizontal
                splitscreenImage.sprite = twoPlayerHorizontalDivision;
        }
        else if (players.Count == 3)
        {
            if (splitscreenPreference == SplitscreenPreference.Vertical)
                splitscreenImage.sprite = threePlayerVerticalDivision;
            else //splitscreenPreference == SplitscreenPreference.Horizontal
                splitscreenImage.sprite = threePlayerHorizontalDivision;
        }
        else if (players.Count == 4)
        {
            splitscreenImage.sprite = splitscreenDivision;
        }
        else
        {
            splitscreenImage.sprite = emptyDivision;
        }

        //---------------------------

        GameObject cam;
        GameObject inv;

        /*TrackingCamera cam1;
        TrackingCamera cam2;
        TrackingCamera cam3;
        TrackingCamera cam4;*/

        //---------------------------

        Rect view = VIEWPORT_FULLSCREEN;

        cam = AddCamera(view);
        //cam.AddTarget(players[0]);
        cameras.Add(cam);

        Vector3[] corners = new Vector3[4];
        (inventoryContainers[0].parent as RectTransform).GetLocalCorners(corners);

        GameObject inventory = Instantiate(inventoryPrefab);
        RectTransform invRect = inventory.GetComponent<RectTransform>();

        inventoryContainers[0].anchorMin = new Vector2(view.x, view.y);
        inventoryContainers[0].anchorMax = new Vector2(view.x + view.width, view.y + view.height);
        inventoryContainers[0].offsetMin = Vector2.zero;
        inventoryContainers[0].offsetMax = Vector2.zero;

        PlayerInventoryDisplay PID = inventory.GetComponent<PlayerInventoryDisplay>();
        PID.inventory = players[0].GetComponent<InventoryComponent>();

        Vector2 invSize = invRect.rect.size;

        invRect.SetParent(inventoryContainers[0], false);
        invRect.localPosition = new Vector3(
            corners[0].x + (invSize.x / 2f) + edgeSpacing.x,
            corners[0].y - (invSize.y / 2f) - edgeSpacing.y,
            0f);

        PID.openPosition = new Vector3(
            invRect.localPosition.x,
            corners[0].y + (invSize.y / 2f) + edgeSpacing.y,
            0f);
        PID.closedPosition = invRect.localPosition;

        //---------------------------

        GameObject HUD = Instantiate(HUDPrefab);
        RectTransform HUDRect = HUD.GetComponent<RectTransform>();

        inventoryContainers[0].anchorMin = new Vector2(view.x, view.y);
        inventoryContainers[0].anchorMax = new Vector2(view.x + view.width, view.y + view.height);
        inventoryContainers[0].offsetMin = Vector2.zero;
        inventoryContainers[0].offsetMax = Vector2.zero;

        PlayerHeadsUpDisplay PUD = HUD.GetComponent<PlayerHeadsUpDisplay>();
        PUD.inventory = players[0].GetComponent<InventoryComponent>();
        PUD.equipment = players[0].GetComponent<PlayerEquipmentComponent>();
        PUD.health = players[0].GetComponent<HealthComponent>();
        PUD.progress = players[0].GetComponent<ProgressComponent>();

        Vector2 HUDSize = HUDRect.rect.size;

        HUDRect.SetParent(inventoryContainers[0], false);
        HUDRect.localPosition = new Vector3(
            corners[0].x + (HUDSize.x / 2f) + edgeSpacing.x,
            corners[0].y + (HUDSize.y / 2f) + edgeSpacing.y,
            0f);

        PUD.openPosition = HUDRect.localPosition;
        PUD.closedPosition = new Vector3(
            HUDRect.localPosition.x,
            corners[0].y - (HUDSize.y / 2f) - edgeSpacing.y,
            0f);

        //---------------------------

        PlayerCameraComponent pcc = players[0].GetComponent<PlayerCameraComponent>();
        pcc.cam = cam;

        //---------------------------

        //add adjust camera viewports depending on splitscreen settings
        /*switch (players.Count)
        {
            case 1:
                cam = AddCamera(VIEWPORT_FULLSCREEN);
                cam.AddTarget(players[0]);
                cameras.Add(cam);
            break;

            case 2:
                if (splitscreenPreference == SplitscreenPreference.Vertical)
                {
                    cam1 = AddCamera(VIEWPORT_LEFT);
                    cam2 = AddCamera(VIEWPORT_RIGHT);
                    cam1.AddTarget(players[0]);
                    cam2.AddTarget(players[1]);
                    cameras.Add(cam1);
                    cameras.Add(cam2);
                }
                else //splitscreenPreference == SplitscreenPreference.Horizontal
                {
                    cam1 = AddCamera(VIEWPORT_TOP);
                    cam2 = AddCamera(VIEWPORT_BOTTOM);
                    cam1.AddTarget(players[0]);
                    cam2.AddTarget(players[1]);
                    cameras.Add(cam1);
                    cameras.Add(cam2);
                }
            break;

            case 3:
                if (splitscreenPreference == SplitscreenPreference.Vertical)
                {
                    cam1 = AddCamera(VIEWPORT_LEFT);
                    cam2 = AddCamera(VIEWPORT_TOPRIGHT);
                    cam3 = AddCamera(VIEWPORT_BOTTOMLEFT);
                    cam1.AddTarget(players[0]);
                    cam2.AddTarget(players[1]);
                    cam2.AddTarget(players[2]);
                    cameras.Add(cam1);
                    cameras.Add(cam2);
                    cameras.Add(cam3);
                }
                else //splitscreenPreference == SplitscreenPreference.Horizontal
                {
                    cam1 = AddCamera(VIEWPORT_TOP);
                    cam2 = AddCamera(VIEWPORT_TOPRIGHT);
                    cam3 = AddCamera(VIEWPORT_BOTTOMLEFT);
                    cam1.AddTarget(players[0]);
                    cam2.AddTarget(players[1]);
                    cam2.AddTarget(players[2]);
                    cameras.Add(cam1);
                    cameras.Add(cam2);
                    cameras.Add(cam3);
                }
            break;

            case 4:
                cam1 = AddCamera(VIEWPORT_TOPLEFT);
                cam2 = AddCamera(VIEWPORT_TOPRIGHT);
                cam3 = AddCamera(VIEWPORT_BOTTOMLEFT);
                cam4 = AddCamera(VIEWPORT_BOTTOMRIGHT);
                cam1.AddTarget(players[0]);
                cam2.AddTarget(players[1]);
                cam2.AddTarget(players[2]);
                cam2.AddTarget(players[3]);
                cameras.Add(cam1);
                cameras.Add(cam2);
                cameras.Add(cam3);
                cameras.Add(cam4);
            break;
        }*/

        //add masks and player HUDs/inventories to screen regions
        //...
    }

    /**
    **/
    private GameObject AddCamera(Rect viewport)
    {
        GameObject go = Instantiate(cameraPrefab);
        go.transform.parent = cameraContainer.transform;

        Camera cam = go.GetComponent<Camera>();
        cam.rect = viewport;

        return go; //.GetComponent<TrackingCamera>();
    }
}
