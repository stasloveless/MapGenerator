using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RamerDuglasPeucker3D
{
    public static class RamerDuglasPeuckerAlgorithm3D
    {
        public static Vector3[] SimplifyMap(Vector3[] heightMap, float e, int mapSize, bool pseudoDistance, float c)
        {
            var extremePoints = FindExtremes(heightMap);
            var planeVectors = FindOrigin(extremePoints);
            var superStructure = FindCornerPoints(heightMap, mapSize, planeVectors);
            var pointsToSort = new List<Vector3>(heightMap);

            foreach (var point in superStructure)
            {
                pointsToSort.Remove(point);
            }

            var sortedPoints = SortPoints(pointsToSort.ToArray(), planeVectors);
            var filteredPoints = FilterPoints(sortedPoints, planeVectors, e,pseudoDistance, c);
            filteredPoints.Add(planeVectors[0]);
            filteredPoints.AddRange(superStructure);

            return filteredPoints.ToArray();
        }

        public static List<Vector3> FindCornerPoints(Vector3[] heightMap, int mapSize, Vector3[] planeVectors)
        {
            var cornerPoints = new List<Vector3>();
            foreach (var point in heightMap)
            {
                if ((int) point.x == 0 && (int) point.z == mapSize - 1)
                {
                    cornerPoints.Add(new Vector3(point.x, point.y, point.z));
                }
                else if ((int) point.x == mapSize - 1 && (int) point.z == mapSize - 1)
                {
                    cornerPoints.Add(new Vector3(point.x, point.y, point.z));
                }
                else if ((int) point.x == 0 && (int) point.z == 0)
                {
                    cornerPoints.Add(new Vector3(point.x, point.y, point.z));
                }
                else if ((int) point.x == mapSize - 1 && (int) point.z == 0)
                {
                    cornerPoints.Add(new Vector3(point.x, point.y, point.z));
                }
            }

            var edgePointsWithoutPlaneVectors = new List<Vector3>(cornerPoints);
            
            foreach (var planeVect in planeVectors)
            {
                edgePointsWithoutPlaneVectors.Remove(planeVect);
            }

            return edgePointsWithoutPlaneVectors;
        }

        private static List<Vector3> FilterPoints(Vector3[] sortedPoints, Vector3[] planeVectors, float e, bool pseudoDistance, float c)
        {
            var keptPointsIds = new List<Vector3>();
            keptPointsIds.Add(planeVectors[1]);
            keptPointsIds.Add(planeVectors[2]);
            var stack = new int[sortedPoints.Length];
            stack[0] = sortedPoints.Length - 1;
            var anchor = 0;
            var middle = 0;

            var i = 0;
            do
            {
                var dMax = 0f;
                var floatPoint = stack[i];
                var j = anchor;
                var plane = new Plane(planeVectors[0], sortedPoints[anchor], sortedPoints[floatPoint]);

                while (true)
                {
                    j++;
                    if (j >= floatPoint) break;
                    var d = Math.Abs(plane.GetDistanceToPoint(sortedPoints[j]));

                    if (pseudoDistance)
                    {
                        var pseudoD = PseudoPointPlaneDistance(d, c, sortedPoints, j, anchor, floatPoint);

                        if (pseudoD > dMax)
                        {
                            middle = j;
                            dMax = pseudoD;
                        }
                    }
                    else
                    {
                        if (d > dMax)
                        {
                            middle = j;
                            dMax = d;
                        }
                    }
                }

                if (dMax > e)
                {
                    keptPointsIds.Add(sortedPoints[middle]);
                    i++;
                    stack[i] = middle;
                }
                else if (i != 0)
                {
                    anchor = floatPoint;
                    i--;
                }
                else
                {
                    break;
                }
                
            } while (true);

            return keptPointsIds;
        }

        public static Vector3[] SortPoints(Vector3[] heightMap, Vector3[] planeVectors)
        {
            var pointsWithoutOrigin = new List<Vector3>(heightMap);
            pointsWithoutOrigin.Remove(planeVectors[0]);
            pointsWithoutOrigin.Remove(planeVectors[2]);
            var sortedPoints = pointsWithoutOrigin.OrderBy(vector => Vector3.Distance(planeVectors[1], vector)).ToList();
            sortedPoints.Add(planeVectors[2]);
            return sortedPoints.ToArray();
        }

        public static Vector3[] FindExtremes(Vector3[] heightMap)
        {
            var extremePoints = new List<Vector3>();
            var maxX = heightMap.Max(point => point.x);
            var maxY = heightMap.Max(point => point.y);
            var maxZ = heightMap.Max(point => point.z);
            var minX = heightMap.Min(point => point.x);
            var minY = heightMap.Min(point => point.y);

            var minZ = heightMap.Min(point => point.z);
            foreach (var point in heightMap)
            {
                if (point.x.Equals(maxX) || point.x.Equals(minX))
                {
                    extremePoints.Add(point);
                }

                else if (point.y.Equals(maxY) || point.y.Equals(minY))
                {
                    extremePoints.Add(point);
                }

                else if (point.y.Equals(maxZ) || point.z.Equals(minZ))
                {
                    extremePoints.Add(point);
                }
            }

            return extremePoints.ToArray();
        }

        public static Vector3[] FindOrigin(Vector3[] extremePoints)
        {
            float maxAbs = 0;
            var origin = new Vector3();
            var pointA = new Vector3();

            var pointB = new Vector3();
            foreach (var o in extremePoints)
            {
                foreach (var a in extremePoints)
                {
                    var oa = new Vector3(a.x - o.x, a.y - o.y, a.z - o.z);
                    foreach (var b in extremePoints)
                    {
                        var ob = new Vector3(b.x - o.x, b.y - o.y, b.z - o.z);

                        if (Math.Abs(Vector3.Cross(oa, ob).magnitude) > maxAbs)
                        {
                            maxAbs = Math.Abs(Vector3.Cross(oa, ob).magnitude);
                            origin = o;
                            pointA = a;
                            pointB = b;
                        }
                    }
                }
            }

            return new[]
            {
                origin, pointA, pointB
            };
        }

        public static float PseudoPointPlaneDistance(float d, float c, Vector3[] points, int currentPointIndex, int anchor, int floatPoint)
        {
            var wK = LonelinessIndex(points, currentPointIndex, anchor, floatPoint);
            return d * (c * wK + 1);
        }

        public static float LonelinessIndex(Vector3[] points, int currentPointIndex, int anchor, int floatPoint)
        {
            var denum = 0f;
            var num = Vector3.Distance(points[currentPointIndex], points[currentPointIndex + 1]) + 
                      Vector3.Distance(points[currentPointIndex], points[currentPointIndex - 1]);
            
            for (int i = anchor; i < floatPoint - 1; i++)
            {
                denum += Vector3.Distance(points[i], points[i + 1]);
            }

            return ((floatPoint - 1) * num) / (2 * denum);
        }
    }
}