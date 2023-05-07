using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using ResourceNameSpace;

public class Resource : MonoBehaviour, Clickable
{
    public GameObject text;
    public string resource_String;
    public int resource;
    public int amount;

    public int posx;
    public int posy;

    GameObject textInWorld;

    public void OnClick(GameObject lastObject)
    {
        if (lastObject == null || lastObject.name == worldgen.name_ground)
        {
            text.GetComponent<TextMesh>().text = resource_String+":"+this.amount;
            textInWorld = GameObject.Instantiate(text);
            textInWorld.transform.position = transform.position;
            Debug.Log("Resource on " + this.posx + ", " + this.posy + ":\nesource = " + this.resource+ ", amount = "+this.amount);
        }
        if(lastObject != null && lastObject.name == worldgen.name_wicht)
        {
            Wichtel w = lastObject.GetComponent<Wichtel>();
            if(w.get_posx() == posx && w.get_posy() == posy && w.get_resource() < 0)
            {
                w.set_resource(this.resource);
                Debug.Log("Collect resource");
                this.amount--;
                if (this.amount <= 0)
                {
                    worldgen.destroy_resource(this.gameObject);
                }
            }
        }
    }

    public void OnGameTick()
    {
        //resources do nothing OnGameTick
    }

    public void unselect()
    {
        //throw new System.NotImplementedException();
        Destroy(textInWorld);
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
