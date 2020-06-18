using System;
using System.Collections.Generic;
using MathCalculations;
using NUnit.Framework;
using RamerDuglasPeucker3D;
using UnityEngine;

namespace Tests
{
    public class TriangleMethodsTests
    {
        [Test]
        public void TestCommonEdge()
        {
            Triangle firstTriangle = new Triangle(new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 3));
            Triangle secondTriangle = new Triangle(new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, -3));
            
            List<Vector2> expected = new List<Vector2>(){firstTriangle.V0, firstTriangle.V1};

            var result = firstTriangle.CommonEdge(secondTriangle);
            
            Assert.AreEqual(expected, result);
        }
    }
}