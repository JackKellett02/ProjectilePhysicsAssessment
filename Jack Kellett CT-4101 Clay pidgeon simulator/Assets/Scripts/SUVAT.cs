using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a library of functions containing all versions of the suvat equations.
/// </summary>
public class SUVAT {
	/// <summary>
	/// v = u + at
	/// </summary>
	/// <param name="u"></param>
	/// <param name="a"></param>
	/// <param name="t"></param>
	/// <returns></returns>
	public static float CalculateFinalVelocity1(float u, float a, float t) {
		return u + a * t;
	}

	/// <summary>
	/// u = v - at
	/// </summary>
	/// <param name="v"></param>
	/// <param name="a"></param>
	/// <param name="t"></param>
	/// <returns></returns>
	public static float CalculateInitialVelocity1(float v, float a, float t) {
		return v - a * t;
	}

	/// <summary>
	/// a = (v - u) / t
	/// </summary>
	/// <param name="v"></param>
	/// <param name="u"></param>
	/// <param name="t"></param>
	/// <returns></returns>
	public static float CalculateAcceleration1(float v, float u, float t) {
		return (v - u) / t;
	}

	/// <summary>
	/// t = (v - u) / a
	/// </summary>
	/// <param name="v"></param>
	/// <param name="u"></param>
	/// <param name="a"></param>
	/// <returns></returns>
	public static float CalculateTime1(float v, float u, float a) {
		return (v - u) / a;
	}

	/// <summary>
	/// s = ut + 1/2at^2
	/// </summary>
	/// <param name="u"></param>
	/// <param name="t"></param>
	/// <param name="a"></param>
	/// <returns></returns>
	public static float CalculateDisplaceMent1(float u, float t, float a) {
		return (u * t) + (0.5f * a * (t * t));
	}

	/// <summary>
	/// u = (s - 1/2at^2) / t
	/// </summary>
	/// <param name="s"></param>
	/// <param name="t"></param>
	/// <param name="a"></param>
	/// <returns></returns>
	public static float CalculateInitialVelocity2(float s, float t, float a) {
		return (s - (0.5f * a * (t * t))) / t;
	}

	/// <summary>
	/// a = (s - ut) / 1/2t^2
	/// </summary>
	/// <param name="s"></param>
	/// <param name="u"></param>
	/// <param name="t"></param>
	/// <returns></returns>
	public static float CalculateAcceleration2(float s, float u, float t) {
		return (s - (u * t)) / (0.5f * (t * t));
	}

	/// <summary>
	/// 1/2at^2 + ut - s = 0
	/// </summary>
	/// <param name="s"></param>
	/// <param name="u"></param>
	/// <param name="a"></param>
	/// <returns></returns>
	public static float CalculateTime2(float s, float u, float a) {
		float timeOutcomeOne = UseQuadraticFormula1(u, 0.5f, (-s));
		float timeOutComeTwo = UseQuadraticFormula2(u, 0.5f, (-s));
		if (timeOutcomeOne > 0.0f) {
			return timeOutcomeOne;
		} else if (timeOutComeTwo > 0.0f) {
			return timeOutComeTwo;
		} else {
			return 0.0f;
		}
	}

	/// <summary>
	/// s = 0.5(u+v)t
	/// </summary>
	/// <param name="u"></param>
	/// <param name="v"></param>
	/// <param name="t"></param>
	/// <returns></returns>
	public static float CalculateDisplacement2(float u, float v, float t) {
		return 0.5f * (u + v) * t;
	}

	/// <summary>
	/// u = s / (0.5t) - v
	/// </summary>
	/// <param name="s"></param>
	/// <param name="v"></param>
	/// <param name="t"></param>
	/// <returns></returns>
	public static float CalculateInitialVelocity3(float s, float v, float t) {
		return (s / (0.5f * t)) - v;
	}

	/// <summary>
	/// v = s / (0.5t) - u
	/// </summary>
	/// <param name="s"></param>
	/// <param name="u"></param>
	/// <param name="t"></param>
	/// <returns></returns>
	public static float CalculateFinalVelocity2(float s, float u, float t) {
		return (s / (0.5f * t)) - u;
	}

	/// <summary>
	/// t = s / 0.5(u + v)
	/// </summary>
	/// <param name="s"></param>
	/// <param name="u"></param>
	/// <param name="v"></param>
	/// <returns></returns>
	public static float CalculateTime3(float s, float u, float v) {
		return (s / (0.5f * (u + v)));
	}

