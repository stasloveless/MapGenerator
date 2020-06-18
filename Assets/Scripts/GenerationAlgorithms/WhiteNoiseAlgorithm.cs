using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteNoiseAlgorithm
{
    public static float[,] GenerateWhiteNoise(int mapWidth, int mapHeight)
    {
        Random.InitState(2);
        float[,] noiseMap = new float[mapWidth, mapHeight];
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                noiseMap[x, y] = Random.value;
            }
        }
        return noiseMap;
    }
}
