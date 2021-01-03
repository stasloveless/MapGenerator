using Generator;
using RamerDuglasPeucker3D;
using UnityEditor;
using UnityEngine;

namespace Editor.GeneratorEditor
{
    public static class RamerDouglasPeckerOptimizer
    {
        private static float _epsilon = 0.001f;
        
        public static void Draw()
        {
            _epsilon = EditorGUILayout.Slider("Epsilon", _epsilon, 0.001f, 0.1f);
        }

        public static Vector3[] Optimize(Vector3[] heightMap, float heightMultiplier)
        {
            return RamerDuglasPeickerAlgorithm2D.OptimizeMap(heightMap, _epsilon, heightMultiplier);
        }
    }
}