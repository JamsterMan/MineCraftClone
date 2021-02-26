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

    private readonly int chunkWidth = 5;
    private readonly int chunkHieght = 10;


    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        verticesIndex = 0;
        visableTriangles = new List<int>();
        visableVertices = new List<Vector3>();
        visableUvs = new List<Vector2>();
        int x, y, z;
        for (x = 0; x < chunkWidth; x++) {
            for (y = 0; y < chunkHieght; y++) {
                for (z = 0; z < chunkWidth; z++) {
                    CreateCube(new Vector3(x, y, z));
                }
            }
        }
        //CreateCube(new Vector3(0, 0, 0) );
        UpdateMesh();
    }

    void CreateCube(Vector3 cubePosition)
    {
        int triangleIndex;
        for (int i = 0; i < cubeSides; i++) {//front, top, right, left, back, bottom
            for (int j = 0; j < cubeSides; j++) {
                //remove non-visable sides
                triangleIndex = CubeData.triangles[i, j];
                visableVertices.Add(cubePosition + CubeData.vertices[triangleIndex]);
                visableTriangles.Add(verticesIndex);
                verticesIndex++;

                visableUvs.Add( CubeData.uvs[j] );
            }
        }
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
