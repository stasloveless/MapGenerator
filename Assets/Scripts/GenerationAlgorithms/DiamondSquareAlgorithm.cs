using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace GenerationAlgorithms
{
    public static class DiamondSquareAlgorithm
    {
        public static Vector3[] GenerateDiamondSquareMap(int mapSize, float roughness, int seed, float heightMultiplier, AnimationCurve heightCurve)
        {
            //TODO:проверка на квадратность и четность
            Random rnd = new Random(seed);
            
            var noiseMap = new float[mapSize, mapSize];

            //от максимального к единице, максимальное значение - это шаг инициализации, поэтому здесь сразу берется след значение
            var currentDivision = (mapSize - 1);
            var iterationNum = 1;

            var initPoints = DiamondSquareInit(mapSize);

            //записали высоты угловых точек шага инициализации
            foreach (var initPoint in initPoints)
            {
                //noiseMap.Add(new Vector3(initPoint.x, NextFloat(), initPoint.y));
                noiseMap[initPoint.Item1, initPoint.Item2] = NextFloat(rnd);
            }

            while (currentDivision % 2 == 0)
            {
                var squareMidPoints = new List<Tuple<int, int>>();
                for (var i = 0; i < mapSize - 1; i += currentDivision)
                {
                    for (var j = 0; j < mapSize - 1; j += currentDivision)
                    {
                        //Square
                        var squarePoints = FindSquareCorners(currentDivision, Tuple.Create(i, j));
                        var squareMidPoint = FindSquareMidPoint(squarePoints);
                        noiseMap[squareMidPoint.Item1, squareMidPoint.Item2] =
                            FindMeanHeight(squarePoints, noiseMap) + NextFloat(roughness, iterationNum, rnd);
                        //сложили все серединные точки квадратов в лист для шага diamond
                        squareMidPoints.Add(squareMidPoint);
                    }
                }

                foreach (var sPoint in squareMidPoints)
                {
                    //Diamond
                    var diamondsMidPoints = FindDiamondsMidPoints(sPoint, currentDivision);

                    foreach (var dPoint in diamondsMidPoints)
                    {
                        noiseMap[dPoint.Item1, dPoint.Item2] =
                            FindMeanHeight(FindDiamondCorners(currentDivision, dPoint, mapSize), noiseMap) +
                            NextFloat(roughness, iterationNum, rnd);
                    }
                }

                currentDivision /= 2;
                iterationNum++;
            }

            var noiseMapVector3 = new List<Vector3>();
            for (var i = 0; i < noiseMap.GetLength(0); i++)
            {
                for (var j = 0; j < noiseMap.GetLength(1); j++)
                {
                    noiseMapVector3.Add(new Vector3(j, noiseMap[j, i], i));
                }
            }

            var finalNoiseMap = noiseMapVector3.ToArray();
            
            for (var i = 0; i < finalNoiseMap.Length; i++)
            {
                finalNoiseMap[i] = Utils.ChangeY(finalNoiseMap[i], heightCurve.Evaluate(finalNoiseMap[i].y) * heightMultiplier);
            }
            
            return finalNoiseMap;
        }

        public static Tuple<int, int>[] DiamondSquareInit(int mapSize)
        {
            var v0 = Tuple.Create(0, 0);
            var v1 = Tuple.Create(mapSize - 1, 0);
            var v2 = Tuple.Create(0, mapSize - 1);
            var v3 = Tuple.Create(mapSize - 1, mapSize - 1);

            return new[] {v0, v1, v2, v3};
        }

        public static Tuple<int, int>[] FindSquareCorners(int currentDivision, Tuple<int, int> currentPoint)
        {
            var v0 = Tuple.Create(currentPoint.Item1, currentPoint.Item2);
            var v1 = Tuple.Create(currentPoint.Item1 + currentDivision, currentPoint.Item2);
            var v2 = Tuple.Create(currentPoint.Item1, currentPoint.Item2 + currentDivision);
            var v3 = Tuple.Create(currentPoint.Item1 + currentDivision, currentPoint.Item2 + currentDivision);

            return new[] {v0, v1, v2, v3};
        }

        public static Tuple<int, int>[] FindDiamondCorners(int currentDivision, Tuple<int, int> currentPoint,
            int mapSize)
        {
            Tuple<int, int> vRight, vLeft, vUp, vDown;

            if (currentPoint.Item1 == 0)
            {
                vUp = Tuple.Create(currentPoint.Item1, currentPoint.Item2 - currentDivision / 2);
                vRight = Tuple.Create(currentPoint.Item1 + currentDivision / 2, currentPoint.Item2);
                vDown = Tuple.Create(currentPoint.Item1, currentPoint.Item2 + currentDivision / 2);

                return new[] {vUp, vRight, vDown};
            }
            else if (currentPoint.Item2 == 0)
            {
                vRight = Tuple.Create(currentPoint.Item1 + currentDivision / 2, currentPoint.Item2);
                vLeft = Tuple.Create(currentPoint.Item1 - currentDivision / 2, currentPoint.Item2);
                vDown = Tuple.Create(currentPoint.Item1, currentPoint.Item2 + currentDivision / 2);

                return new[] {vRight, vLeft, vDown};
            }
            else if (currentPoint.Item1 == mapSize - 1)
            {
                vUp = Tuple.Create(currentPoint.Item1, currentPoint.Item2 - currentDivision / 2);
                vLeft = Tuple.Create(currentPoint.Item1 - currentDivision / 2, currentPoint.Item2);
                vDown = Tuple.Create(currentPoint.Item1, currentPoint.Item2 + currentDivision / 2);

                return new[] {vUp, vLeft, vDown};
            }
            else if (currentPoint.Item2 == mapSize - 1)
            {
                vUp = Tuple.Create(currentPoint.Item1, currentPoint.Item2 - currentDivision / 2);
                vRight = Tuple.Create(currentPoint.Item1 + currentDivision / 2, currentPoint.Item2);
                vLeft = Tuple.Create(currentPoint.Item1 - currentDivision / 2, currentPoint.Item2);
                return new[] {vUp, vRight, vLeft};
            }

            vUp = Tuple.Create(currentPoint.Item1, currentPoint.Item2 - currentDivision / 2);
            vRight = Tuple.Create(currentPoint.Item1 + currentDivision / 2, currentPoint.Item2);
            vLeft = Tuple.Create(currentPoint.Item1 - currentDivision / 2, currentPoint.Item2);
            vDown = Tuple.Create(currentPoint.Item1, currentPoint.Item2 + currentDivision / 2);

            return new[] {vUp, vRight, vLeft, vDown};
        }


        public static Tuple<int, int> FindSquareMidPoint(Tuple<int, int>[] cornerPoints)
        {
            var firstPoint = cornerPoints[0];
            var secondPoint = cornerPoints.Last();

            return new Tuple<int, int>((secondPoint.Item1 + firstPoint.Item1) / 2,
                (secondPoint.Item2 + firstPoint.Item2) / 2);
        }

        public static Tuple<int, int>[] FindDiamondsMidPoints(Tuple<int, int>[] cornerPoints)
        {
            var vUp = Tuple.Create((cornerPoints[1].Item1 + cornerPoints[0].Item1) / 2,
                (cornerPoints[1].Item2 + cornerPoints[0].Item2) / 2);
            var vRight = Tuple.Create((cornerPoints[3].Item1 + cornerPoints[1].Item1) / 2,
                (cornerPoints[3].Item2 + cornerPoints[1].Item2) / 2);
            var vLeft = Tuple.Create((cornerPoints[2].Item1 + cornerPoints[0].Item1) / 2,
                (cornerPoints[2].Item2 + cornerPoints[0].Item2) / 2);
            var vDown = Tuple.Create((cornerPoints[3].Item1 + cornerPoints[2].Item1) / 2,
                (cornerPoints[3].Item2 + cornerPoints[2].Item2) / 2);

            return new[] {vUp, vRight, vLeft, vDown};
        }

        public static Tuple<int, int>[] FindDiamondsMidPoints(Tuple<int, int> squareMidPoint, int currentDivision)
        {
            var vUp = Tuple.Create(squareMidPoint.Item1, squareMidPoint.Item2 - currentDivision / 2);
            var vRight = Tuple.Create(squareMidPoint.Item1 + currentDivision / 2, squareMidPoint.Item2);
            var vLeft = Tuple.Create(squareMidPoint.Item1 - currentDivision / 2, squareMidPoint.Item2);
            var vDown = Tuple.Create(squareMidPoint.Item1, squareMidPoint.Item2 + currentDivision / 2);

            return new[] {vUp, vRight, vLeft, vDown};
        }

        public static float FindMeanHeight(Tuple<int, int>[] pointsCoords, float[,] heights)
        {
            float preMean = 0;

            foreach (var (x, y) in pointsCoords)
            {
                preMean += heights[x, y];
            }

            return preMean / pointsCoords.Length;
        }

        private static float NextFloat(float a, int power, Random rnd)
        {
            var res = Math.Pow(a, power);
            var randNum = rnd.NextDouble();
            return (float) (randNum * 2 * res - res);
        }

        private static float NextFloat(Random rnd)
        {
            return (float) rnd.NextDouble();
        }
    }
}