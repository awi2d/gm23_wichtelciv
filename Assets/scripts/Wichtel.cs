using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResourceNameSpace;

public class Wichtel : MonoBehaviour, Clickable
{
    bool moveable = true;
    public Vector2 inv;
    public int resource;  // <0 := no resource, otherwise as in ResourceNameSpace
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
            this.resource = -1;
        }
        if(lastObject == null || lastObject.name == worldgen.name_ground)
        {
            Debug.Log("Wicht on " + this.posx + ", " + this.posy + ":\nresource = " + resource);
        }
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
        this.resource = -1;
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
