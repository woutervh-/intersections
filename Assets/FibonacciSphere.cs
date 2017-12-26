using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FibonacciSphere : MonoBehaviour
{
    [Range(1, 1000)]
    public int numberOfVertices;

    [Range(-90f, 90f)]
    public float minLatitude;

    [Range(-180f, 180f)]
    public float minLongitude;

    [Range(-90f, 90f)]
    public float maxLatitude;

    [Range(-180f, 180f)]
    public float maxLongitude;

    private Vector3[] vertices;

    void Start()
    {
        GenerateVertices();
    }

    void Update()
    {
        //
    }

    public void GenerateVertices()
    {
        vertices = new Vector3[0]; // Geometry.Sphere.GenerateVertices(numberOfVertices, minLatitude, minLongitude, maxLatitude, maxLongitude);
    }

    void OnDrawGizmos()
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.color = Color.HSVToRGB((float)i / vertices.Length, 1f, 1f);
            Gizmos.DrawSphere(transform.TransformPoint(vertices[i]), 0.01f);
            if (i < vertices.Length - 1)
            {
                Gizmos.DrawLine(transform.TransformPoint(vertices[i]), transform.TransformPoint(vertices[i + 1]));
            }
        }
    }
}
