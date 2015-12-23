/* Mayra Barrera (based on 
** September, 2015 
** Class to create custom projection view
*/

using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class OffAxisProjection : MonoBehaviour {

    // game object that represents the screen inside unity
    public GameObject virtualWindow;

    // game object that represents the head
	public GameObject headPos;

    //one camera for each eye (this script should be in each camera
	public GameObject leftCamera;
	public GameObject rightCamera;

    //to move the center of the tracking to each eye position
    public float eyeShift;

    /// real world screen width (in units - meters)
    public float width;
	/// real world screen height (in units - meters)
	public float height;
	/// The maximum height the camera can have (up axis in local coordinates from  the virtualWindow)
	float windowWidth;
	float windowHeight;

    //to log calculated values
    public bool verbose = false; 
	
	void LateUpdate()
	{
        //due of unity (0,0,0) is in the middle of the screen
		windowWidth = width/2;
		windowHeight = height/2;

		setAsymmetricFrustum(GetComponent<Camera>(), headPos.transform.position);
		
	}

	public void setAsymmetricFrustum(Camera cam,Vector3 pos)
	{

		// Focal length = orthogonal distance to image plane
		Vector3 newpos = pos;

        // shift each camera to correct eye position
		if (this.gameObject.name == "RightCamera") {
			newpos = new Vector3 (newpos.x - eyesShift, newpos.y, newpos.z); 
            rightCamera.transform.position = new Vector3(newpos.x, newpos.y, newpos.z);

		} else if (this.gameObject.name == "LeftCamera") {
			newpos = new Vector3 (newpos.x + eyesShift, newpos.y, newpos.z);
			leftCamera.transform.position = new Vector3(newpos.x, newpos.y, newpos.z);
		}

        //calculate new frustum

        // lower left corner
        Vector3 pa = new Vector3(-windowWidth, -windowHeight,0.0f);
        // lower right corner
        Vector3 pb  = new Vector3(windowWidth, -windowHeight,0.0f);
        // upper left corner
        Vector3 pc  = new Vector3(-windowWidth, windowHeight, 0.0f);

        Vector3 vr = (pb - pa).normalized; // right axis of screen
        Vector3 vu = (pc - pa).normalized; // up axis of screen
        Vector3 vn = -Vector3.Cross(vr, vu).normalized; // normal vector of screen

        Vector3 va = pa - newpos; // from eye position to lower left corner
        Vector3 vb = pb - newpos; // from eye position to lower right corner
        Vector3 vc = pc - newpos; // from eye position to upper left corner

        float n = cam.nearClipPlane;   // distance to near clipping plane (screen)
        float f = newpos.z * (-1.0f);     // distance to far clipping plane

        float  d = -Vector3.Dot(va, vn); // distance from a to plane

        float newLeft = Vector3.Dot(vr, va) * n / d; ;  // distance to left screen edge from center
        float newRight = Vector3.Dot(vr, vb) * n / d;   //distance to right screen edge from center
        float newBottom = Vector3.Dot(vu, va) * n / d;  // distance to bottom screen edge from center
        float newTop = Vector3.Dot(vu, vc) * n / d;     //distance to top screen edge from center

        Matrix4x4 m = PerspectiveOffCenter(newLeft, newRight, newBottom, newTop, n, f);
		cam.projectionMatrix = m;

        if (verbose)
        {
            Debug.Log(newpos.x + ";" + newpos.y + ";" + newpos.z);
            Debug.Log(cam.cameraToWorldMatrix);
        }
    }

	Matrix4x4 PerspectiveOffCenter(float left, float right, float bottom, float top, float near, float far)
	{

		float x = (2.0f * near) / (right - left);
		float y = (2.0f * near) / (top - bottom);
		float a = (right + left) / (right - left);
		float b = (top + bottom) / (top - bottom);
		float c = (far + near) / ( near - far); //change far - near and -
		float d = (2.0f * far * near) / (near - far); //change far - near and -
        float e = -1.0f;
		
        //create asynchronous projection  matrix
		Matrix4x4 m = new Matrix4x4();
		m[0, 0] = x; m[0, 1] = 0; m[0, 2] = a; m[0, 3] = 0;
		m[1, 0] = 0; m[1, 1] = y; m[1, 2] = b; m[1, 3] = 0;
		m[2, 0] = 0; m[2, 1] = 0; m[2, 2] = c; m[2, 3] = d;
		m[3, 0] = 0; m[3, 1] = 0; m[3, 2] = e; m[3, 3] = 0;

        return m;
	}

	/// <summary>
	/// Draws lines in the Edit window for debugging.
	/// </summary>
	public virtual void OnDrawGizmos()
	{
        //y axis
		Gizmos.DrawLine (GetComponent<Camera>().transform.position, GetComponent<Camera>().transform.position+GetComponent<Camera>().transform.up * 5);		
		Gizmos.color = Color.green;
		Gizmos.DrawLine(virtualWindow.transform.position, virtualWindow.transform.position + virtualWindow.transform.up);
		
        //z axis
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(virtualWindow.transform.position - virtualWindow.transform.forward * windowHeight, virtualWindow.transform.position + virtualWindow.transform.forward * windowHeight);
		
        //x axis
		Gizmos.color = Color.red;
		Gizmos.DrawLine(virtualWindow.transform.position - virtualWindow.transform.right * windowWidth, virtualWindow.transform.position + virtualWindow.transform.right * windowWidth);

        //screen position
		Gizmos.color = Color.white;
		Vector3 leftBottom = virtualWindow.transform.position - virtualWindow.transform.right * windowWidth - virtualWindow.transform.up * windowHeight;
		Vector3 leftTop = virtualWindow.transform.position - virtualWindow.transform.right * windowWidth + virtualWindow.transform.up * windowHeight;
		Vector3 rightBottom = virtualWindow.transform.position + virtualWindow.transform.right * windowWidth - virtualWindow.transform.up * windowHeight;
		Vector3 rightTop = virtualWindow.transform.position + virtualWindow.transform.right * windowWidth + virtualWindow.transform.up * windowHeight;
		
		Gizmos.DrawLine(leftBottom,leftTop);
		Gizmos.DrawLine(leftTop,rightTop);
		Gizmos.DrawLine(rightTop,rightBottom);
		Gizmos.DrawLine(rightBottom,leftBottom);		
		Gizmos.color = Color.grey;
		Gizmos.DrawLine(transform.position,leftTop);
		Gizmos.DrawLine(transform.position,rightTop);
		Gizmos.DrawLine(transform.position,rightBottom);
		Gizmos.DrawLine(transform.position,leftBottom);	
	}
}