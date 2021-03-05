using UnityEngine;

namespace Field
{
    public class Connection
    {
        private Vector2Int m_Coordinate;
        private float m_Weight;

        public Vector2Int Coordinate => m_Coordinate;

        public float Weight => m_Weight;

        public Connection(Vector2Int coordinate, float weight)
        {
            m_Coordinate = coordinate;
            m_Weight = weight;
        }
    }
}