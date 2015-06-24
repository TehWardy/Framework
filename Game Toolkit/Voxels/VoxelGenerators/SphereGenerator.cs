using Voxels.Objects;

namespace Voxels.VoxelGenerators
{
    public class SphereGenerator : VoxelGenerator
    {
        public int Size { get; private set; }

        public SphereGenerator(int radius) : base()
        {
            Size = radius;
        }

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
            if (distance < Size)
			return new Voxel { Weight = 255 };

            return Voxel.Empty;
        }
    }
}