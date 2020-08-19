using System.Collections.Generic;
using UnityEngine;

namespace GenerationAlgorithms
{
    public static class PerlinNoiseAlgorithm
    {
        public static Vector3[] GeneratePerlinNoise(int mapSize, float scale, int numOfOctaves, float lacunarity, float persistence, float heightMultiplier, AnimationCurve heightCurve)
        {
            var minNoiseLevel = float.MaxValue;
            var maxNoiseLevel = float.MinValue;
            var noiseMap = new List<Vector3>();

            for (var y = 0; y < mapSize; y++)
            {
                for (var x = 0; x < mapSize; x++)
                {
                    float currentNoiseValue = 0;
                    float frequency = 1;
                    float amplitude = 1;
                
                    for (var i = 0; i < numOfOctaves; i++)
                    {
                        var sampleX = x / scale * frequency;
                        var sampleY = y / scale * frequency;
                        var newNoiseLvl = Mathf.PerlinNoise(sampleX, sampleY);
                        currentNoiseValue += newNoiseLvl * amplitude;
                        frequency *= lacunarity;
                        amplitude *= persistence;
                    }

                    if (currentNoiseValue > maxNoiseLevel)
                    {
                        maxNoiseLevel = currentNoiseValue;
                    }
                    else if (currentNoiseValue < minNoiseLevel)
                    {
                        minNoiseLevel = currentNoiseValue;
                    }
                    
                    noiseMap.Add(new Vector3(x, currentNoiseValue, y));
                }
            }

            for (var i = 0; i < noiseMap.Count; i++)
            {
                var finalValue = Mathf.InverseLerp(minNoiseLevel, maxNoiseLevel, noiseMap[i].y);
                noiseMap[i] = Utils.ChangeY(noiseMap[i], heightCurve.Evaluate(finalValue) * heightMultiplier);
            }

            return noiseMap.ToArray();
        }
    }
}