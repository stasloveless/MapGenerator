using System;
using System.Collections.Generic;
using System.Linq;
using Delaunay;
using DelaunayTriangulator;
using RamerDuglasPeucker3D;
using UnityEngine;
using UnityEngine.Serialization;

namespace Generator
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class MapGenerator : MonoBehaviour
    {
        public enum GenerationAlgorithm
        {
            PerlinNoise,
            DiamondSquare
        };

        public enum OptimizationAlgorithm
        {
            None,
            Regular,
            Irregular
        };
        
        public bool delaunay;
        [Range(1, 255)] public int mapSize;
        [Range(0, 32)] public int levelOfDetail;

        [Range(1, 100)] public float heightMultiplier;
        // public AnimationCurve heightCurve;
        [Range(0.0001f, 1)] public float epsilon;
        public bool pseudoDistance;
        [Range(0f, 1f)] public float c;
        
        public GenerationAlgorithm generationAlgorithm;
        //public OptimizationAlgorithm optimizationAlgorithm

        public void Generate(Vector3[] heightMap)
        {
            var terrainMesh = new Mesh();
            var terrainTexture = Texture2D.whiteTexture;
            var triangulation = new List<Triangle>();

            if (delaunay)
            {
                var optimizedHeightMap = RamerDuglasPeuckerAlgorithm.SimplifyMap(heightMap, epsilon, mapSize, pseudoDistance, c);
                var optimizedVector2Map = ExtractXZToIntVector2(optimizedHeightMap);
                var triangulator = new Triangulator();
                var triangles = triangulator.Triangulation(optimizedVector2Map);
                triangulation = TriadsToTriangles(triangles, optimizedVector2Map);
                terrainMesh = IrregularTerrainMeshGenerator.Generate(optimizedHeightMap, triangulation, heightMultiplier, mapSize);
                terrainTexture = CreateTexture.FromHeightMap(heightMap, mapSize);
            }
            else
            {
                switch (generationAlgorithm)
                {
                    case GenerationAlgorithm.PerlinNoise:
                    {
                        terrainMesh =
                            TerrainMeshGenerator.Generate(mapSize, heightMap, heightMultiplier, levelOfDetail);
                        terrainTexture = CreateTexture.FromHeightMap(heightMap, mapSize);
                        break;
                    }
                    case GenerationAlgorithm.DiamondSquare:
                    {
                        terrainMesh =
                            TerrainMeshGenerator.Generate(mapSize, heightMap, heightMultiplier, levelOfDetail);
                        terrainTexture = CreateTexture.FromHeightMap(heightMap, mapSize);
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
            GetComponent<MeshRenderer>().sharedMaterial.mainTexture = terrainTexture;
            GetComponent<MeshFilter>().mesh = terrainMesh;
            terrainMesh.name = "Procedural Grid";
            
            //Debug.Log("Vertices HP: " + terrainMesh.vertices.Length);
            Debug.Log("Tris HP: " + terrainMesh.triangles.Length / 3);
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