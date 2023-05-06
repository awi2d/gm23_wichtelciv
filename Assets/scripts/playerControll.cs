using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerControll : MonoBehaviour
{

    public float movementSpeed = 0.1f;

    GameObject lastObject;

    private List<int> wichtel_movement_left;
    private GameObject current_selection;
    // Start is called before the first frame update
    void Start()
    {
        this.wichtel_movement_left = new List<int>{};
        this.current_selection = null;
    }

    void FixedUpdate(){
        // Move camera
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        transform.position = transform.position + new Vector3(this.movementSpeed*horizontalInput, 0, this.movementSpeed*verticalInput);
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

    //on rundeweiter:
    //  reset wichtels can_move, do action for every city, adjust schraeglage von ground, check for win.

}
