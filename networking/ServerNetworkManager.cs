/*==============================================================================
Made by Mayra Donaji Barrera Machuca
UTAS 2014
==============================================================================*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerNetworkManager : MonoBehaviour {
	/** class to create a server **/

	//read settings of xml variables
	public TextAsset xmlDefinition;
	private Dictionary<string, string> settings = new Dictionary<string, string>();
	public LoadXML loadXML;
	//general server variables
	private int maxClients = 5;
	private int listenPort = 25000;
	//variables for local server
	private string ipAdress= "192.168.1.100";
	private int masterServerPort = 23466;
	private int facilitatorPort = 50005;

	//client manager
	private ServerClientManager scm;

	//declare events
	void Awake(){
		scm = (ServerClientManager) gameObject.GetComponent(typeof(ServerClientManager));
	}

	// Use this for initialization
	void Start () {
		settings = loadXML.ReadXML(xmlDefinition);
	}

	/** start server **/
	private void StartServer(){
		//start server
		Network.InitializeServer(maxClients, listenPort, !Network.HavePublicAddress());

		//MasterServer.ipAddress = ipAdress;
		//MasterServer.port = masterServerPort;
		//Network.natFacilitatorIP = ipAdress;
		//Network.natFacilitatorPort = facilitatorPort;

		MasterServer.RegisterHost(settings["typeName"],settings["gameName"]);

	}

	void OnServerInitialized(){
		EventManager.Instance.writeLog("Server Initializied",0);
	}

	/** end server**/
	private void EndServer()
	{
		Network.Disconnect();
	}

	void OnPlayerDisconnected(NetworkPlayer player){
		EventManager.Instance.writeLog("Clean up after player" + player,0);
		scm.deletePlayer(player);
	}

	void OnDisconnectedFromServer(NetworkDisconnection info) {
		EventManager.Instance.writeLog("Local server disconnected",0);
	}

	/** spawn new client (call serverclientmanager method) **/
	void OnPlayerConnected(NetworkPlayer player) {
		scm.spawnPlayer(player);
		showClientInformation ();
	}

	/** GUI to control start/end server **/
	void OnGUI ()
	{
		if (Network.peerType == NetworkPeerType.Disconnected) {
			EventManager.Instance.writeLog("Network server is not running.",0);
			if (GUI.Button(new Rect(30, 55, 100, 50),"Start Server")){
				StartServer();  
			}
		}
		else {
			if (Network.peerType == NetworkPeerType.Connecting)
				EventManager.Instance.writeLog("Network server is starting up...",0);
			else {         
				//showServerInformation();    
				//showClientInformation();
			}
			if (GUI.Button(new Rect(30, 55, 100, 50),"Stop Server")){
				EndServer();   
			}
		}
	}
	
	private void showClientInformation() {
		//EventManager.Instance.writeLog ("Clients: " + Network.connections.Length + "/" + maxClients, 0);
		Debug.Log("Clients: " + Network.connections.Length + "/" + maxClients);
		foreach(NetworkPlayer p in Network.connections) {
			//EventManager.Instance.writeLog (" Player from ip/port: " + p.ipAddress + "/" + p.port, 1);
			Debug.Log(" Player from ip/port: " + p.ipAddress + "/" + p.port);
		}
	}
	
	private void showServerInformation() {
		GUILayout.Label("IP: " + Network.player.ipAddress + " Port: " + Network.player.port);  
	}
}