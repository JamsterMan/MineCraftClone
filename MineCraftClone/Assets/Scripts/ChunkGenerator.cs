﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter),typeof(Transform))]
public class ChunkGenerator : MonoBehaviour
{
    //change class name to chuck generator
    private const int cubeSides = 6;
    Mesh mesh;
    
    private List<int> visableTriangles;
    private List<Vector3> visableVertices;
    private List<Vector2> visableUvs;
    private int verticesIndex;
    WorldGenerator world;

    public float noiseScale = 10.0f, offsetScale = 10.0f;//perlin noise in unity changes base on the decimals, offsetScale determines how much the noise changes the height

    byte[,,] isCube = new byte[MeshData.chunkWidth, MeshData.chunkHieghtMax, MeshData.chunkWidth];


    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        world = GameObject.Find("WorldGenerator").GetComponent<WorldGenerator>(); ;

        verticesIndex = 0;
        visableTriangles = new List<int>();
        visableVertices = new List<Vector3>();
        visableUvs = new List<Vector2>();
        FillChunk();
        CreateChunk();
        UpdateMesh();
    }

    void CreateChunk()
    {
        int x, y, z;//, noiseOffset;
        for (x = 0; x < MeshData.chunkWidth; x++) {
            for (z = 0; z < MeshData.chunkWidth; z++) {
                //noiseOffset = Mathf.FloorToInt( Mathf.PerlinNoise( (x+transform.position.x) / noiseScale, (z + transform.position.z) / noiseScale)*offsetScale );//add perlin noise to chunk hieght to vary hieght
                for (y = 0; y < MeshData.chunkHieghtMax; y++) {//MeshData.chunkHieght + noiseOffset
                    CreateCube(new Vector3(x, y, z));//transform position to place cube in the correct chunk
                }
            }
        }
    }

    void FillChunk()
    {
        int x, y, z, noiseOffset, yHieght, dirtHieght = 5;
        for (x = 0; x < MeshData.chunkWidth; x++) {
            for (z = 0; z < MeshData.chunkWidth; z++) {
                noiseOffset = Mathf.FloorToInt(Mathf.PerlinNoise((x + transform.position.x) / noiseScale, (z + transform.position.z) / noiseScale) * offsetScale);//add perlin noise to chunk hieght to vary hieght
                yHieght = MeshData.chunkHieght + noiseOffset;
                for (y = 0; y < MeshData.chunkHieghtMax; y++) {
                    if(y == 0) {
                        isCube[x, y, z] = (byte)CubeData.CubeType.voidStone;
                    }else if(y == yHieght - 1) {
                        isCube[x, y, z] = (byte)CubeData.CubeType.grass;
                    } else if (y > yHieght - dirtHieght && y < yHieght) {//dirtHieght -1 is the number of dirt blocks before stone is placed
                        isCube[x, y, z] = (byte)CubeData.CubeType.dirt;
                    } else if (y < yHieght) {
                        isCube[x, y, z] = (byte)CubeData.CubeType.stone;
                    } else {
                        isCube[x, y, z] = (byte)CubeData.CubeType.air;
                    }
                    /*if (y < MeshData.chunkHieght + perlin noise modifier) {
                        isCube[x, y, z] = 0;
                    } else{
                        isCube[x, y, z] = false;
                    }*/
                }
            }
        }
    }

    void CreateCube(Vector3 cubePosition)
    {
        
        int TextureID;
        CubeData cube = world.CubeTypes[(int)isCube[Mathf.FloorToInt(cubePosition.x), Mathf.FloorToInt(cubePosition.y), Mathf.FloorToInt(cubePosition.z)]];//gets the cubedata of the block being drawn

        if (cube.type == CubeData.CubeType.air)//air is not a visable block
            return;

        for (int i = 0; i < cubeSides; i++) {//front, top, right, left, back, bottom
            //remove non-visable sides
            if (!ShowSide(cubePosition + MeshData.faceCheck[i])) {
                visableVertices.Add(cubePosition + MeshData.vertices[MeshData.triangles[i, 0]] + transform.position);
                visableVertices.Add(cubePosition + MeshData.vertices[MeshData.triangles[i, 1]] + transform.position);
                visableVertices.Add(cubePosition + MeshData.vertices[MeshData.triangles[i, 2]] + transform.position);
                visableVertices.Add(cubePosition + MeshData.vertices[MeshData.triangles[i, 3]] + transform.position);

                TextureID = cube.GetTextureID(i);//gets TextureID for the cube texture, i == the face being drawn
                TextureBlocks(TextureID);//applies the texture based on the TextureID

                visableTriangles.Add(verticesIndex);
                visableTriangles.Add(verticesIndex+1);
                visableTriangles.Add(verticesIndex+2);
                visableTriangles.Add(verticesIndex+2);//repeats cause the 2 triangles that make a square share vertices
                visableTriangles.Add(verticesIndex+1);
                visableTriangles.Add(verticesIndex+3);

                verticesIndex +=4;

            }
        }
    }

    void TextureBlocks(int id)//id == id of the texture in the atlas, different from the cube type id
    {
        float y = id / MeshData.blocksPerAtlasRow;
        float x = id - (y * MeshData.blocksPerAtlasRow);
        float normalizedSize = 1f / MeshData.blocksPerAtlasRow;

        x *= normalizedSize;
        y *= normalizedSize;

        visableUvs.Add(new Vector2(x, y));
        visableUvs.Add(new Vector2(x, y + normalizedSize));
        visableUvs.Add(new Vector2(x + normalizedSize, y));
        visableUvs.Add(new Vector2(x + normalizedSize, y + normalizedSize));
    }

    bool ShowSide(Vector3 cubePosition)//determine if the side is blocked by a block
    {
        int x = Mathf.FloorToInt(cubePosition.x);
        int y = Mathf.FloorToInt(cubePosition.y);
        int z = Mathf.FloorToInt(cubePosition.z);
        if (x < 0 || x > MeshData.chunkWidth-1 || y < 0 || y > MeshData.chunkHieghtMax-1 || z < 0 || z > MeshData.chunkWidth-1 )
            return false;

        return world.CubeTypes[(int)isCube[x, y, z]].isVisable;
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
