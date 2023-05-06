using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ground : MonoBehaviour, Clickable
{
    public int posx;
    public int posy;

    public void OnClick(GameObject lastObject)
    {
        Debug.Log(lastObject.name);
        try
        {
            Wichtel w = lastObject.GetComponent<Wichtel>();
            if (w != null && w.getmoveable() && Mathf.Pow((w.posx-this.posx), 2) + Mathf.Pow((w.posy-this.posy), 2) < 4)
            {
                //TODO check that no Wichtel is on this field
                lastObject.transform.position = this.transform.position+worldgen.wichtel_offset;
                w.posx = this.posx;
                w.posy = this.posy;
                w.setmoveable(false);
            }
        }
        catch(System.Exception e)
        {
            Debug.Log(e);
        }

    }

    public void unselect(){
        
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
