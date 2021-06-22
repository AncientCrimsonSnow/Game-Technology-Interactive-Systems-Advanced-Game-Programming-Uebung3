using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGen : MonoBehaviour
{
    [SerializeField] private GameObject SpherePrefab;

    [SerializeField] private bool Smooth;
    [SerializeField] private bool RadiusMinLook;
    
    [Range(0.001f, 10)] [SerializeField] private float radius;
    [Range(-5, 5)] [SerializeField] private float thickness;
    [Range(2, 64)] [SerializeField] private int divisionsX;
    [Range(2, 64)] [SerializeField] private int divisionsY;
    

    private void FixedUpdate()
    {
        if (RadiusMinLook)
        {
            radius = Mathf.Max(Mathf.Abs(thickness / 2), radius);
        }
        
        if(Smooth)
            GenTorusSmooth();
        else
        {
            GenTorus();
        }
    }
    private Mesh InitMesh(String name)
    {
        Mesh myMesh = new Mesh();
        myMesh.name = name;

        return myMesh;
    }
    public void GenTorusSmooth()
    {
        Mesh myMesh = InitMesh("Torus");
        //Vertices:
        List<Vector3> vertices = new List<Vector3>();

        float TAU = Mathf.PI * 2;
        for (int i = 0; i < divisionsX; i++)
        {
            float t = i / (float) divisionsX;
            float angRad = t * TAU;

            Vector3 point = new Vector3(
                Mathf.Cos(angRad) * radius,
                0,
                Mathf.Sin(angRad) * radius
            );

            for (int j = 0; j < divisionsY; j++)
            {
                t = j / (float) divisionsY;
                angRad = t * TAU;

                Vector3 vertice = point.normalized;

                vertice = new Vector3(
                    vertice.x * Mathf.Cos(angRad),
                    Mathf.Sin(angRad),
                    vertice.z * Mathf.Cos(angRad)
                );

                vertice *= thickness / 2;
                vertices.Add(vertice + point);
            }
        }
        /*
        for (int i = 0; i < vertices.Count; i++)
        {
            var sphere = Instantiate(SpherePrefab, transform);
            sphere.transform.position += vertices[i];
            sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            sphere.name = "V: " + i;
        }
        */
        //Triangles:
        List<int> triangleIndices = new List<int>();

        for (int x = 0; x < divisionsX; x++)
        {
            for (int y = 0; y < divisionsY; y++)
            {
                int rootIndex = (y + divisionsY * x);
                int rootIndexUp = (rootIndex + 1) % divisionsY + x * divisionsY;
                int rootIndexNext = (rootIndex + divisionsY) % vertices.Count;
                int rootIndexUpNext = ((rootIndex + 1) % divisionsY + x * divisionsY + divisionsY) % vertices.Count;
                
                //first triangle:
                triangleIndices.Add(rootIndex);
                triangleIndices.Add(rootIndexUp);
                triangleIndices.Add(rootIndexUpNext);
                
                //second triangle
                triangleIndices.Add(rootIndex);
                triangleIndices.Add(rootIndexUpNext);
                triangleIndices.Add(rootIndexNext);
            }
        }
        myMesh.SetVertices(vertices);
        myMesh.SetTriangles(triangleIndices, 0);
        myMesh.RecalculateNormals();
        GetComponent<MeshFilter>().sharedMesh = myMesh;
        
    }
    public void GenTorus() {

        Vector3 RotateFlat(float angRad)
        {
            return new Vector3(
                Mathf.Cos(angRad) * radius,
                0,
                Mathf.Sin(angRad) * radius
            );
        }
        float TAU = Mathf.PI * 2;
        float GetRadByPercent(float t)
        {
            return t * TAU;
        }
        Vector3 RotateAlongVector(Vector3 rotateDir, float length, float angRad)
        {
            Vector3 result = rotateDir.normalized;
            result = new Vector3(
                result.x * Mathf.Cos(angRad),
                Mathf.Sin(angRad),
                result.z * Mathf.Cos(angRad)
            );
            return result * length;
        }
        float GetPercentage(int value, int maxValue)
        {
            return (value / (float) maxValue) % 1;
        }
        Vector3 GetFinalVertice(Vector3 source, float length, int value, int maxValue)
        {
            Vector3 result = RotateAlongVector(source, length, GetRadByPercent(GetPercentage(value, maxValue)));
            return result + source;
        }
            
        Mesh myMesh = InitMesh("Torus");
        
        //Vertices:
        List<Vector3> vertices = new List<Vector3>();
        
        //Triangles
        List<int> triangleIndices = new List<int>();

        
        for (int i = 0; i < divisionsX; i++)
        {
            float t = i / (float) divisionsX;
            
            Vector3 rootSource = RotateFlat(GetRadByPercent(GetPercentage(i, divisionsX)));
            Vector3 nextSource = RotateFlat(GetRadByPercent(GetPercentage(i+1, divisionsX)));
            
            for (int j = 0; j < divisionsY; j++)
            {
                float length = thickness / 2;
                Vector3 root = GetFinalVertice(rootSource, length, j, divisionsY);
                Vector3 up = GetFinalVertice(rootSource, length, j + 1, divisionsY);
                Vector3 rootNext = GetFinalVertice(nextSource, length, j, divisionsY);
                Vector3 upNext = GetFinalVertice(nextSource, length, j + 1, divisionsY);
                
                int currIndex = vertices.Count;
                
                vertices.Add(root); //currIndex
                vertices.Add(up); //currIndex + 1
                vertices.Add(rootNext); //currIndex + 2
                vertices.Add(upNext); //currIndex +3
                
                
                //Triangles:
                //first triangle:
                triangleIndices.Add(currIndex);
                triangleIndices.Add(currIndex + 1);
                triangleIndices.Add(currIndex + 3);
                
                //second triangle
                triangleIndices.Add(currIndex);
                triangleIndices.Add(currIndex + 3);
                triangleIndices.Add(currIndex + 2);
            }
        }
        /*
        for (int i = 0; i < vertices.Count; i++)
        {
            var sphere = Instantiate(SpherePrefab, transform);
            sphere.transform.position += vertices[i];
            sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            sphere.name = "V: " + i;
        }
        */
        
        
        myMesh.SetVertices(vertices);
        myMesh.SetTriangles(triangleIndices, 0);
        myMesh.RecalculateNormals();
        GetComponent<MeshFilter>().sharedMesh = myMesh;
    }
}
