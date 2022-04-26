using System.Collections.Generic;
using UnityEngine;

namespace Code.Controllers.Roads
{
    public class RoadWebController : MonoBehaviour
    {
        private readonly Dictionary<Vector2Int, RoadElementController> _positionToRoadElementIndex =
            new Dictionary<Vector2Int, RoadElementController>();

        private readonly Dictionary<Vector2Int, List<RoadSignController>> _positionToRoadSignsIndex =
            new Dictionary<Vector2Int, List<RoadSignController>>();

        public List<RoadSignController> GetRoadSigns(Vector2Int position)
        {
            if (!_positionToRoadSignsIndex.TryGetValue(position, out List<RoadSignController> result))
            {
                result = new List<RoadSignController>();
            }

            return result;
        }
        
        public bool TryGetRoadElement(Vector2Int position, out RoadElementController roadElement)
        {
            return _positionToRoadElementIndex.TryGetValue(position, out roadElement);
        }

        public void AddRoadSign(RoadSignController roadSign)
        {
            bool indexExists = _positionToRoadSignsIndex.TryGetValue(
                roadSign.Position,
                out List<RoadSignController> roadSignControllers
            );

            if (!indexExists)
            {
                roadSignControllers = new List<RoadSignController>();
                _positionToRoadSignsIndex[roadSign.Position] = roadSignControllers;
            }
            
            roadSignControllers.Add(roadSign);
        }

        public void AddRoadElement(RoadElementController roadElement)
        {
            Transform roadElementTransform = roadElement.transform;
        
            _positionToRoadElementIndex.Add(roadElement.Position, roadElement);
        
            roadElementTransform.SetParent(transform);
        }
    }
}