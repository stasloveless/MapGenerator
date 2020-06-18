﻿using System;
using System.Collections.Generic;
using System.Linq;
 using RamerDuglasPeucker3D;
 using UnityEngine;

namespace OptimizationAlgorithms
{
    public static class DelaunayAlgorithm
    {
        public static List<Triangle> GenerateDelaunay(Vector3[] pointCloud, int mapSize)
        {
            var superStructure = SuperStructureInit(pointCloud, mapSize);
            var allNodes = ExtractXZToIntVector2(pointCloud);
            var innerNodes = new List<Vector2>(allNodes);

            /*foreach (var node in superStructure)
            {
                innerNodes.Remove(node);
            }*/

            //Инициализация: построение самой первой триангуляции
            var triangulation = new List<Triangle>();
            triangulation.AddRange(TriangulateFragment(superStructure, innerNodes[0]));

            for (var i = 1; i < innerNodes.Count; i++)
            {
                var currentNgon = FindNgon(innerNodes[i], triangulation);
                triangulation.AddRange(TriangulateFragment(currentNgon, innerNodes[i]));
            }

            var finalTriangulation = new List<Triangle>(triangulation);
            foreach (var tris in triangulation)
            {
                if (tris.V0.Equals(superStructure[0]) || tris.V1.Equals(superStructure[0]) ||
                    tris.V2.Equals(superStructure[0]))
                {
                    finalTriangulation.Remove(tris);
                }
                else if (tris.V0.Equals(superStructure[1]) || tris.V1.Equals(superStructure[1]) ||
                         tris.V2.Equals(superStructure[1]))
                {
                    finalTriangulation.Remove(tris);
                }
                else if (tris.V0.Equals(superStructure[2]) || tris.V1.Equals(superStructure[2]) ||
                         tris.V2.Equals(superStructure[2]))
                {
                    finalTriangulation.Remove(tris);
                }
                else if (tris.V0.Equals(superStructure[3]) || tris.V1.Equals(superStructure[3]) ||
                         tris.V2.Equals(superStructure[3]))
                {
                    finalTriangulation.Remove(tris);
                }
            }

            return finalTriangulation;
        }

        public static List<Vector2> FindNgon(Vector2 innerNode, List<Triangle> triangulation)
        {
            var trianglesForDelete = new List<Triangle>();

            foreach (var triangle in triangulation)
            {
                if (NodeInCircle(triangle, innerNode))
                {
                    trianglesForDelete.Add(triangle);
                }
            }

            foreach (var triangle in trianglesForDelete)
            {
                triangulation.Remove(triangle);
            }

            var trisVerts = VertsFromTriangles(trianglesForDelete);
            var sortedVerts = SortNgonPoints(trisVerts);

            return sortedVerts;
        }

        public static List<Vector2> VertsFromTriangles(List<Triangle> trianglesForDelete)
        {
            var allPoints = new List<Vector2>();

            foreach (var triangle in trianglesForDelete)
            {
                allPoints.AddRange(triangle.Tris2PointList());
            }

            return allPoints.Distinct().ToList();
        }

