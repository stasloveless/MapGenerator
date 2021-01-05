using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MathCalculations;
using OptimizationAlgorithms;
using RamerDuglasPeucker3D;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class IrregularTerrainMeshGenerator
{
    private static List<int> vertIdsOfTriangles;
    private static List<Vector3> vertices;
    private static List<Vector2> uvCoords;

    public static Mesh GenerateMesh(Vector3[] pointCloud, List<Triangle> triangulation)
    {
        vertIdsOfTriangles = new List<int>();
        vertices = new List<Vector3>();
        uvCoords = new List<Vector2>();
        Mesh newMesh = new Mesh();

        CreateVerts(pointCloud);
        newMesh.vertices = vertices.ToArray();

        CreateTris(pointCloud, triangulation, newMesh);

        //CreateUvCoords(meshWidth, meshHeight);
        //newMesh.uv = uvCoords.ToArray();

        return newMesh;
    }

    private static void CreateVerts(Vector3[] pointCloud)
    {
        foreach (var point in pointCloud)
        {
            vertices.Add(point);
        }
    }

    /*private static void CreateVerts(int width, int height, float[,] heightMap)
    {
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (heightMap[x, y] < 0) continue;
                vertices.Add(new Vector3(x, heightMap[x, y], y));
            }
        }
    }*/

    private static void CreateTris(Vector3[] pointCloud, List<Triangle> triangulation, Mesh mesh)
    {
        foreach (var tris in triangulation)
        {
            for (var i = 0; i < pointCloud.Length; i++)
            {
                if (tris.V0.x == pointCloud[i].x && tris.V0.y == pointCloud[i].z)
                {
                    vertIdsOfTriangles.Add(i);
                }
            }

            for (var i = 0; i < pointCloud.Length; i++)
            {
                if (tris.V1.x == pointCloud[i].x && tris.V1.y == pointCloud[i].z)
                {
                    vertIdsOfTriangles.Add(i);
                }
            }

            for (var i = 0; i < pointCloud.Length; i++)
            {
                if (tris.V2.x == pointCloud[i].x && tris.V2.y == pointCloud[i].z)
                {
                    vertIdsOfTriangles.Add(i);
                }
            }
        }

        mesh.triangles = vertIdsOfTriangles.ToArray();
        mesh.RecalculateNormals();
    }

/*private static void CreateTris(float[,] heightMap, Mesh mesh)
{
    int firstVertOfTris = 0;
    int v = 0;
    
    for (var i = 0; i < heightMap.GetLength(0) - 1; i++)
    {
        int firstLineVerticesCount = heightMap.GetLength(1) - Count(heightMap, i, -1);
        int secondLineVerticesCount = heightMap.GetLength(1) - Count(heightMap, i + 1, -1);
        
        if (firstLineVerticesCount == secondLineVerticesCount)
        {
            for (int x = 0; x < firstLineVerticesCount - 1; x++, v++)
            {
                vertIdsOfTriangles.Add(v);
                vertIdsOfTriangles.Add(v + firstLineVerticesCount);
                vertIdsOfTriangles.Add(v + firstLineVerticesCount + 1);

                vertIdsOfTriangles.Add(v + firstLineVerticesCount + 1);
                vertIdsOfTriangles.Add(v + 1);
                vertIdsOfTriangles.Add(v);

                firstVertOfTris += 6;
            }

            v++;
        }
        else
        {
            int min = Math.Min(firstLineVerticesCount, secondLineVerticesCount);

            for (var j = 0; j < min - 1; j++, v++)
            {
                vertIdsOfTriangles.Add(v);
                vertIdsOfTriangles.Add(v + firstLineVerticesCount);
                vertIdsOfTriangles.Add(v + firstLineVerticesCount + 1);

                vertIdsOfTriangles.Add(v + firstLineVerticesCount + 1);
                vertIdsOfTriangles.Add(v + 1);
                vertIdsOfTriangles.Add(v);
            }

            int remain = Math.Max(firstLineVerticesCount, secondLineVerticesCount) - min;

            if (firstLineVerticesCount > secondLineVerticesCount)
            {
                var point = v + firstLineVerticesCount;
                for (var j = 0; j < remain; j++, v++)
                {
                    vertIdsOfTriangles.Add(v);
                    vertIdsOfTriangles.Add(point);
                    vertIdsOfTriangles.Add(v + 1);
                }
            }
            else
            {
                for (var j = 0; j < remain; j++)
                {
                    vertIdsOfTriangles.Add(v);
                    vertIdsOfTriangles.Add(v + firstLineVerticesCount + j);
                    vertIdsOfTriangles.Add(v + firstLineVerticesCount + 1 + j);
                }
            }

            v++;
        }
    }
    
    mesh.triangles = vertIdsOfTriangles.ToArray();
    mesh.RecalculateNormals();
}*/

    private static int Count(float[,] array, int i, float number)
    {
        int count = 0;
        for (var j = 0;
            j < array.GetLength(1);
            j++)
        {
            if (Math.Abs(array[i, j] - number) < 0.0001f)
            {
                count++;
            }
        }

        return count;
    }

    private static void CreateUvCoords(int width, int height)
    {
        for (int i = 0, y = 0;
            y < height;
            y++)
        {
            for (int x = 0; x < width; x++, i++)
            {
                uvCoords.Add(new Vector2((float) x / width, (float) y / height));
            }
        }
    }
}