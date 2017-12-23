﻿using UnityEngine;

namespace Geometry
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

        public override string ToString()
        {
            return "position: " + position + " radius: " + radius;
        }
    }
}
