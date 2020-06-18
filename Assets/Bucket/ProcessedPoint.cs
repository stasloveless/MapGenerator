using UnityEngine;

namespace Bucket
{
    public class ProcessedPoint
    {
        private Vector2 coordinates;
        private bool rowStatus;
        private bool columnStatus;

        public ProcessedPoint(Vector2 coordinates)
        {
            this.coordinates = coordinates;
            rowStatus = true;
            columnStatus = true;
        }
    }
}