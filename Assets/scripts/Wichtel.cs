using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResourceNameSpace;

public class Wichtel : MonoBehaviour, Clickable
{
    bool moveable = true;
    public Vector2 inv;
    public Resource resource;
    private ParticleSystem particlesystem;
    public void OnClick(GameObject lastObject)
    {
        Debug.Log("autschie");
        this.particlesystem.enableEmission = true;

    }

    public void unselect(){
        this.particlesystem.enableEmission = false;
    }


    public bool getmoveable()
    {
        return moveable;
    }
    // Start is called before the first frame update
    void Start()
    {
        this.particlesystem = GetComponent<ParticleSystem>();
        this.particlesystem.enableEmission = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnGameTick()
    {
        if(resource != null)
        {

        }
    }
}
