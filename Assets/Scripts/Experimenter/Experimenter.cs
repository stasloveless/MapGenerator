using System;
using GenerationAlgorithms;
using Generator;
using UnityEngine;
using Random = System.Random;

namespace Experimenter
{
    public class Experimenter : MonoBehaviour
    {
        private Random rnd = new Random();
        public int numberOfIterations;
        public int mapSize;

        public AnimationCurve heightCurve;

        public void Start()
        {
            //0 - Perlin, 1 - DiamondSquare
            var algorithmId = Utils.RandomInt(0, 1);
            var levelOfDetail = 0;

            switch (algorithmId)
            {
                case 0:
                {
                    for (var i = 0; i < numberOfIterations; i++)
                    {
                        //This will be our highPoly
                        var heightMap = RandomPerlin();
                        var mapGen = new MapGenerator(mapSize, levelOfDetail, heightMap);
                        var terrainMesh = mapGen.GenerateMesh(false, 0.0f);
                        
                        //Lets calc Hausdorff for all LOD levels
                        for (var j = 0; j < 5; j++)
                        {
                            //var optimizedMap = LODOptimizatorFunction();
                            //add to log number of tris and hausdorff dist
                        }

                        Debug.Log("Tris HP: " + terrainMesh.triangles.Length / 3);
                        //Output log with lods
                        
                        //Next must be RDP/D maps with the same num of tris 

                    }
                    break;
                }
                case 1:
                {
                    for (var i = 0; i < numberOfIterations; i++)
                    {
                        //This will be our highPoly
                        var heightMap = RandomDiamondSquare();
                        var mapGen = new MapGenerator(mapSize, levelOfDetail, heightMap);
                        var terrainMesh = mapGen.GenerateMesh(false, 0.0f);
                    }
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Vector3[] RandomPerlin()
        {
            var heightMultiplier =  Utils.RandomFloat(1, 50);
            var noiseScale = Utils.RandomFloat(1, 50);
            var octaves = Utils.RandomInt(1, 10);
            var lacunarity = Utils.RandomFloat(1, 50);
            var persistence = Utils.RandomFloat(1, 50);
            
            return PerlinNoiseAlgorithm.GeneratePerlinNoise(mapSize, noiseScale, octaves,
                lacunarity, persistence, heightMultiplier, heightCurve);
        }
        
        private Vector3[] RandomDiamondSquare()
        {
            var heightMultiplier =  Utils.RandomFloat(1, 50);
            var r = Utils.RandomFloat(0, 1);
            var seed = Utils.RandomInt(1, 30);
            
            return DiamondSquareAlgorithm.GenerateDiamondSquareMap(mapSize, r, seed, heightMultiplier, heightCurve);
        }
    }
}