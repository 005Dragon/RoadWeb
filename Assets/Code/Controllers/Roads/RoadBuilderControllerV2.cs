using System;
using Code.Storage;
using UnityEngine;
using UnityEngine.UIElements;
using Utils.Directions;

namespace Code.Controllers.Roads
{
    public class RoadBuilderControllerV2 : MonoBehaviour
    {
        [SerializeField] private RoadElementControllerV2 _template;
        [SerializeField] private StorageController _storage;
        
        private Camera _camera;
        
        private bool _buildProcessed;

        private RoadElementControllerV2 _fromRoadElement;
        private RoadElementControllerV2 _toRoadElement;
        private RoadElementControllerV2 _linkRoadElement;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Start()
        {
            _fromRoadElement = Instantiate(_template, transform, true);
            _toRoadElement = Instantiate(_template, transform, true);
            _linkRoadElement = Instantiate(_template, transform, true);

            _toRoadElement.SortingOrder = -1;
            
            DisableRoadElements();
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                StartBuildProcess();
            }

            if (Input.GetMouseButton(1))
            {
                _buildProcessed = false;
                DisableRoadElements();
            }
        }

        private void StartBuildProcess()
        {
            _fromRoadElement.Position = GetPosition();
            
        }

        private void DisableRoadElements()
        {
            _fromRoadElement.gameObject.SetActive(false);
            _toRoadElement.gameObject.SetActive(false);
            _linkRoadElement.gameObject.SetActive(false);
        }

        private Vector2Int GetPosition()
        {
            Vector2 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);

            int x = (int)mousePosition.x;

            if (mousePosition.x < 0)
            {
                x--;
            }

            int y = (int)mousePosition.y;

            if (mousePosition.y < 0)
            {
                y--;
            }

            return new Vector2Int(x, y);
        }
        
        private Direction8 GetDirectionRelationPosition(Vector2Int position)
        {
            Vector2 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 relationPosition = mousePosition - (position + new Vector2(0.5f, 0.5f));

            float angle = Mathf.Atan2(relationPosition.y, relationPosition.x);

            return Direction.ToDirection8(angle);
        }
    }
}