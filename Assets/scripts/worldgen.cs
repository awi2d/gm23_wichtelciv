using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class worldgen : MonoBehaviour
{
    public GameObject ground_tile;
    // Start is called before the first frame update
    void Start()
    {
        //create ground tiles
        for(int x=0; x<5; x++){
            for(int y=0; y<5; y++){
                //TODO calculate correct position in grid.
                GameObject gt = Instantiate(ground_tile, new Vector3(10*x, 0, 10*y), Quaternion.identity);
                gt.GetComponent<ground_position>().posx = x;
                gt.GetComponent<ground_position>().posy = y;

            }
        }
        //get gt script ad set posx and posy.
    }
}
