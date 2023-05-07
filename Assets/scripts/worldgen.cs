using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResourceNameSpace;
using System;

public class worldgen : MonoBehaviour
{
    //to be set to the prefabs in the unity editor
    public GameObject ground_tile;
    public GameObject wichtel;
    public GameObject haus;
    public GameObject resource;
    Vector2Int[] wicht_startpos = new[] { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(0, 4) };
    Vector2Int[] haus_startpos = new[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(4, 0) };
    Vector2Int[] resource_startpos = new[] { new Vector2Int(1, 1), new Vector2Int(2, 2), new Vector2Int(4, 4) };
    public const int mapsize = 5;

    //constants
    public static worldgen singelton_this;
    public static string name_wicht = "wicht";
    public static string name_house = "house";
    public static string name_ground = "ground";
    public static string name_resource = "resource";
    public static GameObject static_ground_tile;
    private static double tilesize = 6.6; // ecke zu ecke
    private static double tilewidth = Mathf.Sqrt(3)*0.5*tilesize;  // distanz kante zu gegenüberliegender kante
    private static double rowheight = 1.5*0.5*tilesize; // length of one edge of the hexagone
    public static Vector3 wichtel_offset = new Vector3((float) (tilesize*0.3), 1, 0);
    public static Vector3 haus_offset = new Vector3(0, 2.5f, (float) (tilesize*0.3));
    public static Vector3 resource_offset = new Vector3((float) (-tilesize * 0.3), 1, 0);
    public static Transform this_transfrom = null;
    public static Quaternion ground_rotation;
    public static Quaternion world_rotation;

    public static void rotate_world(float x, float y){
        if(x < -45.0f){
            Debug.Log("The world lost balance, you lose.");
            x = -45.0f;
        }
        if(x > 45.0f){
            Debug.Log("The world lost balance, you lose.");
            x = 45.0f;
        }

        if(y < -45.0f){
            Debug.Log("The world lost balance, you lose.");
            y = -45.0f;
        }
        if(y > 45.0f){
            Debug.Log("The world lost balance, you lose.");
            y = 45.0f;
        }
        world_rotation.eulerAngles = new Vector3(x, 0, y);
    }
    //cache, sort of
    public static List<GameObject> clickables = new List<GameObject>();  // all objects that implement the Clickable interface, probably

    public static Vector3 intpos2wordpos(int x, int y){
        Vector3 worldpos = new Vector3((float)(tilewidth*(x+0.5*y)), 0, (float)(rowheight*y));
        return worldgen.world_rotation*worldpos;
    }
    public static void spawn_ground(int posx, int posy)
    {
        GameObject gt = Instantiate(worldgen.singelton_this.ground_tile, intpos2wordpos(posx, posy), worldgen.ground_rotation);
        gt.GetComponent<Ground>().posx = posx;
        gt.GetComponent<Ground>().posy = posy;
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
        GameObject haus_obj = Instantiate(worldgen.singelton_this.haus, intpos2wordpos(posx, posy), Quaternion.identity);
        haus_obj.name = worldgen.name_house;
        haus_obj.GetComponent<House>().posx = posx;
        haus_obj.GetComponent<House>().posy = posy;
        haus_obj.transform.parent = this_transfrom;
        worldgen.clickables.Add(haus_obj);
    }
    public static void spawn_resouce(int posx, int posy, rEnum type)
    {
        GameObject resource_obj = Instantiate(worldgen.singelton_this.resource, intpos2wordpos(posx, posy) + resource_offset, Quaternion.identity);
        resource_obj.name = worldgen.name_resource;
        resource_obj.GetComponent<Resource>().posx = posx;
        resource_obj.GetComponent<Resource>().posy = posy;
        resource_obj.GetComponent<Resource>().resource_String = Enum.GetName(typeof(rEnum), type);
        resource_obj.GetComponent<Resource>().resource = (int)type;
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
            if(obj.GetComponent<Clickable>().get_posx() == posx && obj.GetComponent<Clickable>().get_posy() == posy)
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
            if (obj.GetComponent<Clickable>().get_posx() == posx && obj.GetComponent<Clickable>().get_posy() == posy && obj.name == worldgen.name_wicht)
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
            if (obj.GetComponent<Clickable>().get_posx() == posx && obj.GetComponent<Clickable>().get_posy() == posy && obj.name == worldgen.name_house)
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
            if (obj.GetComponent<Clickable>().get_posx() == posx && obj.GetComponent<Clickable>().get_posy() == posy && obj.name == worldgen.name_resource)
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
            worldgen.spawn_resouce(resource_startpos[i].x, resource_startpos[i].y, rEnum.Mushroom);
        }
    }
}
