using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public GameObject chunk;
    public int WorldSize = 2;//number of chunks in active in the world
    public float perlinOffsetX, perlinOffsetZ, perlinSecondOff;
    public float noiseScale = 0.3f, offsetScale = 10.0f;//perlin noise in unity changes base on the decimals, offsetScale determines how much the noise changes the height

    public Transform player;
    public LayerMask layer;

    public CubeData[] CubeTypes;
    private readonly int chunkSize = 10;//number of blocks in a chunk
    private int start, end;
    private Vector2 lastPlayerPosition;

    private Dictionary<Vector2, GameObject> worldMap = new Dictionary<Vector2, GameObject>();//used to keep track of chunks
    private List<Vector2> activeChunks = new List<Vector2>();//keep track of active chunks
    //private List<Vector2> loadedChunks = new List<Vector2>();//to remove unactive chunks when they get two far away, maybe make anouther dictionary to store the byte array for each chunk so the game object can be deleted
    private List<Vector2> chunkQueue = new List<Vector2>();
    private bool isLoadingChunks;


    // Start is called before the first frame update
    void Start()
    {
        isLoadingChunks = false;
        perlinOffsetX = Random.Range(0f, 9999f);//for random world geration
        perlinOffsetZ = Random.Range(0f, 9999f);//for random world geration
        perlinSecondOff = Random.Range(0f, 9999f);
        lastPlayerPosition.x = player.position.x;
        lastPlayerPosition.y = player.position.z;

        start = (int)Mathf.Ceil(WorldSize / 2f);
        end = (int)Mathf.Ceil(WorldSize / 2f);
        for (int x = -start; x < end; x++) {
            for (int z = -start; z < end; z++) {
                Vector2 key = new Vector2(x, z);
                CreateChunk(key);
                activeChunks.Add(key);
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
        if (chunkQueue.Count > 0 && !isLoadingChunks) {
            StartCoroutine(LoadChunks());
        }

    }

    void LoadVisableChunks()//create or load in chunks
    {
        int xOff = Mathf.FloorToInt(player.position.x / chunkSize);//divide by chunkSize to change block pos to chunk pos
        int zOff = Mathf.FloorToInt(player.position.z / chunkSize);
        for (int x = -start + xOff; x < end + xOff; x++) {
            for (int z = -start + zOff; z < end + zOff; z++) {
                Vector2 key = new Vector2(x, z);
                if (!chunkQueue.Contains(key)) {//prevent key being added twice for not being done in the corutine
                    if (!worldMap.ContainsKey(key)) {//checks if chunk has already been made
                        chunkQueue.Add(key);//replaces create chunks cause we want these chunks to load in one at a time per frame
                    } else if (!worldMap[key].activeSelf) {//activeself is true if game object is activated
                        worldMap[key].SetActive(true);
                        activeChunks.Add(key);
                    }//else the chunk is already loaded and active
                }
            }
        }
    }

    void UnloadChunks()
    {
        List<Vector2> stillActiveChunks = new List<Vector2>();
        foreach (Vector2 activeChunkKey in activeChunks) {//check if the active chunks are too far away from the player
            if(Mathf.Abs(activeChunkKey.x - (player.position.x / chunkSize)) > WorldSize/2 || Mathf.Abs(activeChunkKey.y - (player.position.z / chunkSize)) > WorldSize/2) {//player.postion / chunksize = chunk pos
                worldMap[activeChunkKey].SetActive(false);
            } else {
                stillActiveChunks.Add(activeChunkKey);
            }
        }
        activeChunks = stillActiveChunks;
    }

    void CreateChunk(Vector2 key)
    {
        Vector3 pos = new Vector3(key.x * chunkSize, 0, key.y * chunkSize);
        GameObject curChunk = Instantiate(chunk, pos, Quaternion.identity);
        curChunk.transform.parent = transform;
        worldMap.Add(key, curChunk);//chunks # will be player position/10 rounded down
    }

    private IEnumerator LoadChunks()
    {
        isLoadingChunks = true;

        while(chunkQueue.Count > 0) {
            Vector2 key = chunkQueue[0];
            chunkQueue.RemoveAt(0);
            Vector3 pos = new Vector3(key.x * chunkSize, 0, key.y * chunkSize);
            GameObject curChunk = Instantiate(chunk, pos, Quaternion.identity);
            curChunk.transform.parent = transform;
            worldMap.Add(key, curChunk);
            activeChunks.Add(key);
            yield return null;
        }

        isLoadingChunks = false;
    }
}


//use this to change the cube in that postion
//worldMap[new Vector2(0, 0)].GetComponent<ChunkGenerator>().isCube[0, 0, 0];
//use this to remove/readd chunks
//worldMap[new Vector2(1, 1)].SetActive(false);
