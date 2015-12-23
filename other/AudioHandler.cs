/*==============================================================================
Made by Mayra Donaji Barrera Machuca
UTAS 2014
class to handle sounds
==============================================================================*/
using UnityEngine;
using System.Collections;

public class AudioHandler : MonoBehaviour {

	public AudioClip click;
	public AudioClip select;

	/** class received events **/
	void Awake () {
		ClientManager.Instance.MakeSound += new ClientManager.MakeSoundHandler(Sounds);
	}

	/** play **/
	private void Sounds (string type){
		//play sound depending the type
		switch(type){
		case "click":
			audio.PlayOneShot(click);
			break;
		case "select":
			audio.PlayOneShot(select);
			break;
		default:
			EventManager.Instance.writeLog("that sound dont exist",0);
			break;
		}

	}
}