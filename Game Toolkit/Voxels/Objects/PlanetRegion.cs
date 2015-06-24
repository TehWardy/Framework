using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Voxels.Objects
{
    public class PlanetRegion
    {
        public float North { get; set; }
        public float South { get; set; }
        public float West { get; set; }
        public float East { get; set; }

        public PlanetRegion ToRadians()
        {
            var radian = (float)(Math.PI * 2) / 360f;
            return new PlanetRegion {
                West = West * radian,
                East = East * radian,
                North = North * radian,
                South = South * radian
            };
        }

		public override string ToString ()
		{
			return string.Format ("[PlanetRegion: North={0}, South={1}, West={2}, East={3}]", North, South, West, East);
		}
    }
}
