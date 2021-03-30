using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_hotbar : MonoBehaviour
{
    public Text tempHotbar;
    public WorldGenerator world;

    private int itemIndex;
    private string text;
    private CubeData.CubeType type;

    void Start()
    {
        itemIndex = 1;
        text = "Current Block: ";
        type = world.CubeTypes[itemIndex].getCubeType();
        tempHotbar.text = text + type.ToString();
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if( scroll > 0) {//scroll up
            IndexIncrement(1);
            type = world.CubeTypes[itemIndex].getCubeType();
            tempHotbar.text = text + type.ToString();
        } else if (scroll < 0){//scroll down
            IndexIncrement(-1);
            type = world.CubeTypes[itemIndex].getCubeType();
            tempHotbar.text = text + type.ToString();
        }
    }

    void IndexIncrement(int incerment)
    {
        itemIndex += incerment;
        if(itemIndex < 1) {
            itemIndex = world.CubeTypes.Length -1;
        }else if (itemIndex > world.CubeTypes.Length - 1) {
            itemIndex = 1;
        }
    }

    public CubeData.CubeType GetSelectedItem()
    {
        return type;
    }
}
