/**
**/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActorSpawnManager : MonoBehaviour
{
    public  GameObject          actorPrefab;
    public  float               spawnTime;
    public  float               spawnRadius;
    public  int                 maxActors;

    private List<GameObject>    spawned;

    private SceneManager        scene;

    void Start()
    {
        spawned = new List<GameObject>();

        scene = GameObject.Find("Scene Manager").GetComponent<SceneManager>();

        InvokeRepeating("SpawnActor", 0f, spawnTime);
    }

    void Update()
    {
        while (spawned.Contains(null))
            spawned.Remove(null);
    }

    private void SpawnActor()
    {
        if (spawned.Count < maxActors)
        {
            Vector3 spawn = (Vector3)(spawnRadius * Random.insideUnitCircle);

            GameObject actor = Instantiate(
                actorPrefab,
                transform.position + spawn,
                Quaternion.identity
            ) as GameObject;
            actor.transform.SetParent(scene.zombieContainer);

            spawned.Add(actor);
        }
    }
}
