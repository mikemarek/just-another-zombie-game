/**
**/

using System;
using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    [Header("Tracking Properties")]
    public  GameObject[]    players;

    [Header("Level Dimensions")]
    public  int             levelWidth      = 190;
    public  int             levelHeight     = 152;

    [Header("Perlin Map Properties")]
    public  float           noiseScale      = 10f;
    public  Vector2         originPoint     = Vector2.zero;

    [Header("Chunk Properties")]
    public  GameObject      container;
    public  Vector2         dimensions;
    public  GameObject[]    chunks;

    private GameObject[,]   levelChunks;
    private int[,]          chunkTypes;
    private byte[,][]       chunkSeeds;
    private int[,][]        lootTable;
    private Vector2         loadingRadius;


    /**
    **/
    void Start()
    {
        levelChunks     = new GameObject[levelHeight, levelWidth];
        chunkTypes      = new int[levelHeight, levelWidth];
        chunkSeeds      = new byte[levelHeight, levelWidth][];
        loadingRadius   = 1.5f * dimensions;

        GenerateLevelData();
        StartCoroutine(UpdateLevel());
    }


    /**
    **/
    void OnDestroy()
    {
        StopCoroutine(UpdateLevel());
    }


    /**
    **/
    private IEnumerator UpdateLevel()
    {
        while (true)
        {
            Vector2 chunkCenter;
            Vector2 distance;

            for (int y = 0; y < levelHeight; y++)
            {
                for (int x = 0; x < levelWidth; x++)
                {
                    bool unload = false;

                    for (int p = 0; p < players.Length; p++)
                    {
                        chunkCenter.x = x * dimensions.x;
                        chunkCenter.y = y * dimensions.y;


                        distance.x = Mathf.Abs(players[p].transform.position.x - chunkCenter.x);
                        distance.y = Mathf.Abs(players[p].transform.position.y - chunkCenter.y);

                        if ((distance.x > loadingRadius.x + 0.5f * dimensions.x) ||
                            (distance.y > loadingRadius.y + 0.5f * dimensions.y))
                            unload = true;

                        if (distance.x < loadingRadius.x - 0.5f * dimensions.x)
                        {
                            if (distance.y < loadingRadius.y - 0.5f * dimensions.y)
                            {
                                unload = false;
                                LoadChunk(x, y);
                                break;
                            }
                        }
                    }

                    if (unload)
                        UnloadChunk(x, y);
                }
            }

            yield return new WaitForSeconds(0.5f);
        }
    }


    /**
    **/
    private void GenerateLevelData()
    {
        //int[] count = new int[6]{0, 0, 0, 0, 0, 0};

        for (int y = 0; y < levelHeight; y++)
        {
            for (int x = 0; x < levelWidth; x++)
            {
                Vector2 coord = new Vector2(
                    originPoint.x + noiseScale * x / levelWidth,
                    originPoint.y + noiseScale * y / levelHeight);

                float sample = Mathf.PerlinNoise(coord.x, coord.y);
                float scaled = sample * (float)chunks.Length;

                int level;
                if (scaled < 2f)
                    level = 0;
                else if (scaled > chunks.Length-1)
                    level = chunks.Length-1;
                else
                    level = (int)Mathf.Round(scaled-1f);

                //count[level]++;

                levelChunks[y, x] = null;
                //chunkTypes[y, x] = level;
                chunkTypes[y, x] = 0;
                //chunkSeeds[y, x] = GenerateRandomSeed(x, y);
            }
        }

        /*string output = string.Format("(x:{0}, y:{1}, w:{2}, h:{3}, s:{4}) = [",
            originPoint.x, originPoint.y, levelWidth, levelHeight, noiseScale);
        for (int i = 0; i < count.Length; i++)
            output += count[i].ToString() + (i < count.Length-1 ? ", " : "]");
        Debug.Log(output);*/
    }


    /**
    **/
    public void LoadChunk(int x, int y)
    {
        if (levelChunks[y, x] != null)
            return;

        GameObject go = GameObject.Instantiate(chunks[chunkTypes[y, x]], container.transform) as GameObject;
        go.transform.position = new Vector3(dimensions.x * x, dimensions.y * y, 0f);
        levelChunks[y, x] = go;

        Chunk chunk = go.GetComponent<Chunk>();
        chunk.InitializeSeed(ref chunkSeeds[y, x]);
        chunk.Initialize(chunkSeeds[y, x]);
    }


    /**
    **/
    public void UnloadChunk(int x, int y)
    {
        if (levelChunks[y, x] != null)
        {
            Destroy(levelChunks[y, x]);
            levelChunks[y, x] = null;
        }
    }


    /**
    **/
    public byte[] GenerateRandomSeed(int x, int y)
    {
        System.Random rng = new System.Random();

        byte[] bytes;

        switch (chunkTypes[y, x])
        {
            default:
                bytes = new byte[51];
                rng.NextBytes(bytes);
            break;
        }

        //turn roadways on/off depending on adjacent chunks
        //

        return bytes;
    }
}
