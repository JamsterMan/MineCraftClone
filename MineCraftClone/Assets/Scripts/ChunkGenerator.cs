using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class ChunkGenerator : MonoBehaviour
{
    public byte[,,] isCube;//to update this add a public function here

    private const int cubeSides = 6;
    Mesh mesh;
    MeshCollider meshCollider;
    
    private List<int> visableTriangles;
    private List<Vector3> visableVertices;
    private List<Vector2> visableUvs;
    private int verticesIndex;
    public GameObject chunkObject;
    WorldGenerator world;


    // Start is called before the first frame update
    void Start()
    {
        isCube = new byte[MeshData.chunkWidth, MeshData.chunkHieghtMax, MeshData.chunkWidth];
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        meshCollider =  GetComponent<MeshCollider>();
        chunkObject = this.gameObject;
        world = GameObject.Find("WorldGenerator").GetComponent<WorldGenerator>();

        verticesIndex = 0;
        visableTriangles = new List<int>();
        visableVertices = new List<Vector3>();
        visableUvs = new List<Vector2>();

        FillChunk();
        CreateChunk();
        UpdateMesh();
    }

    void CreateChunk()
    {
        int x, y, z;
        for (x = 0; x < MeshData.chunkWidth; x++) {
            for (z = 0; z < MeshData.chunkWidth; z++) {
                for (y = 0; y < MeshData.chunkHieghtMax; y++) {
                    CreateCube(new Vector3Int(x, y, z));//transform position to place cube in the correct chunk
                }
            }
        }
    }

    void FillChunk()
    {
        int x, y, z, dirtHieght = 5;
        for (x = 0; x < MeshData.chunkWidth; x++) {
            for (z = 0; z < MeshData.chunkWidth; z++) {
                int noiseOffset = AddHieght(x,z);//add perlin noise to chunk hieght to vary hieght
                int yHieght = MeshData.chunkHieght + noiseOffset;

                for (y = 0; y < MeshData.chunkHieghtMax; y++) {
                    if(y == 0) {
                        isCube[x, y, z] = (byte)CubeData.CubeType.voidStone;
                    }else if(y == yHieght - 1) {
                        isCube[x, y, z] = (byte)CubeData.CubeType.grass;
                    } else if (y > yHieght - dirtHieght && y < yHieght) {//dirtHieght -1 is the number of dirt blocks before stone is placed
                        isCube[x, y, z] = (byte)CubeData.CubeType.dirt;
                    } else if (y < yHieght) {
                        isCube[x, y, z] = (byte)CubeData.CubeType.stone;
                    } else {
                        isCube[x, y, z] = (byte)CubeData.CubeType.air;
                    }
                }
            }
        }
    }

    int AddHieght(int x, int z)
    {
        int hieght;
        float perlinX = ((float)(x + transform.position.x + 0.1f));
        float perlinZ = ((float)(z + transform.position.z + 0.1f));
        hieght = Mathf.FloorToInt(AddNoise(perlinX, perlinZ, world.noiseScale, 3, world.perlinAmpScale, world.perlinFreqScale) * world.offsetScale); ;
        return hieght;
    }

    float AddNoise(float x, float z, float scale, int levels, float ampScale, float freqScale)//levels = # of layered perlinNoise, ampScale should be 0 to 1
    {
        float noise = 0f;
        float amp = 1f;
        float freq = 1f;

        for (int i = 0; i < levels; i++) {
            float sampleX = (x + world.perlinOffsetX) / scale * freq;
            float sampleZ = (z + world.perlinOffsetZ) / scale * freq;

            float perlinVal = Mathf.PerlinNoise(sampleX, sampleZ);
            noise += perlinVal * amp;

            amp *= ampScale;
            freq *= freqScale;
        }

        return noise;
    }

    //adds visable sides of the cube at cubePosition t othe mesh lists (visableVertices and visableTriangles)
    void CreateCube(Vector3Int cubePosition)
    {
        CubeData cube = world.CubeTypes[(int)isCube[cubePosition.x, cubePosition.y, cubePosition.z]];//gets the cubedata of the block being drawn

        if (cube.type == CubeData.CubeType.air)//air is not a visable block
            return;

        for (int i = 0; i < cubeSides; i++) {//front, top, right, left, back, bottom
            //remove non-visable sides
            if (!ShowSide(cubePosition + MeshData.faceCheck[i])) {
                visableVertices.Add(cubePosition + MeshData.vertices[MeshData.triangles[i, 0]] );
                visableVertices.Add(cubePosition + MeshData.vertices[MeshData.triangles[i, 1]] );
                visableVertices.Add(cubePosition + MeshData.vertices[MeshData.triangles[i, 2]] );
                visableVertices.Add(cubePosition + MeshData.vertices[MeshData.triangles[i, 3]] );

                int TextureID = cube.GetTextureID(i);//gets TextureID for the cube texture, i == the face being drawn
                TextureBlocks(TextureID);//applies the texture based on the TextureID

                visableTriangles.Add(verticesIndex);
                visableTriangles.Add(verticesIndex+1);
                visableTriangles.Add(verticesIndex+2);
                visableTriangles.Add(verticesIndex+2);//repeats cause the 2 triangles that make a square share vertices
                visableTriangles.Add(verticesIndex+1);
                visableTriangles.Add(verticesIndex+3);

                verticesIndex +=4;
            }
        }
    }

    //function to texture blocks based on the id
    void TextureBlocks(int id)//id == id of the texture in the atlas, different from the cube type id
    {
        float y = id / MeshData.blocksPerAtlasRow;
        float x = id - (y * MeshData.blocksPerAtlasRow);
        float normalizedSize = 1f / MeshData.blocksPerAtlasRow;

        x *= normalizedSize;
        y *= normalizedSize;

        visableUvs.Add(new Vector2(x, y));
        visableUvs.Add(new Vector2(x, y + normalizedSize));
        visableUvs.Add(new Vector2(x + normalizedSize, y));
        visableUvs.Add(new Vector2(x + normalizedSize, y + normalizedSize));
    }

    //determines if the side of a cube is visable
    bool ShowSide(Vector3Int cubePosition)//determine if the side is blocked by a block
    {
        int x = cubePosition.x;
        int y = cubePosition.y;
        int z = cubePosition.z;
        if (x < 0 || x > MeshData.chunkWidth-1 || y < 0 || y > MeshData.chunkHieghtMax-1 || z < 0 || z > MeshData.chunkWidth-1 )
            return false;

        return world.CubeTypes[(int)isCube[x, y, z]].isVisable;
    }

    //sets up the mesh for this chunk
    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = visableVertices.ToArray();
        mesh.triangles = visableTriangles.ToArray();
        mesh.uv = visableUvs.ToArray();
        mesh.RecalculateNormals();

        meshCollider.sharedMesh = mesh;
    }

    public void UpdateChunk()
    {
        ClearMeshData();
        CreateChunk();
        UpdateMesh();
    }

    void ClearMeshData()
    {
        verticesIndex = 0;
        visableTriangles.Clear();
        visableVertices.Clear();
        visableUvs.Clear();
    }
}
