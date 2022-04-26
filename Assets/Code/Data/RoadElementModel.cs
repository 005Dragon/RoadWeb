using UnityEngine;

namespace Code.Data
{
    public class RoadElementModel : ScriptableObject
    {
        public RoadWayDirection[] Directions;
        public float Width;
        public float LineWidth;
        public Sprite Sprite;
    }
}