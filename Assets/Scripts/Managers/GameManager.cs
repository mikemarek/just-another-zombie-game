/**
* GameManager.cs
* Created by Michael Marek (2016)
*
* The game manager is responsible for all of the highest-level game logic, and initializes all
* other managers resonsible for running the game.
**/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    public  InputManager            inputManager;
    public  SceneManager            sceneManager;
    public  CameraManager           cameraManager;
    public  LevelManager            levelManager;
    public  DataManager             dataManager;


    /**
    * Aquire references to all other managers and have them initialize the game world.
    *
    * @param    null
    * @return   null
    **/
    void Awake()
    {
        if (inputManager == null)  inputManager = GameObject.Find("Input Manager").GetComponent<InputManager>();
        if (sceneManager == null)  sceneManager = GameObject.Find("Scene Manager").GetComponent<SceneManager>();
        if (cameraManager == null) cameraManager = GameObject.Find("Camera Manager").GetComponent<CameraManager>();
        //if (levelManager == null)  levelManager = GameObject.Find("Level Manager").GetComponent<LevelManager>();
        if (dataManager == null)   dataManager = GameObject.Find("Data Manager").GetComponent<DataManager>();

        inputManager.InitializeControllers();
        sceneManager.InitializePlayers();
        cameraManager.Initialize(sceneManager.playerList);
    }


    /**
    * COMMENT
    *
    * @param    null
    * @return   null
    **/
    void Start()
    {
    }


    /**
    * COMMENT
    *
    * @param    null
    * @return   null
    **/
    void Update()
    {
    }
}
