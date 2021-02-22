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
    void CreateCube()
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
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
