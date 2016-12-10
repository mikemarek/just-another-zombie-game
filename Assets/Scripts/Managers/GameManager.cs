/**
**/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("Prefabs")]
    public  GameObject              playerPrefab;
    public  GameObject              zombiePrefab;

    [Header("Managers")]
    public  CameraManager           cameraManager;

    [Header("Object Containers")]
    public  Transform               playerContainer;
    public  Transform               NPCContainer;
    public  Transform               zombieContainer;
    public  Transform               projectileContainer;
    public  Transform               propContainer;
    public  Transform               environmentContainer;
    public  Transform               canvasContainer;

    private List<Transform>         players;

    /**
    **/
    void Awake()
    {
        //must have this GameObject as root parent in heirarchy!
        //DontDestroyOnLoad(gameObject);

        if (cameraManager == null)
            cameraManager = GameObject.Find("Camera Manager").GetComponent<CameraManager>();

        InitializePlayers();

        cameraManager.Initialize(players);

        //InitializeHUD();
    }

    /**
    **/
    void Start()
    {
    }

    /**
    **/
    void Update()
    {
    }

    /**
    **/
    private void InitializePlayers()
    {
        players = new List<Transform>();

        string[] joysticks = Input.GetJoystickNames();
        for (int i = 0; i < joysticks.Length; i++)
        {
            if (joysticks[i] == "")
                continue;

            GameObject player = Instantiate(playerPrefab) as GameObject;
            player.transform.parent = playerContainer.transform;
            players.Add(player.transform);

            PlayerInputComponent input = player.GetComponent<PlayerInputComponent>();

            input.controllerSlot = (Controller.ControllerSlot)(i + 1);

            switch (joysticks[i])
            {
                case "Wireless Controller": //PS4
                    input.controllerType = Controller.ControllerType.PS4;
                break;

                case "PS3": //PS3
                    input.controllerType = Controller.ControllerType.PS3;
                break;

                case "XBox 360": //XBox 360
                    input.controllerType = Controller.ControllerType.XBox360;
                break;
            }
        }
    }

    /**
    **/
    public  Transform[]         playerList  { get { return players.ToArray();   } }
    public  int                 playerCount { get { return players.Count;       } }
}
