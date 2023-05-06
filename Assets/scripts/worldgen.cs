using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class worldgen : MonoBehaviour
{
    public GameObject ground_tile;
    public GameObject wichtel;
    public GameObject haus;
    // Start is called before the first frame update
    Vector2Int[] wicht_startpos = new[] { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(0, 4) };
    Vector2Int[] haus_startpos = new[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(4, 0) };

    public const int mapsize = 5;
    private static double tilesize = 6.6; // ecke zu ecke
    private static double tilewidth = Mathf.Sqrt(3)*0.5*tilesize;  // distanz kante zu gegenüberliegender kante
    private static double rowheight = 1.5*0.5*tilesize; // length of one edge of the hexagone
    private static Vector3 wichtel_offset = new Vector3((float) (tilesize*0.1), 0, 0);
    Vector3 intpos2wordpos(int x, int y){
        return new Vector3((float)(tilewidth*(x+0.5*y)), 0, (float)(rowheight*y));
    }
    void Start()
    {   
        //create ground tiles
        Quaternion r = Quaternion.identity;
        r.eulerAngles = new Vector3(-90.0f, 0.0f, 0.0f);
        for(int x=0; x<mapsize; x++){
            for(int y=-x; y<mapsize; y++){
                //TODO calculate correct position in grid.
                GameObject gt = Instantiate(ground_tile, intpos2wordpos(x, y), r);
                gt.GetComponent<ground_position>().posx = x;
                gt.GetComponent<ground_position>().posy = y;
                gt.name = "ground(Clone)";
            }
        }

        for(int x=mapsize; x<2*mapsize; x++){
            for(int y=-mapsize+1; y<2*mapsize-x-1; y++){
                GameObject gt = Instantiate(ground_tile, intpos2wordpos(x, y), r);
                gt.GetComponent<ground_position>().posx = x;
                gt.GetComponent<ground_position>().posy = y;
                gt.name = "ground(Clone)";
            }
        }

        //create wichtel
        for(int i=0; i<wicht_startpos.Length; i++){
            GameObject wichtel_obj = Instantiate(wichtel, intpos2wordpos(wicht_startpos[i].x, wicht_startpos[i].y)+wichtel_offset, Quaternion.identity);
            wichtel_obj.name = "wichtel";
        }
        //create haus
        for(int i=0; i<haus_startpos.Length; i++){
            GameObject haus_obj = Instantiate(haus, intpos2wordpos(haus_startpos[i].x, haus_startpos[i].y), Quaternion.identity);
            haus_obj.name = "haus";
        }
    }
}
