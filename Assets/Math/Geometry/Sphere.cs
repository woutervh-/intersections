using UnityEngine;

namespace Math.Geometry
{
    public struct Sphere
    {
        public Vector3 position;
        public float radius;

        public Sphere(Vector3 position, float radius)
        {
            this.position = position;
            this.radius = radius;
        }

        public static Vector3[] GenerateVertices(int samples, float minLatitude = -90f, float minLongitude = -180f, float maxLatitude = 90f, float maxLongitude = 180f)
        {
            float minPhi = Mathf.Deg2Rad * (minLongitude + 180f);
            float maxPhi = Mathf.Deg2Rad * (maxLongitude + 180f);
            float minTheta = Mathf.Deg2Rad * (minLatitude + 90f);
            float maxTheta = Mathf.Deg2Rad * (maxLatitude + 90f);
            samples = (int)(samples / (maxPhi - minPhi) * (2f * Mathf.PI));
            Vector3[] points = new Vector3[samples];
            float offset = 2f / samples;
            float increment = Mathf.PI * (3f - Mathf.Sqrt(5f));

            for (int i = 0; i < samples; i++)
            {
                float phi = ((i + 1) % samples) * increment % (2f * Mathf.PI);
                float y = ((i * offset) - 1) + (offset / 2);
                float theta = Mathf.Acos(-y);
                if (minPhi <= phi && phi <= maxPhi && minTheta <= theta && theta <= maxTheta)
                {
                    float r = Mathf.Sqrt(1 - Mathf.Pow(y, 2));
                    float x = Mathf.Sin(-phi) * r;
                    float z = Mathf.Cos(-phi) * r;
                    points[i] = new Vector3(x, y, z);
                }
            }

            return points;
        }

        public override string ToString()
        {
            return "position: " + position + " radius: " + radius;
        }
    }
}
