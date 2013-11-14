using UnityEngine;
using System.Collections;
using nobnak.Geometry;

public class TestCircumsphere : MonoBehaviour {
	public const float EPSILON = 1e-6f;
	public const string FORMAT_ASSERT_OBJECT = "Objects {0}={1}";
	public const string FORMAT_ASSERT_VALUETYPE = "Value {0}={1}";

	// Use this for initialization
	void Start () {
		TestTriangle (new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 0));
		TestTriangle (new Vector2(-1, 0), new Vector2(0, 1), new Vector2(1, 0));
		
		var triangles = new int[]{ 0, 1, 2, 1, 3, 2 };
		var halfedges = HalfEdge.FromTriangles(triangles);
		Assert(halfedges[0].opposite, null);
		Assert(halfedges[0].vertex.index, 0);
		Assert(halfedges[0].next, halfedges[1]);
		Assert(halfedges[1].opposite, halfedges[5]);
		Assert(halfedges[1].vertex.index, 1);
		Assert(halfedges[1].next, halfedges[2]);
		Assert(halfedges[2].opposite, null);
		Assert(halfedges[2].vertex.index, 2);
		Assert(halfedges[2].next, halfedges[0]);
		Assert(halfedges[3].opposite, null);
		Assert(halfedges[3].vertex.index, 1);
		Assert(halfedges[3].next, halfedges[4]);
		Assert(halfedges[4].opposite, null);
		Assert(halfedges[4].vertex.index, 3);
		Assert(halfedges[4].next, halfedges[5]);
		Assert(halfedges[5].opposite, halfedges[1]);
		Assert(halfedges[5].vertex.index, 2);
		Assert(halfedges[5].next, halfedges[3]);
	}

	void Update() {
		Application.Quit();
	}

	void TestTriangle (Vector2 v0, Vector2 v1, Vector2 v2) {
		Vector2 center;
		float sqrRadius;
		Circumsphere.Calculate(v0, v1, v2, out center, out sqrRadius);
		Assert((v0 - center).sqrMagnitude, sqrRadius, "v0 radius {0} = {1}");
		Assert((v1 - center).sqrMagnitude, sqrRadius, "v1 radius {0} = {1}");
		Assert((v2 - center).sqrMagnitude, sqrRadius, "v2 radius {0} = {1}");
	}
	
	void Assert(float a, float b, string format) {
		var diff = a - b;
		if (diff < -EPSILON || EPSILON < diff)
			throw new System.Exception(string.Format(format, a, b));
	}
	void Assert(System.ValueType a, System.ValueType b) {
		if (!a.Equals(b))
			throw new System.Exception(string.Format(FORMAT_ASSERT_VALUETYPE, a, b));
	}
	void Assert(object a, object b) {
		if (a != b)
			throw new System.Exception(string.Format(FORMAT_ASSERT_OBJECT, a, b));
	}
}
