/* Mayra Donaji Barrera Machuca
 * SFU, 2015
 * generic class for logs */

using System.Collections;
using System;

public class LogEntry {

	public int participant;
	public string time;
	public int task;
	public string action;
	public string value;

	public LogEntry(int participant, string time, int task, string action, string value){
		this.participant = participant;
		this.time = time;
		this.action = action;
		this.type = type;
		this.value = value;
	}
}
