using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vec2 {
	[SerializeField]
	private float x = 0.0f;

	[SerializeField]
	private float y = 0.0f;

	public Vec2() {
		x = 0.0f;
		y = 0.0f;
	}

	public Vec2(float a_x, float a_y) {
		x = a_x;
		y = a_y;
	}

	public Vec2(Vector3 a_pos) {
		x = a_pos.x;
		y = a_pos.y;
	}

	public Vector3 ToVector3() {
		return new Vector3(x, y, 0.0f);
	}

	//Overload the add (+) operator
	public static Vec2 operator +(Vec2 a, Vec2 b) {
		return new Vec2(a.x + b.x, a.y + b.y);
	}

	//Overload the subtract (-) operator
	public static Vec2 operator -(Vec2 a, Vec2 b) {
		return new Vec2(a.x - b.x, a.y - b.y);
	}

	//Overload the multiplication (*) operator
	public static Vec2 operator *(Vec2 a, float b) {
		return new Vec2(a.x * b, a.y * b);
	}

	public float Magnitude() {
		return Mathf.Sqrt(x * x + y * y);
	}
}
