using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class CubeGenerator : MonoBehaviour
{
    private const int cubeSides = 6;
    Mesh mesh;
    
    Vector3[] vertices;
    int[][] triangles;//double array for culling sides of cube not visable
    int[] visableTriangles;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        visableTriangles = new int[36];

        CreateCube();
        UpdateMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateCube()
    {
        vertices = new Vector3[]
        {   //repeated vertices needed to stop unity from smoothing the vertices
            new Vector3 (0,0,0),//0
            new Vector3 (1,0,0),//1
            new Vector3 (1,1,0),//2
            new Vector3 (0,1,0),//3
            new Vector3 (0,1,1),//4
            new Vector3 (1,1,1),//5
            new Vector3 (1,0,1),//6
            new Vector3 (0,0,1),//7

            new Vector3 (0,0,0),//0+8=8
            new Vector3 (1,0,0),//1+8=9
            new Vector3 (1,1,0),//2+8=10
            new Vector3 (0,1,0),//3+8=11
            new Vector3 (0,1,1),//4+8=12
            new Vector3 (1,1,1),//5+8=13
            new Vector3 (1,0,1),//6+8=14
            new Vector3 (0,0,1),//7+8=15
            
            new Vector3 (0,0,0),//0+16=16
            new Vector3 (1,0,0),//1+16=17
            new Vector3 (1,1,0),//2+16=18
            new Vector3 (0,1,0),//3+16=19
            new Vector3 (0,1,1),//4+16=20
            new Vector3 (1,1,1),//5+16=21
            new Vector3 (1,0,1),//6+16=22
            new Vector3 (0,0,1),//7+16=23
            
        };

        //top, left, front one order; bottom, right, back different order for normals
        triangles = new int[][]
        {
            //front
            new int[] { 0, 2, 1, 0, 3, 2},
            //top
            new int[] { 10, 11, 4, 10, 4, 5 },
            //right
            new int[] { 9, 18, 13, 9, 13, 6 },
            //left
            new int[] { 8, 7, 12, 8, 12, 19 },
            //back
            new int[] { 21, 20, 15, 21, 15, 14 },
            //bottom
            new int[] { 16, 22, 23, 16, 17, 22 }

        };
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        for (int i = 0; i < cubeSides; i++) {//front, top, right, left, back, bottom
            for (int j = 0; j < cubeSides; j++) {//
                //remove non-visable sides
                visableTriangles[j+(i*6)] = triangles[i][j];
            }
        }
        mesh.triangles = visableTriangles;
        mesh.RecalculateNormals();
    }
}
