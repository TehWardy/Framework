
using Noise;
using Noise.Modules;
using Voxels.Objects;

namespace Voxels.VoxelGenerators
{
    public class TerrainGenerator : VoxelGenerator
    {
        public ModuleBase NoiseGen { get; set; }
        public int Seed { get; set; }
        public float MaxHeight { get; set; }

		public override Chunk GenerateChunk(IntVector3 start, IntVector3 end, int lod = 1)
        {
            var result = new Chunk(new IntVector3(start), new IntVector3(end));

            if (NoiseGen == null)
                return result;

            float pointHeight = 0f;
            int y = 0;

            var pos = new IntVector3(start);

            // Iterate over every voxel of our volume
            for (int z = result.Start.Z; z <= result.End.Z; z++)
            {
                pos.Z = z;
                for (int x = result.Start.X; x <= result.End.X; x++)
                {
                    pos.X = x;

                    // get height of the terrain at this x,z pos
                    pointHeight = Height(pos.X, pos.Z);

                    if (pointHeight > 0f)
                    {
                        // loop through y voxel positions at x,z
                        for (y = result.Start.Y; y <= result.End.Y; y++)
                        {
                            // generate a voxel for the stuff under the ground within the chunk
                            pos.Y = y;
                            result.SetVoxel(pos, GenerateVoxel(pos, pointHeight));
                        }
                    }
                    else if (result.Start.Y == 0)
                    {
                        // dump in a base rock voxel at y == 0
                        pos.Y = 0;
						result.SetVoxel(pos, new Voxel(255, 0));
                    }
                }
            }

            return result;
        }

        public float Height(int x, int z)
        {
            // always run at least 1 octave of noise
            float pointHeight = (float)NoiseGen.GetValue((double)x, (double)Seed, (double)z);
            pointHeight = (pointHeight + 1f) / 2f; // push to range 0 to 1

            // scale to within the range of our voxel volume and return
            return pointHeight * (float)MaxHeight;
        }

        public override Voxel GenerateVoxel(IntVector3 pos)
        {
            var height = Height(pos.X, pos.Z);
            return GenerateVoxel(pos, height);
        }

        Voxel GenerateVoxel(IntVector3 pos, float height)
        {
            if((int)height+1 == pos.Y)
				return new Voxel((byte)((255 / 100) * (float)((height - (int)height) -0.8f)), (byte)TerrainVoxelType.Dirt);
            else if ((int)height > pos.Y)
				return new Voxel(255, (byte)TerrainVoxelType.Rock);
            else if ((int)height == pos.Y)
				return new Voxel((byte)((255 / 100) * (height - (int)height)), (byte)TerrainVoxelType.Grass);

            return Voxel.Empty;
        }
    }
}