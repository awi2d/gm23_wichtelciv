using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using ResourceNameSpace;

public class Resource : MonoBehaviour, Clickable
{
    public GameObject text;
    public string resource_String;
    private int resource_type;
    public int amount;

    public int posx;
    public int posy;

    GameObject textInWorld;

    public void OnClick(GameObject lastObject)
    {
        if (lastObject == null || lastObject.name != worldgen.name_wicht)
        {
            text.GetComponent<TextMesh>().text = resource_String+":"+this.amount;
            textInWorld = GameObject.Instantiate(text);
            textInWorld.transform.position = transform.position;
            Debug.Log("Resource on " + this.posx + ", " + this.posy + ":\nesource = " + this.get_resourcetype()+ ", amount = "+this.amount);
        }
        if(lastObject != null && lastObject.name == worldgen.name_wicht)
        {
            Wichtel w = lastObject.GetComponent<Wichtel>();
            if(w.get_posx() == posx && w.get_posy() == posy && w.get_resource() < 0)
            {
                w.set_resource(this.get_resourcetype());
                Debug.Log("Collect resource");
                this.amount--;
                if (this.amount <= 0)
                {
                    worldgen.destroy_resource(this.gameObject);
                }
            }
        }
    }

    public void set_resourcetype(int type){
        this.resource_type = type;
        this.transform.GetChild(0).gameObject.SetActive(type==0);
        this.transform.GetChild(1).gameObject.SetActive(type==1);
        this.transform.GetChild(2).gameObject.SetActive(type==2);
    }

    public int get_resourcetype(){
        return this.resource_type;
    }

    public void OnGameTick()
    {
        //resources do nothing OnGameTick
    }

    public void unselect()
    {
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
