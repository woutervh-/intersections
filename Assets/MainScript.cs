using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MainScript : MonoBehaviour
{
    public Material insideMaterial;
    public Material outsideMaterial;

    public Vector3 pointPosition;

    public float sphereRadius;

    public Vector3 spherePosition;

    public Text debugText;

    private GameObject pointDisplay;

    private GameObject sphereDisplay;

    private GameObject sphereCameraPlaneDisplay;

    private Math.Geometry.Frustum frustum;

    private Vector3 cameraPosition;

    private Vector3[] samples;

    void Start()
    {
        pointDisplay = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphereDisplay = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphereCameraPlaneDisplay = GameObject.CreatePrimitive(PrimitiveType.Plane);
        cameraPosition = Camera.main.transform.position;

        //Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        //for (int i = 0; i < planes.Length; i++)
        //{
        //    Plane plane = planes[i];
        //    GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
        //    gameObject.name = "Plane " + i;
        //    gameObject.transform.position = -plane.normal * plane.distance;
        //    gameObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, plane.normal);
        //    frustum[i] = new Math.Geometry.Plane(plane.normal, plane.distance);
        //}

        samples = new Vector3[100 * 50];
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 50; j++)
            {
                float theta = 2.0f * Mathf.PI * i / 100.0f;
                float phi = Mathf.Acos(2.0f * j / 50.0f - 1.0f);
                float x = Mathf.Cos(theta) * Mathf.Sin(phi);
                float y = Mathf.Sin(theta) * Mathf.Sin(phi);
                float z = Mathf.Cos(phi);
                samples[i * 50 + j] = new Vector3(x, y, z);
            }
        }
    }

    void Update()
    {
        {
            pointDisplay.transform.position = pointPosition;
            pointDisplay.transform.localScale = 0.1f * Vector3.one;
            Vector3? intersection = Math.Geometry.Intersections.Intersects(frustum, pointPosition);
            if (intersection.HasValue)
            {
                pointDisplay.GetComponent<Renderer>().material = insideMaterial;
            }
            else
            {
                pointDisplay.GetComponent<Renderer>().material = outsideMaterial;
            }
        }

        sphereCameraPlaneDisplay.transform.position = spherePosition;
        sphereCameraPlaneDisplay.transform.rotation = Quaternion.FromToRotation(Vector3.up, cameraPosition - spherePosition);

        {
            sphereDisplay.transform.position = spherePosition;
            sphereDisplay.transform.localScale = 2f * sphereRadius * Vector3.one;
            bool intersects = Math.Geometry.Intersections.Intersects(frustum, new Math.Geometry.Sphere(spherePosition, sphereRadius)).All((intersection) => intersection != null);
            if (intersects)
            {
                sphereDisplay.GetComponent<Renderer>().material = insideMaterial;
            }
            else
            {
                sphereDisplay.GetComponent<Renderer>().material = outsideMaterial;
            }
        }

        int countInside = 0;
        for (int i = 0; i < samples.Length; i++)
        {
            Vector3 position = samples[i] * sphereRadius + spherePosition;
            bool inside = true;
            if (!Math.Geometry.Intersections.Intersects(frustum, position).HasValue)
            {
                inside = false;
            }
            Vector3 cameraPlaneNormal = (cameraPosition - spherePosition).normalized;
            Math.Geometry.Plane plane = new Math.Geometry.Plane(cameraPlaneNormal, -Vector3.Dot(cameraPlaneNormal, spherePosition));
            if (plane.PlaneEquation(position) < 0.0f)
            {
                inside = false;
            }
            if (inside)
            {
                countInside += 1;
            }
        }
        debugText.text = "Area visible: " + ((float)countInside / samples.Length * 100f) + "%";
    }

    void OnDrawGizmos()
    {
        //Math.Geometry.Intersections.FrustumSphereIntersection?[] intersections = Math.Geometry.Intersections.Intersects(frustum, new Math.Geometry.Sphere(spherePosition, sphereRadius));
        //foreach (Math.Geometry.Intersections.FrustumSphereIntersection intersection in intersections.Where((intersection) => intersection.HasValue).Select((intersection) => intersection.Value))
        //{
        //    Gizmos.color = Color.blue;
        //    Gizmos.DrawLine(spherePosition + intersection.normal * sphereRadius, spherePosition - intersection.normal * intersection.distance);
        //}

        //Vector3[] vertices = sphereDisplay.GetComponent<MeshFilter>().mesh.vertices;
        //foreach (Vector3 vertex in vertices)
        //{
        //    Vector3 position = sphereDisplay.transform.TransformPoint(vertex);
        //    Color color = Color.green;

        //    for (int i = 0; i < 6; i++)
        //    {
        //        if (frustum[i].PlaneEquation(position) < 0.0f)
        //        {
        //            color = Color.red;
        //        }
        //    }

        //    Vector3 cameraPlaneNormal = (cameraPosition - spherePosition).normalized;
        //    // if (new Geometry.Plane(cameraPlaneNormal, -Vector3.Dot(cameraPlaneNormal, spherePosition)).PlaneEquation(position) < 0.0f)
        //    if (Vector3.Dot(cameraPlaneNormal, position) - Vector3.Dot(cameraPlaneNormal, spherePosition) < 0.0f)
        //    {
        //        color = Color.red;
        //    }

        //    Gizmos.color = color;
        //    Gizmos.DrawSphere(position, 0.05f);
        //}

        {
            Vector3 cameraPlaneNormal = (cameraPosition - spherePosition).normalized;
            Math.Geometry.Plane plane = new Math.Geometry.Plane(cameraPlaneNormal, -Vector3.Dot(cameraPlaneNormal, spherePosition));

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(spherePosition + plane.normal * sphereRadius, spherePosition - plane.normal * plane.PlaneEquation(spherePosition));

            // Math.Geometry.Plane plane = frustum.bottom;
            Vector3 closestPointOnPlane = spherePosition - plane.PlaneEquation(spherePosition) * plane.normal;
            // Vector3 perpendicularPlaneNormal = new Vector3(1f, 1f, -(plane.normal.x + plane.normal.y) / plane.normal.z).normalized;
            float theta = Mathf.Acos(plane.normal.z) + Mathf.PI / 2f;
            float phi = Mathf.Atan(plane.normal.y / plane.normal.x);
            Vector3 planePerpendicular1 = new Vector3(Mathf.Sin(theta) * Mathf.Cos(phi), Mathf.Sin(theta) * Mathf.Sin(phi), Mathf.Cos(theta));
            Vector3 planePerpendicular2 = Vector3.Cross(planePerpendicular1, plane.normal);
            float offset = Mathf.Sqrt(Mathf.Pow(sphereRadius, 2) - Mathf.Pow(plane.PlaneEquation(spherePosition), 2));
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(closestPointOnPlane, 0.1f);
            Gizmos.DrawSphere(closestPointOnPlane + planePerpendicular1 * offset, 0.1f);
            Gizmos.DrawSphere(closestPointOnPlane + planePerpendicular2 * offset, 0.1f);
        }
    }
}
