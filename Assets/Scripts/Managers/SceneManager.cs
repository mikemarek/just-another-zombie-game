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
        }
    }


    public  Transform[] playerList  { get { return players.ToArray();   } }
    public  int         playerCount { get { return players.Count;       } }
}
