using System;
using UnityEngine;

namespace Assets.Scripts.Level
{
    public struct Position : IEquatable<Position>
    {
        private readonly int _x;
        private readonly int _z;

        public Position(int x, int z)
        {
            _x = x;
            _z = z;
        }

        public int X
        {
            get { return _x; }
        }

        public int Z
        {
            get { return _z; }
        }

        public bool Equals(Position other)
        {
            return _x == other._x && _z == other._z;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_x*397) ^ _z;
            }
        }

        public static bool operator ==(Position left, Position right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Position left, Position right)
        {
            return !left.Equals(right);
        }

        public static Position operator +(Position left, Position right)
        {
            return new Position(left._x + right._x, left._z + right._z);
        }

        public static Position operator -(Position left, Position right)
        {
            return new Position(left._x - right._x, left._z - right._z);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return obj is Position && Equals((Position) obj);
        }
    }
}
