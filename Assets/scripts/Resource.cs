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

    GameObject textInWorld;

    public void OnClick(GameObject lastObject)
    {
        if (lastObject == null)
        {
            text.GetComponent<TextMesh>().text = resource_String;
            textInWorld = GameObject.Instantiate(text);
            textInWorld.transform.position = transform.position;
            resource = (int)rEnum.Copper;
        }
        try
        {
            Wichtel w = lastObject.GetComponent<Wichtel>();
            if (w != null)
            {
                w.resource = this;
                Debug.Log("On Resource");
            }
        }
        catch
        {

        }
    }

    public void OnGameTick()
    {
        throw new System.NotImplementedException();
    }

    public void unselect()
    {
        //throw new System.NotImplementedException();
        Destroy(textInWorld);
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
