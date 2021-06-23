using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateMaterShader : MonoBehaviour
{
    [SerializeField] private Material mat;
    [SerializeField] private GameObject lightSource;
    [SerializeField] private Color color;
    

    // Update is called once per frame
    void Update()
    {
        mat.SetVector("_Lightsource", lightSource.transform.position);
        mat.SetVector("_WorldPos", transform.position);
        mat.SetColor("_Color", color);
    }
}
