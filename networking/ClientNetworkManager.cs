/*==============================================================================
Made by Mayra Donaji Barrera Machuca
UTAS 2014
==============================================================================*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClientNetworkManager : MonoBehaviour {
	/** class to join a server **/

	//read xml variables
	public TextAsset xmlDefinition;
	private Dictionary<string, string> settings = new Dictionary<string, string>();
	public LoadXML loadXML;

	//connect server variables
	private HostData[] hostList;
	//server variables
	private string ipAdress= "131.217.255.36";
	private int masterServerPort = 23466;
	private int facilitatorPort = 50005;
	
	// Use this for initialization
	void Start () {
		//load settings
		settings = loadXML.ReadXML(xmlDefinition);
	}

	/** find host from settings **/
	private void RefreshHostList(){
		EventManager.Instance.writeLog("looking for host",0);

		MasterServer.RequestHostList(settings["typeName"]);
	}

	void OnMasterServerEvent(MasterServerEvent msEvent){
		if (msEvent == MasterServerEvent.HostListReceived){
			EventManager.Instance.writeLog("host found",0);
			hostList = MasterServer.PollHostList();
		} else {
			EventManager.Instance.writeLog("host not found",0);
		}
	}

	/** join server **/
	private void FindHost(){
		if (hostList != null){
			for (int i = 0; i < hostList.Length; i++){
				JoinServer(hostList[i]);
			}
		} else {
			EventManager.Instance.writeLog("no host",0);
		}
	}

	private void JoinServer(HostData hostData){
		Network.Connect(hostData);
	}

	void OnConnectedToServer(){
		//to pass unique variables to this client
		ClientManager.Instance.myIp = "" + Network.player.ipAddress;
		ClientManager.Instance.status = 0;
	}

	/** disconnect server **/
	private void EndConnection(){
		Network.Disconnect();
	}

	void OnDisconnectedFromServer (NetworkDisconnection info){
		if (info == NetworkDisconnection.LostConnection)
			EventManager.Instance.writeLog("Lost connection to the server",0);
		else{
			EventManager.Instance.writeLog("disconnectd from server",0);
			GameObject[] prefabs = GameObject.FindGameObjectsWithTag("Player");
			foreach(GameObject go in prefabs) {
				Destroy(go);
			}
		}
	}

	/** GUI to join / disconnect server **/
	void OnGUI (){   
		if (Network.peerType == NetworkPeerType.Disconnected) {
			//EventManager.Instance.writeLog("Not connected to server.");
			if (GUI.Button (new Rect (30, 55, 100, 50), "Find Host")) {
					EventManager.Instance.writeLog ("host refresh", 0);
					RefreshHostList ();
					ClientManager.Instance.guiClick = true;
					ClientManager.Instance.makeSound ("click");
			}
			if (GUI.Button (new Rect (30, 110, 100, 50), "Connect")) {
					EventManager.Instance.writeLog ("connecting", 0);
					FindHost ();
					ClientManager.Instance.guiClick = true;
					ClientManager.Instance.makeSound ("click");
			}
		} else {
			if (Network.peerType == NetworkPeerType.Connecting){
				EventManager.Instance.writeLog ("Connecting to server...", 0);
			}else {
				//EventManager.Instance.writeLog("Connected to server.",0);
				GUILayout.Label("IP/port: " + Network.player.ipAddress + "/" + Network.player.port);
			}

			if(GUI.Button(new Rect(30, 55, 100, 50), "Disconnect")){
				EndConnection();
				ClientManager.Instance.guiClick = true;
				ClientManager.Instance.makeSound("click");

				//to pass unique variables to this client
				ClientManager.Instance.myIp = "";
				ClientManager.Instance.status = 2;
			}
		}
	}
}