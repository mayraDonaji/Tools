/* Mayra Donaji Barrera Machuca
 * based on http://forum.unity3d.com/threads/wireframe-3d.8814/
 * SFU, 2015
 * class to select objects with mouse click */
 
private var lines : Vector3[];
private var linesArray : Array;

//lines
private var lineMaterial : Material;
public var shader: Shader;
public var filter: MeshFilter;
 
function Start (){   

	lineMaterial = new Material(shader); 
   	linesArray = new Array();

	var mesh = filter.mesh;
	var vertices = mesh.vertices;

	for (i = 0; i < vertices.length / 3; i++)
	{
		linesArray.Add(vertices[i]);
	}

	var triangles = mesh.triangles;

	for (i = 0; i < triangles.length / 3; i++)
	{
	    linesArray.Add(vertices[triangles[i * 3]]);
	    linesArray.Add(vertices[triangles[i * 3 + 1]]);
	    linesArray.Add(vertices[triangles[i * 3 + 2]]);
	}

	lines = linesArray.ToBuiltin(Vector3);
}
 
 
function OnRenderObject()
{  
    lineMaterial.SetPass(0);
   
    GL.PushMatrix();
    GL.MultMatrix(transform.localToWorldMatrix);
 
    GL.Begin(GL.LINES);
    
    for (i = 0; i < lines.length / 3; i++)
    {
    	GL.Color(new Color(Random.Range(0.0F, 1.0F),Random.Range(0.0F, 1.0F),Random.Range(0.0F, 1.0F),1));
        GL.Vertex(lines[i * 3]);
        GL.Vertex(lines[i * 3 + 1]);
       	
        GL.Vertex(lines[i * 3 + 1]);
        GL.Vertex(lines[i * 3 + 2]);
       
        GL.Vertex(lines[i * 3 + 2]);
        GL.Vertex(lines[i * 3]);
    }
         
    GL.End();
    GL.PopMatrix();
}