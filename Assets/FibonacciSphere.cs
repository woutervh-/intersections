using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private void Start()
    {
        GenerateVertices();
    }

    private void Update()
    {
        //
    }

    public void GenerateVertices()
    {
        Math.Tuple<float[], int[]> regions = Math.Geometry.SphereEqualAreaSmallDiameter.GenerateCaps(2, numberOfVertices);
        float[] colatitudes = regions.Item1;
        int[] regionList = regions.Item2;
        int collarCount = colatitudes.Length;

        int sumRegions = regionList.Sum();
        Vector3[] vertices = new Vector3[sumRegions];

        int index = 0;
        for (int i = 0; i < collarCount; i++)
        {
            float topColatitude = i == 0 ? 0f : colatitudes[i - 1];
            float bottomColatitude = colatitudes[i];
            PutCollarVertices(regionList[i], index, vertices, topColatitude, bottomColatitude);
            index += regionList[i];
        }
        
        this.vertices = vertices;
    }

    private void PutCollarVertices(int regions, int index, Vector3[] vertices, float topColatitude, float bottomColatitude)
    {
        float theta = (topColatitude + bottomColatitude) / 2f;
        float y = Mathf.Cos(theta);
        for (int i = 0; i < regions; i++)
        {
            float phi = (float)i / regions * 2f * Mathf.PI;
            float x = Mathf.Sin(theta) * Mathf.Sin(phi);
            float z = Mathf.Sin(theta) * Mathf.Cos(phi);
            vertices[i + index] = new Vector3(x, y, z);
        }
    }

    private void OnDrawGizmos()
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
