using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public ChunkGenerator chunk;
    public int WorldSize = 2;//number of chunks in active in the world

    public int seed;
    [Range(0, 1)]
    public float perlinAmpScale;
    public float noiseScale = 0.3f, offsetScale = 10.0f;//perlin noise in unity changes base on the decimals, offsetScale determines how much the noise changes the height

    public Transform player;
    public LayerMask layer;

    public CubeData[] CubeTypes;
    public float perlinOffsetX, perlinOffsetZ, perlinFreqScale;

    private readonly int chunkSize = 10;//number of blocks in a chunk
    private int start, end;
    private Vector2Int lastPlayerChunk;

    private Dictionary<Vector2Int, ChunkGenerator> worldMap = new Dictionary<Vector2Int, ChunkGenerator>();//used to keep track of chunks
    private List<Vector2Int> activeChunks = new List<Vector2Int>();//keep track of active chunks
    //private List<Vector2> loadedChunks = new List<Vector2>();//to remove unactive chunks when they get two far away, maybe make anouther dictionary to store the byte array for each chunk so the game object can be deleted
    private List<Vector2Int> chunkQueue = new List<Vector2Int>();
    private bool isLoadingChunks;


    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(seed);
        isLoadingChunks = false;
        perlinOffsetX = Random.Range(0f, 9999f);//for random world geration
        perlinOffsetZ = Random.Range(0f, 9999f);//for random world geration

        start = (int)Mathf.Ceil(WorldSize / 2f);
        end = (int)Mathf.Ceil(WorldSize / 2f);
        for (int x = -start; x < end; x++) {
            for (int z = -start; z < end; z++) {
                Vector2Int key = new Vector2Int(x, z);
                CreateChunk(key);
                activeChunks.Add(key);
            }
        }
        
        //player.position = new Vector3(5f,,5f);//use perlin noise function to find the correct player hieght

        lastPlayerChunk.x = Mathf.FloorToInt(player.position.x / chunkSize);
        lastPlayerChunk.y = Mathf.FloorToInt(player.position.z / chunkSize);
    }

    // Update is called once per frame
    void Update()
    {
        if (lastPlayerChunk.x != Mathf.FloorToInt(player.position.x / chunkSize) || lastPlayerChunk.y != Mathf.FloorToInt(player.position.z / chunkSize)) {//check if the player hased moved to a different chunk
            lastPlayerChunk.x = Mathf.FloorToInt(player.position.x / chunkSize);
            lastPlayerChunk.y = Mathf.FloorToInt(player.position.z / chunkSize);
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
                Vector2Int key = new Vector2Int(x, z);
                if (!chunkQueue.Contains(key)) {//prevent key being added twice for not being done in the corutine
                    if (!worldMap.ContainsKey(key)) {//checks if chunk has already been made
                        chunkQueue.Add(key);//replaces create chunks cause we want these chunks to load in one at a time per frame
                    } else if (!worldMap[key].chunkObject.activeSelf) {//activeself is true if game object is activated
                        worldMap[key].chunkObject.SetActive(true);
                        activeChunks.Add(key);
                    }//else the chunk is already loaded and active
                }
            }
        }
    }

    void UnloadChunks()//unload chunks that are too far away
    {
        List<Vector2Int> stillActiveChunks = new List<Vector2Int>();
        int xOff = Mathf.FloorToInt(player.position.x / chunkSize);//divide by chunkSize to change block pos to chunk pos
        int zOff = Mathf.FloorToInt(player.position.z / chunkSize);
        foreach (Vector2Int activeChunkKey in activeChunks) {//check if the active chunks are too far away from the player
            if(Mathf.Abs(activeChunkKey.x - xOff) > WorldSize/2 || Mathf.Abs(activeChunkKey.y - zOff) > WorldSize/2) {//player.postion / chunksize = chunk pos
                worldMap[activeChunkKey].chunkObject.SetActive(false);
            } else {
                stillActiveChunks.Add(activeChunkKey);
            }
        }
        activeChunks = stillActiveChunks;
    }

    void CreateChunk(Vector2Int key)
    {
        Vector3 pos = new Vector3(key.x * chunkSize, 0, key.y * chunkSize);
        ChunkGenerator curChunk = Instantiate(chunk, pos, Quaternion.identity);
        curChunk.transform.parent = transform;
        worldMap.Add(key, curChunk);//chunks # will be player position/10 rounded down
    }

    private IEnumerator LoadChunks()
    {
        isLoadingChunks = true;

        while(chunkQueue.Count > 0) {
            Vector2Int key = chunkQueue[0];
            chunkQueue.RemoveAt(0);
            Vector3 pos = new Vector3(key.x * chunkSize, 0, key.y * chunkSize);
            ChunkGenerator curChunk = Instantiate(chunk, pos, Quaternion.identity);
            curChunk.transform.parent = transform;
            worldMap.Add(key, curChunk);
            activeChunks.Add(key);
            yield return null;
        }

        isLoadingChunks = false;
    }

    public bool DoesBlockExist(Vector3Int pos)//check if there is a block/voxel at the given position
    {
        Vector2Int key = new Vector2Int(Mathf.FloorToInt((pos.x *1f) / chunkSize), Mathf.FloorToInt((pos.z * 1f) / chunkSize));
        if (worldMap.ContainsKey(key)) {
            return CubeTypes[worldMap[key].isCube[pos.x-(key.x*chunkSize), pos.y, pos.z-(key.y * chunkSize)]].isVisable;
        }
        return false;
    }

    /*
     * changes the cubetype of the block/voxel at pos to cubeType
     * use to add or remove blocks
     */
    public void ChangeBlock(Vector3Int pos, byte cubeType)
    {
        Vector2Int key = new Vector2Int(Mathf.FloorToInt((pos.x * 1f) / chunkSize), Mathf.FloorToInt((pos.z * 1f) / chunkSize));
        if (worldMap.ContainsKey(key)) {
            worldMap[key].isCube[pos.x - (key.x * chunkSize), pos.y, pos.z - (key.y * chunkSize)] = cubeType;//set the block type to air
            worldMap[key].UpdateChunk();
        }
    }

}


//use this to change the cube in that postion
//worldMap[new Vector2(0, 0)].GetComponent<ChunkGenerator>().isCube[0, 0, 0];
//use this to remove/readd chunks
//worldMap[new Vector2(1, 1)].SetActive(false);
