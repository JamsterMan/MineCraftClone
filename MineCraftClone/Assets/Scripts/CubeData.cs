using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CubeData
{
    public enum CubeType //this should be put in meshdata or some other script, 0 to 255 (caused by using byte instead of int), max 255 blocks
    {
        dirt,//0
        grass,//1
        stone,//2
        voidStone,//3



        air//4
    };

    public CubeType type;
    //front, top, right, left, back, bottom
    public int frontTextureID;
    public int topTextureID;
    public int rightTextureID;
    public int leftTextureID;
    public int backTextureID;
    public int bottomTextureID;

    public bool isVisable;
    //add variable for blocks that care about direction

    public int GetTextureID(int faceIndex)
    {
        
        switch (faceIndex) {
            case 0:
                return frontTextureID;
            case 1:
                return topTextureID;
            case 2:
                return rightTextureID;
            case 3:
                return leftTextureID;
            case 4:
                return backTextureID;
            case 5:
            default:
                return bottomTextureID;
        }
    }

    //decide what texture to use by the type
}

