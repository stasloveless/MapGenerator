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
            DiamondSquare,
            None
        }

        public enum Optimization
        {
            None,
            LevelOfDetail,
            RamerDouglasPecker
        }

        public int mapSize;
        public bool importHeightMap;

        public GenerationAlgorithm generationAlgorithm = GenerationAlgorithm.None;
        public Optimization optimizationMethod = Optimization.None;

        public Texture2D externalHeightMap;

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
            generationAlgorithm =
                (GenerationAlgorithm) EditorGUILayout.EnumPopup("Generation Algorithm", generationAlgorithm);
            importHeightMap = EditorGUILayout.Toggle("Import Height Map", importHeightMap);
            optimizationMethod = (Optimization) EditorGUILayout.EnumPopup("Optimization Algorithm", optimizationMethod);
            mapSize = Mathf.RoundToInt(EditorGUILayout.Slider("Map size", mapSize, 1, 255));

            if (importHeightMap)
            {
                generationAlgorithm = GenerationAlgorithm.None;
                externalHeightMap =
                    (Texture2D) EditorGUILayout.ObjectField("Height Map", externalHeightMap, typeof(Texture2D), false);
            }

            SetGenerationAlgorithmUI();
            SetOptimizationAlgorithmUI();
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
                case GenerationAlgorithm.None:
                    EditorGUILayout.Space();
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
            var mapGen = (MapGenerator) target;

            switch (generationAlgorithm)
            {
                case GenerationAlgorithm.PerlinNoise:
                {
                    var heightMap = PerlinNoiseGenerator.Generate(mapSize);
                    switch (optimizationMethod)
                    {
                        case Optimization.LevelOfDetail:
                        {
                            var optimizedMap = LevelOfDetailOptimizer.Optimize(heightMap);
                            mapGen.GenerateRegularMesh(optimizedMap);
                            break;
                        }
                        case Optimization.RamerDouglasPecker:
                        {
                            var optimizedMap =
                                RamerDouglasPeckerOptimizer.Optimize(heightMap, PerlinNoiseGenerator._heightMultiplier);
                            mapGen.GenerateIrregularMesh(optimizedMap, mapSize);
                            break;
                        }
                        case Optimization.None:
                        {
                            mapGen.GenerateRegularMesh(heightMap);
                            break;
                        }
                    }

                    break;
                }

                case GenerationAlgorithm.DiamondSquare:
                {
                    var heightMap = DiamondSquareGenerator.Generate(mapSize);
                    switch (optimizationMethod)
                    {
                        case Optimization.LevelOfDetail:
                        {
                            var optimizedMap = LevelOfDetailOptimizer.Optimize(heightMap);
                            mapGen.GenerateRegularMesh(optimizedMap);
                            break;
                        }
                        case Optimization.RamerDouglasPecker:
                        {
                            var optimizedMap = RamerDouglasPeckerOptimizer.Optimize(heightMap,
                                DiamondSquareGenerator._heightMultiplier);
                            mapGen.GenerateIrregularMesh(optimizedMap, mapSize);
                            break;
                        }
                        case Optimization.None:
                        {
                            mapGen.GenerateRegularMesh(heightMap);
                            break;
                        }
                    }

                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}