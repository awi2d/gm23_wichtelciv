using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using ResourceNameSpace;

public class Ground : MonoBehaviour, Clickable
{
    public int posx;
    public int posy;

    public void OnClick(GameObject lastObject)
    {
        if(lastObject != null && lastObject.name == worldgen.name_wicht)
        {
            Wichtel w = lastObject.GetComponent<Wichtel>();
            if(w == null)
            {
                Debug.Log("TODO misnamed gameObject wichtel");
            }
            w.move(this);
        }

        if(lastObject == null || lastObject.name == worldgen.name_ground)
        {
            Debug.Log("Ground on " + this.posx + ", " + this.posy);
        }

    }

    public void OnGameTick()
    {
        if(Random.Range(0, 100) == 0) // chance das eine Resource gespawnt wird.
        {
            int itype = Random.Range(0, 4);
            rEnum rtype = rEnum.Copper;
            if (itype == 0)
            {
                rtype = rEnum.Wood;
            }
            if (itype == 1)
            {
                rtype = rEnum.Mushroom;
            }

            worldgen.spawn_resouce(this.posx, this.posy, rtype);
        }

    }

    public void unselect(){
        
    }

    public void keyPressed(KeyCode key)
    {

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
