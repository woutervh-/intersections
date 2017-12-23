using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    public Material insideMaterial;
    public Material outsideMaterial;

    public Vector3 pointPosition;

    public float sphereRadius;

    public Vector3 spherePosition;

    private GameObject pointDisplay;

    private GameObject sphereDisplay;

    private Geometry.Frustum frustum;

    void Start()
    {
        pointDisplay = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphereDisplay = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        for (int i = 0; i < planes.Length; i++)
        {
            Plane plane = planes[i];
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
            gameObject.name = "Plane " + i;
            gameObject.transform.position = -plane.normal * plane.distance;
            gameObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, plane.normal);
            frustum[i] = new Geometry.Plane(plane.normal, plane.distance);
        }

        Debug.Log(frustum);
    }

    void Update()
    {
        {
            pointDisplay.transform.position = pointPosition;
            pointDisplay.transform.localScale = 0.1f * Vector3.one;
            Vector3? intersection = Geometry.Intersections.Intersects(frustum, pointPosition);
            if (intersection.HasValue)
            {
                pointDisplay.GetComponent<Renderer>().material = insideMaterial;
            }
            else
            {
                pointDisplay.GetComponent<Renderer>().material = outsideMaterial;
            }
        }

        {
            sphereDisplay.transform.position = spherePosition;
            sphereDisplay.transform.localScale = 2f * sphereRadius * Vector3.one;
            bool intersects = Geometry.Intersections.Intersects(frustum, new Geometry.Sphere(spherePosition, sphereRadius)).All((intersection) => intersection != null);
            if (intersects)
            {
                sphereDisplay.GetComponent<Renderer>().material = insideMaterial;
            }
            else
            {
                sphereDisplay.GetComponent<Renderer>().material = outsideMaterial;
            }
        }
    }

    void OnDrawGizmos()
    {
        Geometry.Intersections.FrustumSphereIntersection?[] intersections = Geometry.Intersections.Intersects(frustum, new Geometry.Sphere(spherePosition, sphereRadius));
        foreach (Geometry.Intersections.FrustumSphereIntersection intersection in intersections.Where((intersection) => intersection.HasValue).Select((intersection) => intersection.Value))
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(spherePosition + intersection.normal * sphereRadius, spherePosition - intersection.normal * intersection.distance);
        }

        Vector3[] vertices = sphereDisplay.GetComponent<MeshFilter>().mesh.vertices;
        foreach (Vector3 vertex in vertices)
        {
            Vector3 position = sphereDisplay.transform.TransformPoint(vertex);
            Color color = Color.green;

            for (int i = 0; i < 6; i++)
            {
                if (frustum[i].PlaneEquation(position) < 0.0f) {
                    color = Color.red;
                }
            }

            Gizmos.color = color;
            Gizmos.DrawSphere(position, 0.05f);
        }
    }
}
