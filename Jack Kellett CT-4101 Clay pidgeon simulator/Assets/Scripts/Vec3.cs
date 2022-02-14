using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vec3 {
	public float x = 0.0f;
	public float y = 0.0f;
	public float z = 0.0f;

	public Vec3() {
		x = 0.0f;
		y = 0.0f;
		z = 0.0f;
	}

	public Vec3(float a_x, float a_y, float a_z) {
		x = a_x;
		y = a_y;
		z = a_z;
	}

	public Vec3(Vector3 a_pos) {
		x = a_pos.x;
		y = a_pos.y;
		z = a_pos.z;
	}

	public Vec3(Quaternion a_rotation) {
		x = a_rotation.x;
		y = a_rotation.y;
		z = a_rotation.z;
	}

	public Vector3 ToVector3() {
		return new Vector3(x, y, z);
	}

	public Quaternion ToQuartenion() {
		float cx = Mathf.Cos(ToRad(x) * 0.5f);
		float cy = Mathf.Cos(ToRad(y) * 0.5f);
		float cz = Mathf.Cos(ToRad(z) * 0.5f);
		float sx = Mathf.Sin(ToRad(x) * 0.5f);
		float sy = Mathf.Sin(ToRad(y) * 0.5f);
		float sz = Mathf.Sin(ToRad(z) * 0.5f);
		Quaternion q;
		q.x = cx * sy * sz + cy * cz * sx;
		q.y = cx * cz * sy - cy * sx * sz;
		q.z = cx * cy * sz - cz * sx * sy;
		q.w = sx * sy * sz + cx * cy * cz;
		return q;
	}

	private float ToRad(float f) {
		return Mathf.Deg2Rad * f;
	}

	//Overload the add (+) operator
	public static Vec3 operator +(Vec3 a, Vec3 b) {
		return new Vec3(a.x + b.x, a.y + b.y, a.z + b.z);
	}

	//Overload the subtract (-) operator
	public static Vec3 operator -(Vec3 a, Vec3 b) {
		return new Vec3(a.x - b.x, a.y - b.y, a.z - b.z);
	}

	//Overload the multiplication (*) operator
	public static Vec3 operator *(Vec3 a, float b) {
		return new Vec3(a.x * b, a.y * b, a.z * b);
	}

	public static Vec3 operator -(Vec3 a) {
		return new Vec3(0 - a.x, 0 - a.y, 0 - a.z);
	}

	public float Magnitude() {
		return Mathf.Sqrt(x * x + y * y + z * z);
	}

	public static Vec3 CrossProduct(Vec3 a, Vec3 b) {
		return new Vec3(a.y * b.z - a.z * b.y,
						a.z * b.x - a.x * b.z,
						a.x * b.y - a.y * b.x);
	}

	public float GetXValue() {
		return x;
	}

	public float GetYValue() {
		return y;
	}

	public float GetZValue() {
		return z;
	}

	public void SetXValue(float a_x) {
		x = a_x;
	}

	public void SetYValue(float a_y) {
		y = a_y;
	}

	public void SetZValue(float a_z) {
		z = a_z;
	}
}