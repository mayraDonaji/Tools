/* Mayra Donaji Barrera Machuca
 * SFU, 2015
 * class to select objects with mouse click */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Selection : MonoBehaviour {

	//camera to send raycast
	public Camera main;

	//colors to show select / deselect
	private Color originalColor;
	public Color selectionColor;

	//raycast variables
	private RaycastHit hit;
	private Transform objectHit;
	private Ray ray;

	void Update () {

		if (Input.GetMouseButtonDown (0)) {

			if (objectHit != null ){
				//deselect object
				objectHit.gameObject.GetComponent<Renderer>().material.color = originalColor;
			}

			testHit(main, Input.mousePosition);
		}
	}

	private void testHit(Camera cam, Vector3 clickPosition){

		//check if click was originated in that camera
		if (cam.pixelRect.Contains(clickPosition)){

			//send ray from camra
			ray = cam.ScreenPointToRay(clickPosition);
				
			if (Physics.Raycast(ray, out hit)) {

				//get hit object
				objectHit = hit.transform;

				//change color
				originalColor = objectHit.gameObject.GetComponent<Renderer>().material.color;
				objectHit.gameObject.GetComponent<Renderer>().material.color = selectionColor;

				//send log
				Manager.Instance.sendLogEvent(new LogEntry(Manager.Instance.participant, DateTime.Now.ToString ("h:mm:ss.fff"),Manager.Instance.task,"selection",objectHit.name));
			}
		}
	}
}