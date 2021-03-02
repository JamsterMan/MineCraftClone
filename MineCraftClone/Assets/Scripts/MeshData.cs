using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshData 
{
    public static readonly int chunkWidth = 16;
    public static readonly int chunkHieght = 20;

    //change this number any time the atlas texture gets more blocks added to it
    public static readonly int blocksPerAtlasRow = 2;//number of different blocks accross a atlas

    public static readonly Vector3[] vertices = new Vector3[8]
        {   //repeated vertices needed to stop unity from smoothing the vertices
            new Vector3 (0f,0f,0f),//0
            new Vector3 (1f,0f,0f),//1
            new Vector3 (1f,1f,0f),//2
            new Vector3 (0f,1f,0f),//3
            new Vector3 (0f,1f,1f),//4
            new Vector3 (1f,1f,1f),//5
            new Vector3 (1f,0f,1f),//6
            new Vector3 (0f,0f,1f),//7
        };

    public static readonly int[,] triangles = new int[6,4]
        {
            //front
            { 0, 3, 1, 2 },
            //top
            { 3, 4, 2, 5 },
            //right
            { 1, 2, 6, 5 },
            //left
            { 7, 4, 0, 3 },
            //back
            { 6, 5, 7, 4 },
            //bottom
            { 1, 6, 0, 7 }

        };

    public static readonly Vector2[] uvs = new Vector2[4]
        {
            new Vector2 ( 0.0f, 0.0f),
            new Vector2 ( 0.0f, 1.0f),
            new Vector2 ( 1.0f, 0.0f),
            new Vector2 ( 1.0f, 1.0f),
        };

    public static readonly Vector3[] faceCheck = new Vector3[6]
        {   //repeated vertices needed to stop unity from smoothing the vertices
            new Vector3 (0f,0f,-1f),//front
            new Vector3 (0f,1f,0f),//top
            new Vector3 (1f,0f,0f),//right
            new Vector3 (-1f,0f,0f),//left
            new Vector3 (0f,0f,1f),//back
            new Vector3 (0f,-1f,0f)//bottom
        };
}
