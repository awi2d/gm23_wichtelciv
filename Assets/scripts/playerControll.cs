using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControll : MonoBehaviour
{

    public float movementSpeed = 0.1f;

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
            System.String name = hit.transform.gameObject.name;
            Debug.Log("player clicked on "+name);
            Debug.Log("current_selection ="+this.current_selection);
            if(this.current_selection == null){
                if(System.String.Equals(name, "wichtel")){
                    Debug.Log("set Selection to "+name);
                    this.current_selection = hit.transform.gameObject;
                }
                if(System.String.Equals(name, "haus")){
                    Debug.Log("haus menue maybe?");
                    //do whatever
                }
            }else{
                if(System.String.Equals(this.current_selection.name, "wichtel") && System.String.Equals(name, "ground(Clone)")){
                    ground_position gp = hit.transform.gameObject.GetComponent<ground_position>();
                    Debug.Log("move wichtel to ("+gp.posx+", "+gp.posy+")");
                    //calculate what ground tile was hit
                    // check if it is within movement range of wichte.
                    // move wichtel there
                    this.current_selection.transform.position = hit.transform.position+new Vector3(0, 1, 0);
                    // substract movement length from wichtel_movement_left
                }
                if(System.String.Equals(this.current_selection.name, "wichtel") && System.String.Equals(name, "haus")){
                    Debug.Log("wichtel nach Haus");
                    //do whatever
                }
                this.current_selection = null;
            }
        }else{
            this.current_selection = null;
        }
    }
 }

    //on rundeweiter:
    //  reset wichtels can_move, do action for every city, adjust schraeglage von ground, check for win.

}
