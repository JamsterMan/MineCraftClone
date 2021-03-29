using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPlacement : MonoBehaviour
{
    public Transform highlight;
    public WorldGenerator world;
    public float increment = 0.2f;
    public float reach = 3.0f;

    private Camera mainCamera;
    private Vector3 placePosition;
    //private Vector3 LastHighlightPos;
    public Transform highlightPlace;

    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        HighlightBlock();
        if (Input.GetMouseButtonDown(0)) {//left click destroy
            DestroyBlock();
        }else if (Input.GetMouseButtonDown(1)) {//right click place
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

                //highlightPlace.position = placePosition;
                //highlightPlace.gameObject.SetActive(true);

                //if the subtraction has two values that are equal or opposite ( 1 and 1, or 1 and -1) then the block will be placed diaganally from the block, so change it to choose a side
                //Debug.Log(highlight.position + " , " + highlightPlace.position + " , " + (highlight.position - highlightPlace.position) );
                return;
            }
            placePosition = new Vector3(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), Mathf.FloorToInt(pos.z)) + new Vector3(0.5f, 0.5f, 0.5f);//use this for placing blocks
            step += increment;
        }
        
        highlight.gameObject.SetActive(false);
        highlightPlace.gameObject.SetActive(false);
    }

    void DestroyBlock()
    {
        Vector3 pos = highlight.position;
        world.ChangeBlock(new Vector3Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), Mathf.FloorToInt(pos.z)), (byte)CubeData.CubeType.air);
    }

    void PlaceBlock()
    {
        Vector3 pos = placePosition;// highlightPlace.position;
        world.ChangeBlock(new Vector3Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), Mathf.FloorToInt(pos.z)), (byte)CubeData.CubeType.stone);//change this to place the block selected on hotbar instead of stone
    }
}
