using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CubeData 
{
    public static readonly Vector3[] vertices = new Vector3[]
        {   //repeated vertices needed to stop unity from smoothing the vertices
            new Vector3 (0,0,0),//0
            new Vector3 (1,0,0),//1
            new Vector3 (1,1,0),//2
            new Vector3 (0,1,0),//3
            new Vector3 (0,1,1),//4
            new Vector3 (1,1,1),//5
            new Vector3 (1,0,1),//6
            new Vector3 (0,0,1),//7
        };


    public static readonly int[,] triangles = new int[6,6]
        {
            //front
            { 0, 3, 1, 1, 3, 2 },
            //top
            { 3, 4, 2, 2, 4, 5 },
            //right
            { 1, 2, 6, 6, 2, 5 },
            //left
            { 7, 4, 0, 0, 4, 3 },
            //back
            { 6, 5, 7, 7, 5, 4 },
            //bottom
            { 1, 6, 0, 0, 6, 7 }

        };

    public static readonly Vector2[] uvs = new Vector2[6]
        {
            new Vector2 ( 0.0f, 0.0f),
            new Vector2 ( 0.0f, 1.0f),
            new Vector2 ( 1.0f, 0.0f),
            new Vector2 ( 1.0f, 0.0f),
            new Vector2 ( 0.0f, 1.0f),
            new Vector2 ( 1.0f, 1.0f),
        };

}
