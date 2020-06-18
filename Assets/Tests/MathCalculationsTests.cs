using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class MathCalculationsTests
    {
        [Test]
        public void TestPointDistance2D()
        {
            var first = new Vector2(0, 0);
            var second = new Vector2(2, 2);

            var expected = 2.828427f;
            var result = MathCalculations.MathCalculations.PointDistance2D(first, second);

            Assert.AreEqual(expected, result, 0.000001f);
        }

        [Test]
        public void TestPointDistance3D()
        {
            var first = new Vector3(0, 0, 0);
            var second = new Vector3(2, 2, 2);

            var expected = 3.464101f;
            var result = MathCalculations.MathCalculations.PointDistance3D(first, second);

            Assert.AreEqual(expected, result, 0.000001f);
        }

        [Test]
        public void TestPointToLineDistance2D()
        {
            Vector2[] line = {new Vector2(0, 0), new Vector2(2, 2)};
            var point = new Vector2(2, 3);

            var expected = 0.707106f;
            var result = MathCalculations.MathCalculations.PointToLineDistance2D(line, point);

            Assert.AreEqual(expected, result, 0.000001f);
        }

        [Test]
        public void TestPointToLineDistance3D()
        {
            Vector3[] line = {new Vector3(0, 0, 0), new Vector3(2, 2, 2)};
            var point = new Vector3(2, 3, 4);

            var expected = 1.414214f;
            var result = MathCalculations.MathCalculations.PointToLineDistance3D(line, point);

            Assert.AreEqual(expected, result, 0.000001f);
        }

        [Test]
        public void TestCircumCircleR()
        {
            Vector2[] triangle = {new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 3)};

            var expected = 1.581139f;
            var result = MathCalculations.MathCalculations.CircumCircleR(triangle);

            Assert.AreEqual(expected, result, 0.000001f);
        }

        [Test]
        public void TestLineMid2D()
        {
            var first = new Vector2(0, 0);
            var second = new Vector2(3, 2);

            var expected = new Vector2(1.5f, 1);
            var result = MathCalculations.MathCalculations.LineMid2D(first, second);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TestMatrixDeterminant3D()
        {
            float[] firstRow = {0, 1, 1};
            float[] secondRow = {2, 0, 3};
            float[] thirdRow = {5, 7, 0};

            var expected = 29f;
            var result = MathCalculations.MathCalculations.MatrixDeterminant3D(firstRow, secondRow, thirdRow);

            Assert.AreEqual(expected, result, 0.1f);
        }
    }
}