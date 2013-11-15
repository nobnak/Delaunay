using UnityEngine;
using System.Collections;
using nobnak.Geometry;

public class TestDelaunay : MonoBehaviour {
	private Mesh _mesh;

	// Use this for initialization
	void Start () {
		var v0 = new Vector2(0f, 0f);
		var v1 = new Vector2(0f, 1f);
		var v2 = new Vector2(1f, 0f);
		var delaunay = new Delaunay(v0, v1, v2);
		
		var p0 = new Vector2(0.1f, 0.1f);
		delaunay.Add(p0);
		
		Vector3[] outVertices;
		int[] outTriangles;
		delaunay.GenerateMesh(out outVertices, out outTriangles);
		
		_mesh = new Mesh();
		_mesh.vertices = outVertices;
		_mesh.triangles = outTriangles;
		_mesh.RecalculateBounds();
		_mesh.RecalculateNormals();
		
		GetComponent<MeshFilter>().mesh = _mesh;
	}
	
	void Update() {
		
	}
}
