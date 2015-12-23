/*==============================================================================
Made by Mayra Donaji Barrera Machuca
UTAS 2014
class to control the state of objects*
==============================================================================*/
using UnityEngine;
using System.Collections;

public class PrefabState : MonoBehaviour {
		
	public string selectedByName; //to know who select this object
	public Color defaultColor; //to return the object to neutral color
	public bool isSelect = false; // to know if object is selected

	public Color ownerColor;
	public bool isMoving; //to know if object is moved
	public bool readyToDelete; //to know if object is ready to delete
	public int myType;

	private GameObject myParent;//to know gameobject parent
	private SignsHandle myParentInfo;

	/** class received events **/
	void Awake(){
		EventManager.Instance.ObjectSelect 		+= new EventManager.ObjectSelectHandler(MakeSelection);
		EventManager.Instance.ObjectDeselect 	+= new EventManager.ObjectDeselecHandler(RemoveSelection);
		EventManager.Instance.MoveObject 		+= new EventManager.MoveObjectHandler(MovePrefab);
		EventManager.Instance.RotateObject 		+= new EventManager.RotateObjectHandler(RotatePrefab);
		EventManager.Instance.ScaleObject 		+= new EventManager.ScaleObjectHandler(ScalePrefab);
		EventManager.Instance.SendWait 			+= new EventManager.SendWaitHandler(Wait); 
	}
	
	void Start () {
		//defaultColor = new Color(0.5f,0.5f,0.5f,1f);
		myParent = this.gameObject.transform.parent.gameObject;
		myParentInfo = (SignsHandle)myParent.GetComponent ("SignsHandle");
	}

	/** delete gameobject **/
	private void Delete(string touchObject){
		if (this.name.Equals (touchObject)) {
			//remove events
			EventManager.Instance.ObjectSelect 		-= new EventManager.ObjectSelectHandler(MakeSelection);
			EventManager.Instance.ObjectDeselect 	-= new EventManager.ObjectDeselecHandler(RemoveSelection);
			EventManager.Instance.MoveObject 		-= new EventManager.MoveObjectHandler(MovePrefab);
			EventManager.Instance.RotateObject 		-= new EventManager.RotateObjectHandler(RotatePrefab);
			EventManager.Instance.ScaleObject 		-= new EventManager.ScaleObjectHandler(ScalePrefab);
			EventManager.Instance.SendWait 			-= new EventManager.SendWaitHandler(Wait); 
			EventManager.Instance.ObjectDelete		-= new EventManager.ObjectDeleteHandler(Delete);
			VirtualSpaceManager.Instance.SendStatus -= new VirtualSpaceManager.SendStatusHandler (ChangeDisplay);
			
			//return control variables to initial state
			selectedByName = "";
			isSelect = false;
			ownerColor = defaultColor;
			renderer.material.color = defaultColor;
			//make object redy to delete
			readyToDelete = true;
		}
	}
	
	/** selection **/
	private void MakeSelection(string gameobjectValues, Vector3 clientColor){
		//values [0]=selected gameobject name [1]=client who selected it
		string[] values = gameobjectValues.Split(',');
		if(values[0].Equals(this.name) && !isSelect){
			//change control variables to selection
			isSelect = true;
			selectedByName = values[1];
			ownerColor = new Color(clientColor.x,clientColor.y,clientColor.z);
			this.gameObject.renderer.material.color = ownerColor;
			//send object name and display
			string newValues = this.name+","+display+","+myType+","+myParentInfo.myText;
			//inform client that object was successfully selected
			EventManager.Instance.sendObject(selectedByName, newValues, isSelect, false);
		}
	}

	/** deselection **/	
	private void RemoveSelection(string objectName, string name){
		//confirm that client who want to deselect, deselecte selected gameobject
		if(isSelect && this.name.Equals(objectName) && selectedByName.Equals(name)){
			//change control variables to normal
			isSelect = false;
			ownerColor = defaultColor;
			renderer.material.color = defaultColor;
			//inform client that object was successfully deselected
			EventManager.Instance.sendObject (selectedByName, "", isSelect, false);
			selectedByName = "";
		}
	}

	/** noInput **/	
	//IMPORTANT FOR PASS FROM PUBLIC TO VIRTUAL//
	private void Wait(string clientName){
		if (selectedByName.Equals (clientName)) {
			//client is not doing any input
			isMoving = false;
		}
	}
	
	/** move **/	
	private void MovePrefab (string objectName, string clientName, Vector3 newPosition){
		//confirm that client who want to move, move selected gameobject
		if(isSelect && this.name.Equals(objectName) && selectedByName.Equals(clientName)){
			myParent.transform.position = new Vector3(newPosition.x,newPosition.y,newPosition.z*-1f);
			//client is doing an input
			isMoving = true;
		}
	}

	/** rotate **/
	private void RotatePrefab(string objectName, string name, float angle){
		if (isSelect && this.name.Equals (objectName) && selectedByName.Equals (name)) {
			this.transform.Rotate (Vector3.back * (-angle) / 1.5f, Space.World);
		}
	}

	/** scale **/
	private void ScalePrefab (string objectName, string name, float distance){
		if(isSelect && this.name.Equals(objectName) && selectedByName.Equals(name))
			this.transform.localScale = new Vector3(distance/100, distance/100, distance/100);
	}
}