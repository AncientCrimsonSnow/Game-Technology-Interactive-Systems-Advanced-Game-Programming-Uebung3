  
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour {

	/*
	 * https://github.com/Brackeys/Gravity-Simulation-Tutorial
	 *
	 * Von hier geklaut. Leicht verändert, da:
	 *
	 * Objekte sich nicht gegenseitig beeinflussen sollen sondern nur das BlackHole eine Anziehung auf alle haben soll.
	 */

	
	const float G = 667.4f;

	public static List<Attractor> Attractors;

	public Rigidbody rb;

	void FixedUpdate ()
	{
		/*
		 * auf (0,0,0) befindet sich das Black Hole, nur dieses soll andere Objekte anziehen.
		 * JA, unsauberer und fehleranfälliger hätte man es wahrscheinlich nicht machen können ABER die Zeit wurde knapp und
		 * wenden würde das überarbeitet werden, wenn man weiter dran arbeiten würde. Das gilt auch für die das if Statement
		 * int Zeile 48.
		 */

		
		if (transform.position.Equals(Vector3.zero))
		{
			foreach (Attractor attractor in Attractors)
			{
				Attract(attractor);
			}
		}
	}

	void OnEnable ()
	{
		if (Attractors == null)
			Attractors = new List<Attractor>();
		
		//lazy way to say, only the black hole ist allowed to attract
		if (!transform.position.Equals(Vector3.zero))
		{
			Attractors.Add(this);
		}
	}

	void OnDisable ()
	{
		Attractors.Remove(this);
	}

	void Attract (Attractor objToAttract)
	{
		Rigidbody rbToAttract = objToAttract.rb;

		Vector3 direction = -rbToAttract.position;
		float distance = direction.magnitude;

		if (distance == 0f)
			return;

		float forceMagnitude = G * 5 / Mathf.Pow(distance, 2);
		Vector3 force = direction.normalized * forceMagnitude;

		rbToAttract.AddForce(force);
	}

}
