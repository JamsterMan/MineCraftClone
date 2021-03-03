using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter),typeof(Transform))]
public class ChunkGenerator : MonoBehaviour
{
    //change class name to chuck generator
    private const int cubeSides = 6;
    Mesh mesh;
    
    private List<int> visableTriangles;
    private List<Vector3> visableVertices;
    private List<Vector2> visableUvs;
    private int verticesIndex;
    WorldGenerator world;

    byte[,,] isCube = new byte[MeshData.chunkWidth, MeshData.chunkHieghtMax, MeshData.chunkWidth];


    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        world = GameObject.Find("WorldGenerator").GetComponent<WorldGenerator>(); ;

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
        //float noiseScale = 10.0f;//perlin noise in unity changes base on the decimals
        for (x = 0; x < MeshData.chunkWidth; x++) {
            for (z = 0; z < MeshData.chunkWidth; z++) {
                //add perlin noise to chunk hieght to vary hieght
                for (y = 0; y < MeshData.chunkHieght; y++) {
                    CreateCube(new Vector3(x, y, z));//transform position to place cube in the correct chunk
                    /*
                     * perlin noise is only needed for 
                     */
                    //Debug.Log(  Mathf.PerlinNoise(x /noiseScale, z /noiseScale) ); add a transform.postion component so that all chunks are formed differently
                    //Debug.Log(  Mathf.PerlinNoise(x + transform.position.x, z + transform.position.z) );
                }
            }
        }
    }

    void FillChunk()
    {
        int x, y, z;
        for (x = 0; x < MeshData.chunkWidth; x++) {
            for (z = 0; z < MeshData.chunkWidth; z++) {
                for (y = 0; y < MeshData.chunkHieght; y++) {
                    if(y == 0) {
                        isCube[x, y, z] = (byte)CubeData.CubeType.voidStone;
                    }else if(y == MeshData.chunkHieght - 1) {
                        isCube[x, y, z] = (byte)CubeData.CubeType.grass;
                    } else if (y > MeshData.chunkHieght - 4) {
                        isCube[x, y, z] = (byte)CubeData.CubeType.dirt;
                    } else {
                        isCube[x, y, z] = (byte)CubeData.CubeType.stone;
                    }
                    /*if (y < MeshData.chunkHieght + perlin noise modifier) {
                        isCube[x, y, z] = 0;
                    } else{
                        isCube[x, y, z] = false;
                    }*/
                }
            }
        }
    }

    void CreateCube(Vector3 cubePosition)
    {
        int TextureID = 0;
        for (int i = 0; i < cubeSides; i++) {//front, top, right, left, back, bottom
            //remove non-visable sides
            if (!ShowSide(cubePosition + MeshData.faceCheck[i])) {
                visableVertices.Add(cubePosition + MeshData.vertices[MeshData.triangles[i, 0]] + transform.position);
                visableVertices.Add(cubePosition + MeshData.vertices[MeshData.triangles[i, 1]] + transform.position);
                visableVertices.Add(cubePosition + MeshData.vertices[MeshData.triangles[i, 2]] + transform.position);
                visableVertices.Add(cubePosition + MeshData.vertices[MeshData.triangles[i, 3]] + transform.position);

                //TextureBlocks(0);// 0 == dirt
                TextureID = world.CubeTypes[(int)isCube[Mathf.FloorToInt(cubePosition.x), Mathf.FloorToInt(cubePosition.y), Mathf.FloorToInt(cubePosition.z)]].GetTextureID(i);//gets TextureID for the cube texture, i == the face being drawn
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

    bool ShowSide(Vector3 cubePosition)//determine if the side is blocked by a block
    {
        int x = Mathf.FloorToInt(cubePosition.x);
        int y = Mathf.FloorToInt(cubePosition.y);
        int z = Mathf.FloorToInt(cubePosition.z);
        if (x < 0 || x > MeshData.chunkWidth-1 || y < 0 || y > MeshData.chunkHieght-1 || z < 0 || z > MeshData.chunkWidth-1 )
            return false;

        return world.CubeTypes[(int)isCube[x, y, z]].isVisable;
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = visableVertices.ToArray();
        mesh.triangles = visableTriangles.ToArray();
        mesh.uv = visableUvs.ToArray();
        mesh.RecalculateNormals();
    }
}
