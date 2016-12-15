/**
* SceneManager.cs
* Created by Michael Marek (2016)
*
* The scene manager contains references for all parent container game objects in the scene so that
* spawned objects can be localized within these containers. Also responsible for spawning the
* initial player Game Objects in the scene.
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
    * Initialize players in the game. The number of players initialized is (currently) dependent on
    * the number of gamepads plugged into the computer during game start.
    *
    * @param    null
    * @return   null
    **/
    public void InitializePlayers()
    {
        InputManager input = GameObject.Find("Input Manager").GetComponent<InputManager>();
        players = new List<Transform>();

        int slot = 0;
        Controller[] controllers = input.connectedControllers;

        for (int i = 0; i < controllers.Length; i++)
        {
            if (controllers[i] == null)
                continue;

            GameObject player = Instantiate(playerPrefab) as GameObject;
            player.transform.parent = playerContainer.transform;
            players.Add(player.transform);

            PlayerInputComponent pin = player.GetComponent<PlayerInputComponent>();
            pin.SetController(controllers[i]);
            pin.SetControllerSlot((ControllerSlot)(i + 1));

            slot++;
        }
    }


    public  Transform[] playerList  { get { return players.ToArray();   } }
    public  int         playerCount { get { return players.Count;       } }
}
