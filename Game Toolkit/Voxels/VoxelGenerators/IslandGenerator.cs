using Noise;
using Noise.Modules;
using Voxels.Objects;

namespace Voxels.VoxelGenerators
{
    public class IslandGenerator : VoxelGenerator
    {
        public int Seed { get; private set; }
        public int Size { get; private set; }

        //ModuleBase gen;
        float half;
        
        public IslandGenerator(int seed, int size) : base()
        {
            Seed = seed;
            Size = size;
            half = (float)size / 2f;
            //BuildModules();
        }

        /*
        void BuildModules()
        {
            gen = new Perlin
            {
                Seed = Seed,
                OctaveCount = 2,
                Frequency = 0.06f
            };
        }
         */

        public override Chunk GenerateChunk(IntVector3 start, IntVector3 end, int lod = 1)
        {
            var result = new Chunk(new IntVector3(start), new IntVector3(end));
            var pos = new IntVector3(start);

            // Iterate over every voxel of our volume
            for (int x = result.Start.X; x <= result.End.X; x++)
            {
                pos.X = x;
                for (int z = result.Start.Z; z <= result.End.Z; z++)
                {
                    pos.Z = z;
                    for (int y = result.Start.Y; y <= result.End.Y; y++)
                    {
                        pos.Y = y;
                        result.SetVoxel(pos, GenerateVoxel(pos));
                    }
                }
            }

            return result;
        }

        public override Voxel GenerateVoxel(IntVector3 pos)
        {
            var distance = pos.DistanceTo(IntVector3.Zero);
            if (distance < half)
			return new Voxel { Weight = 255 };

            /*
            var polarPos = SphericalPosition.FromCartesian(pos.X, pos.Y, pos.Z);

            if (polarPos.Radius < half)
            {
                //var noiseVal = (float)gen.GetValue(polarPos.Azimuth, polarPos.Inclination, Seed);
                //var radHere = (float)(noiseVal + 1f / 2f) * Size;

                //if (radHere < half)
                    return new Voxel(1f);
            }

            // the easy way to generate a sphere
            //if(pos.DistanceTo(IntVector3.Zero) < half)
            //    return new Voxel(1f);
                */

            return Voxel.Empty;
        }
    }
}