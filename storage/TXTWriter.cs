/* Mayra Donaji Barrera Machuca
 * SFU, 2015
 * class to store data in a TXT for short logs */

using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class TXTWriter : MonoBehaviour {

	private string directory;
	private string logFilePath;

	private LogEntry entry;
	
	void Awake(){
		
		Manager.Instance.SendLog += new Manager.SendLogHandler (GetData);
		
		directory = "Test" + "/log";
		if (!Directory.Exists (directory)) {
			Directory.CreateDirectory(directory);
		}
	}

	void Start () {

		string baseName = DateTime.Now.ToString ("h-mm-ss");
		logFilePath = directory + "/experiment - " + Manager.Instance.participant + " - " + baseName + ".txt";

		// start line date
		string lines = "start program: "+DateTime.Now;
		
		// Write the string to a file.
		StreamWriter file = new StreamWriter(logFilePath,true);
		file.WriteLine(lines);
		file.Close();
	}

	private void GetData(LogEntry newEntry){

		string newLine = 
			"[" + newEntry.participant + 
			";" + newEntry.time +
			";" + newEntry.task +
			";" + newEntry.action +
			";" + newEntry.value + "]";

		StreamWriter sw = File.AppendText(logFilePath);
		sw.WriteLine(newLine);
		sw.Close();

	}
}