using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils.Directions;

namespace Code.Controllers.Roads
{
    public class RoadWebCursor
    {
        public Vector2Int Position { get; }

        public IEnumerable<Direction8> Directions => Links.Select(x => x.Key);

        public Dictionary<Direction8, RoadWebCursor> Links { get; } =
            new Dictionary<Direction8, RoadWebCursor>();

        public RoadWebCursor(Vector2Int position)
        {
            Position = position;
        }

        public Vector2Int GetRelativePosition(Direction8 direction)
        {
            switch (direction)
            {
                case Direction8.Right: return new Vector2Int(Position.x + 1, Position.y);
                case Direction8.DownRight: return new Vector2Int(Position.x + 1, Position.y - 1);
                case Direction8.Down: return new Vector2Int(Position.x, Position.y - 1);
                case Direction8.DownLeft: return new Vector2Int(Position.x - 1, Position.y - 1);
                case Direction8.Left: return new Vector2Int(Position.x - 1, Position.y);
                case Direction8.UpLeft: return new Vector2Int(Position.x - 1, Position.y + 1);
                case Direction8.Up: return new Vector2Int(Position.x, Position.y + 1);
                case Direction8.UpRight: return new Vector2Int(Position.x + 1, Position.y + 1);
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
    }
}