using System.Collections.Generic;
using System.Linq;
using DelaunayTriangulator;
using OptimizationAlgorithms;
using RamerDuglasPeucker3D;
using UnityEngine;
using UnityEngine.Assertions;

namespace Bucket
{
    public class DelaunayTests
    {
        public void TestSuperStructureInit()
        {
            var mapSize = 3;
            Vector3[] pointCloud =
            {
                new Vector3(0, 1, 0), new Vector3(2, 1, 0), new Vector3(0, 1, 1), new Vector3(1, 1, 1),
                new Vector3(0, 1, 2), new Vector3(1, 1, 2), new Vector3(2, 1, 2)
            };

            var expected = new List<Vector2>()
            {
                new Vector2(0, 1), new Vector2(0, 2), new Vector2(1, 2),
                new Vector2(2, 2), new Vector2(2, 0), new Vector2(0, 0)
            };

            var result = DelaunayAlgorithm.SuperStructureInit(pointCloud, mapSize);

            Assert.AreEqual(expected, result);
        }

        
        public void TestTriangulateFragment()
        {
            var ngonPoints = new List<Vector2>()
            {
                new Vector2(0, 1), new Vector2(0, 2), new Vector2(1, 2),
                new Vector2(2, 2), new Vector2(2, 0), new Vector2(0, 0)
            };

            var point = new Vector2(1, 1);

            var expected = new List<Triangle>()
            {
                new Triangle(point, ngonPoints[0], ngonPoints[1]),
                new Triangle(point, ngonPoints[1], ngonPoints[2]),
                new Triangle(point, ngonPoints[2], ngonPoints[3]),
                new Triangle(point, ngonPoints[3], ngonPoints[4]),
                new Triangle(point, ngonPoints[4], ngonPoints[5]),
                new Triangle(point, ngonPoints[5], ngonPoints[0])
            };

            var result = DelaunayAlgorithm.TriangulateFragment(ngonPoints, point);

            Assert.AreEqual(expected, result);
        }

        
        public void TestNodeInCircle()
        {
            var triangle = new Triangle(new Vector2(2, 1), new Vector2(1, 0), new Vector2(0, 0));
            var point = new Vector2(1, 3);

            var expected = false;
            var result = DelaunayAlgorithm.NodeInCircle(triangle, point);

            Assert.AreEqual(expected, result);
        }

        
        public void TestVertsFromTriangles()
        {
            var triangles = new List<Triangle>()
            {
                new Triangle(new Vector2(1, 1), new Vector2(0, 2), new Vector2(1, 2)),
                new Triangle(new Vector2(1, 1), new Vector2(0, 1), new Vector2(0, 2)),
                new Triangle(new Vector2(1, 1), new Vector2(2, 2), new Vector2(1, 2))
            };

            var expected = new List<Vector2>()
            {
                new Vector2(1, 1),
                new Vector2(0, 2),
                new Vector2(1, 2),
                new Vector2(0, 1),
                new Vector2(2, 2)
            };

            var result = DelaunayAlgorithm.VertsFromTriangles(triangles);

            Assert.AreEqual(expected, result);
        }

        
        public void TestFindNgon()
        {
            var point = new Vector2(2, 2);
            var triangulation = new List<Triangle>()
            {
                new Triangle(new Vector2(1, 1), new Vector2(0, 0), new Vector2(0, 1)),
                new Triangle(new Vector2(1, 1), new Vector2(0, 1), new Vector2(0, 3)),
                new Triangle(new Vector2(1, 1), new Vector2(0, 3), new Vector2(3, 3)),
                new Triangle(new Vector2(1, 1), new Vector2(3, 3), new Vector2(3, 0)),
                new Triangle(new Vector2(1, 1), new Vector2(3, 0), new Vector2(0, 0))
            };

            var expected = new List<Vector2>()
            {
                new Vector2(0, 3),
                new Vector2(1, 1),
                new Vector2(3, 3),
                new Vector2(3, 0)
            };

            var result = DelaunayAlgorithm.FindNgon(point, triangulation);

            Assert.AreEqual(expected, result);
        }
        
        
        public void TestFindNgon_RealData()
        {
            var point = new Vector2(1, 3);
            var triangulation = new List<Triangle>()
            {
                new Triangle(new Vector2(2, 1), new Vector2(0, 1), new Vector2(0, 2)),
                new Triangle(new Vector2(2, 1), new Vector2(0, 2), new Vector2(0, 3)),
                new Triangle(new Vector2(2, 1), new Vector2(0, 3), new Vector2(0, 4)),
                new Triangle(new Vector2(2, 1), new Vector2(0, 4), new Vector2(1, 4)),
                new Triangle(new Vector2(2, 1), new Vector2(1, 4), new Vector2(4, 4)),
                new Triangle(new Vector2(2, 1), new Vector2(4, 4), new Vector2(4, 0)),
                new Triangle(new Vector2(2, 1), new Vector2(4, 0), new Vector2(1, 0)),
                new Triangle(new Vector2(2, 1), new Vector2(1, 0), new Vector2(0, 0)),
                new Triangle(new Vector2(2, 1), new Vector2(0, 0), new Vector2(0, 1)),
            };
            
            var expected = new List<Vector2>()
            {
                new Vector2(0, 2), new Vector2(0, 3), new Vector2(0, 4),
                new Vector2(1, 4), new Vector2(4, 4)
            };
            
            var result = DelaunayAlgorithm.FindNgon(point, triangulation);

            Assert.AreEqual(expected, result);
        }
        
        
        public void TestNgonIntersection()
        {
            var ngonPoints = new List<Vector2>()
            {
                new Vector2(2, 2), new Vector2(2, 0), new Vector2(0, 1),
                new Vector2(0, 2), new Vector2(1, 2), new Vector2(0, 0)
            };

            var expected = new List<Vector2>
            {
                new Vector2(0, 0),
                new Vector2(2, 2)
            };

            var result = DelaunayAlgorithm.NgonIntersection(ngonPoints);

            Assert.AreEqual(expected, result);
        }
        
        
        public void TestNgonIntersection_RealData()
        {
            var ngonPoints = new List<Vector2>()
            {
                new Vector2(4, 3), new Vector2(0, 4), new Vector2(0, 8),
                new Vector2(1, 8), new Vector2(8, 8)
            };

            var expected = new List<Vector2>
            {
                new Vector2(0, 4),
                new Vector2(8, 8)
            };

            var result = DelaunayAlgorithm.NgonIntersection(ngonPoints);

            Assert.AreEqual(expected, result);
        }

        
        public void TestSortNgonPoints()
        {
            var ngonPoints = new List<Vector2>()
            {
                new Vector2(2, 2), new Vector2(2, 0), new Vector2(0, 1),
                new Vector2(0, 2), new Vector2(1, 2), new Vector2(0, 0)
            };

            var expected = new List<Vector2>
            {
                new Vector2(0, 1), new Vector2(0, 2), new Vector2(1, 2),
                new Vector2(2, 2), new Vector2(2, 0), new Vector2(0, 0)
            };
            
            var result = DelaunayAlgorithm.SortNgonPoints(ngonPoints);
            
            Assert.AreEqual(expected, result);
        }
        
        
        public void TestSortNgonPoints_RealData()
        {
            var ngonPoints = new List<Vector2>()
            {
                new Vector2(4, 3), new Vector2(0, 4), new Vector2(0, 8),
                new Vector2(1, 8), new Vector2(8, 8)
            };

            var expected = new List<Vector2>
            {
                new Vector2(4, 3), new Vector2(8, 8), new Vector2(1, 8),
                new Vector2(0, 8), new Vector2(0, 4)
            };
            
            var result = DelaunayAlgorithm.SortNgonPoints(ngonPoints);
            
            foreach (var res in result)
            {
                Debug.Log(res);
            }
            
            Assert.AreEqual(expected, result);
        }

        
        public void TestGenerateDelaunay()
        {
            var mapSize = 4;
            Vector3[] pointCloud =
            {
                new Vector3(0, 1, 0), new Vector3(3, 1, 0), new Vector3(0, 1, 1), new Vector3(1, 1, 1),
                new Vector3(0, 1, 3), new Vector3(3, 1, 3)
            };

            var expected = new List<Triangle>()
            {
                new Triangle(new Vector2(1, 1), new Vector2(0, 1), new Vector2(0, 3)),
                new Triangle(new Vector2(1, 1), new Vector2(0, 3), new Vector2(3, 3)),
                new Triangle(new Vector2(1, 1), new Vector2(3, 3), new Vector2(3, 0)),
                new Triangle(new Vector2(1, 1), new Vector2(3, 0), new Vector2(0, 0)),
                new Triangle(new Vector2(1, 1), new Vector2(0, 0), new Vector2(0, 1))
            };

            var result = DelaunayAlgorithm.GenerateDelaunay(pointCloud, mapSize);

            Assert.AreEqual(expected, result);
        }

        
        public void TestMakeClockwise()
        {
            Vector2[] pointCloud =
            {
                new Vector2(0, 0), new Vector2(2,  0), new Vector2(0,  1), new Vector2(1, 4),
                new Vector2(0, 3), new Vector2(3, 3)
            };
            
            var tris = new Triad(3, 0, 1);
            tris.MakeClockwise(pointCloud.ToList());
            var expected = new Triad(3, 1, 0);
            
            Assert.AreEqual(expected, tris);
        }
    }
}