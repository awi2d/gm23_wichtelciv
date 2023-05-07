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
            if (w.getmoveable() && Mathf.Pow((w.posx - this.posx), 2) + Mathf.Pow((w.posy - this.posy), 2) < 4)
            {
                if(worldgen.get_wicht(this.posx, this.posy) == null)
                {
                    lastObject.transform.position = this.transform.position + worldgen.wichtel_offset;
                    w.posx = this.posx;
                    w.posy = this.posy;
                    w.setmoveable(false);
                }
                else
                {
                    Debug.Log("cant move to already occupied field");
                }
                
            }
        }

        if(lastObject == null || lastObject.name == worldgen.name_ground)
        {
            Debug.Log("Ground on " + this.posx + ", " + this.posy);
        }

    }

    public void OnGameTick()
    {
        if(Random.Range(0, 10) >= 9)  // 0.1 chance das eine neue Resource entsteht
        {
            GameObject res = worldgen.get_resource(this.posx, this.posy);
            if(res == null)
            {
                int itype = Random.Range(0, 4);
                rEnum rtype = rEnum.Copper;
                if(itype == 0){
                    rtype = rEnum.Wood;
                }
                if(itype == 1){
                    rtype = rEnum.Mushroom;
                }
                worldgen.spawn_resouce(this.posx, this.posy, rtype);
            }
            else
            {
                res.GetComponent<Resource>().amount++;
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
