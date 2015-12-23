/* Mayra Donaji Barrera Machuca
 * SFU, 2015
 * class to control camera movement */

using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	//mouse clicks
	private bool mouseRightClick = false;
	private bool mouseWheelClick = false;
	
	//camera rotation speed
	private float rotationSpeed = 20f;

	//camera translation speed
	private float translationSpeed = 10f;

	//camera zoomIn speed
	private float zoomInSpeed = 10f;
	
	//camera to be controler (optional if not substitute with transform.position)
	public GameObject cameraOne;

	void Update () {
		
		#region mouse clicks
		
		//right mouse click - translation
		if (Input.GetMouseButtonDown (1)) {
			
			if(!mouseRightClick){
				Manager.Instance.sendLogEvent(new LogEntry(Manager.Instance.participant, 
				DateTime.Now.ToString ("h:mm:ss.fff"),Manager.Instance.task,"translation",mouseRightClick));
			}

			mouseRightClick = true;
		}
		
		if(Input.GetMouseButtonUp(1)){
			
			if(mouseRightClick){
				Manager.Instance.sendLogEvent(new LogEntry(Manager.Instance.participant, 
				DateTime.Now.ToString ("h:mm:ss.fff"),Manager.Instance.task,"translation",mouseRightClick));
			}

			mouseRightClick = false;
		}
		
		//mouse wheel click - rotation
		if (Input.GetMouseButtonDown (2)) {		

			if(!mouseWheelClick){
				Manager.Instance.sendLogEvent(new LogEntry(Manager.Instance.participant, 
				DateTime.Now.ToString ("h:mm:ss.fff"),Manager.Instance.task,"rotation",mouseWheelClick));
			}

			mouseWheelClick = true;
		}

		if(Input.GetMouseButtonUp(2)){			

			if(!mouseWheelClick){
				Manager.Instance.sendLogEvent(new LogEntry(Manager.Instance.participant, 
				DateTime.Now.ToString ("h:mm:ss.fff"),Manager.Instance.task,"rotation",mouseWheelClick));
			}

			mouseWheelClick = false;
		}
		#endregion
		
		#region camera movement
		//mouse move left
		if(Input.GetAxis("Mouse X")<0){
			
			if (mouseWheelClick){
				(cameraOne, Vector3.up, 1);
			}
			
			if (mouseRightClick){				
				translation (cameraOne, cameraOne.transform.right, 1 );
			}
		}
		
		//mouse move right
		if(Input.GetAxis("Mouse X")>0){
			
			if (mouseWheelClick){
				(cameraOne, Vector3.up, -1);
			}
			
			if (mouseRightClick){				
				translation (cameraOne, cameraOne.transform.right, -1 );
			}
		}
		
		//mouse move up
		if(Input.GetAxis("Mouse Y")>0){
			
			if (mouseWheelClick){
				(cameraOne, Vector3.left, 1);
			}
			
			if (mouseRightClick){
				translation (cameraOne, cameraOne.transform.up, -1 );
			}
		}
		
		//mouse move down
		if(Input.GetAxis("Mouse Y")<0){
			
			if (mouseWheelClick){
				(cameraOne, Vector3.left, -1);
			}
			
			if (mouseRightClick){
				translation (cameraOne, cameraOne.transform.up, 1 );
			}
		}
		
		//wheel back - zoom out
		if (Input.GetAxis("Mouse ScrollWheel") < 0)
		{
			zoomIn(cameraOne, one.transform.forward, -1);
		}
		
		//wheel forward - zoom in
		if (Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			zoomIn(cameraOne, one.transform.forward, 1);
		}
		#endregion
	}


	private void zoomIn(GameObject camera, Vector3 axis, int direction){
		Vector3 localForward = camera.transform.worldToLocalMatrix.MultiplyVector(axis);
		one.transform.Translate((direction *localForward.x)/zoomInSpeed,(direction *localForward.y)/zoomInSpeed,(direction * localForward.z)/zoomInSpeed);
	}

	private void translation (GameObject camera, Vector3 axis, int direction ){

		if (camera.gameObject.GetComponent<Camera>().pixelRect.Contains(Input.mousePosition)){
			Vector3 localSide = camera.transform.worldToLocalMatrix.MultiplyVector(axis);
			camera.transform.Translate((direction * localSide.x)/translationSpeed,(direction *localSide.y)/translationSpeed,(direction *localSide.z)/translationSpeed);
		}
	}

	private void rotation(GameObject rotationObj, Vector3 axis, int direction){
		transform.RotateAround (rotationObj, axis, direction * rotationSpeed * Time.deltaTime);
	}
}
