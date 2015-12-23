/*==============================================================================
Made by Mayra Donaji Barrera Machuca
UTAS 2014
==============================================================================*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerClientManager : MonoBehaviour {
	/** class to control clients **/
	
	private Dictionary<NetworkPlayer, Client> clients = new Dictionary<NetworkPlayer, Client>();
	private int clientCount;

	void Start(){
		clientCount = 0;
	}

	/** new client **/
	public void spawnPlayer(NetworkPlayer player) {
		//unique client identifier
		clientCount++;
		//new client settings
		string clientName = "client"+clientCount;
		Vector3 clientColor = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
		string clientIP = player.ipAddress;
		//initialice new client object
		GameObject newClientGameObject = (GameObject) Network.Instantiate(EventManager.Instance.client, new Vector3(0f, 5f, 0f), Quaternion.identity,0);
		newClientGameObject.name = clientName;
		ClientInfo serverClient = newClientGameObject.GetComponent<ClientInfo> ();
		serverClient.MyInfo(clientName, clientColor);
		//add new client to clients list
		Client newClient = new Client(clientName,clientColor,newClientGameObject);
		clients[player] = newClient;
		//send info to all clients
		networkView.RPC("SendUniqueClientID",RPCMode.Others, clientIP, clientName, clientColor);
	}

	//** delete client **/
	public void deletePlayer(NetworkPlayer player){
		//get client
		Client deadClient = clients[player];
		GameObject client = (GameObject) deadClient.gameObject;
		//remove all clients objects
		Network.RemoveRPCs(client.networkView.viewID); 
		Network.Destroy(client); 
		clients.Remove(player); 
	}
}