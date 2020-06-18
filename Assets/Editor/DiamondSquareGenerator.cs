using System;
using System.Collections;
using System.Collections.Generic;
using GenerationAlgorithms;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class DiamondSquareGenerator
    {
        private static float R = 0.6f;
        private static int Seed = 1;

        public static void Draw()
        {
            R = EditorGUILayout.Slider("Roughness",R, 0, 1);
            Seed = Mathf.RoundToInt(EditorGUILayout.Slider("Seed",Seed, 1, 30));
        }

        public static Vector3[] Generate(int mapSize)
        {
            Vector3[] heightMap = DiamondSquareAlgorithm.GenerateDiamondSquareMap(mapSize, R, Seed);
            return heightMap;
        }
    }
}
