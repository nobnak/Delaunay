using UnityEngine;
using System.Collections;
using nobnak.Geometry;

public class TestDelaunay : MonoBehaviour {
	public Vector2[] largeTriangle;
	
	private Mesh _mesh;
	private Delaunay _delaunay;

	// Use this for initialization
	void Start () {
		GetComponent<MeshFilter>().mesh = _mesh = new Mesh();
		_delaunay = new Delaunay(largeTriangle[0], largeTriangle[1], largeTriangle[2]);
		UpdateMesh (_delaunay);
	}
	
	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			try {
				var p = GeneratePoint();
				_delaunay.Add(p);
				UpdateMesh(_delaunay);
			} catch (System.Exception e) {
				Debug.Log(e);
			}
		}
	}

	void UpdateMesh (Delaunay delaunay) {
		Vector3[] outVertices;
		int[] outTriangles;
		delaunay.GenerateMesh(out outVertices, out outTriangles);
		
		_mesh.vertices = outVertices;
		_mesh.triangles = outTriangles;
		_mesh.RecalculateBounds();
		_mesh.RecalculateNormals();
	}
	
	Vector2 GeneratePoint() {
		var u = Random.value;
		var v = Random.value;
		var uv = u + v;
		if (uv > 1) {
			u = 1f - u;
			v = 1f - v;
		}
		var w = 1f - (u + v);
		return largeTriangle[0] * w + largeTriangle[1] * u + largeTriangle[2] * v;
	}
	
	bool InFace(Vector2 p) {
		for (var i = 0; i < 3; i++) {
			var v0 = largeTriangle[i % 3];
			var v1 = largeTriangle[(i + 1) % 3];
			var v01 = v1 - v0;
			var vp = p - v0;
			
			var cross = vp.x * v01.y - v01.x * vp.y;
			if (cross <= 0)
				return false;
		}
		return true;
	}
}
