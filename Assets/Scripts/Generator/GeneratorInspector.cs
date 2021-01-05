using System;
using UnityEngine;

namespace Generator
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class GeneratorInspector : MonoBehaviour
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

        [Range(1, 255)] public int mapSize;

        public GenerationAlgorithm generationAlgorithm = GenerationAlgorithm.PerlinNoise;
        public Optimization optimizationMethod = Optimization.None;

        
        public void Generate(Vector3[] heightMap)
        {
            var mapGen = new MapGenerator(mapSize, heightMap);
            Mesh terrainMesh;
            //Texture2D terrainTexture;

            terrainMesh = mapGen.GenerateMesh(optimizationMethod);
            //terrainTexture = mapGen.GenerateTexture();
            terrainMesh.name = "Procedural map";
            //GetComponent<MeshRenderer>().sharedMaterial.mainTexture = terrainTexture;
            GetComponent<MeshFilter>().mesh = terrainMesh;
            GetComponent<MeshCollider>().sharedMesh = terrainMesh;

            Debug.Log(terrainMesh.triangles.Length);
        }
    }
}