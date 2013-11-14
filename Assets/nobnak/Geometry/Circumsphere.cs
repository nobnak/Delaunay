using UnityEngine;
namespace nobnak.Geometry {
	
	public static class Circumsphere {
		public static void Calculate(Vector2 v0, Vector2 v1, Vector2 v2, out Vector2 center, out float sqrRadius) {
			var e1 = v1 - v0;
			var e2 = v2 - v0;
			var sqrV1 = e1.x * e1.x + e1.y * e1.y;
			var sqrV2 = e2.x * e2.x + e2.y * e2.y;
			var b = new Vector2(sqrV1, sqrV2);
			
			var det = 2f * (e1.x * e2.y - e1.y * e2.x);
			var rDet = 1f / det;
			var x = rDet * (b.x * e2.y - b.y * e1.y);
			var y = rDet * (b.y * e1.x - b.x * e2.x);
			
			center = new Vector2(x + v0.x, y + v0.y);
			sqrRadius = x * x + y * y;
		}
	}
}
