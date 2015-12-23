/*==============================================================================
Made by Mayra Donaji Barrera Machuca
UTAS 2014
touch input control 
==============================================================================*/
using UnityEngine;
using System.Collections;

public class InputControl : MonoBehaviour {

	//control variables
	public bool isObjectSelected = false;

	//variables for rotate/translate
	private float turnAngle = 0;
	private float turnAngleDelta = 0;
	private float pinchDistance;
	private float pinchDistanceDelta;

	void Update () {

		switch (Input.touchCount) {
			case 0:
				fingerOnScreen = false;
				break;
			case 1:			
				if (!fingerOnScreen) { //select
					ClientManager.Instance.makeSound ("click");				
					Select ();
					fingerOnScreen = true;
				} else { //translate			
					Translate ();
				}
				break;
			case 2:
				if (!fingerOnScreen) {
					ClientManager.Instance.makeSound ("click");
					//select
					Select ();
				}
				
				if (isObjectSelected) {
					//rotate or scale
					Rotate ();
					Scale ();
				}
				break;
			default:
				//wait
				NoAction ();
				break;
			}
		}
	}

	/** select **/
	private void Select (){
		selectedObjectPosition = Input.GetTouch (0).position;
		ClientManager.Instance.selectInputEvent (Input.GetTouch (0).position);
	}
	
	/** wait **/
	private void NoAction (){
		ClientManager.Instance.noActionEvent ();
	}
	
	/** translate **/
	private void Translate (){
		if (Input.GetTouch (0).phase == TouchPhase.Moved) {
			ClientManager.Instance.moveInputEvent (Input.GetTouch (0).position);
		}
	}
	
	/** rotate **/
	private void Rotate (){
		turnAngle = turnAngleDelta = 0;
		if (Input.GetTouch (0).phase == TouchPhase.Moved || Input.GetTouch (1).phase == TouchPhase.Moved) {
			turnAngle = MathLibrary.GetAngle (Input.GetTouch (0).position, Input.GetTouch (1).position);
			float prevTurn = MathLibrary.GetAngle (Input.GetTouch (0).position - Input.GetTouch (0).deltaPosition, Input.GetTouch (1).position - Input.GetTouch (1).deltaPosition);
			
			turnAngleDelta = Mathf.DeltaAngle (prevTurn, turnAngle);
			if (Mathf.Abs (turnAngleDelta) > 0) {
				turnAngleDelta *= (Mathf.PI / 2);
			} else {
				turnAngle = turnAngleDelta = 0;
			}
		}
		ClientManager.Instance.rotateInputEvent (turnAngleDelta);
	}
	
	/** scale **/
	private void Scale (){
		if (Input.GetTouch (0).phase == TouchPhase.Moved || Input.GetTouch (1).phase == TouchPhase.Moved) {
			//get positions of fingers
			Vector3 pos0 = Input.GetTouch (0).position;
			Vector3 pos1 = Input.GetTouch (1).position;
			ClientManager.Instance.scaleInputEvent (Vector3.Distance (pos0, pos1));
		}
	}
}