/*==============================================================================
Made by Mayra Donaji Barrera Machuca
UTAS 2014
==============================================================================*/
using UnityEngine;
using System.Collections;

public class UserInputManager : MonoBehaviour {
	/** class to control info recived from userInput  **/
	
	private Vector3 value; //to initializate the output value from TryGetValue
	private ClientInfo settings; //this client info

	/** class received events **/
	void Awake(){
		ClientManager.Instance.SelectInput	 	+= new ClientManager.SelectInputHandler(Select);
		ClientManager.Instance.MoveInput 		+= new ClientManager.MoveInputHandler(Translate);
		ClientManager.Instance.RotateInput 		+= new ClientManager.RotateInputHandler(Rotate);
		ClientManager.Instance.ScaleInput 		+= new ClientManager.ScaleInputHandler(Scale);
		ClientManager.Instance.DeletePrefab 	+= new ClientManager.DeletePrefabHandler (Delete);
		ClientManager.Instance.NoAction 		+= new ClientManager.NoActionHandler (Wait);
		ClientManager.Instance.FoundMarker      += new ClientManager.FoundMarkerHandler (Marker);
		ClientManager.Instance.SendMoreInfo += new ClientManager.SendMoreInfoHandler (More);
	}
	
	void Start () {
		this.transform.parent = GameObject.Find("Manager").transform;
		settings = (ClientInfo) gameObject.GetComponent(typeof(ClientInfo));
	}

	/** delete **/
	private void Delete(Vector3 inputPosition){
		//raycast to know if there is an object on that point
		Ray ray=Camera.main.ScreenPointToRay(inputPosition);
		RaycastHit hit;
		
		if(Physics.Raycast(ray,out hit)){
			//send which object was hit
			EventManager.Instance.objectDeleteEvent(hit.collider.gameObject.name);
			networkView.RPC("LogSelect",RPCMode.Server,2, 8, settings.myName, "");
		}
	}

	/** select **/
	private void Select(Vector3 inputPosition){
		//raycast to know if there is an object on that point
		Ray ray=Camera.main.ScreenPointToRay(inputPosition);
		RaycastHit hit;

		if(Physics.Raycast(ray,out hit)){
			//create variable with object name and who select it
			string objectSenderNames = hit.collider.gameObject.name + "," + settings.myName;
			//to deselect other object if selected
			if(settings.objectSelected){
				Deselect();
			}
			//event to gameobject to select
			EventManager.Instance.objectSelectEvent(objectSenderNames,settings.myColor);
			networkView.RPC("LogSelect",RPCMode.Server,2, 1, settings.myName, hit.collider.gameObject.name);

		}
	}

	/** deselect **/
	private void Deselect(){
		//create variable with object name and who select it
		string tempName = settings.selectedObjectName+","+settings.selectedObjectDisplay;
		//change event on server
		networkView.RPC("SendDeselect", RPCMode.Server,settings.myName, tempName);
		//change event on client
		SendDeselect(settings.myName, tempName);
		settings.objectSelected = false;
	}

	[RPC] void SendDeselect(string clientName, string objectName){
			//event to gameobject to deselect
		string[] words = objectName.Split(',');
		int display = int.Parse(words[1]);
		if (display == 2) {
			VirtualSpaceManager.Instance.deselectObjectVirtualSpace(words[0],clientName);
		} else {
			EventManager.Instance.objectDeselectEvent (objectName, clientName);
		}
	}

	/** idle **/
	private void Wait(){
		//event to inform no finger on tablet
		networkView.RPC("SendWait", RPCMode.Server,settings.myName);
		networkView.RPC("LogSelect",RPCMode.Server,2, 2, settings.myName, "");
	}
	
	[RPC] void SendWait(string clientName){
		EventManager.Instance.sendWaitEvent(clientName);
	}

	/** translate **/
	private void Translate(Vector3 position){
		Ray ray=Camera.main.ScreenPointToRay(position);
		RaycastHit hit;

		if(Physics.Raycast(ray,out hit)){	
			if (settings.objectSelected) {
				//create name with object selected and in which display it is
				string tempName = settings.selectedObjectName+","+settings.selectedObjectDisplay;
				networkView.RPC("LogSelect",RPCMode.Server,2, 3, settings.myName, tempName);
				// if object in private display only send event to the client
				if(settings.selectedObjectDisplay==3){ 
					SendTranslate(hit.collider.gameObject.name, settings.myName, tempName);
				} 
				// if object is virtual/public send event to server
				else { 
					networkView.RPC("SendTranslate", RPCMode.Server,hit.collider.gameObject.name, settings.myName, tempName);
				}
			}
		}
	}

	[RPC] void SendTranslate(string tileName, string clientName, string objectValues){
		//get tile position in v3 (change depeding server/client)
		if (EventManager.Instance.positionsInPlane.TryGetValue(tileName, out value)){
			//event to gameobject to move
			EventManager.Instance.moveObjectEvent(objectValues, clientName, value);
		}
	}

	/** rotate **/
	private void Rotate(float angle){
		if (settings.objectSelected){
			//create name with object selected and in which display it is
			string tempName = settings.selectedObjectName+","+settings.selectedObjectDisplay;
			networkView.RPC("LogSelect",RPCMode.Server,2, 4, settings.myName, tempName);
			if(settings.selectedObjectDisplay==3){
				SendRotate (angle, settings.myName, tempName);
			} else {
				networkView.RPC ("SendRotate", RPCMode.Server, angle, settings.myName, tempName);
			}
		}
	}

	[RPC] void SendRotate(float angle, string clientName, string objectValues){
		//event to gameobject to rotate
		EventManager.Instance.rotateObjectEvent(objectValues,clientName, angle);
	}

	/** scale **/
	private void Scale(float dist){
		if (settings.objectSelected) {
			//create name with object selected and in which display it is
			string tempName = settings.selectedObjectName+","+settings.selectedObjectDisplay;
			networkView.RPC("LogSelect",RPCMode.Server,2, 5, settings.myName, tempName);
			if(settings.selectedObjectDisplay==3){
				SendScale (dist, settings.myName, tempName);
			} else {
				networkView.RPC ("SendScale", RPCMode.Server, dist, settings.myName, tempName);
			}
		}
	}

	[RPC] void SendScale(float dis, string clientName, string objectValues){
		//event to gameobject to scale
		EventManager.Instance.scaleObjectEvent(objectValues, clientName, dis);
	}

	private void More(string type){
		networkView.RPC("LogSelect",RPCMode.Server,2, 9, settings.myName, type);
	}
	#region writeLog
	private void Marker(bool found){
		networkView.RPC("LogSelect",RPCMode.Server,1, 3, settings.myName, "marker"+found);
	}

	[RPC] void LogSelect(int category, int type, string clientName, string value){
		ClientManager.Instance.sendLogEvent(new LogEntry (category, type, clientName, value));
	}
	#endregion
}