using System;
using GenerationAlgorithms;
using Generator;
using Hausdorff;
using LevelOfDetail;
using RamerDuglasPeucker3D;
using UnityEngine;
using Random = System.Random;

namespace Experimenter
{
    public class Experimenter
    {
        private Random rnd = new Random();
        public int numberOfIterations;
        public int mapSize;
        public int levelOfDetail;

        public AnimationCurve heightCurve = AnimationCurve.Linear(0, 0, 10, 10);

        public void StartExperiment()
        {
            /*Random rnd = new Random();
            
            for (var i = 1; i <= numberOfIterations; i++)
            {
                Debug.Log("Number: " + i);
                var heightMultiplier = Utils.RandomFloat(10, 30, rnd);
                //var heightMultiplier = 30.0f;
                Debug.Log("Height multiplier: " + heightMultiplier);
                var noiseScale = Utils.RandomFloat(2, 50, rnd);
                //var noiseScale = 10.0f;
                Debug.Log("Noise scale: " + noiseScale);
                
                //This will be our highPoly
                var heightMap = PerlinNoiseAlgorithm.GeneratePerlinNoise(mapSize, noiseScale, 1,
                    1, 1, heightMultiplier, heightCurve);

                var highPolyMesh = new MapGenerator(mapSize, heightMap);
                var terrainMesh = highPolyMesh.GenerateRegularMesh(GeneratorInspector.Optimization.None);
                Debug.Log("Tris HP: " + terrainMesh.triangles.Length / 3);

                //Lets calc Hausdorff for LOD level
                var factors = MathCalculations.MathCalculations.CalculateFactors(mapSize - 1);
                var lodOptimizedMap = SimpleLevelOfDetail.Optimize(heightMap, factors[levelOfDetail]);
                var lodMesh = new MapGenerator(mapSize, lodOptimizedMap);
                var lodOptimizedTerrainMesh = lodMesh.GenerateRegularMesh(GeneratorInspector.Optimization.LevelOfDetail);

                var n = lodOptimizedTerrainMesh.triangles.Length / 3;
                var epsilon = 0.05f;
                var m = 0;
                var step = epsilon;
                Vector3[] rdpOptimizedMap;

                do
                {
                    rdpOptimizedMap = RamerDuglasPeickerAlgorithm2D.OptimizeMap(heightMap, epsilon, heightMultiplier);
                    var rdpGen = new MapGenerator(mapSize, rdpOptimizedMap);
                    var rdpOptimizedTerrainMesh =
                        rdpGen.GenerateRegularMesh(GeneratorInspector.Optimization.RamerDouglasPecker);
                    m = rdpOptimizedTerrainMesh.triangles.Length / 3;
                    step /= 2;
                    if (m > n)
                    {
                        epsilon += step;
                    }
                    else if(m < n)
                    {
                        epsilon -= step;
                    }
                    else
                    {
                        break;
                    }
                } while (/*Math.Abs(m - n) > 0 ||#1# step >= 1e-6);

                //Output Hausdorff and number of tris
                var lodHausdorffDistance = HausdorffDistance.Calculate(heightMap, lodOptimizedMap);
                var rdpHausdorffDistance = HausdorffDistance.Calculate(heightMap, rdpOptimizedMap);
                Debug.Log("Tris LOD " + levelOfDetail + ": " + n + " Hausdorff: " + lodHausdorffDistance);
                Debug.Log("Tris RDP: " + m + " epsilon: " + epsilon + " Hausdorff: " + rdpHausdorffDistance);*/
        }

        /*private Vector3[] RandomPerlin()
        {
            var heightMultiplier = Utils.RandomFloat(10, 50, rnd);
            var noiseScale = Utils.RandomFloat(2, 50, rnd);
            //var octaves = rnd.Next(1, 10);
            //var lacunarity = Utils.RandomFloat(1, 50);
            //var persistence = Utils.RandomFloat(1, 50);

            return PerlinNoiseAlgorithm.GeneratePerlinNoise(mapSize, noiseScale, 1,
                1, 1, heightMultiplier, heightCurve);
        }*/
    }
}