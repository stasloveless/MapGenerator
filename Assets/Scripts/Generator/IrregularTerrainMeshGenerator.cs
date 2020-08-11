using System.Collections.Generic;
using RamerDuglasPeucker3D;
using UnityEngine;

namespace Generator
{
    public class IrregularTerrainMeshGenerator
    {
        private static List<int> vertIdsOfTriangles;
        private static List<Vector3> vertices;
        private static List<Vector2> uvCoords;

        //Generates object Mesh based on received points array and its triangulation
        //At first, vertices are created, y-coordinate of each point is multiplied on the heightMultiplier (if value between 0 and 1),
        //it's necessary for better visualization.
        //Secondly, triangles are created based on list of Triangle objects
        //Finally, uv-coordinates are created
        //Returns Mesh object
        public static Mesh Generate(Vector3[] pointCloud, List<Triangle> triangulation, float heightMultiplier,
            int mapSize, AnimationCurve heightCurve)
        {
            vertIdsOfTriangles = new List<int>();
            vertices = new List<Vector3>();
            uvCoords = new List<Vector2>();
            var newMesh = new Mesh();

            CreateVerts(pointCloud, heightMultiplier, newMesh, heightCurve);
            CreateTris(vertices.ToArray(), triangulation, newMesh);
            CreateUvCoords(mapSize, newMesh);

            return newMesh;
        }

        private static void CreateVerts(Vector3[] pointCloud, float heightMultiplier, Mesh mesh, AnimationCurve heightCurve)
        {
            foreach (var point in pointCloud)
            {
                vertices.Add(new Vector3(point.x, heightCurve.Evaluate(point.y) * heightMultiplier, point.z));
            }
            mesh.vertices = vertices.ToArray();
        }

        //Function creates triangles based on received triangulation
        //Assigns triangles to the received mesh
        private static void CreateTris(Vector3[] pointCloud, List<Triangle> triangulation, Mesh mesh)
        {
            foreach (var tris in triangulation)
            {
                for (var i = 0; i < pointCloud.Length; i++)
                {
                    if (tris.V0.x.Equals(pointCloud[i].x) && tris.V0.y.Equals(pointCloud[i].z))
                    {
                        vertIdsOfTriangles.Add(i);
                    }
                }

                for (var i = 0; i < pointCloud.Length; i++)
                {
                    if (tris.V1.x.Equals(pointCloud[i].x) && tris.V1.y.Equals(pointCloud[i].z))
                    {
                        vertIdsOfTriangles.Add(i);
                    }
                }

                for (var i = 0; i < pointCloud.Length; i++)
                {
                    if (tris.V2.x.Equals(pointCloud[i].x) && tris.V2.y.Equals(pointCloud[i].z))
                    {
                        vertIdsOfTriangles.Add(i);
                    }
                }
            }

            mesh.triangles = vertIdsOfTriangles.ToArray();
            mesh.RecalculateNormals();
        }

        //This function creates uv-coordinates for generated mesh
        //Assigns coordinates to the received mesh
        private static void CreateUvCoords(int mapSize, Mesh mesh)
        {
            foreach (var point in mesh.vertices)
            {
                uvCoords.Add(new Vector2(point.x / mapSize, point.z / mapSize));
            }
            mesh.uv = uvCoords.ToArray();
        }
    }
}