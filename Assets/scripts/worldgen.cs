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
        double tilesize = 20.0; // ecke zu ecke
        double tilewidth = Mathf.Sqrt(3)*0.5*tilesize;  // distanz kante zu gegenüberliegender kante
        double rowheight = 1.5*0.5*tilesize; // length of one edge of the hexagone
        for(int x=0; x<5; x++){
            for(int y=0; y<5; y++){
                //TODO calculate correct position in grid.
                GameObject gt = Instantiate(ground_tile, new Vector3((float)tilewidth*x, 0, (float) (rowheight*y+0.5*tilesize*x)), Quaternion.identity);
                gt.GetComponent<ground_position>().posx = x;
                gt.GetComponent<ground_position>().posy = y;

            }
        }
    }
}
