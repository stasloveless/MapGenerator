using System;
using Generator;
using UnityEditor;
using UnityEngine;

namespace Editor.GeneratorEditor
{
    [CustomEditor(typeof(GeneratorInspector))]
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
            var genInsp = (GeneratorInspector) target;
            generationAlgorithm = (GenerationAlgorithm)EditorGUILayout.EnumPopup("Generation Algorithm",generationAlgorithm);
            importHeightMap = EditorGUILayout.Toggle("Import Height Map", importHeightMap);
            optimizationMethod = (Optimization)EditorGUILayout.EnumPopup("Optimization Algorithm",optimizationMethod);
            mapSize = Mathf.RoundToInt(EditorGUILayout.Slider("Map size", mapSize, 1, 255));

            if (importHeightMap)
            {
                generationAlgorithm = GenerationAlgorithm.None;
                externalHeightMap = (Texture2D) EditorGUILayout.ObjectField("Height Map", externalHeightMap, typeof(Texture2D), false);

            }
            
            //DrawDefaultInspector();

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

            switch (optimizationMethod)
            {
                case Optimization.LevelOfDetail:
                    LevelOfDetailOptimizer.Draw(genInsp.mapSize);
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

            if (GUILayout.Button("Generate"))
            {
                switch (generationAlgorithm)
                {
                    case GenerationAlgorithm.PerlinNoise:
                    {
                        var heightMap = PerlinNoiseGenerator.Generate(genInsp.mapSize);
                        switch (optimizationMethod)
                        {
                            case Optimization.LevelOfDetail:
                            {
                                var optimizedMap = LevelOfDetailOptimizer.Optimize(heightMap);
                                genInsp.Generate(optimizedMap);
                                break;
                            }
                            case Optimization.RamerDouglasPecker:
                            {
                                var optimizedMap = RamerDouglasPeckerOptimizer.Optimize(heightMap, PerlinNoiseGenerator._heightMultiplier);
                                genInsp.Generate(optimizedMap);
                                break;
                            }
                            case Optimization.None:
                            {
                                genInsp.Generate(heightMap);
                                break;
                            }
                        }

                        break;
                    }

                    case GenerationAlgorithm.DiamondSquare:
                    {
                        var heightMap= DiamondSquareGenerator.Generate(genInsp.mapSize);
                        switch (optimizationMethod)
                        {
                            case Optimization.LevelOfDetail:
                            {
                                var optimizedMap = LevelOfDetailOptimizer.Optimize(heightMap);
                                genInsp.Generate(optimizedMap);
                                break;
                            }
                            case Optimization.RamerDouglasPecker:
                            {
                                var optimizedMap = RamerDouglasPeckerOptimizer.Optimize(heightMap, DiamondSquareGenerator._heightMultiplier);
                                genInsp.Generate(optimizedMap);
                                break;
                            }
                            case Optimization.None:
                            {
                                genInsp.Generate(heightMap);
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
}