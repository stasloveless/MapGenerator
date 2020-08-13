using UnityEngine;

namespace RamerDuglasPeucker3D
{
    public class ProcessedPoint
    {
        public Vector3 coordinates;
        public bool rowStatus;
        public bool columnStatus;
        public bool intermediateStatus;

        public ProcessedPoint(Vector3 coordinates)
        {
            this.coordinates = coordinates;
            rowStatus = false;
            columnStatus = false;
            intermediateStatus = false;
        }
    }
}