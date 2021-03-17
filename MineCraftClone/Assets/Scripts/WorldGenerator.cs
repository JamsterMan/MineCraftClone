using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public GameObject chunk;
    public int WorldSize = 2;//number of chunks in active in the world
    public float perlinOffsetX, perlinOffsetZ;
    public float noiseScale = 0.3f, offsetScale = 10.0f;//perlin noise in unity changes base on the decimals, offsetScale determines how much the noise changes the height

    public Transform player;
    public LayerMask layer;

    public CubeData[] CubeTypes;
    private readonly int chunkSize = 10;//number of blocks in a chunk
    private int start, end;
    private Vector2 lastPlayerPosition;

    private Dictionary<Vector2, GameObject> worldMap = new Dictionary<Vector2, GameObject>();//used to keep track of chunks
    private List<Vector2> activeChunks = new List<Vector2>();


    // Start is called before the first frame update
    void Start()
    {
        //worldMap = new Dictionary<Vector2, GameObject>();
        //activeChunks = new List<GameObject>();
        perlinOffsetX = Random.Range(0f, 9999f);//for random world geration
        perlinOffsetZ = Random.Range(0f, 9999f);//for random world geration
        lastPlayerPosition.x = player.position.x;
        lastPlayerPosition.y = player.position.z;

        start = (int)Mathf.Floor(WorldSize / 2f);
        end = (int)Mathf.Ceil(WorldSize / 2f);
        for (int x = -start; x < end; x++) {
            for (int z = -start; z < end; z++) {
                Vector2 key = new Vector2(x, z);
                CreateChunk(key, x, z);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lastPlayerPosition.x != player.position.x || lastPlayerPosition.y != player.position.z) {//check if the player hased moved
            lastPlayerPosition.x = player.position.x;
            lastPlayerPosition.y = player.position.z;
            LoadVisableChunks();
            UnloadChunks();
        }

    }

    void LoadVisableChunks()//create or load in chunks
    {
        int xOff = Mathf.FloorToInt(player.position.x / chunkSize);//divide by chunkSize to change block pos to chunk pos
        int zOff = Mathf.FloorToInt(player.position.z / chunkSize);
        for (int x = -start + xOff; x < end + xOff; x++) {
            for (int z = -start + zOff; z < end + zOff; z++) {
                Vector2 key = new Vector2(x, z);
                if (!worldMap.ContainsKey(key)) {//checks if chunk has already been made
                    CreateChunk(key, x, z);
                } else if (!worldMap[key].activeSelf) {//activeself is true if game object is activated
                    worldMap[key].SetActive(true);
                }//else the chunk is already loaded and active
            }
        }
    }

    void UnloadChunks()
    {
        foreach (Vector2 activeChunkKey in activeChunks) {//check if the active chunks are too far away from the player
            if(Mathf.Abs(activeChunkKey.x - (player.position.x / chunkSize)) > WorldSize/2 || Mathf.Abs(activeChunkKey.y - (player.position.z / chunkSize)) > WorldSize/2) {//player.postion / chunksize = chunk pos
                worldMap[activeChunkKey].SetActive(false);
            }
        }
    }

    void CreateChunk(Vector2 key, int x, int z)
    {
        Vector3 pos = new Vector3(x * chunkSize, 0, z * chunkSize);
        GameObject curChunk = Instantiate(chunk, pos, Quaternion.identity);
        curChunk.transform.parent = transform;
        worldMap.Add(key, curChunk);//chunks # will be player position/10 rounded down
        activeChunks.Add(key);
    }
}


//use this to change the cube in that postion
//worldMap[new Vector2(0, 0)].GetComponent<ChunkGenerator>().isCube[0, 0, 0];
//use this to remove/readd chunks
//worldMap[new Vector2(1, 1)].SetActive(false);
