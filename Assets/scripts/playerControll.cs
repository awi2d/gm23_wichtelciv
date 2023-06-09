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

    private float x_yaw = 0;
    private float y_yaw = 0;

    private static KeyCode[] hotkeys = new KeyCode[] { KeyCode.R, KeyCode.T, KeyCode.Z, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.C, KeyCode.V, KeyCode.B};
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
        //TODO movement is unaffected by camara rotation
        transform.position = transform.position + Quaternion.Euler(new Vector3(0, this.transform.rotation.eulerAngles[1], 0)) * new Vector3(this.movementSpeed*horizontalInput, 0, this.movementSpeed*verticalInput);
        if (Input.GetKey(KeyCode.Q))
        {
            //rotate camera left
            transform.rotation = Quaternion.Euler(new Vector3(0, -1, 0)) * transform.rotation;
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 1, 0)) * transform.rotation;
        }
        
    }

    private void set_lastObject(GameObject new_lastObject)
    {
        if (lastObject != null)
        {
            //worldgen.get_clicker(lastObject).unselect();
        }
        if(new_lastObject != null)
        {
            Debug.Log("set lastObject to " + new_lastObject.name);
        }
        else
        {
            Debug.Log("set lastObject to null");
        }
        
        lastObject = new_lastObject;
    }
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
                Clickable clicker = worldgen.get_clicker(hit.transform.gameObject);

                if(clicker != null){
                    if(this.lastObject != null && hit.transform.gameObject != this.lastObject)
                        {
                            worldgen.get_clicker(this.lastObject).unselect();
                        }
                    clicker.OnClick(lastObject);

                    if (lastObject != null && lastObject.name == worldgen.name_wicht && hit.transform.gameObject.name == "Cylinder")
                    {
                        //keep wichtel selected after move action
                    }
                    else
                    {
                        // set this.last_object
                        this.set_lastObject(hit.transform.gameObject);
                    }
                        
                }else{
                    Debug.Log("clicker of "+hit.transform.gameObject.name+" not found");
                }
            }
        }else{
            this.set_lastObject(null);
        }
    }
    // give keypresses to this.lastObject to make hotkeys work
    if(this.lastObject != null)
    {
        foreach(KeyCode key in playerControll.hotkeys)
            {
                if (Input.GetKeyUp(key))
                {
                    worldgen.get_clicker(this.lastObject).keyPressed(key);
                }
                
            }
    }
    if (Input.GetKeyUp(KeyCode.Return))
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
            float x_axis_weight = 0f;  // [Nm] drehmoment
            float y_axis_weight = 0f;
            float x_axis_absweight = 0f;  // [?] inertia
            float y_axis_absweight = 0f;
            float center_x = 0f;
            float center_z = 0f;
            int groundcount = 0;
            //re-calculating the center every turn seems be unnesary, but for unknown reasons, it changes. (maybe becouse the ground tiles get rotatet?)
            foreach (GameObject clickable in worldgen.clickables)
            {
                if(clickable.name == worldgen.name_ground)
                {
                    center_x += clickable.transform.position.x;
                    center_z += clickable.transform.position.z;
                    groundcount++;
                }
            }
            center_x /= groundcount;
            center_z /= groundcount;
            Debug.Log("center = (" + center_x + ", " + center_z + ")");
            foreach (GameObject clickable in worldgen.clickables){
                float w = 0f;
                if(clickable.name == worldgen.name_wicht){
                    w = 2.0f;
                }
                if(clickable.name == worldgen.name_house){
                    w = 4.0f;
                }
                if(clickable.name == worldgen.name_resource){
                    w = (float)(0.02f*clickable.GetComponent<Resource>().amount);
                }
                if (clickable.name == worldgen.name_ground)
                {
                    w = 0.1f; // Der Boden besteht aus Hohlen plastik-attrappen, deswegen ist der so Leicht.
                }
                float posx = clickable.transform.position.x;
                float posz = clickable.transform.position.z;
                x_axis_weight += w*(posx - center_x); // 4.5f, 0.5f is center of world, approximatly TODO calculate center so that ground is balanced
                y_axis_weight += w*(posz - center_z);
                x_axis_absweight += w * Mathf.Abs(posx - center_x);
                y_axis_absweight += w * Mathf.Abs(posz - center_z);
            }
            //maybe devide by total weight?
            Debug.Log("axis weight: "+x_axis_weight/x_axis_absweight+", "+y_axis_weight/y_axis_absweight);
            this.x_yaw += 10*x_axis_weight/x_axis_absweight;
            this.y_yaw += 10*y_axis_weight/y_axis_absweight;
            worldgen.rotate_world(this.y_yaw, -this.x_yaw);// 1.-parameter: positiv kippt nach hinten. y-achse: positiv kippt nach links
            if(this.x_yaw < -90.0f || this.x_yaw > 90.0f || this.y_yaw < -90.0f || this.y_yaw > 90.0f){
                Debug.Log("The world lost balance, you lose.");
                Application.Quit();
            }
            foreach(GameObject clickable in worldgen.clickables){
                Clickable tmp = worldgen.get_clicker(clickable);
                clickable.transform.position = worldgen.intpos2wordpos(tmp.get_posx(), tmp.get_posy());
                    if(clickable.name == worldgen.name_wicht){
                        clickable.transform.position += worldgen.wichtel_offset;
                    }
                    if(clickable.name == worldgen.name_resource){
                        clickable.transform.position += worldgen.resource_offset[clickable.GetComponent<Resource>().get_resourcetype()];
                    }
                    if(clickable.name == worldgen.name_house){
                        clickable.transform.position += worldgen.haus_offset;
                    }
                    clickable.transform.rotation = worldgen.world_rotation;
            }
            // check for victory
            if(worldgen.get_house(0, 0) != null && worldgen.get_house(4, -4) != null && worldgen.get_house(9, -4) != null && worldgen.get_house(9, 0) != null && worldgen.get_house(5, 4) != null && worldgen.get_house(0, 4) != null){
                Debug.Log("Ritual completet, you won!");

            }

            if (lastObject != null)
            {
                worldgen.get_clicker(lastObject).unselect();
                lastObject = null;
            }
        }
 }

}
