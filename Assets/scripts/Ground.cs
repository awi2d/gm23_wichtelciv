using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ground : MonoBehaviour, Clickable
{
    public void OnClick(GameObject lastObject)
    {
        Debug.Log(lastObject.name);
        try
        {
            Wichtel w = lastObject.GetComponent<Wichtel>();
            if (w != null && w.getmoveable())
            {
                lastObject.transform.position = this.transform.position+worldgen.wichtel_offset;
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
