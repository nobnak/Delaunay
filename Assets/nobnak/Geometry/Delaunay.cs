using System.Collections.Generic;
using UnityEngine;

namespace nobnak.Geometry  {
	
	public class Delaunay {
		public List<Vector2> vertices;
		public List<HalfEdge> halfEdges;
		public HashSet<HalfEdge.Face> faces;
		
		private Stack<HalfEdge> _flipCheck;
		
		public Delaunay(Vector2 v0, Vector2 v1, Vector2 v2) {
			vertices = new List<Vector2>(new Vector2[]{ v0, v1, v2 });
			halfEdges = new List<HalfEdge>(HalfEdge.FromTriangles(new int[]{ 0, 1, 2 }));
			faces = new HashSet<HalfEdge.Face>();
			foreach (var he in halfEdges)
				faces.Add(he.face);
			
			_flipCheck = new Stack<HalfEdge>();
		}
		
		public void Add(Vector2 p) {
			 var face = FindFaceContains(p);
			if (face == null)
				throw new System.ArgumentException("Face not found");
			
			SplitTriangle(face, p);
		}
		
		public HalfEdge[] SplitTriangle(HalfEdge.Face f, Vector2 p) {
			faces.Remove(f);
			var he = f.halfedge;
			var f0 = new HalfEdge.Face(){ halfedge = he };
			var f1 = new HalfEdge.Face(){ halfedge = he.next };
			var f2 = new HalfEdge.Face(){ halfedge = he.next.next };
			he.face = f0;
			he.next.face = f1;
			he.next.next.face = f2;
			faces.Add(f0); faces.Add(f1); faces.Add(f2);
			
			var v = new HalfEdge.Vertex(){ index = vertices.Count };
			vertices.Add(p);
			
			var he0a = new HalfEdge(){ vertex = v, next = he, face = f0 };
			var he0b = new HalfEdge(){ vertex = he.next.vertex, next = he0a, face = f0 };
			he = he.next;
			he0a.next.next = he0b;
			var he1a = new HalfEdge(){ vertex = v, next = he, face = f1 };
			var he1b = new HalfEdge(){ vertex = he.next.vertex, next = he1a, face = f1 };
			he = he.next;
			he1a.next.next = he1b;
			var he2a = new HalfEdge(){ vertex = v, next = he, face = f2 };
			var he2b = new HalfEdge(){ vertex = he.next.vertex, next = he2a, face = f2 };
			he = he.next;
			he2a.next.next = he2b;
			he0a.opposite = he2b; he2b.opposite = he0a;
			he1a.opposite = he0b; he0b.opposite = he1a;
			he2a.opposite = he1b; he1b.opposite = he2a;
			halfEdges.AddRange(new HalfEdge[]{ he0a, he0b, he1a, he1b, he2a, he2b });
			return new HalfEdge[]{ f0.halfedge, f1.halfedge, f2.halfedge };
		}
		
		public HalfEdge.Face FindFaceContains(Vector2 p) {
			foreach (var f in faces) {
				var found = true;
				var he = f.halfedge;
				do {
					var v0 = vertices[he.vertex.index];
					var v1 = vertices[he.next.vertex.index];
					var e1 = v1 - v0;
					var ep = p - v0;
					var area = ep.x * e1.y - e1.x * ep.y;
					if (area <= 0) {
						found = false;
						break;
					}
				} while ((he = he.next) != f.halfedge);
				if (found)
					return f;
			}
			return null;
		}
		
		public void GenerateMesh(out Vector3[] outVertices, out int[] outTriangles) {
			outVertices = new Vector3[this.vertices.Count];
			for (var i = 0; i < outVertices.Length; i++)
				outVertices[i] = vertices[i];
			
			outTriangles = new int[3 * faces.Count];
			var triIndex = 0;
			foreach (var f in faces) {
				var he = f.halfedge;
				outTriangles[triIndex++] = he.vertex.index;
				outTriangles[triIndex++] = (he = he.next).vertex.index;
				outTriangles[triIndex++] = (he = he.next).vertex.index;
			}
		}
	}
}