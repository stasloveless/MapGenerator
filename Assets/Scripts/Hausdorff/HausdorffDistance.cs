using System;
using UnityEngine;

namespace Hausdorff
{
    public class HausdorffDistance
    {
        public static float Calculate(Vector3[] highPoly, Vector3[] lowPoly)
        {
            float maxYX = 0;
            float minYX = 100;

            float maxXY = 0;
            float minXY = 100;

            foreach (var y in highPoly)
            {
                foreach (var x in lowPoly)
                {
                    if (Vector3.Distance(x, y) > maxYX)
                    {
                        maxYX = Vector3.Distance(x, y);
                    }
                }

                if (maxYX < minYX)
                {
                    minYX = maxYX;
                }
            }

            foreach (var x in lowPoly)
            {
                foreach (var y in highPoly)
                {
                    if (Vector3.Distance(y, x) > maxXY)
                    {
                        maxXY = Vector3.Distance(y, x);
                    }
                }

                if (maxXY < minXY)
                {
                    minXY = maxXY;
                }
            }

            return Math.Max(minXY, minYX);
        }
    }
}