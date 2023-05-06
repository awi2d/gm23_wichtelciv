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
    public static Vector3 wichtel_offset = new Vector3((float) (tilesize*0.1), 1, 0);
    public static Vector3 haus_offset = new Vector3(0, 2, (float) (tilesize*0.1));
    Vector3 intpos2wordpos(int x, int y){
        return new Vector3((float)(tilewidth*(x+0.5*y)), 0, (float)(rowheight*y));
    }
    void Start()
    {
        Transform this_transfrom = this.GetComponent<Transform>();
        //create ground tiles
        Quaternion r = Quaternion.identity;
        r.eulerAngles = new Vector3(-90.0f, 0.0f, 0.0f);
        for(int x=0; x<mapsize; x++){
            for(int y=-x; y<mapsize; y++){
                //TODO calculate correct position in grid.
                GameObject gt = Instantiate(ground_tile, intpos2wordpos(x, y), r);
                gt.GetComponent<Ground>().posx = x;
                gt.GetComponent<Ground>().posy = y;
                gt.name = "ground(Clone)";
                gt.transform.parent = this_transfrom;
            }
        }


        for(int x=mapsize; x<2*mapsize; x++){
            for(int y=-mapsize+1; y<2*mapsize-x; y++){
                GameObject gt = Instantiate(ground_tile, intpos2wordpos(x, y), r);
                gt.GetComponent<Ground>().posx = x;
                gt.GetComponent<Ground>().posy = y;
                gt.name = "ground(Clone)";
                gt.transform.parent = this_transfrom;
            }
        }

        //create wichtel
        for(int i=0; i<wicht_startpos.Length; i++){
            GameObject wichtel_obj = Instantiate(wichtel, intpos2wordpos(wicht_startpos[i].x, wicht_startpos[i].y)+wichtel_offset, Quaternion.identity);
            wichtel_obj.name = "wichtel";
            wichtel_obj.GetComponent<Wichtel>().posx = wicht_startpos[i].x;
            wichtel_obj.GetComponent<Wichtel>().posy = wicht_startpos[i].y;
            wichtel_obj.transform.parent = this_transfrom;
        }
        //create haus
        for(int i=0; i<haus_startpos.Length; i++){
            GameObject haus_obj = Instantiate(haus, intpos2wordpos(haus_startpos[i].x, haus_startpos[i].y), Quaternion.identity);
            haus_obj.name = "haus";
            haus_obj.GetComponent<House>().posx = haus_startpos[i].x;
            haus_obj.GetComponent<House>().posy = haus_startpos[i].y;
            haus_obj.transform.parent = this_transfrom;
        }
    }
}
