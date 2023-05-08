using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResourceNameSpace;
using System;

public class worldgen : MonoBehaviour
{
    //to be set to the prefabs in the unity editor
    public GameObject ground_tile;
    public GameObject ground_tile1;
    public GameObject ground_tile2;
    public GameObject ground_tile3;
    public GameObject wichtel;
    public GameObject haus;
    public GameObject resource;
    Vector2Int[] wicht_startpos = new[] {new Vector2Int(4, 0) };
    Vector2Int[] haus_startpos = new[] {new Vector2Int(5, 0)};
    Vector2Int[] resource_startpos = new[] {new Vector2Int(4, 1), new Vector2Int(5, -1) };
    public const int mapsize = 5;

    //constants
    public static worldgen singelton_this;
    public static string name_wicht = "wicht";
    public static string name_house = "house";
    public static string name_ground = "ground";
    public static string name_resource = "resource";
    public static GameObject[] static_ground_tile;
    private static double tilesize = 6.6; // ecke zu ecke
    private static double tilewidth = Mathf.Sqrt(3)*0.5*tilesize;  // distanz kante zu gegenüberliegender kante
    private static double rowheight = 1.5*0.5*tilesize; // length of one edge of the hexagone
    public static Vector3 wichtel_offset = new Vector3((float) (tilesize*0.2), 1, 0);
    public static Vector3 haus_offset = new Vector3(0, 1.1f, (float) (tilesize*0.2));
    public static Dictionary<rEnum, Vector3> resource_offset = new Dictionary<rEnum, Vector3>() { 
        {rEnum.Copper, new Vector3((float)(-tilesize * 0.3), 1, 0) }, 
        {rEnum.Wood, new Vector3((float)(-tilesize * 0.15), 1, (float)(-tilesize * 0.2f)) },
        {rEnum.Mushroom, new Vector3((float)(-tilesize * 0.15), 1, (float)(tilesize * 0.1f)) }
    };
    public static Transform this_transfrom = null;
    public static Quaternion ground_rotation;
    public static Quaternion world_rotation;

    public static Clickable get_clicker(GameObject obj){
        if(obj.name == worldgen.name_ground){
            return obj.transform.Find("Cylinder").gameObject.GetComponent<Clickable>();
        }else{
            return obj.GetComponent<Clickable>();
        }
    }
    public static void rotate_world(float x, float y){
        if(x < -45.0f){
            x = -45.0f;
        }
        if(x > 45.0f){
            x = 45.0f;
        }

        if(y < -45.0f){
            y = -45.0f;
        }
        if(y > 45.0f){
            y = 45.0f;
        }
        world_rotation.eulerAngles = new Vector3(x, 0, y);
    }
    //cache, sort of
    public static List<GameObject> clickables = new List<GameObject>();  // all objects that implement the Clickable interface, probably

    public static Vector3 intpos2wordpos(int x, int y){
        Vector3 worldpos = new Vector3((float)(tilewidth*(x+0.5*y)-25.72), 0, (float)(rowheight*y-0)); // 25.72, 0 is so that center of world is at (0, 0), because world rotates around (0, 0)
        return worldgen.world_rotation*worldpos;
    }
    public static void spawn_ground(int posx, int posy)
    {
        GameObject gt = Instantiate(worldgen.static_ground_tile[UnityEngine.Random.Range(1, 4)], intpos2wordpos(posx, posy), Quaternion.identity);
        //"Cylinder"
        gt.transform.Find("Cylinder").gameObject.GetComponent<Ground>().posx = posx;
        gt.transform.Find("Cylinder").gameObject.GetComponent<Ground>().posy = posy;
        gt.name = worldgen.name_ground;
        
        gt.transform.parent = this_transfrom;
        worldgen.clickables.Add(gt);
    }
    public static void spawn_wicht(int posx, int posy)
    {
        GameObject wichtel_obj = Instantiate(worldgen.singelton_this.wichtel, intpos2wordpos(posx, posy) + wichtel_offset, Quaternion.identity);
        wichtel_obj.name = worldgen.name_wicht;
        wichtel_obj.GetComponent<Wichtel>().posx = posx;
        wichtel_obj.GetComponent<Wichtel>().posy = posy;
        wichtel_obj.transform.parent = this_transfrom;
        worldgen.clickables.Add(wichtel_obj);
    }
    public static void spawn_house(int posx, int posy)
    {
        Debug.Log("worldgen.singelton_this.haus = "+ worldgen.singelton_this.haus);
        GameObject haus_obj = Instantiate(worldgen.singelton_this.haus, intpos2wordpos(posx, posy)+worldgen.haus_offset, Quaternion.identity);
        haus_obj.name = worldgen.name_house;
        haus_obj.GetComponent<House>().posx = posx;
        haus_obj.GetComponent<House>().posy = posy;
        haus_obj.transform.parent = this_transfrom;
        worldgen.clickables.Add(haus_obj);
    }
    public static void spawn_resouce(int posx, int posy, rEnum type)
    {
        foreach (GameObject obj in worldgen.clickables)
        {
            if (obj.name == worldgen.name_resource && worldgen.get_clicker(obj).get_posx() == posx && worldgen.get_clicker(obj).get_posy() == posy)
            {
                if(obj.GetComponent<Resource>().get_resourcetype() == type)
                {
                    obj.GetComponent<Resource>().amount++;
                    return;
                }
            }
        }
        GameObject resource_obj = Instantiate(worldgen.singelton_this.resource, intpos2wordpos(posx, posy) + resource_offset[type], Quaternion.identity);
        resource_obj.name = worldgen.name_resource;
        resource_obj.GetComponent<Resource>().posx = posx;
        resource_obj.GetComponent<Resource>().posy = posy;
        resource_obj.GetComponent<Resource>().resource_String = Enum.GetName(typeof(rEnum), type);
        resource_obj.GetComponent<Resource>().set_resourcetype(type);
        resource_obj.GetComponent<Resource>().amount = 1;
        resource_obj.transform.parent = this_transfrom;
        worldgen.clickables.Add(resource_obj);
    }

