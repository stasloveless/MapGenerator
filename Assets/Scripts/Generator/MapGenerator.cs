using System;
using System.Collections.Generic;
using System.Linq;
using Delaunay;
using DelaunayTriangulator;
using RamerDuglasPeucker3D;
using UnityEngine;

namespace Generator
{
    public class MapGenerator
    {
        public int mapSize;
        public Vector3[] heightMap;

        public MapGenerator(int mapSize, Vector3[] heightMap)
        {
            this.mapSize = mapSize;
            this.heightMap = heightMap;
        }

        public Mesh GenerateMesh(GeneratorInspector.Optimization optimizationMethod)
        {
            Mesh terrainMesh;
            
            switch (optimizationMethod)
            {
                case GeneratorInspector.Optimization.LevelOfDetail:
                {
                    terrainMesh = TerrainMeshGenerator.Generate(heightMap);
                    break;
                }
                case GeneratorInspector.Optimization.RamerDouglasPecker:
                {
                    var optimizedVector2Map = ExtractXZToIntVector2(heightMap);
                    var triangulator = new Triangulator();
                    var triangles = triangulator.Triangulation(optimizedVector2Map);
                    var triangulation = TriadsToTriangles(triangles, optimizedVector2Map);
                    terrainMesh = IrregularTerrainMeshGenerator.Generate(heightMap, triangulation, mapSize);
                    break;
                }
                case GeneratorInspector.Optimization.None:
                {
                    terrainMesh = TerrainMeshGenerator.Generate(heightMap);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            return terrainMesh;
        }

        public Texture2D GenerateTexture()
        {
            return CreateTexture.FromHeightMap(heightMap);
        }

        private List<Triangle> TriadsToTriangles(List<Triad> triangles, List<Vector2> heightMap)
        {
            var trisList = new List<Triangle>();
            foreach (var triad in triangles)
            {
                trisList.Add(new Triangle(heightMap[triad.a], heightMap[triad.b], heightMap[triad.c]));
            }

            return trisList;
        }

        private Vector3[] FloatArrayToVectorArray(float[,] array)
        {
            var result = new List<Vector3>();
            for (var i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    result.Add(new Vector3(j, array[j, i], i));
                }
            }

            return result.ToArray();
        }

        public static List<Vector2> ExtractXZToIntVector2(Vector3[] array)
        {
            return array.Select(element => new Vector2(element.x, element.z)).ToList();
        }
    }
}