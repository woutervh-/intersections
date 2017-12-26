using System;
using UnityEngine;

namespace Math.Geometry
{
    public static class Intersections
    {
        public static Vector3? Intersects(Frustum frustum, Vector3 point)
        {
            for (int i = 0; i < 6; i++)
            {
                if (frustum[i].PlaneEquation(point) < 0.0f)
                {
                    return null;
                }
            }

            return point;
        }

        public static FrustumSphereIntersection?[] Intersects(Frustum frustum, Sphere sphere)
        {
            FrustumSphereIntersection?[] intersections = new FrustumSphereIntersection?[6];

            for (int i = 0; i < 6; i++)
            {
                float side = frustum[i].PlaneEquation(sphere.position);
                if (side < -sphere.radius)
                {
                    intersections[i] = null;
                }
                else
                {
                    intersections[i] = new FrustumSphereIntersection(frustum[i].normal, side);
                }
            }

            return intersections;
        }

        public struct FrustumSphereIntersection
        {
            public Vector3 normal;
            public float distance;

            public FrustumSphereIntersection(Vector3 normal, float distance)
            {
                this.normal = normal;
                this.distance = distance;
            }
        }
    }
}
