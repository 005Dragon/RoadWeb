using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(fileName = "Road sign", menuName = "Storage elements/Road sign")]
    public class RoadSignModel : ScriptableObject
    {
        public RoadSign Sign;
        public Sprite Sprite;
    }
}