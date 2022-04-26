using System;
using System.Collections.Generic;
using System.Linq;
using Code.Data;
using Code.Storage;
using UnityEngine;
using Utils.Directions;

namespace Code.Controllers.Roads
{
    public class RoadElementBuilder
    {
        public RoadWayDirection Direction { get; set; }
        public Vector2Int Position => _roadElement.Position;
        
        public bool Valid { get; private set; }

        private readonly RoadStorageController _storage;
        private readonly RoadElementController _roadElement;
        private readonly Func<Vector2Int, RoadElementController> _getRoadElement;

        public RoadElementBuilder(
            RoadElementController roadElement, 
            Func<Vector2Int, RoadElementController> getRoadElement, 
            RoadStorageController storage)
        {
            _roadElement = roadElement;
            _getRoadElement = getRoadElement;
            _storage = storage;
        }

        public void SetPosition(Vector2Int position) => _roadElement.SetPosition(position);
        public void SetColor(Color color) => _roadElement.Color = color;
        public void SetActive(bool value)
        {
            if (value)
            {
                RoadWayDirection[] roadWayDirections = GetRoadWayDirections().ToArray();
                
                _roadElement.SetModel(_storage.GetElement(roadWayDirections), _storage.GetElement());
            }

            foreach (SpriteRenderer spriteRenderer in _roadElement.GetRenderers())
            {
                spriteRenderer.enabled = value;
            }
        }

        public bool Validate()
        {
            Valid = true;
            bool isDiagonal = false;

            foreach (Direction8 direction in Utils.Directions.Direction.Diagonals.Select(x => x.ToDirection8()))
            {
                if (Direction.From == direction)
                {
                    isDiagonal = true;
                    break;
                }
                
                if (Direction.To == direction)
                {
                    isDiagonal = true;
                    break;
                }
            }

            if (isDiagonal)
            {
                if (CheckRelativeRoadElements(Direction.From) || CheckRelativeRoadElements(Direction.To))
                {
                    Valid = false;
                }
            }
            
            return Valid;
        }

        public IEnumerable<RoadWayDirection> GetRoadWayDirections()
        {
            RoadElementController roadElement = _getRoadElement.Invoke(_roadElement.Position);

            if (roadElement == null)
            {
                yield return Direction;
            }
            else
            {
                RoadWayDirection firstDirection = roadElement.Model.Directions[0];
                
                if (roadElement.Model.Directions.Length == 1)
                {
                    yield return new RoadWayDirection(Direction.From, firstDirection.To);
                    yield return new RoadWayDirection(firstDirection.From, Direction.To);
                }
                else 
                {
                    yield return Direction;
                    
                    if (roadElement.Model.Directions.Length == 2)
                    {
                        yield return new RoadWayDirection(firstDirection.From, firstDirection.From);
                        yield return new RoadWayDirection(firstDirection.To, firstDirection.To);
                    }
                    
                    foreach (RoadWayDirection direction in roadElement.Model.Directions)
                    {
                        yield return direction;
                        yield return new RoadWayDirection(Direction.From, direction.To);
                        yield return new RoadWayDirection(direction.From, Direction.To);
                    }
                }
            }
        }

        private void GetComponentDirection(Direction8 direction, out Vector2Int position1, out Vector2Int position2)
        {
            switch (direction)
            {
                case Direction8.DownRight:
                    position1 = Position + new Vector2Int(0, -1);
                    position2 = Position + new Vector2Int(1, 0);
                    break;
                case Direction8.DownLeft:
                    position1 = Position + new Vector2Int(0, -1);
                    position2 = Position + new Vector2Int(-1, 0);
                    break;
                case Direction8.UpLeft:
                    position1 = Position + new Vector2Int(0, 1);
                    position2 = Position + new Vector2Int(-1, 0);
                    break;
                case Direction8.UpRight:
                    position1 = Position + new Vector2Int(0, 1);
                    position2 = Position + new Vector2Int(1, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        private bool CheckRelativeRoadElements(Direction8 direction)
        {
            GetComponentDirection(direction, out Vector2Int position1, out Vector2Int position2);

            return _getRoadElement.Invoke(position1) != null && _getRoadElement.Invoke(position2) != null;
        }
    }
}