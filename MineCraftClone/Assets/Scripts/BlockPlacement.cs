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

    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        HighlightBlock();
    }

    void HighlightBlock()
    {
        float step = 0f;
        Vector3 LastPos;
        while(step < reach) {//fake raycast used cause normal raycast hits with vals that are of the next block
            Vector3 pos = mainCamera.transform.position + (mainCamera.transform.forward * step);
            Vector3Int posInt = new Vector3Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), Mathf.FloorToInt(pos.z));
            if (world.DoesBlockExist(posInt)) {
                highlight.position = new Vector3(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), Mathf.FloorToInt(pos.z)) + new Vector3(0.5f,0.5f,0.5f);
                highlight.gameObject.SetActive(true);
                return;
            }
            LastPos = new Vector3(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), Mathf.FloorToInt(pos.z)) + new Vector3(0.5f, 0.5f, 0.5f);//use this for placing blocks
            step += increment;
        }
        
        highlight.gameObject.SetActive(false);
    }
}
