using UnityEngine;

namespace Geometry
{
    public struct Plane
    {
        public Vector3 normal;
        public float distance;

        public Plane(Vector3 normal, float distance)
        {
            this.normal = normal;
            this.distance = distance;
        }

        public float PlaneEquation(Vector3 point)
        {
            return Vector3.Dot(point, normal) + distance;
        }

        public override string ToString()
        {
            return normal.x + "x + " + normal.y + "y + " + normal.z + "z + " + distance + " = 0";
        }
    }
}
