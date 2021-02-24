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
    List<int> visableTriangles;
    List<Vector3> visableVertices;
    int verticesIndex;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        verticesIndex = 0;
        visableTriangles = new List<int>();
        visableVertices = new List<Vector3>();
        CreateCube(new Vector3(0, 0, 0) );
        CreateCube(new Vector3(0, 2, 0) );
        CreateCube(new Vector3(-4, 0, 0) );
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
            }
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = visableVertices.ToArray();
        mesh.triangles = visableTriangles.ToArray();
        mesh.RecalculateNormals();
    }
}
