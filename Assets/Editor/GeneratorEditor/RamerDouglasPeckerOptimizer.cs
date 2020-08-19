using RamerDuglasPeucker3D;
using UnityEditor;
using UnityEngine;

namespace Editor.GeneratorEditor
{
    public static class RamerDouglasPeckerOptimizer
    {
        private static float _epsilon = 0.0f;
        
        public static void Draw()
        {
            _epsilon = EditorGUILayout.Slider("Epsilon", _epsilon, 0, 1);
        }

        public static Vector3[] Optimize(Vector3[] heightMap)
        {
            return RamerDuglasPeickerAlgorithm2D.OptimizeMap(heightMap, _epsilon);
        }
    }
}