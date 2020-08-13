using System;
using System.Collections.Generic;
using UnityEngine;

namespace RamerDuglasPeucker3D
{
    //Class implements the majority version of the Ramer-Duglas-Peucker algorithm
    //Algorithm is calculated first for rows and then for columns
    //For each point are calculated two statuses: rowStatus and columnStatus
    //If point passed the column or the row check - corresponding status is true
    //If point has both positive status - it is included in the final optimization
    public class RamerDuglasPeickerAlgorithm2D
    {
        //This function creates optimized point array based on received height map and epsilon (e) parameter
        //Returns Vector3 array of the points
        public static Vector3[] OptimizeMap(Vector3[] heightMap, float e, int mapSize)
        {
            var processedPointHeightMap = new List<ProcessedPoint>();
            foreach (var point in heightMap)
            {
                processedPointHeightMap.Add(new ProcessedPoint(point));
            }

            RamerDuglasPeuckerRow(processedPointHeightMap, mapSize, e);
            RamerDuglasPeuckerColumn(processedPointHeightMap, mapSize, e);

            var optimizedHeightMap = SelectKeptPoints(processedPointHeightMap);

            return optimizedHeightMap;
        }

        //This function checks statuses of the points
        //Returns array of the kept points
        private static Vector3[] SelectKeptPoints(List<ProcessedPoint> processedPointHeightMap)
        {
            var keptPoints = new List<Vector3>();
            foreach (var processedPoint in processedPointHeightMap)
            {
                if (processedPoint.columnStatus && processedPoint.rowStatus)
                {
                    keptPoints.Add(processedPoint.coordinates);
                }
            }

            return keptPoints.ToArray();
        }

        //This function calls RamerDuglasPeucker function for columns
        //Sets columnStatus for each point
        private static void RamerDuglasPeuckerColumn(List<ProcessedPoint> processedPointHeightMap, int mapSize, float e)
        {
            for (var i = 0; i < mapSize; i++)
            {
                var currentColumn = GetColumn(processedPointHeightMap, i);
                RamerDuglasPeucker(currentColumn, e);
            }

            foreach (var point in processedPointHeightMap)
            {
                if (point.intermediateStatus)
                {
                    point.intermediateStatus = false;
                    point.columnStatus = true;
                }
            }
        }
        
        //This function calls RamerDuglasPeucker function for rows
        //Sets rowStatus for each point
        private static void RamerDuglasPeuckerRow(List<ProcessedPoint> processedPointHeightMap, int mapSize, float e)
        {
            for (var i = 0; i < mapSize; i++)
            {
                var currentColumn = GetRow(processedPointHeightMap, i);
                RamerDuglasPeucker(currentColumn, e);
            }

            foreach (var point in processedPointHeightMap)
            {
                if (point.intermediateStatus)
                {
                    point.intermediateStatus = false;
                    point.rowStatus = true;
                }
            }
        }

        //This is Ramer-Duglas-Peucker recursive implementation for line of the points
        public static void RamerDuglasPeucker(List<ProcessedPoint> pointsLine, float e)
        {
            var pointsToProcess = pointsLine.ToArray();

            var startPoint = pointsToProcess[0];
            var endPoint = pointsLine[pointsToProcess.Length - 1];
            var edgePoints = new[] {startPoint, endPoint};

            float maxDistance = 0;
            var indexOfMax = 0;

            for (var i = 1; i < pointsToProcess.Length - 1; i++)
            {
                var curDistance = PointToLineDistance(edgePoints, pointsToProcess[i]);
                if (curDistance > maxDistance)
                {
                    maxDistance = curDistance;
                    indexOfMax = i;
                }
            }
            
            if (maxDistance > e)
            {
                RamerDuglasPeucker(GetArrayRange(pointsToProcess, 0, indexOfMax), e);
                RamerDuglasPeucker(GetArrayRange(pointsToProcess, indexOfMax, pointsToProcess.Length - 1), e);
            }
            else
            {
                pointsToProcess[0].intermediateStatus = true;
                pointsToProcess[pointsToProcess.Length - 1].intermediateStatus = true;
            }
        }

        public static List<ProcessedPoint> GetRow(List<ProcessedPoint> points, int z)
        {
            var row = new List<ProcessedPoint>();
            foreach (var point in points)
            {
                if (point.coordinates.z.Equals(z))
                {
                    row.Add(point);
                }
            }

            return row;
        }

        public static List<ProcessedPoint> GetColumn(List<ProcessedPoint> points, int x)
        {
            var column = new List<ProcessedPoint>();
            foreach (var point in points)
            {
                if (point.coordinates.x.Equals(x))
                {
                    column.Add(point);
                }
            }

            return column;
        }

        public static float PointToLineDistance(ProcessedPoint[] edgePoints, ProcessedPoint currentPoint)
        {
            /////
            var a = Vector3.Distance(edgePoints[0].coordinates, currentPoint.coordinates);
            var b = Vector3.Distance(edgePoints[1].coordinates, currentPoint.coordinates);
            var c = Vector3.Distance(edgePoints[0].coordinates, edgePoints[1].coordinates);

            var halfP = (a + c + b) / 2;
            var preSquare = halfP * (halfP - a) * (halfP - b) * (halfP - c);
            var square = (float) Math.Sqrt(preSquare);

            var distance = (2 * square) / c;

            return distance;
        }

        public static List<ProcessedPoint> GetArrayRange(ProcessedPoint[] array, int startIndex, int endIndex)
        {
            var result = new List<ProcessedPoint>();
            for (var i = startIndex; i <= endIndex; i++)
            {
                result.Add(array[i]);
            }

            return result;
        }
    }
}