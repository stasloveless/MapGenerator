using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace GenerationAlgorithms
{
    public static class PerlinNoiseAlgorithm
    {
        public static Vector3[] GeneratePerlinNoise(int mapSize, float scale, int numOfOctaves, float lacunarity, float persistence)
        {
            var minNoiseLevel = float.MaxValue;
            var maxNoiseLevel = float.MinValue;
            //var noiseMap = new float[mapSize, mapSize];
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

                    //noiseMap[x, y] = currentNoiseValue;
                    noiseMap.Add(new Vector3(x, currentNoiseValue, y));
                }
            }

            for (var i = 0; i < noiseMap.Count; i++)
            {
                noiseMap[i] = Utils.ChangeY(noiseMap[i], Mathf.InverseLerp(minNoiseLevel, maxNoiseLevel, noiseMap[i].y));
            }
            
            /*for (var y = 0; y < mapSize; y++)
            {
                for (var x = 0; x < mapSize; x++)
                {
                    noiseMap[x, y] = Mathf.InverseLerp(minNoiseLevel, maxNoiseLevel, noiseMap[x, y]);
                }
            }*/

            return noiseMap.ToArray();
        }
    }
}