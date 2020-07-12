using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

    private static ObservableCollection<string> selectedShips;
    
    // Start is called before the first frame update
    void Start()
    {
        playerShips = ShipSelectionController.GetChosenShips();
        lastPosition = GameObject.Find("LeftBorder").transform.position.x + spacing;
        rightEdge = GameObject.Find("RightBorder").transform.position.x - spacing;
        selectedShips = new ObservableCollection<string>();
        selectedShips.CollectionChanged += HandleChange;

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

    public static ObservableCollection<string> GetSelectedShips() {
        return selectedShips;
    }

    public void HandleChange(object sender, NotifyCollectionChangedEventArgs e) {

        if (e.Action == NotifyCollectionChangedAction.Add) {
            foreach (GameObject ship in GetAllShipTypes((string)e.NewItems[0])) {
                ToggleShipSelection(ship);
            }
        }

        if (e.Action == NotifyCollectionChangedAction.Remove) {
            foreach (GameObject ship in GetAllShipTypes((string)e.OldItems[0])) {
                ToggleShipSelection(ship);
            }
        }
        

        if (e.Action == NotifyCollectionChangedAction.Reset) {
            BackgroundScript bgScript = GameObject.FindGameObjectsWithTag("spacebg")[0].GetComponent<BackgroundScript>();
            foreach (string ship in bgScript.GetOldShips()) {
                foreach (GameObject shipObj in GetAllShipTypes(ship)) {
                    ToggleShipSelection(shipObj);
                }
            }
        }
    }

    private void ToggleShipSelection(GameObject ship) {
        Behaviour shipHalo = (Behaviour)ship.GetComponent("Halo");
        ShipController shipController = ship.GetComponent<ShipController>();
        bool shipSelected = shipController.IsSelected();
        shipHalo.enabled = !shipSelected;
        shipController.Select(!shipSelected);
    }

    public static void AddToSelectedShips(string shipToAdd) {
        selectedShips.Add(shipToAdd);
    }

    public static void RemoveFromSelectedShips(string shipToAdd) {
        selectedShips.Remove(shipToAdd);
    }

    public static void RemoveAllSelectShips() {
        selectedShips.Clear();
    }
}
