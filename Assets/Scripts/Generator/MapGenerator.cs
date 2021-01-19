using System;
using System.Collections.Generic;
using System.Linq;
using Delaunay;
using DelaunayTriangulator;
using RamerDuglasPeucker3D;
using UnityEngine;

namespace Generator
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class MapGenerator : MonoBehaviour
    {
        public Mesh GenerateRegularMesh(Vector3[] heightMap)
        {
            //Texture2D terrainTexture;
            //terrainTexture = GenerateTexture();

            var terrainMesh = TerrainMeshGenerator.Generate(heightMap);

            terrainMesh.name = "Procedural map";
            //GetComponent<MeshRenderer>().sharedMaterial.mainTexture = terrainTexture;
            GetComponent<MeshFilter>().mesh = terrainMesh;
            GetComponent<MeshCollider>().sharedMesh = terrainMesh;

            return terrainMesh;
        }

        public Mesh GenerateIrregularMesh(Vector3[] heightMap, int mapSize)
        {
            var optimizedVector2Map = ExtractXZToIntVector2(heightMap);
            var triangulator = new Triangulator();
            var triangles = triangulator.Triangulation(optimizedVector2Map);
            var triangulation = TriadsToTriangles(triangles, optimizedVector2Map);
            
            var terrainMesh = IrregularTerrainMeshGenerator.Generate(heightMap, triangulation, mapSize);

            terrainMesh.name = "Procedural map";
            //GetComponent<MeshRenderer>().sharedMaterial.mainTexture = terrainTexture;
            GetComponent<MeshFilter>().mesh = terrainMesh;
            GetComponent<MeshCollider>().sharedMesh = terrainMesh;

            return terrainMesh;
        }

        public Texture2D GenerateTexture(Vector3[] heightMap)
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

        private static List<Vector2> ExtractXZToIntVector2(Vector3[] array)
        {
            return array.Select(element => new Vector2(element.x, element.z)).ToList();
        }
    }
}