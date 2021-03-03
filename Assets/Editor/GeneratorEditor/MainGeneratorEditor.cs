using System;
using Generator;
using UnityEditor;
using UnityEngine;

namespace Editor.GeneratorEditor
{
    [CustomEditor(typeof(MapGenerator))]
    public class MainGeneratorEditor : UnityEditor.Editor
    {
        public enum GenerationAlgorithm
        {
            PerlinNoise,
            DiamondSquare
        }

        public enum Optimization
        {
            None,
            LevelOfDetail,
            RamerDouglasPecker
        }

        public int mapSize;
        public bool importHeightMap;

        public GenerationAlgorithm generationAlgorithm = GenerationAlgorithm.PerlinNoise;
        public Optimization optimizationMethod = Optimization.None;

        private Vector3[] heightMap;
        private Vector3[] optimizedMap;

        public override void OnInspectorGUI()
        {
            DrawCustomInspector();

            if (GUILayout.Button("Generate"))
            {
                StartGenerating();
            }
        }

        private void DrawCustomInspector()
        {
            importHeightMap = EditorGUILayout.Toggle("Import Height Map", importHeightMap);
            optimizationMethod = (Optimization) EditorGUILayout.EnumPopup("Optimization Algorithm", optimizationMethod);

            if (importHeightMap)
            {
                ExternalHeightMapReceiver.Draw();
                SetOptimizationAlgorithmUI();
            }
            else
            {
                generationAlgorithm =
                    (GenerationAlgorithm) EditorGUILayout.EnumPopup("Generation Algorithm", generationAlgorithm);
                mapSize = Mathf.RoundToInt(EditorGUILayout.Slider("Map size", mapSize, 1, 255));
                SetGenerationAlgorithmUI();
                SetOptimizationAlgorithmUI();
            }
        }

        private void SetGenerationAlgorithmUI()
        {
            switch (generationAlgorithm)
            {
                case GenerationAlgorithm.PerlinNoise:
                    PerlinNoiseGenerator.Draw();
                    break;
                case GenerationAlgorithm.DiamondSquare:
                    DiamondSquareGenerator.Draw();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetOptimizationAlgorithmUI()
        {
            switch (optimizationMethod)
            {
                case Optimization.LevelOfDetail:
                    LevelOfDetailOptimizer.Draw(mapSize);
                    break;
                case Optimization.RamerDouglasPecker:
                    RamerDouglasPeckerOptimizer.Draw();
                    break;
                case Optimization.None:
                    EditorGUILayout.Space();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void StartGenerating()
        {
            if (importHeightMap)
            {
                heightMap = ExternalHeightMapReceiver.Generate();
                Debug.Log("Height map");
            }
            else
            {
                switch (generationAlgorithm)
                {
                    case GenerationAlgorithm.PerlinNoise:
                    {
                        heightMap = PerlinNoiseGenerator.Generate(mapSize);
                        break;
                    }
                    case GenerationAlgorithm.DiamondSquare:
                    {
                        heightMap = DiamondSquareGenerator.Generate(mapSize);
                        break;
                    }
                }
            }

            var mapGen = (MapGenerator) target;
            switch (optimizationMethod)
            {
                case Optimization.LevelOfDetail:
                {
                    optimizedMap = LevelOfDetailOptimizer.Optimize(heightMap);
                    mapGen.GenerateRegularMesh(optimizedMap);
                    break;
                }
                case Optimization.RamerDouglasPecker:
                {
                    optimizedMap =
                        RamerDouglasPeckerOptimizer.Optimize(heightMap, PerlinNoiseGenerator._heightMultiplier);
                    mapGen.GenerateIrregularMesh(optimizedMap, mapSize);
                    break;
                }
                case Optimization.None:
                {
                    mapGen.GenerateRegularMesh(heightMap);
                    Debug.Log("Bulding");
                    break;
                }
            }
        }
    }
}