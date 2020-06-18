using System;
using Generator;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(MapGenerator))]
    public class MainGeneratorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            MapGenerator mapGen = (MapGenerator) target;
            DrawDefaultInspector();

            switch (mapGen.generationAlgorithm)
            {
                case MapGenerator.GenerationAlgorithm.PerlinNoise:
                    PerlinNoiseGenerator.Draw();
                    break;
                case MapGenerator.GenerationAlgorithm.DiamondSquare:
                    DiamondSquareGenerator.Draw();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            /*switch (mapGen.optimizationAlgorithm)
            {
                case MapGenerator.OptimizationAlgorithm.None:
                    break;
                case MapGenerator.OptimizationAlgorithm.Regular:
                    RegularOptimizator.Draw();
                    break;
                case MapGenerator.OptimizationAlgorithm.Irregular:
                    IrregularOptimizator.Draw();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }*/
            
            if (GUILayout.Button("Generate"))
            {
                switch (mapGen.generationAlgorithm)
                {
                    case MapGenerator.GenerationAlgorithm.PerlinNoise:
                        /*switch (mapGen.optimizationAlgorithm)
                        {
                            case MapGenerator.OptimizationAlgorithm.None:
                                mapGen.Generate(PerlinNoiseGenerator.Generate(mapGen.mapSize));
                                break;
                            case MapGenerator.OptimizationAlgorithm.Regular:
                                mapGen.Generate(PerlinNoiseGenerator.Generate(mapGen.mapSize));
                                mapGen.optimizationAlgorithm = MapGenerator.OptimizationAlgorithm.Regular;
                                RegularOptimizator.Draw();
                                break;
                            case MapGenerator.OptimizationAlgorithm.Irregular:
                                mapGen.Generate(PerlinNoiseGenerator.Generate(mapGen.mapSize));
                                mapGen.optimizationAlgorithm = MapGenerator.OptimizationAlgorithm.Irregular;
                                IrregularOptimizator.Draw();
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }*/
                        mapGen.Generate(PerlinNoiseGenerator.Generate(mapGen.mapSize));
                        break;
                    case MapGenerator.GenerationAlgorithm.DiamondSquare:
                        /*switch (mapGen.optimizationAlgorithm)
                        {
                            case MapGenerator.OptimizationAlgorithm.None:
                                mapGen.Generate(DiamondSquareGenerator.Generate(mapGen.mapSize));
                                mapGen.optimizationAlgorithm = MapGenerator.OptimizationAlgorithm.None;
                                break;
                            case MapGenerator.OptimizationAlgorithm.Regular:
                                mapGen.Generate(DiamondSquareGenerator.Generate(mapGen.mapSize));
                                mapGen.optimizationAlgorithm = MapGenerator.OptimizationAlgorithm.Regular;
                                RegularOptimizator.Draw();
                                break;
                            case MapGenerator.OptimizationAlgorithm.Irregular:
                                mapGen.Generate(DiamondSquareGenerator.Generate(mapGen.mapSize));
                                mapGen.optimizationAlgorithm = MapGenerator.OptimizationAlgorithm.Irregular;
                                IrregularOptimizator.Draw();
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }*/
                        mapGen.Generate(DiamondSquareGenerator.Generate(mapGen.mapSize));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                /*switch (mapGen.optimizationAlgorithm)
                {
                    case MapGenerator.OptimizationAlgorithm.None:
                        break;
                    case MapGenerator.OptimizationAlgorithm.Regular:
                        RegularOptimizator.Draw();
                        break;
                    case MapGenerator.OptimizationAlgorithm.Irregular:
                        IrregularOptimizator.Draw();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }*/
            }
        }
    }
}
