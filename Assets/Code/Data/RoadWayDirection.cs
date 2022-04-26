using System;
using Utils.Directions;

namespace Code.Data
{
    [Serializable]
    public class RoadWayDirection : IEquatable<RoadWayDirection>
    {
        public Direction8 From;
        public Direction8 To;

        public RoadWayDirection(Direction8 from, Direction8 to)
        {
            From = from;
            To = to;
        }

        public bool Equals(RoadWayDirection other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }
            
            return From == other.From && To == other.To;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RoadWayDirection)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int)From * 8) + (int)To;
            }
        }

        public override string ToString()
        {
            return $"F{(int)From}T{(int)To}";
        }
    }
}