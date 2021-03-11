using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public GameObject chunk;
    public int WorldSize = 2;
    public float perlinOffsetX, perlinOffsetZ;
    public float noiseScale = 0.3f, offsetScale = 10.0f;//perlin noise in unity changes base on the decimals, offsetScale determines how much the noise changes the height

    public Transform player;
    public LayerMask layer;

    public CubeData[] CubeTypes;
    private readonly int chunkSize = 10;

    //private Dictionary<Vector3, Object> worldMap;


    // Start is called before the first frame update
    void Start()
    {
        perlinOffsetX = Random.Range(0f, 9999f);//for random world geration
        perlinOffsetZ = Random.Range(0f, 9999f);//for random world geration

        int start = (int)Mathf.Floor(WorldSize / 2);
        int end = (int)Mathf.Ceil(WorldSize / 2);
        for (int i = -start; i < end; i++) {
            for (int j = -start; j < end; j++) {
                Instantiate(chunk, new Vector3(i*chunkSize, 0, j*chunkSize), Quaternion.identity);
            }
        }

        //set player to ground here
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
