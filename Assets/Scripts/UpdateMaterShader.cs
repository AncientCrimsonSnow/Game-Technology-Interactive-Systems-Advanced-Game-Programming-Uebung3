using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class UpdateMaterShader : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    private Material mat;
    [SerializeField] private Vector3 lightSource;


    // Update is called once per frame
    private void OnEnable()
    {
        mat = meshRenderer.material;
        mat.SetVector("_Lightsource", lightSource);
        mat.SetColor("_Color", Random.ColorHSV());
    }
    void Update()
    {
        mat.SetVector("_WorldPos", transform.position);
    }
}
