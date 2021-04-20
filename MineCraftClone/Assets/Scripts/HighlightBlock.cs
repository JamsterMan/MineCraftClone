using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightBlock : MonoBehaviour
{
    private BlockPlacement bp;
    // Start is called before the first frame update
    void Start()
    {
        bp = (BlockPlacement)GameObject.FindGameObjectWithTag("Player").GetComponent("BlockPlacement");
    }

    public void TestEvent()
    {
        bp.DestroyBlock();
    }
}
