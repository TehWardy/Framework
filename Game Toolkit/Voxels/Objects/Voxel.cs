using System;
using System.Collections.Generic;
using System.Linq;

namespace Voxels.Objects
{
    /// <summary>
    /// Basic Voxel
    /// </summary>
    public struct Voxel : IEquatable<Voxel>
    {
		static Voxel _empty = new Voxel(0, (byte)VoxelType.Empty);
		public static Voxel Empty { get { return _empty; } }

        /// <summary>
        /// Determines how much of the voxel area is filled with type of material 
        /// determined by Type below
        /// </summary>
        /// <value>The weight.</value>
        public byte Weight { get; set; }

		/// <summary>
		/// Gets or sets the type of the voxel.
		/// This could be "dirt", "Rock", "Wood", ect
		/// </summary>
		/// <value>The type of the block.</value>
		public byte Type { get; set; }

		public Voxel (byte weight, byte type) : this()
		{
			Weight = weight;
			Type = type;
		}

        public static bool operator ==(Voxel first, Voxel second)
        {
            if ((object)first == null)
                return false;

            return first.Equals(second);
        }

        public static bool operator !=(Voxel first, Voxel second)
        {
            if ((object)first == null)
                return false;

            return !first.Equals(second);
        }

        public bool Equals(Voxel other)
        {
            // can't really do equality on floats so lets say within 0.0001 is considered "equal enough"
            return other.Weight > (Weight - 0.0001) && 
				   other.Weight < (Weight + 0.0001) && 
				   Type == other.Type;
        }

		public override bool Equals (object obj)
		{
			if (obj == null || !(obj is Voxel))
				return false;

			return Equals((Voxel)obj);
		}

        public override int GetHashCode()
        {
            return Weight.GetHashCode();
        }
    }
}