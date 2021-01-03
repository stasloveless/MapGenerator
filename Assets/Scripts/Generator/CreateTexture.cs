using System;
using UnityEngine;

namespace Generator
{
    public static class CreateTexture
    {
        public static Texture2D FromHeightMap(Vector3[] heightMap)
        {
            var mapSize = (int) Math.Sqrt(heightMap.Length);
            var colorMap = new Color[mapSize * mapSize];

            for (var i = 0; i < colorMap.Length; i++)
            {
                colorMap[i] = Color.Lerp(Color.black, Color.white, heightMap[i].y);
            }

            var texture = new Texture2D(mapSize, mapSize);
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.SetPixels(colorMap);
            texture.Apply();

            return texture;
        }
    }
}