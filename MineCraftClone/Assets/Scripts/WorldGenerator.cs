using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public GameObject chunk;
    public int WorldSize = 2;
    public float perlinOffsetX, perlinOffsetZ;
    public float noiseScale = 0.3f, offsetScale = 10.0f;//perlin noise in unity changes base on the decimals, offsetScale determines how much the noise changes the height

    public CubeData[] CubeTypes;
    private readonly int chunkSize = 10;


    // Start is called before the first frame update
    void Start()
    {
        perlinOffsetX = Random.Range(0f, 9999f);//for random world geration
        perlinOffsetZ = Random.Range(0f, 9999f);//for random world geration

        for (int i = 0; i < WorldSize; i++) {
            for (int j = 0; j < WorldSize; j++) {
                Instantiate(chunk, new Vector3(i*chunkSize, 0, j*chunkSize), Quaternion.identity);
            }
        }
        //Instantiate(chunk, new Vector3(0, 0, 5), Quaternion.identity);
        //Instantiate(chunk, new Vector3(5, 0, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