        public static bool NodeInCircle(Triangle triangle, Vector2 innerNode)
        {
            var r = MathCalculations.MathCalculations.CircumCircleR(triangle.V0, triangle.V1, triangle.V2);
            var a = MathCalculations.MathCalculations.MatrixDeterminant3D(
                new[] {triangle.V0.x, triangle.V0.y, 1},
                new[] {triangle.V1.x, triangle.V1.y, 1},
                new[] {triangle.V2.x, triangle.V2.y, 1});

            var b = MathCalculations.MathCalculations.MatrixDeterminant3D(
                new[]
                {
                    (int) Math.Pow(triangle.V0.x, 2) + (int) Math.Pow(triangle.V0.y, 2), triangle.V0.y, 1
                },
                new[]
                {
                    (int) Math.Pow(triangle.V1.x, 2) + (int) Math.Pow(triangle.V1.y, 2), triangle.V1.y, 1
                },
                new[]
                {
                    (int) Math.Pow(triangle.V2.x, 2) + (int) Math.Pow(triangle.V2.y, 2), triangle.V2.y, 1
                });

            var c = MathCalculations.MathCalculations.MatrixDeterminant3D(
                new[]
                {
                    (int) Math.Pow(triangle.V0.x, 2) + (int) Math.Pow(triangle.V0.y, 2), triangle.V0.x, 1
                },
                new[]
                {
                    (int) Math.Pow(triangle.V1.x, 2) + (int) Math.Pow(triangle.V1.y, 2), triangle.V1.x, 1
                },
                new[]
                {
                    (int) Math.Pow(triangle.V2.x, 2) + (int) Math.Pow(triangle.V2.y, 2), triangle.V2.x, 1
                });

            var d = MathCalculations.MathCalculations.MatrixDeterminant3D(
                new[]
                {
                    (int) Math.Pow(triangle.V0.x, 2) + (int) Math.Pow(triangle.V0.y, 2), triangle.V0.x,
                    triangle.V0.y
                },
                new[]
                {
                    (int) Math.Pow(triangle.V1.x, 2) + (int) Math.Pow(triangle.V1.y, 2), triangle.V1.x,
                    triangle.V1.y
                },
                new[]
                {
                    (int) Math.Pow(triangle.V2.x, 2) + (int) Math.Pow(triangle.V2.y, 2), triangle.V2.x,
                    triangle.V2.y
                });

            var circleCenter = new Vector2(b / (2 * a), -c / (2 * a));

            return !(Math.Pow(innerNode.x - circleCenter.x, 2) + Math.Pow(innerNode.y - circleCenter.y, 2) >=
                     Math.Pow(r, 2));
        }

        public static List<Vector2> SuperStructureInit(Vector3[] pointCloud, int mapSize)
        {
            var resultList = new List<Vector2>();

            /*foreach (var point in pointCloud)
            {
                if (point.x.Equals(0) || point.x.Equals( mapSize - 1))
                {
                    resultList.Add(new Vector2(point.x, point.z));
                }
                else if ( point.z.Equals(0) || point.z.Equals(mapSize - 1))
                {
                    resultList.Add(new Vector2(point.x, point.z));
                }
            }*/
            
            resultList.Add(new Vector2(-1, -1));
            resultList.Add(new Vector2(-1, mapSize));
            resultList.Add(new Vector2(mapSize, mapSize));
            resultList.Add(new Vector2(mapSize, -1));

            return SortNgonPoints(resultList);
        }

        public static List<Vector2> ExtractXZToIntVector2(Vector3[] array)
        {
            return array.Select(element => new Vector2(element.x, element.z)).ToList();
        }

        public static List<Triangle> TriangulateFragment(List<Vector2> ngonPoints,
            Vector2 newNode)
        {
            var resultList = new List<Triangle>();

            for (var i = 0; i < ngonPoints.Count - 1; i++)
            {
                if ((int) ngonPoints[0].y == 0)
                {
                    resultList.Add(new Triangle(newNode, ngonPoints[i], ngonPoints[i + 1]));
                }

                resultList.Add(new Triangle(newNode, ngonPoints[i], ngonPoints[i + 1]));
            }

            resultList.Add(new Triangle(newNode, ngonPoints[ngonPoints.Count - 1], ngonPoints[0]));

            return resultList;
        }

        public static List<Vector2> NgonIntersection(List<Vector2> ngon)
        {
            var intersectPoints = new List<Vector2>();
            ngon.Sort((a, b) =>
            {
                if ((int) a.x != (int) b.x) return a.x.CompareTo(b.x);
                return a.y.CompareTo(b.y);
            });

            intersectPoints.Add(ngon[0]);
            intersectPoints.Add(ngon[ngon.Count - 1]);

            return intersectPoints;
        }

        public static List<Vector2> SortNgonPoints(List<Vector2> ngon)
        {
            var upHalf = new List<Vector2>();
            var downHalf = new List<Vector2>();

            var intersectPoints = NgonIntersection(ngon);

            foreach (var point in ngon)
            {
                var comp = MathCalculations.MathCalculations.PointLocationRelativeToLine(intersectPoints[0],
                    intersectPoints[1], point).CompareTo(0);
                if (comp == 1)
                {
                    upHalf.Add(point);
                }
                else
                {
                    downHalf.Add(point);
                }
            }

            var sortedUpHalf = upHalf.OrderBy(vector => Vector2.Distance(intersectPoints[0], vector));
            var sortedDownHalf = downHalf.OrderBy(vector => Vector2.Distance(intersectPoints[1], vector));

            var resultList = new List<Vector2>(sortedUpHalf);
            resultList.AddRange(sortedDownHalf);

            return resultList;
        }
    }
}