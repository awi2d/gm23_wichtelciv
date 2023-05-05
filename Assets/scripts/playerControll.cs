using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControll : MonoBehaviour
{

    private List<int> wichtel_movement_left;
    private GameObject current_selection;
    // Start is called before the first frame update
    void Start()
    {
        this.wichtel_movement_left = new List<int>{};
        this.current_selection = null;
    }

    // Update is called once per frame
 void Update() {
    //if mouse is clicked:
    //  get what was clicked on (unit, city or tile) and act accordingly (selecting that thing or moving the selected unit to that tile)
    if (Input.GetMouseButtonDown(0)) {
        Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        RaycastHit hit;
        if (Physics.Raycast( ray, out hit, 100 )) {
            Debug.Log( hit.transform.gameObject.name );
            if(this.current_selection == null){
                this.current_selection = hit.transform.gameObject;
            }
        }else{
            this.current_selection = null;
        }
    }
 }

    //on rundeweiter:
    //  reset wichtels can_move, do action for every city, adjust schraeglage von ground, check for win.

}
