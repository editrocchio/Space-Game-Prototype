using System;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 targetPosition;
    private Vector3 directionVector;
    private Quaternion lookRotation;
    private int positionInGroup;
    private GameObject parentShip;

    public float speed = 50F;
    public float rotationSpeed = 2f;
    public bool selected;
    private Behaviour halo;
    public bool parent;
    private string shipName;

    public const string bgName = "spacebg";

    // Start is called before the first frame update
    void Start()
    {
        selected = false;
        halo = (Behaviour)gameObject.GetComponent("Halo");
        halo.enabled = false;
        targetPosition = transform.position;
        if (gameObject.name.Contains("Parent")) {
            shipName = gameObject.name.Substring(6);
        } else {
            shipName = gameObject.name.Substring(0, gameObject.name.Length - 7);
        }
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(1) && selected) {
            mainCamera = FindCamera();

            // We need to actually hit an object
            RaycastHit hit;
            if (!Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 100)) {
                return;
            }
            // We need to hit something (with a collider on it)
            if (!hit.transform) {
                return;
            }

            
            // Get input vector from kayboard or analog stick and make it length 1 at most
            targetPosition = hit.point;
            lookRotation = Quaternion.LookRotation(targetPosition);
            parentShip = GameObject.Find("Parent" + shipName); 
            if (!parent) {
                positionInGroup = Array.IndexOf(GameObject.FindGameObjectsWithTag(shipName), gameObject);
            }
        }

        // transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        if (parent) {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        } else {
            if (parentShip != null) {
                transform.position = parentShip.transform.position + parentShip.transform.TransformDirection(new Vector3(Int16.Parse(positionInGroup + "0"), 0, -1));
            }
        }

    }

    void OnMouseDown() {
        if (!selected) {
            BattleSceneController.AddToSelectedShips(shipName);
        } else {
            BattleSceneController.RemoveFromSelectedShips(shipName);
        }
    }

    public bool IsSelected() {
        return selected;
    }

    public void Select(bool selected) {
        this.selected = selected;
    }

    public GameObject[] GetAllShipsOfThisType() {

        return BattleSceneController.GetAllShipTypes(shipName);
    }

    private Camera FindCamera() {
        Camera camera = GetComponent<Camera>();
        if (camera)
            return camera;
        else
            return Camera.main;
    }

    public void SetParent(bool parent) {
        this.parent = parent;
    }
}
