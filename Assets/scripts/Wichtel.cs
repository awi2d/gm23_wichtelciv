using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResourceNameSpace;
using UnityEngine.UI;

public class Wichtel : MonoBehaviour, Clickable
{
    public Sprite icon_mushroom;
    public Sprite icon_wood;
    public Sprite icon_copper;
    bool moveable = true;
    private int resource;  // <0 := no resource, otherwise as in ResourceNameSpace
    private ParticleSystem particlesystem;
    
    public int posx;
    public int posy;
    
    public void OnClick(GameObject lastObject)
    {
        if(this.moveable){
            this.particlesystem.enableEmission = true;
        }
        if (lastObject == this.gameObject && resource >= 0 && worldgen.get_house(this.posx, this.posy) == null)
        {
            Debug.Log("bould house");
            worldgen.spawn_house(this.posx, this.posy);
            this.set_resource(-1);
        }
        if(lastObject == null || lastObject.name == worldgen.name_ground)
        {
            Debug.Log("Wicht on " + this.posx + ", " + this.posy + ":\nresource = " + resource);
        }
    }

    public void set_resource(int type){
        this.resource = type;
        if(type < 0){
            this.transform.GetChild(0).gameObject.SetActive(false);
        }else{
            this.transform.GetChild(0).gameObject.SetActive(true);
            if(type == 0){
                this.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = this.icon_copper;// canvas is only child
            }
            if(type == 1){
                this.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = this.icon_wood;
            }
            if(type == 2){
                this.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = this.icon_mushroom;
            }
        }
        
        // set canvas-child.image to correct icon
    }
    public int get_resource(){
        return this.resource;
    }

    public void unselect(){
        this.particlesystem.enableEmission = false;
    }
    public int get_posx()
    {
        return this.posx;
    }
    public int get_posy()
    {
        return this.posy;
    }

    public bool getmoveable()
    {
        return moveable;
    }
    public void setmoveable(bool sm){
        this.moveable = sm;
    }
    // Start is called before the first frame update
    void Start()
    {
        this.particlesystem = GetComponent<ParticleSystem>();
        this.particlesystem.enableEmission = false;
        this.set_resource(-1);
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnGameTick()
    {
        this.moveable = true;
    }
}
