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
    
    //Die Kraft, mit der wir das Objekt einmal anschubsen.
    private float InitForce = 500;
    
    
    public Rigidbody rb;
    private void OnEnable()
    {
        //change if planet has belt:
        if (randomBoolean() && belt != null)
        {
            belt.SetActive(true);
        }
        
        
        transform.position = GetStartPosition();
        //Zum Loch gucken
        transform.LookAt(Vector3.zero);

        //Die kraft soll immer etwas anders sein. Sieht besser aus
        float forceNoise = Random.Range(0.6f, 1f);
        
        rb.AddForce(transform.right * InitForce * forceNoise);
    }

    private static Vector3 GetStartPosition()
    {
        //Der RAdius soll auch um 30% variieren. 
        float radiusNoise = Random.Range(0.7f, 1.3f);
        return RandomPointOnCircleEdge(25 * radiusNoise);
    }
    private static Vector3 RandomPointOnCircleEdge(float radius)
    {
        //zufälliger Punkt auf einem Kreis
        var vector2 = Random.insideUnitCircle.normalized * radius;
        return new Vector3(vector2.x, 0, vector2.y);
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
