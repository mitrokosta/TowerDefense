using System.Collections.Generic;
using Enemy;
using UnityEngine;

namespace Field
{
    public class Node
    {
        public Vector3 Position;
        
        public Node NextNode;
        public bool IsOccupied;

        public float PathWeight;

        public List<EnemyData> EnemyDatas;

        public OccupationAvailability m_OccupationAvailability;

        public Node(Vector3 position)
        {
            Position = position;
            EnemyDatas = new List<EnemyData>();
        }

        public void ResetWeight()
        {
            PathWeight = float.MaxValue;
        }
    }
}