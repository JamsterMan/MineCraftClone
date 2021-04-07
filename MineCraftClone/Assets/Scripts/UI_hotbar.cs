using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_hotbar : MonoBehaviour
{
    public Text tempHotbar;
    public WorldGenerator world;
    public RectTransform hotbarHighlight;
    public Vector3 highlightStartPos;

    private int itemIndex;
    private string text;
    private CubeData.CubeType type;

    private float hotbarItemSize = 55.7894f;
    private int hotbarSize = 9;

    void Start()
    {
        itemIndex = 1;
        text = "Current Block: ";
        type = world.CubeTypes[itemIndex].getCubeType();
        tempHotbar.text = text + type.ToString();
        highlightStartPos = hotbarHighlight.localPosition;
        hotbarItemSize = hotbarHighlight.sizeDelta.x;
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll < 0) {//scroll up
            IndexIncrement(1);
            if (itemIndex <= world.CubeTypes.Length - 1) {
                type = world.CubeTypes[itemIndex].getCubeType();
            } else {
                type = world.CubeTypes[0].getCubeType();//air
            }
            tempHotbar.text = text + type.ToString();
        } else if (scroll > 0){//scroll down
            IndexIncrement(-1);
            if (itemIndex <= world.CubeTypes.Length - 1) {
                type = world.CubeTypes[itemIndex].getCubeType();
            } else {
                type = world.CubeTypes[0].getCubeType();//air
            }
            tempHotbar.text = text + type.ToString();
        }
    }

    void IndexIncrement(int incerment)
    {
        itemIndex += incerment;
        hotbarHighlight.localPosition += new Vector3(incerment * hotbarItemSize,0,0);
        if(itemIndex < 1) {
            hotbarHighlight.localPosition = highlightStartPos + new Vector3((hotbarSize-1) * hotbarItemSize, 0, 0);
            itemIndex = hotbarSize;
        }else if (itemIndex > hotbarSize) {
            hotbarHighlight.localPosition = highlightStartPos;
            itemIndex = 1;
        }
    }

    public CubeData.CubeType GetSelectedItem()
    {
        return type;
    }
}
