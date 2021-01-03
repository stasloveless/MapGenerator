using GenerationAlgorithms;
using UnityEditor;
using UnityEngine;

namespace Editor.GeneratorEditor
{
    public static class PerlinNoiseGenerator
    {
        private static AnimationCurve _heightCurve = AnimationCurve.Linear(0, 0, 1, 1);
        private static float _scale = 5.1f;
        private static int _octaves = 1;
        private static float _lacunarity = 1;
        private static float _persistence = 1;
        public static float _heightMultiplier = 1;

        public static void Draw()
        {
            _heightMultiplier = EditorGUILayout.Slider("Height Multiplier", _heightMultiplier, 1, 50);
            _heightCurve = EditorGUILayout.CurveField("Height Curve", _heightCurve);
            _scale = EditorGUILayout.Slider("Noise Scale",_scale, 1, 50);
            //_octaves = EditorGUILayout.IntSlider("Octaves",_octaves, 1, 10);
            //_lacunarity = EditorGUILayout.Slider("Lacunarity",_lacunarity, 1, 10);
            //_persistence = EditorGUILayout.Slider("Persistence",_persistence, 1, 10);
        }

        public static Vector3[] Generate(int mapSize)
        {
            return PerlinNoiseAlgorithm.GeneratePerlinNoise(mapSize, _scale, _octaves, _lacunarity, _persistence, _heightMultiplier, _heightCurve);
        }
    }
}