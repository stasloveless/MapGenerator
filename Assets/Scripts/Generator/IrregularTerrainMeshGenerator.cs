using System.Collections.Generic;
using RamerDuglasPeucker3D;
using UnityEngine;

namespace Generator
{
    public static class IrregularTerrainMeshGenerator
    {
        private static List<int> _vertIdsOfTriangles;
        private static List<Vector2> _uvCoords;

        //Generates object Mesh based on received points array and its triangulation
        //At first, vertices are created, y-coordinate of each point is multiplied on the heightMultiplier (if value between 0 and 1),
        //it's necessary for better visualization.
        //Secondly, triangles are created based on list of Triangle objects
        //Finally, uv-coordinates are created
        //Returns Mesh object
        public static Mesh Generate(Vector3[] pointCloud, List<Triangle> triangulation,
            int mapSize)
        {
            _vertIdsOfTriangles = new List<int>();
            _uvCoords = new List<Vector2>();
            
            var newMesh = new Mesh {vertices = pointCloud};
            
            CreateTris(pointCloud, triangulation, newMesh);
            CreateUvCoords(mapSize, newMesh);

            return newMesh;
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
                        _vertIdsOfTriangles.Add(i);
                    }
                }

                for (var i = 0; i < pointCloud.Length; i++)
                {
                    if (tris.V1.x.Equals(pointCloud[i].x) && tris.V1.y.Equals(pointCloud[i].z))
                    {
                        _vertIdsOfTriangles.Add(i);
                    }
                }

                for (var i = 0; i < pointCloud.Length; i++)
                {
                    if (tris.V2.x.Equals(pointCloud[i].x) && tris.V2.y.Equals(pointCloud[i].z))
                    {
                        _vertIdsOfTriangles.Add(i);
                    }
                }
            }

            mesh.triangles = _vertIdsOfTriangles.ToArray();
            mesh.RecalculateNormals();
        }

        //This function creates uv-coordinates for generated mesh
        //Assigns coordinates to the received mesh
        private static void CreateUvCoords(int mapSize, Mesh mesh)
        {
            foreach (var point in mesh.vertices)
            {
                _uvCoords.Add(new Vector2(point.x / mapSize, point.z / mapSize));
            }
            mesh.uv = _uvCoords.ToArray();
        }
    }
}