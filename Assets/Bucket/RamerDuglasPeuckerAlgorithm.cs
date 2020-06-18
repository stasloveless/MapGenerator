using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class RamerDuglasPeickerAlgorithm
{
    public static Vector3[] OptimizeMap(Vector3[] heightMap, float e)
    {
        var optimizedHeightMap = new List<Vector3>();
        for (var j = 1; j < heightMap.GetLength(1) - 1; j++)
        {
            var currentRow = GetRow(heightMap, j);
            var changedRow = RamerDuglasPeucker(currentRow, e);

            for (var i = 0; i < changedRow.Length; i++)
            {
                optimizedHeightMap.Add(changedRow[i]);
            }
        }
        for (var i = 0; i < heightMap.GetLength(1); i++) optimizedHeightMap[i] = heightMap[i];
        for (var i = 0; i < heightMap.GetLength(1); i++) 
            optimizedHeightMap[i] = heightMap[i];

        return optimizedHeightMap.ToArray();
    }

    public static Vector2[] RamerDuglasPeucker(Vector2[] points, float e)
    {
        List<Vector2> keptPoints = new List<Vector2>();
        var startPoint = points[0];
        var endPoint =  points[points.Length - 1];
        var edgePoints = new[] {startPoint, endPoint};

        float maxDistance = 0;
        var indexOfMax = 0;

        for (var i = 1; i < points.Length - 1; i++)
        {
            var curDistance = PointToLineDistance(edgePoints, points[i]);
            if (curDistance > maxDistance)
            {
                maxDistance = curDistance;
                indexOfMax = i;
            }
        }

        if (maxDistance >= e)
        {
            var leftKeptPoints = RamerDuglasPeucker(GetArrayRange(points, 0, indexOfMax), e);
            var rightKeptPoints = RamerDuglasPeucker(GetArrayRange(points, indexOfMax, points.Length - 1), e);
            keptPoints.AddRange(leftKeptPoints);
            keptPoints.RemoveAt(keptPoints.Count - 1);
            keptPoints.AddRange(rightKeptPoints);
        }
        else
        {
            keptPoints.Add(points[0]);
            keptPoints.Add(points[points.Length - 1]);
        }

        //Assert.AreEqual(points.Length, keptPoints.Count);
        return keptPoints.ToArray();
    }

    public static Vector2[] GetRow(Vector3[] points, int z)
    {
        var row = new List<Vector2>();
        foreach (var point in points)
        {
            if (point.z.Equals(z))
            {
                row.Add(point);
            } 
        }

        return row.ToArray();
    }
    
    public static Vector2[] GetColumn(Vector3[] points, int x)
    {
        var column = new List<Vector2>();
        foreach (var point in points)
        {
            if (point.x.Equals(x))
            {
                column.Add(point);
            } 
        }
        
        return column.ToArray();
    }
    
    public static float PointToLineDistance(Vector2[] edgePoints, Vector2 currentPoint)
    {
        var a = Vector2.Distance(edgePoints[0], currentPoint);
        var b = Vector2.Distance(edgePoints[1], currentPoint);
        var c = Vector2.Distance(edgePoints[0], edgePoints[1]);

        var halfP = (a + c + b) / 2;
        var preSquare = halfP * (halfP - a) * (halfP - b) * (halfP - c);
        var square = (float) Math.Sqrt(preSquare);

        var distance = (2 * square) / c;

        return distance;
    }

    public static int FindMax(Vector2[] points)
    {
        var startPoint = points[0];
        var endPoint =  points[points.Length - 1];
        var edgePoints = new[] {startPoint, endPoint};

        float maxDistance = 0;
        var indexOfMax = 0;

        for (var i = 1; i < points.Length - 1; i++)
        {
            var curDistance = PointToLineDistance(edgePoints, points[i]);
            if (curDistance > maxDistance)
            {
                maxDistance = curDistance;
                indexOfMax = i;
            }
        }

        return indexOfMax;
    }

    public static Vector2[] GetArrayRange(Vector2[] array, int startIndex, int endIndex)
    {
        var result = new List<Vector2>();
        for (var i = startIndex; i <= endIndex; i++)
        {
            result.Add(array[i]);
        }

        return result.ToArray();
    }
}