/*==============================================================================
Made by Mayra Donaji Barrera Machuca
UTAS 2014
==============================================================================*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventManager : MonoBehaviour {
	/**class that manage the Server/Client events **/

	/** class instance to create singleton **/
	private static EventManager instance;
	private EventManager(){}
	public static EventManager Instance{
		get     
		{       
			if (instance ==  null)
				instance = GameObject.FindObjectOfType(typeof(EventManager)) as  EventManager;      
			return instance;
		}
	}

	public Dictionary<string,Vector3> positionsInPlane = new Dictionary<string, Vector3>();//dictionary with tile info

	#region player
	/** info from client to same client in server (and viceversa) **/

	public GameObject client; //gameObject to initialize a client

	/* unique identifiers (assigned from server) */
	public delegate void GiveClientInfoHandler(string IP, string name, Vector3 color);
	public event GiveClientInfoHandler GiveClientInfo;

	[RPC] void SendUniqueClientID(string IP, string name, Vector3 color){
		GiveClientInfo(IP, name, color);
	}

	/* object selection gameobject to client*/
	public delegate void SelectObjectHandler(string client, string objectSelected, bool state, bool trail);
	public event SelectObjectHandler SelectObject;

	public void sendObject (string client, string objectS, bool state, bool trail){
		networkView.RPC("Selected",RPCMode.Others,client, objectS, state, trail);
		Selected(client, objectS, state, trail);
	}

	[RPC] void Selected(string client, string objectSelected, bool state, bool trail){
		SelectObject(client, objectSelected, state, trail);
	}

	/* camera rotation */
	public delegate void SendCameraHandler(string client, Quaternion newRotation, Vector3 newPosition);
	public event SendCameraHandler SendCamera;
	
	public void CameraTransformEvent(string clientName, Transform clientTransform){
		networkView.RPC ("SendTransform", RPCMode.Server, clientName, clientTransform.rotation, clientTransform.position);
	}

	[RPC] void SendTransform(string clientName, Quaternion newRotation, Vector3 newPosition){
		SendCamera (clientName, newRotation, newPosition);
	}

	/* no input */
	public delegate void SendWaitHandler(string client);
	public event SendWaitHandler SendWait;

	public void sendWaitEvent (string client){
		SendWait (client);
	}

	/* write log */
	public delegate void LogHandler(string text, int type);
	public event LogHandler Log;
	
	public void writeLog(string text, int type){
		Log(text, type);
	}
	#endregion

	#region PrefabActions
	/** to control Gameobject movement according to user input **/

	/* gameobject delete */
	public delegate void ObjectDeleteHandler(string name);
	public event ObjectDeleteHandler ObjectDelete;
	
	public void objectDeleteEvent(string name){
		//name is hit object name
		ObjectDelete(name);
	}

	/* gameobject selection */
	public delegate void ObjectSelectHandler(string values, Vector3 clientColor);
	public event ObjectSelectHandler ObjectSelect;

	public void objectSelectEvent(string values, Vector3 clientColor){
		//values are selected gameobject and clients name
		ObjectSelect(values, clientColor);
	}

	/* gameobject deselection */
	public delegate void ObjectDeselecHandler(string valuesGameobject, string clientName);
	public event ObjectDeselecHandler ObjectDeselect;

	public void objectDeselectEvent(string valuesGameobject, string clientName){
		//valuesGameobject are [1]=display / [0]=gameobject name
		string[] values = valuesGameobject.Split(',');
		//display is virtual layer order in server and to clients
		if (int.Parse (values [1]) == 2) {
			VirtualSpaceManager.Instance.deselectObjectVirtualSpace(values[0],clientName);
		} 
		//display public / private only locally
		else {
			ObjectDeselect (values[0], clientName);
		}
	}

	/* gameobject translation */
	public delegate void MoveObjectHandler(string valuesGameobject, string clientName, Vector3 newGameobjectPosition);
	public event MoveObjectHandler MoveObject;

	public void moveObjectEvent(string valuesGameobject, string clientName, Vector3 newGameobjectPosition){
		string[] values = valuesGameobject.Split(',');
		if (int.Parse (values [1]) != 2) {
			MoveObject (values[0], clientName, newGameobjectPosition);
		}
	}

	/* gameobject rotation */
	public delegate void RotateObjectHandler(string valuesGameobject, string clientName, float newGameobjectAngle);
	public event RotateObjectHandler RotateObject;

	public void rotateObjectEvent(string valuesGameobject, string clientName, float newGameobjectAngle){
		string[] values = valuesGameobject.Split(',');
		
		if (int.Parse (values [1]) != 2) {
			RotateObject (values[0], clientName, newGameobjectAngle);
		}
	}

	/* gameobject scalation */
	public delegate void ScaleObjectHandler(string valuesGameobject, string clientName, float newGameobjectScale);
	public event ScaleObjectHandler ScaleObject;

	public void scaleObjectEvent(string valuesGameobject, string clientName, float newGameobjectScale){
		string[] values = valuesGameobject.Split(',');
		
		if (int.Parse (values [1]) == 2) {
			VirtualSpaceManager.Instance.scaleObjectVirtualSpace(values[0],clientName,newGameobjectScale);
		} else {
			ScaleObject (values[0], clientName, newGameobjectScale);
		}
	}
	
	#endregion	
}