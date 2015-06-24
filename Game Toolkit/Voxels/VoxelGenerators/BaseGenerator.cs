using System.Diagnostics;
using System.Threading.Tasks;
using Voxels.Objects;

namespace Voxels.VoxelGenerators
{
    public abstract class VoxelGenerator
    {
        public abstract Voxel GenerateVoxel(IntVector3 position);

        public virtual VoxelVolume GenerateVolume(IntVector3 volumeSizeInVoxels, int lod = 1)
        {
            var pos = new IntVector3();
            var result = new VoxelVolume();

			for (pos.X = 0; pos.X < volumeSizeInVoxels.X; pos.X += lod)
				for (pos.Y = 0; pos.Y < volumeSizeInVoxels.Y; pos.Y += lod)
					for (pos.Z = 0; pos.Z < volumeSizeInVoxels.Z; pos.Z += lod)
                        result.SetVoxel(pos, GenerateVoxel(pos));

            return result;
        }

		public virtual Chunk GenerateChunk(IntVector3 start, IntVector3 end, int lod = 1)
        {
			Debug.WriteLine("Generating Chunk at " + start.ToString());
            var chunk = new Chunk(start, end);
            var pos = new IntVector3(start);

			for (pos.X = chunk.Start.X; pos.X < chunk.End.X; pos.X += lod)
				for (pos.Y = chunk.Start.Y; pos.Y < chunk.End.Y; pos.Y += lod)
					for (pos.Z = chunk.Start.Z; pos.Z < chunk.End.Z; pos.Z += lod)
                        chunk.SetVoxel(pos, GenerateVoxel(pos));

            return chunk;
        }

        public virtual Task<VoxelVolume> GenerateVolumeAsync(IntVector3 position, int lod = 1)
        {
			return Task.Factory.StartNew(() => { return GenerateVolume(position, lod); });
        }

		public virtual Task<Chunk> GenerateChunkAsync(IntVector3 start, IntVector3 end, int lod = 1)
        {         
			return Task.Factory.StartNew(() => { return GenerateChunk(start, end, lod); });
        }
    }
}
