using System;
using Generator;
using UnityEditor;
using UnityEngine;

namespace Editor.GeneratorEditor
{
    [CustomEditor(typeof(GeneratorInspector))]
    public class MainGeneratorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var genInsp = (GeneratorInspector) target;
            DrawDefaultInspector();

            switch (genInsp.generationAlgorithm)
            {
                case GeneratorInspector.GenerationAlgorithm.PerlinNoise:
                    PerlinNoiseGenerator.Draw();
                    break;
                case GeneratorInspector.GenerationAlgorithm.DiamondSquare:
                    DiamondSquareGenerator.Draw();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (genInsp.optimizationMethod)
            {
                case GeneratorInspector.Optimization.LevelOfDetail:
                    LevelOfDetailOptimizer.Draw(genInsp.mapSize);
                    break;
                case GeneratorInspector.Optimization.RamerDouglasPecker:
                    RamerDouglasPeckerOptimizer.Draw();
                    break;
                case GeneratorInspector.Optimization.None:
                    EditorGUILayout.Space();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (GUILayout.Button("Generate"))
            {
                switch (genInsp.generationAlgorithm)
                {
                    case GeneratorInspector.GenerationAlgorithm.PerlinNoise:
                    {
                        var heightMap = PerlinNoiseGenerator.Generate(genInsp.mapSize);
                        switch (genInsp.optimizationMethod)
                        {
                            case GeneratorInspector.Optimization.LevelOfDetail:
                            {
                                var optimizedMap = LevelOfDetailOptimizer.Optimize(heightMap);
                                genInsp.Generate(optimizedMap);
                                break;
                            }
                            case GeneratorInspector.Optimization.RamerDouglasPecker:
                            {
                                var optimizedMap = RamerDouglasPeckerOptimizer.Optimize(heightMap, PerlinNoiseGenerator._heightMultiplier);
                                genInsp.Generate(optimizedMap);
                                break;
                            }
                            case GeneratorInspector.Optimization.None:
                            {
                                genInsp.Generate(heightMap);
                                break;
                            }
                        }

                        break;
                    }

                    case GeneratorInspector.GenerationAlgorithm.DiamondSquare:
                    {
                        var heightMap= DiamondSquareGenerator.Generate(genInsp.mapSize);
                        switch (genInsp.optimizationMethod)
                        {
                            case GeneratorInspector.Optimization.LevelOfDetail:
                            {
                                var optimizedMap = LevelOfDetailOptimizer.Optimize(heightMap);
                                genInsp.Generate(optimizedMap);
                                break;
                            }
                            case GeneratorInspector.Optimization.RamerDouglasPecker:
                            {
                                var optimizedMap = RamerDouglasPeckerOptimizer.Optimize(heightMap, DiamondSquareGenerator._heightMultiplier);
                                genInsp.Generate(optimizedMap);
                                break;
                            }
                            case GeneratorInspector.Optimization.None:
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