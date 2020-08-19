using GenerationAlgorithms;
using UnityEditor;
using UnityEngine;

namespace Editor.GeneratorEditor
{
    public static class DiamondSquareGenerator
    {
        private static float _heightMultiplier = 1;
        private static AnimationCurve _heightCurve =  AnimationCurve.Linear(0, 0, 1, 1);
        private static float _r = 0.6f;
        private static int _seed = 1;

        public static void Draw()
        {
            _heightMultiplier = EditorGUILayout.Slider("Height Multiplier", _heightMultiplier, 1, 50);
            _heightCurve = EditorGUILayout.CurveField("Height Curve", _heightCurve);
            _r = EditorGUILayout.Slider("Roughness",_r, 0, 1);
            _seed = Mathf.RoundToInt(EditorGUILayout.Slider("Seed",_seed, 1, 30));
        }

        public static Vector3[] Generate(int mapSize)
        {
            return DiamondSquareAlgorithm.GenerateDiamondSquareMap(mapSize, _r, _seed, _heightMultiplier, _heightCurve);
        }
    }
}
