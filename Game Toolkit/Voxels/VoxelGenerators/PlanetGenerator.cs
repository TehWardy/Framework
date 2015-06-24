using System;
using Voxels.Objects;
using Voxels;
using Noise;

namespace Voxels.VoxelGenerators
{
    /// <summary>
    /// Generates voxel planets
    /// </summary>
    public class PlanetGenerator : VoxelGenerator
    {
		public int Lod;

		ModuleBase planetGen;

		public PlanetGenerator() : base()
		{
			planetGen = new Noise.Modules.Complex.Planet
			{
				Frequency = 1f,
				Lacunarity = 2,
				OctaveCount = 12,
				Persistence = 0.7f
			};
		}

        public override Voxel GenerateVoxel(IntVector3 position)
        {
			var pos = SphericalPosition.FromCartesian(position.X, position.Y, position.Z);
			var r = Math.Cos(Mathf.Deg2Rad * pos.Inclination);
			var radiusThisWay = planetGen.GetValue(
				r * Math.Cos(Mathf.Deg2Rad * pos.Azimuth), 
               Math.Sin(Mathf.Deg2Rad * pos.Inclination), 
               r * Math.Sin(Mathf.Deg2Rad * pos.Azimuth)
			);

			// this voxel is roughly on the surface (based on current LOD)
			if (pos.Radius > radiusThisWay && pos.Radius < radiusThisWay - (float)Lod) 
            {
                byte weight = (byte)((255 / 100) * ((float)(radiusThisWay - (int)radiusThisWay)));
                return new Voxel(weight, (byte)TerrainVoxelType.Dirt);
            }

			// this voxel is below the surface
			else if (pos.Radius < radiusThisWay) 
				return new Voxel(255, (byte)radiusThisWay);

			// this voxel is above the surface
			return Voxel.Empty; 
        }
    }
}
