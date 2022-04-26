using System;
using UnityEngine;
using Utils.Directions;

namespace Code.Data
{
    public class RoadWayModel : ScriptableObject, IEquatable<RoadWayModel>
    {
        public RoadWayDirection Direction;
        public Vector2[] Points;

        public bool Equals(RoadWayModel other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }
            
            return base.Equals(other) && Equals(Direction, other.Direction);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            
            return Equals((RoadWayModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ (Direction != null ? Direction.GetHashCode() : 0);
            }
        }
    }
}