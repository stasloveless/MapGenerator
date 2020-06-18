using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace Generator
{
    public class TerrainMeshGenerator
    {
        private static int[] vertIdsOfTriangles;
        private static List<Vector3> vertices;
        private static List<Vector2> uvCoords;

        public static Mesh Generate(int meshSize, Vector3[] heightMap, float heightMultiplier, int lod)
        {
            var detailIncrement = lod == 0 ? 1 : lod * 2;
            var verticesPerLine = (meshSize - 1) / detailIncrement + 1;

            vertIdsOfTriangles = new int[(verticesPerLine - 1) * (verticesPerLine - 1) * 6];
            vertices = new List<Vector3>();
            uvCoords = new List<Vector2>();
            var newMesh = new Mesh();

            CreateVerts(meshSize, heightMap, heightMultiplier, detailIncrement, verticesPerLine);
            newMesh.vertices = vertices.ToArray();

            CreateTris(verticesPerLine);
            newMesh.triangles = vertIdsOfTriangles;
            newMesh.RecalculateNormals();

            CreateUvCoords(meshSize, detailIncrement);
            newMesh.uv = uvCoords.ToArray();

            return newMesh;
        }

        private static void CreateVerts(int meshSize, Vector3[] heightMap, float heightMultiplier, int detailIncrement, int verticesPerLine)
        {
            /*for (var y = 0; y < meshSize; y += detailIncrement)
            {
                for (var x = 0; x < meshSize; x += detailIncrement)
                {
                    vertices[currentVertIndex] =
                        new Vector3(x, heightMap[x, y] * heightMultiplier, y);
                    currentVertIndex++;
                }
            }*/

            for (var i = 0; i < heightMap.Length; i += 1 + meshSize * (detailIncrement - 1))
            {
                for (var j = 0; j < verticesPerLine; j++)
                {
                    vertices.Add(new Vector3(heightMap[i].x, heightMap[i].y * heightMultiplier, heightMap[i].z));
                    i += detailIncrement;
                }

                i -= detailIncrement;
            }
        }

        private static void CreateTris(int verticesPerLine)
        {
            var firstVertOfTris = 0;
            var v = 0;

            for (var y = 0; y < verticesPerLine - 1; y++, v++)
            {
                for (var x = 0; x < verticesPerLine - 1; x++, v++)
                {
                    vertIdsOfTriangles[firstVertOfTris] = v;
                    vertIdsOfTriangles[firstVertOfTris + 1] = v + verticesPerLine;
                    vertIdsOfTriangles[firstVertOfTris + 2] = v + verticesPerLine + 1;

                    vertIdsOfTriangles[firstVertOfTris + 3] = v + verticesPerLine + 1;
                    vertIdsOfTriangles[firstVertOfTris + 4] = v + 1;
                    vertIdsOfTriangles[firstVertOfTris + 5] = v;

                    firstVertOfTris += 6;
                }
            }
        }

        private static void CreateUvCoords(int meshSize, int detailIncrement)
        {
            for (int i = 0, y = 0; y < meshSize; y += detailIncrement)
            {
                for (var x = 0; x < meshSize; x += detailIncrement, i++)
                {
                    uvCoords.Add( new Vector2((float) x / meshSize, (float) y / meshSize));
                }
            }
        }
    }
}