    public static void destroy_resource(GameObject resource_obj)
    {
        worldgen.clickables.Remove(resource_obj);
        Destroy(resource_obj);
    }

    public static List<GameObject> get_clickables(int posx, int posy)
    {
        List<GameObject> r = new List<GameObject> ();
        foreach(GameObject obj in worldgen.clickables)
        {
            if(worldgen.get_clicker(obj).get_posx() == posx && worldgen.get_clicker(obj).get_posy() == posy)
            {
                r.Add(obj);
            }
        }
        return r;
    }

    public static GameObject get_wicht(int posx, int posy)
    {
        foreach (GameObject obj in worldgen.clickables)
        {
            if (obj.name == worldgen.name_wicht && worldgen.get_clicker(obj).get_posx() == posx && worldgen.get_clicker(obj).get_posy() == posy)
            {
                return obj;
            }
        }
        return null;
    }

    public static GameObject get_house(int posx, int posy)
    {
        foreach (GameObject obj in worldgen.clickables)
        {
            if (worldgen.get_clicker(obj).get_posx() == posx && worldgen.get_clicker(obj).get_posy() == posy && obj.name == worldgen.name_house)
            {
                return obj;
            }
        }
        return null;
    }

    public static GameObject get_resource(int posx, int posy)
    {
        foreach (GameObject obj in worldgen.clickables)
        {
            if (worldgen.get_clicker(obj).get_posx() == posx && worldgen.get_clicker(obj).get_posy() == posy && obj.name == worldgen.name_resource)
            {
                return obj;
            }
        }
        return null;
    }
    void Start()
    {
        if(worldgen.this_transfrom == null)
        {
            worldgen.this_transfrom = this.GetComponent<Transform>();
            worldgen.singelton_this = this;
            worldgen.world_rotation = Quaternion.identity;
            worldgen.static_ground_tile = new GameObject[]{this.ground_tile, this.ground_tile1, this.ground_tile2, this.ground_tile3};
        }
        else
        {
            Debug.Log("start should only be called once, right?");
            return;
        }
        
        
        ground_rotation = Quaternion.identity;
        ground_rotation.eulerAngles = new Vector3(-90.0f, 0.0f, 0.0f);
        //create ground tiles
        for (int x=0; x<mapsize; x++){
            for(int y=-x; y<mapsize; y++){
                worldgen.spawn_ground(x, y);
            }
        }


        for(int x=mapsize; x<2*mapsize; x++){
            for(int y=-mapsize+1; y<2*mapsize-x; y++){
                worldgen.spawn_ground(x, y);
            }
        }

        //create wichtel
        for(int i=0; i<wicht_startpos.Length; i++){
            worldgen.spawn_wicht(wicht_startpos[i].x, wicht_startpos[i].y);
        }
        //create haus
        for(int i=0; i<haus_startpos.Length; i++){
            worldgen.spawn_house(haus_startpos[i].x, haus_startpos[i].y);
        }
        for(int i=0; i< resource_startpos.Length; i++)
        {
            worldgen.spawn_resouce(resource_startpos[i].x, resource_startpos[i].y, rEnum.Wood);
        }
    }
}
