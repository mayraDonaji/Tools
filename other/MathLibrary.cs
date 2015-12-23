/*==============================================================================
Made by Mayra Donaji Barrera Machuca
UTAS 2014
==============================================================================*/
using UnityEngine;
using System.Collections;

public static class MathLibrary {
	/** class that extends the unity math library **/

	//method to get angle between two vectors
	public static float GetAngle (Vector2 pos1, Vector2 pos2) 
	{
		Vector2 from = pos2 - pos1;
		Vector2 to = new Vector2(1, 0);
		
		float result = Vector2.Angle(from, to);
		Vector3 cross = Vector3.Cross(from, to);
		
		if(cross.z > 0) 
		{
			result = 360f - result;
		}		
		return result;
	}

	public static float DistanceBetweenTwoVectors(Vector3 pos1, Vector3 pos2){

		float x = pos1.x - pos2.x;
		float y = pos1.y - pos2.y;
		float z = pos1.z - pos2.z;
	
		return  Mathf.Sqrt(Mathf.Pow(x,2) + Mathf.Pow(y,2) + Mathf.Pow(z,2));
	}
}