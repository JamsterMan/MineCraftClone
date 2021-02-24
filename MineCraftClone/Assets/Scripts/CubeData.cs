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


    public static readonly int[,] triangles = new int[,]
        {
            //front
            { 0, 2, 1, 0, 3, 2},
            //top
            { 2, 3, 4, 2, 4, 5 },
            //right
            { 1, 2, 5, 1, 5, 6 },
            //left
            { 0, 7, 4, 0, 4, 3 },
            //back
            { 5, 4, 7, 5, 7, 6 },
            //bottom
            { 0, 6, 7, 0, 1, 6 }

        };

}
