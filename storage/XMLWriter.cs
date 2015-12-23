/* Mayra Donaji Barrera Machuca
*  SFU, 2015 
*  class to save log in an xml long logs */

using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Xml;

public class XMLWriter : MonoBehaviour {

	private XmlWriterSettings fragmentSetting;
	private string directory;
	private string logFilePath;
	private FileStream logFile;

	private LogEntry entry;

	void Awake(){

        //to receive custom events
		Manager.Instance.SendLog += new Manager.SendLogHandler (WriteData);
		Manager.Instance.SendEnd += new Manager.SendEndHandler (Close);

        //create folders to save logs
		directory = "Test" + "/log";
		if (!Directory.Exists (directory)) {
			Directory.CreateDirectory(directory);
		}
	}

	void Start(){

        //create xml file
		XmlWriterSettings wrapperSettings = new XmlWriterSettings ();
		wrapperSettings.Indent = true;

        //write head file
		string baseName = DateTime.Now.ToString ("h-mm");
		string wrapperName = directory + "/wrapper-" + baseName + ".xml";
		logFilePath = directory + "/log-" + baseName + ".xml";

		using (XmlWriter writer = XmlWriter.Create (wrapperName,wrapperSettings)) {

			writer.WriteStartDocument();
			writer.WriteStartElement("logData");

			//meta
			writer.WriteStartElement("meta");

			writer.WriteStartElement("startTime");
			writer.WriteValue(DateTime.Now);

            writer.WriteValue(Manager.Instance.round);

            writer.WriteEndElement();

			writer.Close();
		}

		logFile = new FileStream (logFilePath, FileMode.Append, FileAccess.Write, FileShare.Read);
		fragmentSetting = new XmlWriterSettings ();
		fragmentSetting.ConformanceLevel = ConformanceLevel.Fragment;
	}

    //write new record
	private void WriteData(LogEntry newEntry){

        using (XmlWriter writer1 = XmlWriter.Create(logFile, fragmentSetting)) {
            
            //write info in a single line
			string newLine = 
				newEntry.participant + 
				";" + newEntry.time +
				";" + newEntry.task +
				";" + newEntry.action +
				";" + newEntry.value;

			writer1.WriteStartElement("newEntry");
			writer1.WriteValue(newLine);
			writer1.WriteEndElement();


            //write info in correct xml form

            /*writer1.WriteStartElement("newEntry");

	            writer1.WriteStartAttribute("participant");
	            	writer1.WriteValue(newEntry.participant);
	            writer1.WriteEndAttribute();

	            writer1.WriteStartAttribute("time");
	            	writer1.WriteValue(newEntry.time);
	            writer1.WriteEndAttribute();

	            writer1.WriteStartElement("task");
	           		writer1.WriteValue(newEntry.task);
	            writer1.WriteEndElement();

	            writer1.WriteStartElement("action");
	            	writer1.WriteValue(newEntry.action);
	            writer1.WriteEndElement();

	            writer1.WriteStartElement("value");
	            	writer1.WriteValue(newEntry.value);
	            writer1.WriteEndElement();

            writer1.WriteEndElement();*/

            writer1.Flush();
    	}
    	logFile.Flush ();
    }

    //close file
    private void Close(){
		logFile.Close ();
	}
}