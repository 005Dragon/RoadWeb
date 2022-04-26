using System;
using System.Linq;
using Code.Data;
using Code.Storage;
using UnityEngine;
using Utils.Directions;
using Vector2 = UnityEngine.Vector2;

namespace Code.Controllers.Roads
{
    public class RoadBuilderController : MonoBehaviour
    {
        [SerializeField] private StorageController _storage;
        [SerializeField] private RoadWebController _roadWebController;
        [SerializeField] private RoadElementController _roadElementTemplate;
        [SerializeField] private Color _enableBuildColor;
        [SerializeField] private Color _disableBuildColor;
        
        private Camera _camera;

        private bool _buildProcessed;
        private RoadElementBuilder _fromRoadBuilder;
        private RoadElementBuilder _toRoadBuilder;
        
        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Start()
        {
            _fromRoadBuilder = CreateBuilder();
            _toRoadBuilder = CreateBuilder();
            
            _fromRoadBuilder.SetColor(_enableBuildColor);
            _toRoadBuilder.SetColor(_enableBuildColor);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!_buildProcessed)
                {
                    _buildProcessed = true;
                    _fromRoadBuilder.SetPosition(GetPosition());
                }
                else if (_fromRoadBuilder.Valid && _toRoadBuilder.Valid)
                {
                    _buildProcessed = false;
                    _fromRoadBuilder.SetActive(false);
                    _toRoadBuilder.SetActive(false);

                    Apply(_fromRoadBuilder);
                    Apply(_toRoadBuilder);
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                _buildProcessed = false;
                _fromRoadBuilder.SetActive(false);
                _toRoadBuilder.SetActive(false);
            }

            if (_buildProcessed)
            {
                Direction8 fromDirection = GetDirectionRelationPosition(_fromRoadBuilder.Position);
                Direction8 toDirection = fromDirection.Inverse();

                _fromRoadBuilder.Direction = new RoadWayDirection(fromDirection, fromDirection);
                _toRoadBuilder.Direction = new RoadWayDirection(toDirection, toDirection);
                
                _toRoadBuilder.SetPosition(GetPositionFromDirection(_fromRoadBuilder.Position, fromDirection));
                
                _fromRoadBuilder.SetColor(_fromRoadBuilder.Validate() ? _enableBuildColor : _disableBuildColor);
                _toRoadBuilder.SetColor(_toRoadBuilder.Validate() ? _enableBuildColor : _disableBuildColor);
                
                _fromRoadBuilder.SetActive(true);
                _toRoadBuilder.SetActive(true);
            }
        }

        private void Apply(RoadElementBuilder builder)
        {
            bool elementExists =
                _roadWebController.TryGetRoadElement(builder.Position, out RoadElementController roadElement);
            
            if (!elementExists)
            {
                roadElement = Instantiate(_roadElementTemplate);
                roadElement.SetPosition(builder.Position);
            }

            roadElement.SetModel(
                _storage.Roads.GetElement(builder.GetRoadWayDirections().ToArray()),
                _storage.Roads.GetElement()
            );

            if (!elementExists)
            {
                _roadWebController.AddRoadElement(roadElement);
            }
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

        private Vector2Int GetPositionFromDirection(Vector2Int position, Direction8 direction)
        {
            switch (direction)
            {
                case Direction8.Right: return position + new Vector2Int(1, 0);
                case Direction8.DownRight: return position + new Vector2Int(1, -1);
                case Direction8.Down: return position + new Vector2Int(0, -1);
                case Direction8.DownLeft: return position + new Vector2Int(-1, -1);
                case Direction8.Left: return position + new Vector2Int(-1, 0);
                case Direction8.UpLeft: return position + new Vector2Int(-1, 1);
                case Direction8.Up: return position + new Vector2Int(0, 1);
                case Direction8.UpRight: return position + new Vector2Int(1, 1);
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        private RoadElementBuilder CreateBuilder()
        {
            return new RoadElementBuilder(
                Instantiate(_roadElementTemplate, transform, true),
                position => _roadWebController.TryGetRoadElement(position, out var roadElement) ? roadElement : null,
                _storage.Roads
            );
        }
    }
    // public class RoadBuilderController : MonoBehaviour
    // {
    //     [SerializeField] private StorageController _storage;
    //     [SerializeField] private RoadWebController _roadWebController;
    //     [SerializeField] private RoadElementController _roadElementTemplate;
    //     [SerializeField] private RoadSignController _roadSignTemplate;
    //
    //     private Camera _camera;
    //     
    //     private void Awake()
    //     {
    //         _camera = Camera.main;
    //     }
    //
    //     private void Update()
    //     {
    //         if (Input.GetMouseButtonDown(0))
    //         {
    //             var position = GetPosition();
    //         
    //             if (_roadWebController.TryGetRoadElement(position, out _))
    //             {
    //                 return;
    //             }
    //
    //             RoadElementController roadElement = Instantiate(_roadElementTemplate);
    //             roadElement.SetPosition(position);
    //         
    //             _roadWebController.AddRoadElement(roadElement);
    //         }
    //
    //         if (Input.GetMouseButtonDown(1))
    //         {
    //             var position = GetPosition();
    //
    //             if (!_roadWebController.TryGetRoadElement(position, out RoadElementController roadElement))
    //             {
    //                 return;
    //             }
    //             
    //             Vector2 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
    //             IEnumerable<RoadWayModel> roadWayModels = 
    //                 roadElement.Model.Directions.Select(_storage.RoadWays.GetElement);
    //
    //             Direction8 direction = GetSignDirection(mousePosition - position, roadWayModels);
    //
    //             RoadSignController roadSign = Instantiate(_roadSignTemplate);
    //             roadSign.SetModel(
    //                 _storage.RoadSigns.GetElement(RoadSign.BusStop),
    //                 roadElement,
    //                 direction
    //             );
    //             
    //             _roadWebController.AddRoadSign(roadSign);
    //         }
    //     }
    //
    //     private Direction8 GetSignDirection(Vector2 position, IEnumerable<RoadWayModel> roadWays)
    //     {
    //         float minDistance = float.MaxValue;
    //         RoadWayModel minDistanceRoadWay = null;
    //         
    //         foreach (RoadWayModel roadWay in roadWays)
    //         {
    //             foreach (Vector2 point in roadWay.Points)
    //             {
    //                 float distance = (position -  point).magnitude;
    //
    //                 if (distance < minDistance)
    //                 {
    //                     minDistance = distance;
    //                     minDistanceRoadWay = roadWay;
    //                 }
    //             }
    //         }
    //
    //         // ReSharper disable once PossibleNullReferenceException
    //         return minDistanceRoadWay.Direction.From;
    //     }
    // }
}