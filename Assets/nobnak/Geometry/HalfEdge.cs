using System.Collections.Generic;

namespace nobnak.Geometry {
	
	public class HalfEdge {
		public HalfEdge next;
		public HalfEdge opposite;
		public Vertex vertex;
		public Face face;
		
		public override string ToString () { return string.Format ("[HalfEdge] ({0},{1})", vertex.index, next.vertex.index); }
		
		public class Vertex {
			public int index;
			public HalfEdge halfedge;
			
			public override string ToString () { return string.Format("[Vertex] ({0})", index); }
		}
		
		public class Face {
			public HalfEdge halfedge;
			
			public override string ToString () { return string.Format("[Face] ({0},{1},{2})", 
					halfedge.vertex.index, halfedge.next.vertex.index, halfedge.next.next.vertex.index); }
		}
		
		public static HalfEdge[] FromTriangles(int[] triangles) {
			var index2vertex = new Dictionary<int, Vertex>();
			var halfEdges = new List<HalfEdge>();
			var vertex2halfEdges = new Dictionary<Vertex, LinkedList<HalfEdge>>();
			for (var i = 0; i < triangles.Length; i+=3) {
				var face = new Face();
				var i0 = triangles[i];
				var v0 = index2vertex.ContainsKey(i0) ? index2vertex[i0] : (index2vertex[i0] = new Vertex(){ index = i0 });
				var he0 = new HalfEdge() { vertex = v0, face = face };
				halfEdges.Add(he0);
				v0.halfedge = he0;
				var hedges0 = vertex2halfEdges.ContainsKey(v0) ? vertex2halfEdges[v0] : (vertex2halfEdges[v0] = new LinkedList<HalfEdge>());
				hedges0.AddLast(he0);
				
				var i1 = triangles[i+1];
				var v1 = index2vertex.ContainsKey(i1) ? index2vertex[i1] : (index2vertex[i1] = new Vertex(){ index = i1 });
				var he1 = new HalfEdge() { vertex = v1, face = face };
				halfEdges.Add(he1);
				v1.halfedge = he1;
				var hedges1 = vertex2halfEdges.ContainsKey(v1) ? vertex2halfEdges[v1] : (vertex2halfEdges[v1] = new LinkedList<HalfEdge>());
				hedges1.AddLast(he1);
				
				var i2 = triangles[i+2];
				var v2 = index2vertex.ContainsKey(i2) ? index2vertex[i2] : (index2vertex[i2] = new Vertex(){ index = i2 });
				var he2 = new HalfEdge() { vertex = v2, face = face };
				halfEdges.Add(he2);
				v2.halfedge = he2;
				var hedges2 = vertex2halfEdges.ContainsKey(v2) ? vertex2halfEdges[v2] : (vertex2halfEdges[v2] = new LinkedList<HalfEdge>());
				hedges2.AddLast(he2);
				
				face.halfedge = he0;
				he0.next = he1;
				he1.next = he2;
				he2.next = he0;
			}
			
			foreach (var he in halfEdges) {
				if (he.opposite != null)
					continue;
				
				var opposites = vertex2halfEdges[he.next.vertex];
				foreach (var opposite in opposites) {
					if (opposite.next.vertex == he.vertex) {
						he.opposite = opposite;
						opposite.opposite = he;
						break;
					}
				}
			}
			
			return halfEdges.ToArray();
		}
	}
}

