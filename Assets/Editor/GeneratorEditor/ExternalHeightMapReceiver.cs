using GenerationAlgorithms;
using HeightMapReader;
using UnityEditor;
using UnityEngine;

namespace Editor.GeneratorEditor
{
    public class ExternalHeightMapReceiver
    {
        private static Texture2D _heightMap;
        private static float _heightMultiplier;
        
        public static void Draw()
        {
            _heightMultiplier = EditorGUILayout.Slider("Height Multiplier", _heightMultiplier, 0.01f, 1);
            _heightMap = (Texture2D) EditorGUILayout.ObjectField("Height Map", _heightMap, typeof(Texture2D), false);
        }

        public static Vector3[] Generate()
        {
            return ExternalHeightMapReader.ReadHeightMap(_heightMap, _heightMultiplier);
        }
    }
}