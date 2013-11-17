using UnityEngine;
using System.Collections;
using nobnak.Algebra;

public class TestDeterminant : MonoBehaviour {
	public const float EPSILON = 1e-6f;

	// Use this for initialization
	void Start () {
		Test01 ();
		Test02 ();
		Test03();
	}

	void Test01 () {
		var det = Determinant.Det(new float[]{ 1f, 0f, 0f, 1f }, 2);
		Assert(det, 1f);
	}

	void Test02 () {
		var det = Determinant.Det(new float[]{ 
			1f, 0f, 1f, 1f,
			1f, 0f, -1f, 1f,
			1f, -1f, 0f, 1f,
			1f, 2f, 0f, 4f }, 4);
		Assert(det, -6f);
	}
	
	void Test03 () {
		var det = Determinant.Det(new float[]{ 
			1f, 0f, 1f, 1f,
			1f, 0f, -1f, 1f,
			1f, -1f, 0f, 1f,
			1f, 0.5f, 0f, 0.25f }, 4);
		Assert(det, 1.5f);
	}
	
	void Assert(float a, float b) {
		var diff = a - b;
		if (diff < -EPSILON || EPSILON < diff)
			throw new System.InvalidOperationException();
	}		
}
