using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerControll : MonoBehaviour
{

    public float movementSpeed = 0.1f;

    GameObject lastObject;
    GameObject mapHolding;

    // Start is called before the first frame update
    void Start()
    {
        this.lastObject = null;
        this.mapHolding = GameObject.FindGameObjectsWithTag("clickable_holding")[0];  // assumes exactly one map_holding in each scene
    }

    void FixedUpdate(){
        // Move camera
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        transform.position = transform.position + new Vector3(this.movementSpeed*horizontalInput, 0, this.movementSpeed*verticalInput);
        if (Input.GetKey(KeyCode.Return))
        {
            Debug.Log("runde weiter");
            Wichtel[] wichtel = (Wichtel[])GameObject.FindObjectsOfType(typeof(Wichtel));
            foreach (Wichtel wicht in wichtel)
            {
                wicht.GetComponent<Clickable>().OnGameTick();
            }
            House[] houses = (House[])GameObject.FindObjectsOfType(typeof(House));
            foreach (House house in houses)
            {
                house.GetComponent<Clickable>().OnGameTick();
            }
            Ground[] grounds = (Ground[])GameObject.FindObjectsOfType(typeof(Ground));
            foreach (Ground ground in grounds)
            {
                ground.GetComponent<Clickable>().OnGameTick();
            }

            // kipp welt
            // kipp about axis-x
            float x_axis_weight = 0f;
            float y_axis_weight = 0f;
            foreach(GameObject clickable in worldgen.clickables){
                float w = 0f;
                if(clickable.name == worldgen.name_wicht){
                    w = 0.2f;
                }
                if(clickable.name == worldgen.name_house){
                    w = 0.6f;
                }
                if(clickable.name == worldgen.name_resource){
                    w = 0.002f;
                }
                Clickable tmp = clickable.GetComponent<Clickable>();
                x_axis_weight += w*(4.5f-tmp.get_posx()); // 4.5f, 0.5f is center of world, approximatly
                y_axis_weight += w*(tmp.get_posy()-0.5f);
            }
            //maybe devide by total weight?
            Debug.Log("axis weight: "+x_axis_weight+", "+y_axis_weight);
            worldgen.rotate_world(y_axis_weight, x_axis_weight);// 1.-parameter: positiv kippt nach hinten. y-achse: positiv kippt nach links
            foreach(GameObject clickable in worldgen.clickables){
                Clickable tmp = clickable.GetComponent<Clickable>();
                clickable.transform.position = worldgen.intpos2wordpos(tmp.get_posx(), tmp.get_posy());
                if(clickable.name == worldgen.name_ground){
                    clickable.transform.rotation = worldgen.world_rotation*worldgen.ground_rotation;
                }else{
                    if(clickable.name == worldgen.name_wicht){
                        clickable.transform.position += worldgen.wichtel_offset;
                    }
                    if(clickable.name == worldgen.name_resource){
                        clickable.transform.position += worldgen.resource_offset;
                    }
                    if(clickable.name == worldgen.name_house){
                        clickable.transform.position += worldgen.haus_offset;
                    }
                    clickable.transform.rotation = worldgen.world_rotation;
                }
            }
            // check for victory
            if(worldgen.get_house(0, 0) != null && worldgen.get_house(4, -4) != null && worldgen.get_house(9, -4) != null && worldgen.get_house(9, 0) != null && worldgen.get_house(5, 4) != null && worldgen.get_house(0, 4) != null){
                Debug.Log("Ritual completet, you won!");
            }
        }
    }

    // Update is called once per frame
 void Update() {
    //if mouse is clicked:
    //  get what was clicked on (unit, city or tile) and act accordingly (selecting that thing or moving the selected unit to that tile)
    if (Input.GetMouseButtonDown(0)) {
        Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        RaycastHit hit;
        if (Physics.Raycast( ray, out hit, 100 )) {
            //raycast hit something <-> player clicked on something.
                if (hit.transform.name != null)
                {
                    try
                    {
                        Clickable clicker = hit.transform.gameObject.GetComponent<Clickable>();
                        clicker.OnClick(lastObject);
                        if(lastObject != null){
                            lastObject.GetComponent<Clickable>().unselect();
                        }
                        lastObject = hit.transform.gameObject;
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }   
                }
                else
                {
                    
                }
        }else{
            if(lastObject != null){
                lastObject.GetComponent<Clickable>().unselect();
                lastObject = null;
            }
            
        }
    }
 }

}
