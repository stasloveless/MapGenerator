using GenerationAlgorithms;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class PerlinNoiseGenerator
    {
        //private static int Seed = 0;
        private static float Scale = 5.1f;
        private static int Octaves = 1;
        private static float Lacunarity = 1;
        private static float Persistence = 1;

        public static void Draw()
        {
            Scale = EditorGUILayout.Slider("Noise Scale",Scale, 1, 50);
            Octaves = Mathf.RoundToInt(EditorGUILayout.Slider("Octaves",Octaves, 1, 10));
            Lacunarity = EditorGUILayout.Slider("Lacunarity",Lacunarity, 1, 10);
            Persistence = EditorGUILayout.Slider("Persistence",Persistence, 1, 10);
        }

        public static Vector3[] Generate(int mapSize)
        {
            var heightMap = PerlinNoiseAlgorithm.GeneratePerlinNoise(mapSize, Scale, Octaves, Lacunarity, Persistence);
            return heightMap;
        }
    }
}