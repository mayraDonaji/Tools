/*==============================================================================
Made by Mayra Donaji Barrera Machuca
UTAS 2014
class to create new prefabs
==============================================================================*/
using UnityEngine;
using System.Collections;

public class PrefabCreation : MonoBehaviour {
	
	//prefab options
	public GameObject newCube;
	public GameObject newComment;
	public GameObject newWarning;
	public GameObject newHighpoint;

	private int siguiente; //unique name variable

	void Awake (){
		ClientManager.Instance.CreateSelectedPrefab += new ClientManager.CreateSelectedPrefabHandler (CreateSelecPrefab);		
	}

	void Start(){
		siguiente = 0;
	}

	//create gameobject with parend and unique properties
	private void CreateSelecPrefab(Color myColor, Vector3 myPos, string myNameObject, int prefabType, string text){

		GameObject parent;
		GameObject child;

		switch(prefabType){
			case 1:
				parent = (GameObject)Instantiate (newWarning, new Vector3 (0, 0, 0), newWarning.transform.rotation);
				child = GameObject.Find ("warning");
				break;
			case 2:
				parent = (GameObject)Instantiate (newHighpoint, new Vector3 (0, 0, 0), newHighpoint.transform.rotation);
				child = GameObject.Find ("highPoint");
				break;
			case 3:
				parent = (GameObject)Instantiate (newComment, new Vector3 (0, 0, 0), newComment.transform.rotation);
				child = GameObject.Find ("comment");
				break;
			default:
				parent = (GameObject)Instantiate (newCube, new Vector3 (0, 0, 0), newCube.transform.rotation);
				child = GameObject.Find ("CubePrefab");
				break;
		}

		//parent properites
		parent.name = myNameObject + "parent"; //gameobject name
		SignsHandle thisSignInfo = (SignsHandle)parent.GetComponent ("SignsHandle"); //get script
		thisSignInfo.myText = text; //change script variable

		//child properties
		PrefabState thisChildState = (PrefabState)child.GetComponent ("PrefabState"); //get child script

		child.renderer.material.color = myColor; //gameobject variable
		child.name = myNameObject; //gameobject name

		thisChildState.isSelect = true; //script variables
		thisChildState.selectedByName = owner;
		thisChildState.display = display;
		thisChildState.ownerColor = myColor;

		//put parend / child gameobject inside other
		parent.transform.parent = GameObject.Find ("ImageTarget").transform;
		parent.transform.position = new Vector3(0, 0, 0);
		parent.transform.rotation = new Quaternion (0, 0, 0, 0);
	}
}