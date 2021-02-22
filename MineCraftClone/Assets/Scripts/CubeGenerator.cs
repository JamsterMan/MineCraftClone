using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class CubeGenerator : MonoBehaviour
{
    
    Mesh mesh;
    
    Vector3[] vertices;
    int[] triangles;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateCube();
        UpdateMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //
    /*void CreateCube()//unity smoothes vertices together
    {
        vertices = new Vector3[]
        {
            new Vector3 (0,0,0),//0
            new Vector3 (1,0,0),//1
            new Vector3 (1,1,0),//2
            new Vector3 (0,1,0),//3
            new Vector3 (0,1,1),//4
            new Vector3 (1,1,1),//5
            new Vector3 (1,0,1),//6
            new Vector3 (0,0,1),//7
            
        };

        //top, left, front one order; bottom, right, back different order for normals
        triangles = new int[]
        {
            //front
            0, 2, 1,
            0, 3, 2,
            //top
            2, 3, 4,
            2, 4, 5,
            //right
            1, 2, 5,
            1, 5, 6,
            //left
            0, 7, 4,
            0, 4, 3,
            //back
            5, 4, 7,
            5, 7, 6,
            //bottom
            0, 6, 7,
            0, 1, 6,

        };
    }*/

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
        triangles = new int[]
        {
            //front
            0, 2, 1,
            0, 3, 2,
            //top
            10, 11, 4,
            10, 4, 5,
            //right
            9, 18, 13,
            9, 13, 6,
            //left
            8, 7, 12,
            8, 12, 19,
            //back
            21, 20, 15,
            21, 15, 14,
            //bottom
            16, 22, 23,
            16, 17, 22,

        };
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
