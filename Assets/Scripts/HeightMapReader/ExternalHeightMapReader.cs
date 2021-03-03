using System.Collections.Generic;
using UnityEngine;

namespace HeightMapReader
{
    public static class ExternalHeightMapReader
    {
        public static Vector3[] ReadHeightMap(Texture2D importedMap, float heightMultiplier)
        {
            var heightMap = new List<Vector3>();
            var pixels = importedMap.GetPixels32();

            if (importedMap.height != importedMap.width)
            {
                Debug.Log("Image must be square");
            }

            var mapSize = importedMap.height;

            var x = 0;
            var y = 0;
            foreach (var pixel in pixels)
            {
                var grayPixel = (float) (pixel.r * 0.3 + 0.59 * pixel.g + pixel.b * 0.11);
                heightMap.Add(new Vector3(x,  grayPixel * heightMultiplier, y));
                x = (x + 1) % mapSize;
                if (x == 0)
                {
                    y++;
                }
            }
            return heightMap.ToArray();
        }
    }
}