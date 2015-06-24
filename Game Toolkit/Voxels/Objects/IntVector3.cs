
using System;
namespace Voxels.Objects
{
    public struct IntVector3 : IEquatable<IntVector3>
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        static IntVector3 zero = new IntVector3();
        public static IntVector3 Zero { get { return zero; } }

        /// <summary>
        /// builds from given values
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public IntVector3(int x, int y, int z)
            : this()
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Builds copy of given intvector3
        /// </summary>
        /// <param name="pos"></param>
        public IntVector3(IntVector3 pos) : this()
        {
            X = pos.X;
            Y = pos.Y;
            Z = pos.Z;
        }

        public bool IsWithin(IntVector3 pos1, IntVector3 pos2)
        {
            bool min = X >= pos1.X && Y >= pos1.Y && Z >= pos1.Z,
                 max = X <= pos2.X && Y <= pos2.Y && Z <= pos2.Z;
            return min && max;
        }

        /// <summary>
        /// Creates an IntVector3 by taking the lowest values from the given IntVector3 pair.
        /// </summary>
        /// <returns>The from.</returns>
        /// <param name="a">The a component.</param>
        /// <param name="b">The a component.</param>
        public static IntVector3 LowsFrom(IntVector3 a, IntVector3 b)
        {
            int X = (a.X < b.X ? a.X : b.X);
            int Y = (a.Y < b.Y ? a.Y : b.Y);
            int Z = (a.Z < b.Z ? a.Z : b.Z);
            return new IntVector3(X, Y, Z);
        }

        public static IntVector3 HighsFrom(IntVector3 a, IntVector3 b)
        {
            int X = (a.X > b.X ? a.X : b.X);
            int Y = (a.Y > b.Y ? a.Y : b.Y);
            int Z = (a.Z > b.Z ? a.Z : b.Z);
            return new IntVector3(X, Y, Z);
        }

        public static float Distance(IntVector3 v1, IntVector3 v2)
        {
            return (float)Math.Sqrt(
                   ((float)v1.X - (float)v2.X) * ((float)v1.X - (float)v2.X) +
                   ((float)v1.Y - (float)v2.Y) * ((float)v1.Y - (float)v2.Y) +
                   ((float)v1.Z - (float)v2.Z) * ((float)v1.Z - (float)v2.Z)
               );
        }

        public float DistanceTo(IntVector3 other)
        {
            return Distance(this, other);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "(" + X + ", " + Y + ", " + Z + ")";
        }

        public static IntVector3 operator +(IntVector3 first, IntVector3 second)
        {
            return new IntVector3(first.X + second.X, first.Y + second.Y, first.Z + second.Z);
        }

        public static IntVector3 operator -(IntVector3 first, IntVector3 second)
        {
            return new IntVector3(first.X - second.X, first.Y - second.Y, first.Z - second.Z);
        }

        public static IntVector3 operator -(IntVector3 value, int amount)
        {
            return new IntVector3(value.X - amount, value.Y - amount, value.Z - amount);
        }

        public static IntVector3 operator +(IntVector3 value, int amount)
        {
            return new IntVector3(value.X + amount, value.Y + amount, value.Z + amount);
        }

        public static IntVector3 operator /(IntVector3 first, IntVector3 second)
        {
            return new IntVector3(first.X / second.X, first.Y / second.Y, first.Z / second.Z);
        }

        public static IntVector3 operator /(IntVector3 value, int amount)
        {
            return new IntVector3(value.X / amount, value.Y / amount, value.Z / amount);
        }

        public static IntVector3 operator *(IntVector3 value, int amount)
        {
            return new IntVector3(value.X * amount, value.Y * amount, value.Z * amount);
        }

        public static bool operator ==(IntVector3 first, IntVector3 second)
        {
            object a = first as object;
            object b = second as object;

            if (a == null || b == null)
                return false;

            return first.X == second.X &&
				   first.Y == second.Y &&
				   first.Z == second.Z;
        }

        public static bool operator !=(IntVector3 first, IntVector3 second)
        {
            return !(first == second);
        }

        public override bool Equals(object obj)
        {
            if (obj is IntVector3)
                return this == (IntVector3)obj;

            return false;
        }

		public override int GetHashCode()
		{
			unchecked
			{
				int result = X * 73856093;
				result = result ^ (Y * 19349663);
				result = result ^ (Z * 83492791);
				return  result;
			}
		}

        public bool Equals(IntVector3 other)
        {
            return this == other;
        }
    }
}