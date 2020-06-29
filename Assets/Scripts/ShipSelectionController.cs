using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipSelectionController : MonoBehaviour 
{
	public GameObject element;
	public float offsetValue;
	public float smoothTime;
	private Ray ray;
	private Vector3 initPos;
	private Collider elementCol;

	private MeshRenderer rend;
	private Color hoverColor = Color.red;
	private Color hoverOffColor = Color.white;

	private GameObject corners;
	private GameObject text;
	private GameObject icon;

	public GameObject ship;

	private static GameObject displayedShip;
	private static GameObject lastClickedBox;
	private static GameObject lastHoveredBox;

	private GameObject slotFilled;
	private GameObject slotEmpty;

	private ArrayList chosenShips;

	void Start()
	{
		initPos = element.transform.localPosition;
		elementCol = GetComponent<Collider>();

		slotFilled = GameObject.Find(gameObject.name + "/DangerIcon");
		slotEmpty = GameObject.Find(gameObject.name + "/Hint_Ghost");
		slotEmpty.SetActive(false);

		GameObject.Find("DoneBtn").GetComponent<Button>().onClick.AddListener(OnDoneBtnClick);

		chosenShips = new ArrayList();
	}
	void Update()
	{
		RaycastHit hit;
		element.transform.localPosition = initPos;
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (GameObject.Find("ShipStats") == null) {
			if (elementCol.Raycast(ray, out hit, 100.0f)) {
				element.transform.localPosition = Vector3.Lerp(element.transform.localPosition, new Vector3(element.transform.localPosition.x, element.transform.localPosition.y, offsetValue), smoothTime);

				corners = element.transform.GetChild(0).gameObject;
				text = element.transform.GetChild(1).gameObject;
				icon = element.transform.GetChild(2).gameObject;

				if (lastHoveredBox != null && lastClickedBox == null) {
					ChangeBoxColor(lastHoveredBox.transform.GetChild(0).gameObject,
						lastHoveredBox.transform.GetChild(1).gameObject,
						lastHoveredBox.transform.GetChild(2).gameObject, null,
						hoverOffColor);
				}

				if (element.name.Contains("Container") && lastClickedBox == null) {
					ChangeBoxColor(corners, text, icon, null, hoverColor);
				}

				lastHoveredBox = element;

			} else {
				if (lastClickedBox == null) {
					ChangeBoxColor(corners, text, icon, null, hoverOffColor);
				}
			}

			if (displayedShip != null) {
				displayedShip.transform.Rotate(Vector3.up * Time.deltaTime);
			}
		}
    }

	void OnMouseDown() {
		if (GameObject.Find("ShipStats") == null) {
			if (element.name.Contains("Container")) {
				ChangeBoxColor(corners, text, icon, null, hoverColor);

				if (lastClickedBox != null) {
					ChangeBoxColor(lastClickedBox.transform.GetChild(0).gameObject,
						lastClickedBox.transform.GetChild(1).gameObject,
						lastClickedBox.transform.GetChild(2).gameObject, null,
						hoverOffColor);

				}

				if (displayedShip != null) {
					Destroy(displayedShip);
				}

				if (lastClickedBox != null && lastClickedBox.name.Equals(element.name)) {
					lastClickedBox = null;
					Destroy(displayedShip);
				} else {
					lastClickedBox = element;
					displayedShip = Instantiate(ship);
				}

			}
		} 
    }

	private void ChangeBoxColor(GameObject boxCorners, GameObject boxText, GameObject boxIcon, GameObject ghostIcon, Color color) {
		SetColors(boxCorners, color);
		SetColors(boxText, color);
		SetColor(boxIcon, color);

		if(ghostIcon != null) {
			SetColors(ghostIcon, color);
        }
	}
	private void SetColors(GameObject go, Color color) {
		if (go != null) {
			foreach (Transform item in go.transform) {
				rend = item.GetComponent<MeshRenderer>();
				if (rend.material.color == color) {
					break;
				}
				rend.material.SetColor("_Color", color);			
			}
		}
	}
	private void SetColor(GameObject go, Color color) {
		if (go != null) {
			rend = go.GetComponent<MeshRenderer>();
			if (rend.material.color != color) {
				rend.material.SetColor("_Color", color);
			}
		}
	}

	public GameObject GetDisplayedShip() {
		return displayedShip;
    }

	public void SelectShip(bool selected) {
		slotFilled.SetActive(!selected);
		slotEmpty.SetActive(selected);

		if (selected) {
			SetColors(lastClickedBox.transform.GetChild(0).gameObject, hoverOffColor);
			SetColors(lastClickedBox.transform.GetChild(1).gameObject, hoverOffColor);
			SetColors(lastClickedBox.transform.GetChild(3).gameObject, hoverColor);

		} else {
			ChangeBoxColor(lastClickedBox.transform.GetChild(0).gameObject,
							lastClickedBox.transform.GetChild(1).gameObject,
							lastClickedBox.transform.GetChild(2).gameObject,
							null,
							hoverOffColor);

		}

		lastClickedBox = null;
	}

	public void OnDoneBtnClick() {
		foreach(string s in chosenShips) {
			Debug.Log(s);
		}
    }

	public void AddChosenShip(string chosenShip) {
		chosenShips.Add(chosenShip);
    }

	public void RemoveChosenShip(string chosenShip) {
		chosenShips.Remove(chosenShip);
    }
}
