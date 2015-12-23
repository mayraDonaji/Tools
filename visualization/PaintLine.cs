/* Mayra Donaji Barrera Machuca, 
 * based on http://www.everyday3d.com/blog/index.php/2010/03/15/3-ways-to-draw-3d-lines-in-unity3d/
 * SFU, 2015
 * class to draw colorful lines */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class PaintLine : MonoBehaviour {

	//custom shader with color property on
	public Shader shader;
	
	//new material that used custom shader
	private static Material material;

	//gameobject used to paint line
	private GameObject container;

	//line vertex array
	private Vector3[] lp;

	//previous vertex
	private Vector3 s;

	//color variables
	private List<Color> color;
	private Color newColor;
	private Color lastColor;

	private int cont;

	void Start () {

		material = new Material(shader);

		container = new GameObject("line");
		//if container needs special interaction add script here
		//container.AddComponent<Script>();

		lp = new Vector3[0];
		color = new List<float>();

	}

	void Update () {

		if(s != Vector3.zero) {

				lp = AddLine(lp, s, Input.mousePosition,false);

				//important if not color list will be missing half of points
				newColor = nw Color(Random.Range(0.0F, 1.0F),Random.Range(0.0F, 1.0F),Random.Range(0.0F, 1.0F),1);
				color.Add(lastColor);
				color.Add(newColor);
			}
			
			s = Input.mousePosition;
			lastColor = newColor

		} else {
			s = Vector3.zero;
		}
	}

	Vector3[] AddLine(Vector3[] l, Vector3 s, Vector3 e, bool tmp) {
		int vl = l.Length;
		if(!tmp || vl == 0) l = resizeVertices(l, 2);
		else vl -= 2;
		
		l[vl] = s;
		l[vl+1] = e;
		return l;
	}
	
	Vector3[] resizeVertices(Vector3[] ovs, int ns) {
		Vector3[] nvs = new Vector3[ovs.Length + ns];
		for(int i = 0; i < ovs.Length; i++) nvs[i] = ovs[i];
		return nvs;
	}

	void OnPostRender() {

		m.SetPass (0);
		GL.PushMatrix ();
		GL.MultMatrix (g.transform.transform.localToWorldMatrix);
		GL.Begin (GL.LINES);

		for (int i = 0; i < lp.Length; i++) {
			GL.Color( color[i] );
			GL.Vertex3 (lp [i].x, lp [i].y, lp [i].z);
		}
	
		GL.End ();
		GL.PopMatrix ();
	}
}