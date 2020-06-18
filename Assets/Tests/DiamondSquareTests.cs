using System;
using System.Collections;
using System.Collections.Generic;
using GenerationAlgorithms;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class DiamondSquareTests
    {
        [Test]
        public void TestDiamondSquareInit()
        {
            var mapSize = 3;

            var result = DiamondSquareAlgorithm.DiamondSquareInit(mapSize);
            Tuple<int, int>[] expected = {new Tuple<int, int>(0,0), new Tuple<int, int>(2,0), new Tuple<int, int>(0,2), new Tuple<int, int>(2,2)};
            
            Assert.AreEqual(expected, result);
        }
        
        [Test]
        public void TestFindSquareMidPoint()
        {
            Tuple<int, int>[] points = {new Tuple<int, int>(0,0), new Tuple<int, int>(2,0), new Tuple<int, int>(0,2), new Tuple<int, int>(2,2)};

            var result = DiamondSquareAlgorithm.FindSquareMidPoint(points);
            var expected =  Tuple.Create(1, 1);
            
            Assert.AreEqual(expected, result);
        }
        
        [Test]
        public void TestFindDiamondsMidPoints()
        {
            Tuple<int, int>[] points = {new Tuple<int, int>(0,0), new Tuple<int, int>(4,0), new Tuple<int, int>(0,4), new Tuple<int, int>(4,4)};

            var result = DiamondSquareAlgorithm.FindDiamondsMidPoints(points);
            Tuple<int, int>[] expected =  {new Tuple<int, int>(2,0), new Tuple<int, int>(4,2), new Tuple<int, int>(0,2), new Tuple<int, int>(2,4)};
            
            Assert.AreEqual(expected, result);
        }
        
        [Test]
        public void TestFindSquareCorners()
        {
            var division = 2;
            var point = Tuple.Create(2, 2);

            var result = DiamondSquareAlgorithm.FindSquareCorners(division, point);
            Tuple<int, int>[] expected =  {new Tuple<int, int>(2,2), new Tuple<int, int>(4,2), new Tuple<int, int>(2,4), new Tuple<int, int>(4,4)};
            
            Assert.AreEqual(expected, result);
        }
        
        [Test]
        public void TestFindDiamondCornersFull()
        {
            var mapSize = 5;
            var division = 2;
            
            var point = Tuple.Create(1, 1);

            var result = DiamondSquareAlgorithm.FindDiamondCorners(division, point, mapSize);
            Tuple<int, int>[] expected =  {new Tuple<int, int>(1,0), new Tuple<int, int>(2,1), new Tuple<int, int>(0,1), new Tuple<int, int>(1,2)};
            
            Assert.AreEqual(expected, result);
        }
        
        [Test]
        public void TestFindDiamondCornersLeft()
        {
            var mapSize = 5;
            var division = 2;
            
            var point = Tuple.Create(0, 1);

            var result = DiamondSquareAlgorithm.FindDiamondCorners(division, point, mapSize);
            Tuple<int, int>[] expected =  {new Tuple<int, int>(0,0), new Tuple<int, int>(1,1), new Tuple<int, int>(0,2)};
            
            Assert.AreEqual(expected, result);
        }
        
        [Test]
        public void TestFindDiamondCornersRight()
        {
            var mapSize = 5;
            var division = 2;
            
            var point = Tuple.Create(mapSize - 1, 2);

            var result = DiamondSquareAlgorithm.FindDiamondCorners(division, point, mapSize);
            Tuple<int, int>[] expected =  {new Tuple<int, int>(4,1), new Tuple<int, int>(3,2), new Tuple<int, int>(4,3)};
            
            Assert.AreEqual(expected, result);
        }
        
        [Test]
        public void TestFindDiamondCornersUp()
        {
            var mapSize = 5;
            var division = 2;
            
            var point = Tuple.Create(1, 0);

            var result = DiamondSquareAlgorithm.FindDiamondCorners(division, point, mapSize);
            Tuple<int, int>[] expected =  {new Tuple<int, int>(2,0), new Tuple<int, int>(0,0), new Tuple<int, int>(1,1)};
            
            Assert.AreEqual(expected, result);
        }
        
        [Test]
        public void TestFindDiamondCornersDown()
        {
            var mapSize = 5;
            var division = 2;
            
            var point = Tuple.Create(1, mapSize - 1);

            var result = DiamondSquareAlgorithm.FindDiamondCorners(division, point, mapSize);
            Tuple<int, int>[] expected =  {new Tuple<int, int>(1,3), new Tuple<int, int>(2,4), new Tuple<int, int>(0,4)};
            
            Assert.AreEqual(expected, result);
        }
        
        [Test]
        public void TestFindMeanHeight()
        {
            Tuple<int, int>[] coords =  {new Tuple<int, int>(0,0), new Tuple<int, int>(0,1), new Tuple<int, int>(1,1), new Tuple<int, int>(1,0)};
            float[,] heights = {{1, 2} , {3, 4}};

            var mean = DiamondSquareAlgorithm.FindMeanHeight(coords, heights);
            var expected = 2.5f;
            
            Assert.AreEqual(expected, mean);
        }
    }
}
