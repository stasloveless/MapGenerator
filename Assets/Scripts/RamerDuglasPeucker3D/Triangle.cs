using System.Collections.Generic;
using UnityEngine;

namespace RamerDuglasPeucker3D
{
    public class Triangle
    {
        public Vector2 V0 { get; }
        public Vector2 V1 { get; }
        public Vector2 V2 { get; }

        public Triangle() { }

        public Triangle(Vector2 v0, Vector2 v1, Vector2 v2)
        {
            V0 = v0;
            V1 = v1;
            V2 = v2;
        }

        public List<Vector2> CommonEdge(Triangle other)
        {
            List<Vector2> commonPoints = new List<Vector2>();
            if (V0.Equals(other.V0) || V0.Equals(other.V1) || V0.Equals(other.V2))
            {
                commonPoints.Add(V0);
            }

            if (V1.Equals(other.V0) || V1.Equals(other.V1) || V1.Equals(other.V2))
            {
                commonPoints.Add(V1);
            }

            if (V2.Equals(other.V0) || V2.Equals(other.V1) || V2.Equals(other.V2))
            {
                commonPoints.Add(V2);
            }

            return commonPoints;
        }

        public List<Vector2> Tris2PointList()
        {
            return new List<Vector2>{V0, V1, V2};
        }

        public override bool Equals(object obj)
        {
            var other = (Triangle) obj;
            return V0.Equals(other.V0) && V1.Equals(other.V1) && V2.Equals(other.V2);
        }

        public override string ToString()
        {
            return "v0 = " + V0 + "; v1 = " + V1 + "; v2 = " + V2;
        }
    }
}