using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPlacement : MonoBehaviour
{
    public Transform highlight;
    public WorldGenerator world;
    public UI_hotbar hotbar;
    public Animator DestroyAni;
    public float increment = 0.05f;
    public float reach = 3.0f;

    private Camera mainCamera;
    private Vector3 placePosition;
    private bool blockHighlighted;

    void Start()
    {
        mainCamera = Camera.main;
        blockHighlighted = false;
    }

    // Update is called once per frame
    void Update()
    {
        HighlightBlock();
        if (Input.GetMouseButtonDown(0) && blockHighlighted) {//left click destroy
            DestroyAni.SetBool("StartDestroy", true);
            DestroyAni.SetBool("StopDestroy", false);
        }
        if (Input.GetMouseButtonUp(0) && blockHighlighted) {//left click destroy
            DestroyAni.SetBool("StopDestroy", true);
            DestroyAni.SetBool("StartDestroy", false);
        }
        if (Input.GetMouseButtonDown(1) && blockHighlighted) {//right click place
            PlaceBlock();
        }
    }

    void HighlightBlock()
    {
        float step = 0f;
        //Vector3 LastPos;
        while(step < reach) {//fake raycast used cause normal raycast hits with vals that are of the next block
            Vector3 pos = mainCamera.transform.position + (mainCamera.transform.forward * step);
            Vector3Int posInt = new Vector3Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), Mathf.FloorToInt(pos.z));
            if (world.DoesBlockExist(posInt)) {
                highlight.position = new Vector3(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), Mathf.FloorToInt(pos.z)) + new Vector3(0.5f,0.5f,0.5f);
                highlight.gameObject.SetActive(true);
                blockHighlighted = true;

                return;
            }
            placePosition = new Vector3(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), Mathf.FloorToInt(pos.z)) + new Vector3(0.5f, 0.5f, 0.5f);//use this for placing blocks, could be adjusted to stop diaganal placements
            step += increment;
        }
        
        highlight.gameObject.SetActive(false);
        blockHighlighted = false;
    }

    //called by animation state
    public void DestroyBlock()
    {
        Vector3 pos = highlight.position;
        hotbar.AddItemToHotbar(world.GetBlockType(new Vector3Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), Mathf.FloorToInt(pos.z))), 1);
        world.ChangeBlock(new Vector3Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), Mathf.FloorToInt(pos.z)), (byte)CubeData.CubeType.air);
    }

    void PlaceBlock()
    {
        Vector3 pos = placePosition;// highlightPlace.position;
        world.ChangeBlock(new Vector3Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), Mathf.FloorToInt(pos.z)), (byte)hotbar.GetSelectedItem());//change this to place the block selected on hotbar instead of stone
        hotbar.RemoveItemToHotbar();
    }
}
