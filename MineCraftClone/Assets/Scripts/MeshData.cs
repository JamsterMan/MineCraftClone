using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshData 
{
    public static readonly int chunkWidth = 10;
    public static readonly int chunkHieght = 50;//hieght blocks will start at
    public static readonly int chunkHieghtMax = 100;//hieght limit for the world

    //change this number any time the atlas texture gets more blocks added to it
    public static readonly int blocksPerAtlasRow = 16;//number of different blocks accross a atlas

    public static readonly Vector3Int[] vertices = new Vector3Int[8]//vertices used for drawing cubes
        {
            new Vector3Int (0,0,0),//0
            new Vector3Int (1,0,0),//1
            new Vector3Int (1,1,0),//2
            new Vector3Int (0,1,0),//3
            new Vector3Int (0,1,1),//4
            new Vector3Int (1,1,1),//5
            new Vector3Int (1,0,1),//6
            new Vector3Int (0,0,1),//7
        };

    public static readonly int[,] triangles = new int[6,4]//ints in this 2d array reffer to the vertices in the vertices array
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

    public static readonly Vector2[] uvs = new Vector2[4]//used to make sure textures are the correct direction
        {
            new Vector2 ( 0.0f, 0.0f),
            new Vector2 ( 0.0f, 1.0f),
            new Vector2 ( 1.0f, 0.0f),
            new Vector2 ( 1.0f, 1.0f),
        };

    public static readonly Vector3Int[] faceCheck = new Vector3Int[6]//these vectors are used to check if each side of a cube should be drawn
        {
            new Vector3Int (0,0,-1),//front
            new Vector3Int (0,1,0),//top
            new Vector3Int (1,0,0),//right
            new Vector3Int (-1,0,0),//left
            new Vector3Int (0,0,1),//back
            new Vector3Int (0,-1,0)//bottom
        };

    /*
    static readonly WorldGenerator world = GameObject.Find("WorldGenerator").GetComponent<WorldGenerator>();
    public static float AddNoise(float x, float z, float scale, int levels, float ampScale, float freqScale)//levels = # of layered perlinNoise, ampScale should be 0 to 1
    {
        float noise = 0f;
        float amp = 1f;
        float freq = 1f;

        for (int i = 0; i < levels; i++) {
            float sampleX = (x + world.perlinOffsetX) / scale * freq;
            float sampleZ = (z + world.perlinOffsetZ) / scale * freq;

            float perlinVal = Mathf.PerlinNoise(sampleX, sampleZ);
            noise += perlinVal * amp;

            amp *= ampScale;
            freq *= freqScale;
        }

        return noise;
    }*/
}
