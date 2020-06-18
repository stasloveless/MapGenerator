using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using RamerDuglasPeucker3D;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class RamerDuglasPeuckerAlgorithmTests
    {
        [Test]
        public void TestFindOrigin()
        {
            var extremePoints = new[]
            {
                new Vector3(0, 1, 0),
                new Vector3(5, 3, 0),
                new Vector3(0, 2, 4),
                new Vector3(5, 2, 4)
            };

            var expected = new[]
            {
                new Vector3(0, 1, 0),
                new Vector3(5, 3, 0),
                new Vector3(0, 2, 4)
            };

            var result = RamerDuglasPeuckerAlgorithm.FindOrigin(extremePoints);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TestFindExtremes()
        {
            var pointCloud = new[]
            {
                new Vector3(0, 1, 0),
                new Vector3(5, 3, 0),
                new Vector3(1, 1, 1),
                new Vector3(1, 2, 3),
                new Vector3(5, 2, 4)
            };

            var expected = new[]
            {
                new Vector3(0, 1, 0),
                new Vector3(5, 3, 0),
                new Vector3(1, 1, 1),
                new Vector3(5, 2, 4)
            };

            var result = RamerDuglasPeuckerAlgorithm.FindExtremes(pointCloud);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TestFindCornerPoints()
        {
            const int mapSize = 4;
            Vector3[] pointCloud =
            {
                new Vector3(0, 1, 0), new Vector3(3, 1, 0), new Vector3(0, 1, 1), new Vector3(1, 1, 1),
                new Vector3(0, 1, 3), new Vector3(3, 1, 3)
            };

            Vector3[] planeVectors =
            {
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 1), 
                new Vector3(3, 1, 3)
            };

            var expected = new List<Vector3>
            {
                new Vector3(3, 1, 0),
                new Vector3(0, 1, 3),
            };

            var result = RamerDuglasPeuckerAlgorithm.FindCornerPoints(pointCloud, mapSize, planeVectors);
            
            Assert.AreEqual(expected, result);
        }
    }
}