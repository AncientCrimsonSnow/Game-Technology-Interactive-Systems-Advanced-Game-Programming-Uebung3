using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParaMeshGen : MonoBehaviour
{
    [Range(2, 64)] [SerializeField] private int divisionsX = 12;
    [Range(2, 64)] [SerializeField] private int divisionsY = 12;
    [Range(0.1f, 10)] [SerializeField] private float scale = 1;

    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private Shapes shape = Shapes.Default;

    private MeshCollider MeshCollider;
    
    
    private Mesh mesh;

    private enum Shapes
    {
        Spiraldings,
        Torus,
        Sphere,
        KomischerTrichter,
        OakPost,
        Cylinder,
        Cone,
        Default
    }

    private void Start()
    {
        MeshCollider = GetComponent<MeshCollider>();
    }

    private void FixedUpdate()
    {
        Generate();
        if (MeshCollider != null)
        {
            MeshCollider.sharedMesh = mesh;
        }
    }

    /*
     * x = u.x
     * y = u.y
     * z = v.x
     * w = v.y
     */
    Vector4 GetUVBounds()
    {
        mesh = new Mesh();
        switch (shape)
        {
            case Shapes.Spiraldings:
                return new Vector4(0, 50 * Mathf.PI, 1, 2);
            case Shapes.Torus:
                return new Vector4(0, 2* Mathf.PI, 0, 2*Mathf.PI);
            case Shapes.Sphere:
                return new Vector4(0, Mathf.PI, 0, 2*Mathf.PI);
            case Shapes.KomischerTrichter:
                return new Vector4(0, 6.28f, 0, 9);
            case Shapes.OakPost:
                return new Vector4(0, 60, 0, 2 * Mathf.PI);
            case Shapes.Cylinder:
                return new Vector4(0, 2 * Mathf.PI, -1, 1);
            case Shapes.Cone:
                return new Vector4(0, 2 * Mathf.PI, 0, 2);
            default:
                return new Vector4(-1, 1, -1, 1);
        }
    }
    Vector3 GetVertex(float u, float v)
    {
        switch (shape)
        {
            case Shapes.Spiraldings:
                return new Vector3(
                    cos(u) * sin(v) ,
                    sin(u) * sin(v) ,
                    cos(v/1.1f) + log(tan(v/2)* 2) + 0.1f * u);
            case Shapes.Torus:
                float R = 1;
                float r = 0.2f;
                return new Vector3(
                    (R + r * cos(u)) * cos(v),
                    (R + r * cos(u)) * sin(v),
                    r * sin(u));
            case Shapes.Sphere:
                return new Vector3(
                    sin(u) * cos(v),
                    sin(u) * sin(v),
                    cos(u));
            case Shapes.KomischerTrichter:
                return new Vector3(
                    v * cos(u),
                    v * sin(u),
                    v + sin(3*v)/3 - 4);
            case Shapes.OakPost:
                return new Vector3(
                        5 + cos((Mathf.PI * u / 6)) * cos(v),
                        5 + cos((Mathf.PI * u / 6)) * sin(v),
                        u
                    );
            case Shapes.Cylinder:
                return new Vector3(
                       cos(u),
                       sin(u),
                       v
                );
            case Shapes.Cone:
                float R1 = 1;
                float h = 2;
                return new Vector3(
                        (R1 - (R1 / h) * v) * cos(u),
                        (R1 - (R1 / h) * v) * sin(u),
                        v
                    );
            default:
                return Vector3.one;
        } 
    }
    float cos(float value)
    {
        return Mathf.Cos(value);
    }
    float sin(float value)
    {
        return Mathf.Sin(value);
    }
    float tan(float value)
    {
        return Mathf.Tan(value);
    }
    float log(float value)
    {
        return Mathf.Log(value);
    }

    void Generate()
     {
         mesh = new Mesh();

         var divisions = new Vector2Int(divisionsX, divisionsY);     
         var vertexSize = divisions + new Vector2Int(1, 1);
         var uvBounds = GetUVBounds();
                                                                                        
         var vertices = new Vector3[vertexSize.x * vertexSize.y];                     
         var uvs = new Vector2[vertices.Length];                                     

         for (var y = 0; y < vertexSize.y; y++)                            
         {
             var vNormalized = (1f / divisions.y) * y;
             var v = Mathf.Lerp(uvBounds.z, uvBounds.w, vNormalized); // boundary check

             for (var x = 0; x < vertexSize.x; x++)
             {
                 var uNormalized = (1f / divisions.x) * x;
                 var u = Mathf.Lerp(uvBounds.x, uvBounds.y, uNormalized); // boundary check  

                 var vertex = GetVertex(u, v) * scale;

                 var uv = new Vector2(u, v);

                 var arrayIndex = x + y * vertexSize.x;

                 vertices[arrayIndex] = vertex;
                 uvs[arrayIndex] = uv;
             }
         }

         //triangles
         var triangles = new int[divisions.x * divisions.y * 6];

         for (var i = 0; i < divisions.x * divisions.y; i++)
         {
             var triangleIndex = (i % divisions.x) + (i / divisions.x) * vertexSize.x;

             var indexer = i * 6;

             triangles[indexer + 0] = triangleIndex;
             triangles[indexer + 1] = triangleIndex + divisions.x + 1;
             triangles[indexer + 2] = triangleIndex + 1;
             
             triangles[indexer + 3] = triangleIndex + 1;
             triangles[indexer + 4] = triangleIndex + divisions.x + 1;
             triangles[indexer + 5] = triangleIndex + divisions.x + 2;

         }
         mesh.SetVertices(vertices);  
         mesh.SetUVs(0, uvs);
         mesh.SetTriangles(triangles, 0);
         mesh.RecalculateBounds();
         mesh.RecalculateNormals();
         mesh.RecalculateTangents();

         meshFilter.mesh = mesh;
     }
}
