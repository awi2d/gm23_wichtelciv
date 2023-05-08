using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour, Clickable
{
    public int posx;
    public int posy;
    public void OnClick(GameObject lastObject)
    {
        if(lastObject == null || lastObject.name == worldgen.name_ground)
        {
            Debug.Log("House on " + this.posx + ", " + this.posy + ":\nnum_resources = ");
        }
    }

    public void keyPressed(KeyCode key)
    {

    }

    public void OnGameTick()
    {
        if(worldgen.get_wicht(this.posx, this.posy) == null)
        {
            GameObject res = worldgen.get_resource(this.posx, this.posy);
            if (res != null)
            {
                res.GetComponent<Resource>().gather_this_resource();
                worldgen.spawn_wicht(this.posx, this.posy);
                return;
            }
        }
        
    }

    public void unselect(){

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
