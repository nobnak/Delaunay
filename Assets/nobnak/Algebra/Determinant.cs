
namespace nobnak.Algebra {

	public static class Determinant {
		public static float Det(float[] a, int n) {
			var res = 1f;
			for (var i = 0; i < n; i++) {
				var max = a[I (i, i, n)];
				var maxRow = i;
				for (var j = i + 1; j < n; j++) {
					var cell = a[I (j, i, n)];
					if (cell > max) {
						max = cell;
						maxRow = j;
					}
				}
				res *= max;
				if (maxRow != i) {
					res *= -1;
					SwapRow(a, i, maxRow, n);
				}
				
				if (max == 0f)				
					return 0f;
				for (var j = i + 1; j < n; j++) {
					var mul = a[I (j, i, n)] / max;
					for (var k = i; k < n; k++) {
						a[I (j, k, n)] -= mul * a[I (i, k, n)];
					}
				}
			}
			return res;
		}
		
		public static void SwapRow(float[] a, int row0, int row1, int n) {
			for (var i = 0; i < n; i++) {
				var tmp = a[I (row0, i, n)]; a[I (row0, i, n)] = a[I (row1, i, n)]; a[I (row1, i, n)] = tmp;
			}
		}
		
		public static int I(int row, int col, int n) {
			return row * n + col;
		}			
	}
}