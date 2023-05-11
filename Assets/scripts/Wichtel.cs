using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResourceNameSpace;
using UnityEngine.UI;
using System;

public class Wichtel : MonoBehaviour, Clickable
{
    public Sprite icon_mushroom;
    public Sprite icon_wood;
    public Sprite icon_copper;
    private int max_moves = 2;
    private int moves_left;
    private rEnum resource;  // <0 := no resource, otherwise as in ResourceNameSpace
    private ParticleSystem particlesystem;
    
    public int posx;
    public int posy;
    
    //TODO particle keep spawning after movement, as this wichtel is not deselected.
    public void OnClick(GameObject lastObject)
    {
        InWorldUi.draw_lines(this.posx, this.posy, this.moves_left);
        if (lastObject == this.gameObject && resource != rEnum.None && worldgen.get_house(this.posx, this.posy) == null)
        {
            Debug.Log("bould house");
            worldgen.spawn_house(this.posx, this.posy);
            this.set_resource(rEnum.None);
        }
        if(lastObject == null || lastObject.name == worldgen.name_ground)
        {
            Debug.Log("Wicht on " + this.posx + ", " + this.posy + ":\nresource = " + Enum.GetName(typeof(rEnum), this.resource));
        }
    }

    public void keyPressed(KeyCode key)
    {
        //require hotkey to move?
        if(key == KeyCode.R)
        {
            //take resource from current hex. (if more than one is available: take the one that exists for the longest
            if (this.get_resource() == rEnum.None)
            {
                GameObject resource = worldgen.get_resource(this.posx, this.posy);
                if (resource != null)
                {
                    this.set_resource(resource.GetComponent<Resource>().get_resourcetype());
                    resource.GetComponent<Resource>().gather_this_resource();
                }
            }
            
            
        }
        if(key == KeyCode.T)
        {
            //drop resource to hex
            if(this.resource != rEnum.None)
            {
                worldgen.spawn_resouce(this.get_posx(), this.get_posy(), this.resource);
                this.set_resource(rEnum.None);
            }
        }
        if(key == KeyCode.Z)
        {
            //nothing, so far
        }
        if(key == KeyCode.F)
        {
            //build house
            if(resource != rEnum.None && worldgen.get_house(this.posx, this.posy) == null)
            {
                Debug.Log("bould house");
                worldgen.spawn_house(this.posx, this.posy);
                this.set_resource(rEnum.None);
            }
        }

        //build other house types
    }

    public void move(Ground target)
    {
        //center = (0, 0), linksdrüber = (1, -1), rechtsdrüber = (0, -1), rechts = (-1, 0), rechtsdrunter = (-1, 1), linksdrunter = (0, 1), links = (1, 1)
        //not in list: (1, 0), (-1, -1)
        int dist = worldgen.get_hex_dist(this.posx, this.posy, target.posx, target.posy);
        if (dist <= this.moves_left)
        {
            this.transform.position = target.transform.position + worldgen.wichtel_offset;
            this.posx = target.posx;
            this.posy = target.posy;
            this.moves_left -= dist;
            InWorldUi.draw_lines(this.posx, this.posy, this.moves_left);
        }
        
    }

    public void set_resource(rEnum type){
        this.resource = type;
        if(type == rEnum.None)
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
        }else{
            this.transform.GetChild(0).gameObject.SetActive(true);
            if(type == rEnum.Copper){
                this.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = this.icon_copper;// canvas is only child
            }
            if(type == rEnum.Wood){
                this.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = this.icon_wood;
            }
            if(type == rEnum.Mushroom){
                this.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = this.icon_mushroom;
            }
        }
        
        // set canvas-child.image to correct icon
    }
    public rEnum get_resource(){
        return this.resource;
    }

    public void unselect(){
        //remove InWorldUI
        InWorldUi.remove_lines();
    }
    public int get_posx()
    {
        return this.posx;
    }
    public int get_posy()
    {
        return this.posy;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.particlesystem = GetComponent<ParticleSystem>();
        //this.particlesystem.enableEmission = false;
        this.particlesystem.Stop();
        this.set_resource(rEnum.None);
        this.moves_left = this.max_moves;
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnGameTick()
    {
        this.moves_left = this.max_moves;
    }
}
