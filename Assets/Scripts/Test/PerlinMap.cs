/**
**/

using UnityEngine;
using System.Collections;

public class PerlinMap : MonoBehaviour
{
    [Header("Map Dimensions")]
    public  int         mapWidth    = 10;
    public  int         mapHeight   = 10;

    [Header("Perlin Noise Properties")]
    public  float       noiseScale  = 50f;
    public  Vector2     noiseOrigin = new Vector2();

    private Renderer[,] chunks;

    private int[] count;

    /**
    **/
    void Start()
    {
        count = new int[6]{0, 0, 0, 0, 0, 0};

        //-----
        //noiseOrigin = new Vector2(Random.value * 100f, Random.value * 100f);
        //chunkScale += Random.value * -2f + 1f;
        //noiseScale += Random.value * -10f + 5f;
        //-----

        chunks = new Renderer[mapHeight, mapWidth];

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                GameObject chunk = GameObject.CreatePrimitive(PrimitiveType.Cube);
                chunk.transform.SetParent(transform);
                chunk.transform.position = new Vector3(
                    x - mapWidth / 2,
                    y - mapHeight / 2,
                    0f);

                Renderer renderer = chunk.GetComponent<Renderer>();
                chunks[y, x] = renderer;

                Vector2 pCoord = new Vector2(
                    noiseOrigin.x + noiseScale * x / mapWidth,
                    noiseOrigin.y + noiseScale * y / mapHeight);

                float sample = Mathf.PerlinNoise(pCoord.x, pCoord.y);

                //-----
                int i;
                float s = sample * 6f;
                if (s < 2f) i = 0;
                else if (s > 5f) i = 5;
                else i = (int)Mathf.Round(s-1f);
                count[i]++;
                //-----

                Vector3 position = chunks[y, x].gameObject.transform.position;
                //position.z = -sample;
                chunks[y, x].gameObject.transform.position = position;

                chunks[y, x].material.color = ((float)i * Color.white) / 5f;
            }
        }

        string output = string.Format("(x:{0}, y:{1}, w:{2}, h:{3}, s:{4}) = [",
            noiseOrigin.x, noiseOrigin.y, mapWidth, mapHeight, noiseScale);
        for (int i = 0; i < count.Length; i++)
            output += count[i].ToString() + (i < count.Length-1 ? ", " : "]");
        Debug.Log(output);
	}
}
