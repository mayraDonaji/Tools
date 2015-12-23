using UnityEngine;
using System.Collections;

public class StateManager : MonoBehaviour {

	public GameObject startButton;
	public GameObject helpButton;
	public GameObject backButton;

	public GameObject instrucciones;
	public GameObject logo;

	// Use this for initialization
	void Start () {

		instrucciones.SetActive(false);
		backButton.SetActive (false);

	}

	public void onClick(string name){

		if(name.Equals("start")){
			Application.LoadLevel("mainScene");
		}

		if (name.Equals ("help")) {

			instrucciones.SetActive(true);
			backButton.SetActive (true);

			logo.SetActive(false);
			helpButton.SetActive(false);
			startButton.SetActive(false);

		}

		if (name.Equals ("back")) {

			instrucciones.SetActive(false);
			backButton.SetActive (false);

			logo.SetActive(true);
			helpButton.SetActive(true);
			startButton.SetActive(true);

		}

	}
}
