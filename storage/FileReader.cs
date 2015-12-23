/* Mayra Donaji Barrera Machuca
 * SFU, 2015
 * class to read a xml and store data */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class FileReader : MonoBehaviour {

	/** class instance to create singleton **/
	private static FileReader instance;
	private FileReader(){}
	public static FileReader Instance{
		get     
		{       
			if (instance ==  null)
				instance = GameObject.FindObjectOfType(typeof(FileReader)) as  FileReader;      
			return instance;    
		}
	}

	//file reader variables
	protected FileInfo theSourceFile = null;
	protected StreamReader reader = null;
	
	/*invidiual files variables - examples:
	public int selectedCase;
	public Vector3 newVertex;
	public float acceleration; */

	void Start () {

		theSourceFile = new FileInfo ("name.txt");
		reader = theSourceFile.OpenText();

		while(!reader.EndOfStream){

			text = reader.ReadLine();
			string[] values = text.Split(',');

			/*section for individual files, example:
			selectedCase = int.Parse(values[1].Substring(0,2));
			newVertex = new Vector3(float.Parse(values[4])*10,float.Parse(values[5])*10,float.Parse(values[6])*10);
			acceleration = float.Parse(values[8])*10000;*/
		}

		reader.Close();
		reader.Dispose();
	}
}
