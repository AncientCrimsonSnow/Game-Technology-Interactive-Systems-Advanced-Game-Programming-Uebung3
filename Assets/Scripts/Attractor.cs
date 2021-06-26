  
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour {

	const float G = 667.4f;

	public static List<Attractor> Attractors;

	public Rigidbody rb;

	void FixedUpdate ()
	{
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
