using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_hotbar : MonoBehaviour
{
    public WorldGenerator world;
    public RectTransform hotbarHighlight;
    public Vector3 highlightStartPos;

    public ItemData[] hotbarItems;
    public Image[] hotbarIcons;
    public Text[] hotbarCounts;

    private int itemIndex;

    private float hotbarItemSize = 55.7894f;
    private int hotbarSize = 9;

    void Start()
    {
        itemIndex = 1;
        highlightStartPos = hotbarHighlight.localPosition;
        hotbarItemSize = hotbarHighlight.sizeDelta.x;
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll < 0) {//scroll down, move right on hotbar
            IndexIncrement(1);
        } else if (scroll > 0) {//scroll up, move left on hotbar
            IndexIncrement(-1);
        }
    }

    void IndexIncrement(int incerment)
    {
        itemIndex += incerment;
        hotbarHighlight.localPosition += new Vector3(incerment * hotbarItemSize,0,0);
        if(itemIndex < 1) {
            hotbarHighlight.localPosition = highlightStartPos + new Vector3((hotbarSize-1) * hotbarItemSize, 0, 0);//moves hotbar highlight
            itemIndex = hotbarSize;
        }else if (itemIndex > hotbarSize) {
            hotbarHighlight.localPosition = highlightStartPos;//moves hotbar highlight
            itemIndex = 1;
        }
    }

    public CubeData.CubeType GetSelectedItem()
    {
        if (hotbarIcons[itemIndex-1].isActiveAndEnabled)
            return hotbarItems[itemIndex-1].GetItemType();
        return CubeData.CubeType.air;
    }

    public void AddItemToHotbar(CubeData.CubeType type, int amount)
    {
        int index = -1;
        for (int i = 0; i < 9; i++) {//looks to see if item already in hotbar
            if (hotbarItems[i].CompareItemType(type) && !hotbarItems[i].IsAtMaxCount()) {
                index = i;
                break;
            }
        }
        if(index == -1) {
            for (int i = 0; i < 9; i++) {//looks for empty hotbar slot
                if (hotbarItems[i].CompareItemType(CubeData.CubeType.air)) {
                    index = i;
                    break;
                }
            }
            if (index == -1) {
                Debug.Log("Inventory Full");
                return;
            }
            //new item added to hotbar
            hotbarIcons[index].gameObject.SetActive(true);
            hotbarItems[index].SetItemType(type);
            hotbarIcons[index].sprite = world.CubeTypes[(int)type].HotbarIcon;
            hotbarItems[index].IncermentItemCount(amount);
            hotbarCounts[index].text = hotbarItems[index].GetItemCount().ToString();
            return;
        }
        //added more to item in hotbar
        hotbarItems[index].IncermentItemCount(amount);
        hotbarCounts[index].text = hotbarItems[index].GetItemCount().ToString();
    }

    public void RemoveItemToHotbar()
    {
        hotbarItems[itemIndex - 1].IncermentItemCount(-1);
        hotbarCounts[itemIndex - 1].text = hotbarItems[itemIndex-1].GetItemCount().ToString();
        if (hotbarItems[itemIndex-1].IsAtMinCount()) {
            hotbarIcons[itemIndex - 1].gameObject.SetActive(false);
            hotbarItems[itemIndex - 1].SetItemType(CubeData.CubeType.air);
        }
    }
}
