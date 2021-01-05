using System;
using UnityEngine;

namespace Hausdorff
{
    public class HausdorffDistance
    {
        public static float Calculate(Vector3[] highPoly, Vector3[] lowPoly)
        {
            var maxYX = float.MinValue;
            var minYX = float.MaxValue;

            var maxXY = float.MinValue;
            var minXY = float.MaxValue;

            foreach (var y in highPoly)
            {
                foreach (var x in lowPoly)
                {
                    if (Vector3.Distance(x, y) < minYX)
                    {
                        minYX = Vector3.Distance(x, y);
                    }
                }

                if (minYX > maxYX)
                {
                    maxYX = minYX;
                }

                minYX = float.MaxValue;
            }

            foreach (var x in lowPoly)
            {
                foreach (var y in highPoly)
                {
                    if (Vector3.Distance(x, y) < minXY)
                    {
                        minXY = Vector3.Distance(x, y);
                    }
                }

                if (minXY > maxXY)
                {
                    maxXY = minXY;
                }

                minXY = float.MaxValue;
            }

            var res = Math.Max(maxXY, maxYX);

            return res;
        }
    }
}