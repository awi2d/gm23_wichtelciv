using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour, Clickable
{
    public int posx;
    public int posy;
    int num_resources = 0;
    public void OnClick(GameObject lastObject)
    {
        if(lastObject != null && lastObject.name == worldgen.name_wicht)
        {
            Wichtel w = lastObject.GetComponent<Wichtel>();
            if (w.posx == this.posx && w.posy == this.posy && w.resource >= 0)
            {
                this.num_resources++;
                w.resource = -1;
                Debug.Log("add Resource to house");

            }
        }
        if(lastObject == null)
        {
            Debug.Log("House on " + this.posx + ", " + this.posy + ":\nnum_resources = " + this.num_resources);
        }
    }

    public void OnGameTick()
    {
        if(this.num_resources > 0 && worldgen.get_wicht(this.posx, this.posy) == null)
        {
            Debug.Log("Spawn wicht on house");
            worldgen.spawn_wicht(this.posx, this.posy);
            this.num_resources--;
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
