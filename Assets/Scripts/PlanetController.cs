using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class PlanetController : MonoBehaviour
{
    [SerializeField] private GameObject belt;
    
    
    private float InitForce = 500;
    

    private enum Plane
    {
        X,Y,Z
    }
    
    public Rigidbody rb;
    private void OnEnable()
    {
        //change if planet has belt:
        if (randomBoolean() && belt != null)
        {
            belt.SetActive(true);
        }
        transform.position = GetStartPosition();
        transform.LookAt(Vector3.zero);

        float forceNoise = Random.Range(0.6f, 1f);
        
        rb.AddForce(rotateVectorOnPlane(Plane.X, -90) * InitForce * forceNoise);
    }

    private static Vector3 GetStartPosition()
    {
        float radiusNoise = Random.Range(0.7f, 1.3f);
        return RandomPointOnCircleEdge(25 * radiusNoise);
    }
    private static Vector3 RandomPointOnCircleEdge(float radius)
    {
        var vector2 = Random.insideUnitCircle.normalized * radius;
        return new Vector3(vector2.x, 0, vector2.y);
    }
    
    private Vector3 rotateVectorOnPlane(Plane plane, float angle)
    {
        switch (plane)
        { 
            case Plane.X:
                //transform.rotation.x += new Vector3(90,0,0);
                break;
            case Plane.Y:
                
                break;
            case Plane.Z:
                
                break;
        }
        return transform.right;
    }
    static bool randomBoolean ()
    {
        if (Random.value >= 0.5)
        {
            return true;
        }
        return false;
    }
}