	/// <summary>
	/// v = (u^2 + 2as)^0.5
	/// </summary>
	/// <param name="u"></param>
	/// <param name="a"></param>
	/// <param name="s"></param>
	/// <returns></returns>
	public static float CalculateFinalVelocity3(float u, float a, float s) {
		return Mathf.Sqrt(u * u + 2 * a * s);
	}

	/// <summary>
	/// u = (v^2 - 2as)^0.5
	/// </summary>
	/// <param name="v"></param>
	/// <param name="a"></param>
	/// <param name="s"></param>
	/// <returns></returns>
	public static float CalculateInitialVelocity4(float v, float a, float s) {
		return Mathf.Sqrt(v * v - 2 * a * s);
	}

	/// <summary>
	/// a = (v^2 - u^2) / 2s
	/// </summary>
	/// <param name="v"></param>
	/// <param name="u"></param>
	/// <param name="s"></param>
	/// <returns></returns>
	public static float CalculateAcceleration3(float v, float u, float s) {
		return ((v * v) - (u * u)) / (2 * s);
	}

	/// <summary>
	/// s = (v^2 - u^2) / 2a
	/// </summary>
	/// <param name="v"></param>
	/// <param name="u"></param>
	/// <param name="a"></param>
	/// <returns></returns>
	public static float CalculateDisplacement3(float v, float u, float a) {
		return ((v * v) - (u * u)) / (2 * a);
	}

	/// <summary>
	/// s = vt - 0.5at^2
	/// </summary>
	/// <param name="v"></param>
	/// <param name="t"></param>
	/// <param name="a"></param>
	/// <returns></returns>
	public static float CalculateDisplacement4(float v, float t, float a) {
		return (v * t) - (0.5f * a * (t * t));
	}

	/// <summary>
	/// v = (s + 1/2at^2) / t
	/// </summary>
	/// <param name="s"></param>
	/// <param name="t"></param>
	/// <param name="a"></param>
	/// <returns></returns>
	public static float CalculateFinalVelocity4(float s, float t, float a) {
		return (s + 0.5f * a * (t * t)) / t;
	}

	/// <summary>
	/// a = (vt - s) / (0.5t^2)
	/// </summary>
	/// <param name="s"></param>
	/// <param name="v"></param>
	/// <param name="t"></param>
	/// <returns></returns>
	public static float CalculateAcceleration4(float s, float v, float t) {
		return (v * t - s) / (0.5f * (t * t));
	}

	/// <summary>
	/// 1/2at^2 - vt + s
	/// </summary>
	/// <param name="s"></param>
	/// <param name="v"></param>
	/// <param name="a"></param>
	/// <returns></returns>
	public static float CalculateTime4(float s, float v, float a) {
		float timeOutcomeOne = UseQuadraticFormula1(-v, 0.5f, s);
		float timeOutComeTwo = UseQuadraticFormula2(-v, 0.5f, s);
		if (timeOutcomeOne > 0.0f) {
			return timeOutcomeOne;
		} else if (timeOutComeTwo > 0.0f) {
			return timeOutComeTwo;
		} else {
			return 0.0f;
		}
	}

	/// <summary>
	/// u = (s / 0.5t) / 2.0f
	/// OR
	/// v = (s / 0.5) / 2.0f
	/// ONLY WORKS WHEN A = 0. AKA WHEN VELOCITY DOESN'T CHANGE.
	/// </summary>
	/// <param name="s"></param>
	/// <param name="t"></param>
	/// <returns></returns>
	public static float CalculateInitialVelocity5(float s, float t) {
		return (s / (0.5f * t)) / 2.0f;
	}

	/// <summary>
	/// Returns one outcome of quadratic equation.
	/// </summary>
	/// <param name="b"></param>
	/// <param name="a"></param>
	/// <param name="c"></param>
	/// <returns></returns>
	public static float UseQuadraticFormula1(float b, float a, float c) {
		return ((-b) + Mathf.Sqrt((b * b) - 4 * a * c)) / 2 * a;
	}

	/// <summary>
	/// Returns one outcome of quadratic equation.
	/// </summary>
	/// <param name="b"></param>
	/// <param name="a"></param>
	/// <param name="c"></param>
	/// <returns></returns>
	public static float UseQuadraticFormula2(float b, float a, float c) {
		return ((-b) - Mathf.Sqrt((b * b) - 4 * a * c)) / 2 * a;
	}
}
