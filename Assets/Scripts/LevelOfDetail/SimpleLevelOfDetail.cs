using System.Collections.Generic;
using UnityEngine;

namespace LevelOfDetail
{
    public class SimpleLevelOfDetail
    {
        public static Vector3[] Optimize(int meshSize, Vector3[] heightMap, int lod)
        {
            var detailIncrement = lod == 0 ? 1 : lod * 2;
            var verticesPerLine = (meshSize - 1) / detailIncrement + 1;
            var optimizedHeightMap = new List<Vector3>();
            
            for (var i = 0; i < heightMap.Length; i += 1 + meshSize * (detailIncrement - 1))
            {
                for (var j = 0; j < verticesPerLine; j++)
                {
                    optimizedHeightMap.Add(new Vector3(heightMap[i].x, heightMap[i].y, heightMap[i].z));
                    i += detailIncrement;
                }
                i -= detailIncrement;
            }

            return optimizedHeightMap.ToArray();
        }
    }
}