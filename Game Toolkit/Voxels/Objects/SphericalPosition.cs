using System;

namespace Voxels.Objects
{
    /// <summary>
    /// SphericalPosition type that stores the radius, inclination and azimuth.
    /// Provides methods to convert to appropriate types.
    /// </summary>
    public class SphericalPosition
    {
        public float Radius { get; set; }
        public float Inclination { get;set; }
        public float Azimuth { get; set; }

        /// <summary>
        /// Default Ctor
        /// </summary>
        public SphericalPosition()
        {

        }

        /// <summary>
        /// Constructs a SphericalPosition based upon the values passed in
        /// </summary>
        /// <param name="radius">The radius</param>
        /// <param name="inclination">The inclination</param>
        /// <param name="azimuth">The azimuth</param>
        public SphericalPosition(float radius, float inclination, float azimuth)
        {
            Radius = radius;
            Inclination = inclination;
            Azimuth = azimuth;
        }

        /// <summary>
        /// Constructs a SphericalPosition based upon a set of Cartesian coordinates (x, y, z)
        /// </summary>
        /// <param name="cartesian">The Cartesian coodinates</param>
        public static SphericalPosition FromCartesian(float x, float y, float z)
        {
            var r = (float)(Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2)));

            var result = new SphericalPosition
            {
                Radius = r,
                Inclination = (float)(Math.Acos(z / r)),
                Azimuth = (float)(Math.Atan(y / x))
            };

            if (float.IsNaN(result.Radius)) result.Radius = 0;
            if (float.IsNaN(result.Inclination)) result.Inclination = 0;
            if (float.IsNaN(result.Azimuth)) result.Azimuth = 0;

            return result;
        }

        /// <summary>
        /// Converts the SphericalPosition to Cartesian coordinates
        /// </summary>
        /// <returns>A Vector3 containing the Cartesian coordinates</returns>
        public void ToCartesian(out float x, out float y, out float z)
        {
            x = (float)(Radius * Math.Sin(Inclination) * Math.Cos(Azimuth));
            y = (float)(Radius * Math.Sin(Inclination) * Math.Sin(Azimuth));
            z = (float)(Radius * Math.Cos(Inclination));

            if (x == float.NaN) x = 0;
            if (y == float.NaN) y = 0;
            if (z == float.NaN) z = 0;
        }

        public override string ToString()
        {
            return "(az:" + Azimuth + ", Inc:" + Inclination + ", Rad:" + Radius + ")";
        }
    }
}
