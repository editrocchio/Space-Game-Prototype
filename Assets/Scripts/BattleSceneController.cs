using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneController : MonoBehaviour
{

    private ArrayList playerShips;
    private float lastPosition;
    private float rightEdge;
    private const int frigateCount = 5;
    private const int corvetteCount = 3;
    private const int destroyerCount = 2;
    public const int spacing = 10;
    
    // Start is called before the first frame update
    void Start()
    {
        playerShips = ShipSelectionController.GetChosenShips();
        lastPosition = GameObject.Find("LeftBorder").transform.position.x + spacing;
        rightEdge = GameObject.Find("RightBorder").transform.position.x - spacing;

        foreach (string ship in playerShips) {
            if(ship.Equals("Frigate")) {
                for (int i = 0; i < frigateCount; i++) {
                    InstantiateShips("Frigate", new Vector3(0.30f, 0.30f, 0.30f), new Vector3(lastPosition, -20f, -7f));
                }
                SetParentShip("Frigate");
            } else if(ship.Equals("Corvette")) {
                for (int i = 0; i < corvetteCount; i++) {
                    InstantiateShips("Corvette", new Vector3(0.30f, 0.30f, 0.30f), new Vector3(lastPosition, -20f, -7f));
                }
                SetParentShip("Corvette");
            } else if (ship.Equals("Destroyer")) {
                for (int i = 0; i < destroyerCount; i++) {
                    InstantiateShips("Destroyer", new Vector3(0.30f, 0.30f, 0.30f), new Vector3(lastPosition, -20f, -7f));
                }
                SetParentShip("Destroyer");
            } else if(ship.Equals("Battleship")) {
                InstantiateShips("Battleship", new Vector3(1f, 1f, 1f), new Vector3(lastPosition += 10, -20f, -7f));
                lastPosition += 10;
                SetParentShip("Battleship");
            } else if(ship.Equals("Hubship")) {
                InstantiateShips("Hubship", new Vector3(0.30f, 0.30f, 0.30f), new Vector3(lastPosition, -20f, -7f));
                SetParentShip("Hubship");
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject InstantiateShips(string name, Vector3 size, Vector3 position) {
        GameObject ship = (GameObject)Instantiate(Resources.Load("Prefabs/" + name), position, Quaternion.Euler(-90f, 0f, 0f));
        ship.transform.localScale += size;
        lastPosition += 10;

        Destroy(ship.GetComponent<DisplayShipController>());
        ship.AddComponent<ShipController>();
        ship.AddComponent<Light>();

        return ship;
    }

    public static GameObject[] GetAllShipTypes(string type) {
        return GameObject.FindGameObjectsWithTag(type);
    }

    public static int GetSpacing() {
        return spacing;
    }

   public void SetParentShip(string parent) {
        GameObject.Find(parent + "(Clone)").GetComponent<ShipController>().SetParent(true);
        GameObject.Find(parent + "(Clone)").name = "Parent" + parent;
    }

}
