using System.Collections.Generic;
using LevelOfDetail;
using UnityEditor;
using UnityEngine;

namespace Editor.GeneratorEditor
{
    public static class LevelOfDetailOptimizer
    {
        private static int _levelOfDetail = 0;
        private static List<int> _factors;
        
        public static void Draw(int meshSize)
        {
            _factors = MathCalculations.MathCalculations.CalculateFactors(meshSize - 1);
            _levelOfDetail = EditorGUILayout.IntSlider("Level of Detail", _levelOfDetail, 0, _factors.Count - 1);
        }

        public static Vector3[] Optimize(Vector3[] heightMap)
        {
            var opt = SimpleLevelOfDetail.Optimize(heightMap, _factors[_levelOfDetail]);
            return opt;
        }
    }
}