using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wichtel : MonoBehaviour, Clickable
{
    bool moveable = true;
    public void OnClick(GameObject lastObject)
    {
        Debug.Log("autschie");
    }


    public bool getmoveable()
    {
        return moveable;
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
