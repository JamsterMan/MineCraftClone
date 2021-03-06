using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public GameObject chunk;
    public int WorldSize = 2;
    public CubeData[] CubeTypes;
    
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < WorldSize; i++) {
            for (int j = 0; j < WorldSize; j++) {
                Instantiate(chunk, new Vector3(i*5, 0, j*5), Quaternion.identity);
            }
        }
        //Instantiate(chunk, new Vector3(0, 0, 5), Quaternion.identity);
        //Instantiate(chunk, new Vector3(5, 0, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
