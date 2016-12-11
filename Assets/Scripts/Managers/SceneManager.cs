/**
* SceneManager.cs
* Created by Michael Marek (2016)
*
* Contains references for all parent container game objects in the scene so that spawned objects
* can be localized within these containers.
**/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneManager : MonoBehaviour
{
    [Header("Prefabs")]
    public  GameObject              playerPrefab;
    public  GameObject              zombiePrefab;

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
    public void InitializePlayers()
    {
        players = new List<Transform>();

        string[] joynames = Input.GetJoystickNames();
        for (int i = 0; i < joynames.Length; i++)
        {
            if (joynames[i] == "")
                continue;

            GameObject player = Instantiate(playerPrefab) as GameObject;
            player.transform.parent = playerContainer.transform;
            players.Add(player.transform);

            PlayerInputComponent input = player.GetComponent<PlayerInputComponent>();
            input.controllerSlot = (Controller.ControllerSlot)(i + 1);

            switch (joynames[i])
            {
                case "PLAYSTATION(R)3 Controller": //PS3
                    input.controllerType = Controller.ControllerType.PS3;
                break;

                case "Wireless Controller": //PS4
                    input.controllerType = Controller.ControllerType.PS4;
                break;

                case "XBox 360": //XBox 360
                    input.controllerType = Controller.ControllerType.XBox360;
                break;

                default:
                    input.controllerType = Controller.ControllerType.None;
                break;
            }
        }
    }


    /**
    **/
    public  Transform[] playerList  { get { return players.ToArray();   } }
    public  int         playerCount { get { return players.Count;       } }
}
