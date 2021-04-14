using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    [SerializeField]
    private CubeData.CubeType itemType;
    private int itemCount;

    private readonly int maxItemStack = 64;

    public ItemData()
    {
        itemType = CubeData.CubeType.air;
        itemCount = 0;
    }

    public int GetItemCount()
    {
        return itemCount;
    }

    public void IncermentItemCount(int incerment)//could return amount unused (leftover)
    {
        itemCount += incerment;
        if (itemCount > maxItemStack)
            itemCount = maxItemStack;
        if (itemCount < 0)
            itemCount = 0;
    }

    public bool IsAtMaxCount()
    {
        return itemCount >= maxItemStack;
    }

    public bool IsAtMinCount()
    {
        return itemCount <= 0;
    }

    public CubeData.CubeType GetItemType()
    {
        return itemType;
    }

    public void SetItemType(CubeData.CubeType type)//could return amount unused (leftover)
    {
        itemType = type;
    }

    public bool CompareItemType(CubeData.CubeType type)
    {
        return itemType == type;
    }
}
