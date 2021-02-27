using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class CubeGenerator : MonoBehaviour
{
    //change class name to chuck generator
    private const int cubeSides = 6;
    Mesh mesh;
    
    //Vector3[] vertices;
    //int[][] triangles;//double array for culling sides of cube not visable
    private List<int> visableTriangles;
    private List<Vector3> visableVertices;
    private List<Vector2> visableUvs;
    private int verticesIndex;

    bool[,,] isCube = new bool[CubeData.chunkWidth, CubeData.chunkHieght, CubeData.chunkWidth];


    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        verticesIndex = 0;
        visableTriangles = new List<int>();
        visableVertices = new List<Vector3>();
        visableUvs = new List<Vector2>();
        FillIsBool();
        CreateChunk();
        UpdateMesh();
    }

    void CreateChunk()
    {
        int x, y, z;
        //float noiseScale = 10.0f;//perlin noise in unity changes base on the decimals
        for (x = 0; x < CubeData.chunkWidth; x++) {
            for (z = 0; z < CubeData.chunkWidth; z++) {
                //add perlin noise to chunk hieght to vary hieght
                for (y = 0; y < CubeData.chunkHieght; y++) {
                    CreateCube(new Vector3(x, y, z) + transform.position);
                    /*
                     * perlin noise is only needed for 
                     */
                    //Debug.Log(  Mathf.PerlinNoise(x /noiseScale, z /noiseScale) );
                    //Debug.Log(  Mathf.PerlinNoise(x + transform.position.x, z + transform.position.z) );
                }
            }
        }
    }

    void FillIsBool()
    {
        int x, y, z;
        for (x = 0; x < CubeData.chunkWidth; x++) {
            for (z = 0; z < CubeData.chunkWidth; z++) {
                for (y = 0; y < CubeData.chunkHieght; y++) {
                    isCube[x, y, z] = true;
                }
            }
        }
    }

    void CreateCube(Vector3 cubePosition)
    {
        int triangleIndex;
        for (int i = 0; i < cubeSides; i++) {//front, top, right, left, back, bottom
            //remove non-visable sides
            if (!ShowSide(cubePosition + CubeData.faceCheck[i])) {
                for (int j = 0; j < cubeSides; j++) {
                    triangleIndex = CubeData.triangles[i, j];
                    visableVertices.Add(cubePosition + CubeData.vertices[triangleIndex]);
                    visableTriangles.Add(verticesIndex);
                    verticesIndex++;

                    visableUvs.Add(CubeData.uvs[j]);
                }
            }
        }
    }

    bool ShowSide(Vector3 cubePosition)//determine if the side is blocked by a block
    {
        int x = Mathf.FloorToInt(cubePosition.x);
        int y = Mathf.FloorToInt(cubePosition.y);
        int z = Mathf.FloorToInt(cubePosition.z);
        if (x < 0 || x > CubeData.chunkWidth-1 || y < 0 || y > CubeData.chunkHieght-1 || z < 0 || z > CubeData.chunkWidth-1 )
            return false;

        return isCube[x, y, z];
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
