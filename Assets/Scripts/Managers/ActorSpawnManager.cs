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

    private GameManager         gm;

    void Start()
    {
        spawned = new List<GameObject>();

        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();

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
            actor.transform.SetParent(gm.zombieContainer);

            spawned.Add(actor);
        }
    }
}
