using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeData
{
    public enum CubeType //this should be put in meshdata or some other script
    {
        dirt,//0
        grass,
        stone,//2
        voidStone,//3
    };

    CubeType type = CubeType.dirt;

    //decide what texture to use by the type
}

