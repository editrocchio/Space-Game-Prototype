using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class DisplayShipController : MonoBehaviour
{

    public GameObject shipStats;
    private string shipName;
    private string shipPrefix;

    void OnMouseOver() {
        if (gameObject.GetComponent<Light>() == null) {
            gameObject.AddComponent<Light>();
            Light light = gameObject.GetComponent<Light>();

            light.intensity = 5;
            light.range = 100;
            light.color = Color.red;
            light.type = LightType.Directional;
        }
    }

    void OnMouseExit() {
        Destroy(gameObject.GetComponent<Light>());
    }

    void OnMouseDown() {
        Instantiate(shipStats);
        shipPrefix = gameObject.name.Substring(0, 3);
        shipName = gameObject.name.Substring(0, gameObject.name.Length - 7);
        Instantiate(Resources.Load("Prefabs/" + shipPrefix + "TextContainer"));
        Instantiate(Resources.Load("Prefabs/" + "Tier1"));
        GameObject btn = GameObject.Find("CancelBtn");
        btn.GetComponent<Button>().onClick.AddListener(OnStatsCancelClick);
        GameObject.Find("SelectBtn").GetComponent<Button>().onClick.AddListener(OnStatsSelectClick);
        gameObject.SetActive(false);
    }

    public void OnStatsSelectClick() {
        AddSelection(true);
    }

    public void OnStatsCancelClick() {
        AddSelection(false);
    }

    public void CloseShipStatsMenu() {
        Destroy(gameObject);
        Destroy(GameObject.Find("ShipStats(Clone)"));
        Destroy(GameObject.Find("Tier1(Clone)"));
        Destroy(GameObject.Find(shipPrefix + "TextContainer(Clone)"));
    }

    public void AddSelection(bool selected) {
        ShipSelectionController shipSelectionController = GameObject.Find(shipName + "Container").GetComponent<ShipSelectionController>();
        shipSelectionController.SelectShip(selected);
        if (selected) {
            ShipSelectionController.AddChosenShip(gameObject.name.Substring(0, gameObject.name.Length - 7));
        } else {
            ShipSelectionController.RemoveChosenShip(gameObject.name.Substring(0, gameObject.name.Length - 7));
        }
        CloseShipStatsMenu();
    }
}
