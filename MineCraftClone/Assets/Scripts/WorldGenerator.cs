using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public ChunkGenerator chunk;
    public int WorldSize = 2;
    public float perlinOffsetX, perlinOffsetZ;
    public float noiseScale = 0.3f, offsetScale = 10.0f;//perlin noise in unity changes base on the decimals, offsetScale determines how much the noise changes the height

    public Transform player;
    public LayerMask layer;

    public CubeData[] CubeTypes;
    private readonly int chunkSize = 10;

    private Dictionary<Vector2, ChunkGenerator> worldMap;//used to keep track of chunks


    // Start is called before the first frame update
    void Start()
    {
        worldMap = new Dictionary<Vector2, ChunkGenerator>();
        perlinOffsetX = Random.Range(0f, 9999f);//for random world geration
        perlinOffsetZ = Random.Range(0f, 9999f);//for random world geration

        int start = (int)Mathf.Floor(WorldSize / 2f);
        int end = (int)Mathf.Ceil(WorldSize / 2f);
        for (int i = -start; i < end; i++) {
            for (int j = -start; j < end; j++) {
                Vector2 key = new Vector2(i * chunkSize, j * chunkSize);
                Vector3 pos = new Vector3(i * chunkSize, 0, j * chunkSize);
                worldMap.Add(key, Instantiate(chunk, pos, Quaternion.identity));//chunks are saved every chunkSize distance apart
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
