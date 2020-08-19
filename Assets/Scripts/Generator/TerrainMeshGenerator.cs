using System;
using System.Collections.Generic;
using UnityEngine;

namespace Generator
{
    public static class TerrainMeshGenerator
    {
        private static int[] _vertIdsOfTriangles;
        private static List<Vector2> _uvCoords;
        private static int _mapSize;

        public static Mesh Generate(Vector3[] heightMap)
        {
            _mapSize = (int) Math.Sqrt(heightMap.Length);
            _vertIdsOfTriangles = new int[(_mapSize - 1) * (_mapSize - 1) * 6];
            _uvCoords = new List<Vector2>();
            
            var newMesh = new Mesh {vertices = heightMap};
            
            CreateTris();
            newMesh.triangles = _vertIdsOfTriangles;
            newMesh.RecalculateNormals();

            CreateUvCoords();
            newMesh.uv = _uvCoords.ToArray();

            return newMesh;
        }

        private static void CreateTris()
        {
            var firstVertOfTris = 0;
            var v = 0;

            for (var y = 0; y < _mapSize - 1; y++, v++)
            {
                for (var x = 0; x < _mapSize - 1; x++, v++)
                {
                    _vertIdsOfTriangles[firstVertOfTris] = v;
                    _vertIdsOfTriangles[firstVertOfTris + 1] = v + _mapSize;
                    _vertIdsOfTriangles[firstVertOfTris + 2] = v + _mapSize + 1;

                    _vertIdsOfTriangles[firstVertOfTris + 3] = v + _mapSize + 1;
                    _vertIdsOfTriangles[firstVertOfTris + 4] = v + 1;
                    _vertIdsOfTriangles[firstVertOfTris + 5] = v;

                    firstVertOfTris += 6;
                }
            }
        }

        private static void CreateUvCoords()
        {
            for (int i = 0, y = 0; y < _mapSize; y++)
            {
                for (var x = 0; x < _mapSize; x++, i++)
                {
                    _uvCoords.Add( new Vector2((float) x / _mapSize, (float) y / _mapSize));
                }
            }
        }
    }
}