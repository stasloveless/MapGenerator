using System;
using System.Collections.Generic;
using UnityEngine;

namespace MathCalculations
{
    public static class MathCalculations
    {
        public static float PointDistance2D(Vector2 first, Vector2 second)
        {
            var preDistance = Math.Pow(second.x - first.x, 2) +
                              Math.Pow(second.y - first.y, 2);

            return (float) Math.Sqrt(preDistance);
        }

        public static float PointDistance3D(Vector3 first, Vector3 second)
        {
            var preDistance = Math.Pow(second.x - first.x, 2) +
                              Math.Pow(second.y - first.y, 2) +
                              Math.Pow(second.z - first.z, 2);

            return (float) Math.Sqrt(preDistance);
        }

        public static float PointToLineDistance2D(Vector2[] linePoints, Vector2 point)
        {
            var ab = PointDistance2D(linePoints[0], point);
            var bc = PointDistance2D(linePoints[1], point);
            var ca = PointDistance2D(linePoints[0], linePoints[1]);

            var halfP = (ab + ca + bc) / 2;
            var preSquare = halfP * (halfP - ab) * (halfP - bc) * (halfP - ca);
            var square = (float) Math.Sqrt(preSquare);

            var distance = (2 * square) / ca;

            return distance;
        }

        public static float PointToLineDistance3D(Vector3[] linePoints, Vector3 point)
        {
            var ab = PointDistance3D(linePoints[0], point);
            var bc = PointDistance3D(linePoints[1], point);
            var ca = PointDistance3D(linePoints[0], linePoints[1]);

            var halfP = (ab + ca + bc) / 2;
            var preSquare = halfP * (halfP - ab) * (halfP - bc) * (halfP - ca);
            var square = (float) Math.Sqrt(preSquare);

            var distance = (2 * square) / ca;

            return distance;
        }

        public static float CircumCircleR(Vector2[] trianglePoints)
        {
            var ab = PointDistance2D(trianglePoints[0], trianglePoints[1]);
            var bc = PointDistance2D(trianglePoints[1], trianglePoints[2]);
            var ca = PointDistance2D(trianglePoints[2], trianglePoints[0]);

            var halfP = (ab + ca + bc) / 2;
            var preSquare = halfP * (halfP - ab) * (halfP - bc) * (halfP - ca);
            var square = (float) Math.Sqrt(preSquare);

            return (ab * bc * ca) / (4 * square);
        }

        public static float CircumCircleR(Vector2 a, Vector2 b, Vector2 c)
        {
            var ab = PointDistance2D(a, b);
            var bc = PointDistance2D(b, c);
            var ca = PointDistance2D(c, a);

            var halfP = (ab + ca + bc) / 2;
            var preSquare = halfP * (halfP - ab) * (halfP - bc) * (halfP - ca);
            var square = (float) Math.Sqrt(preSquare);

            return (ab * bc * ca) / (4 * square);
        }

        public static Vector2 LineMid2D(Vector2 first, Vector2 second)
        {
            var x = (first.x + second.x) / 2;
            var y = (first.y + second.y) / 2;

            return new Vector2(x, y);
        }

        public static float MatrixDeterminant3D(float[] matrixRow0, float[] matrixRow1, float[] matrixRow2)
        {
            var det = matrixRow0[0] * matrixRow1[1] * matrixRow2[2] + matrixRow0[1] * matrixRow1[2] * matrixRow2[0] +
                      matrixRow0[2] * matrixRow1[0] * matrixRow2[1] -
                      matrixRow0[2] * matrixRow1[1] * matrixRow2[0] - matrixRow0[1] * matrixRow1[0] * matrixRow2[2] -
                      matrixRow0[0] * matrixRow1[2] * matrixRow2[1];

            return det;
        }

        public static float PointLocationRelativeToLine(Vector2 first, Vector2 second, Vector2 outerPoint)
        {
            var k = (first.y - second.y) / (first.x - second.x);
            var b = second.y - k * second.x;

            return outerPoint.y - k * outerPoint.x - b;
        }

        public static List<int> CalculateFactors(int number)
        {
            var factors = new List<int>();

            var max = (int) Math.Sqrt(number);
            for (var i = 1; i <= max; i++)
            {
                if (number % i == 0)
                {
                    factors.Add(i);
                }
            }

            return factors;
        }
    }
}