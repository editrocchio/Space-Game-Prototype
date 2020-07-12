using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BackgroundScript : MonoBehaviour
{
    private List<string> oldShips;
    // Start is called before the first frame update
    void Start()
    {
        oldShips = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)) {
                //if we hit the background deselect all
                if (hit.transform.gameObject.CompareTag("spacebg")) {
                    oldShips = BattleSceneController.GetSelectedShips().ToList();

                    BattleSceneController.RemoveAllSelectShips();
                } 
            }

            
        }
    }

    public List<string> GetOldShips() {
        return oldShips;
    }

}
