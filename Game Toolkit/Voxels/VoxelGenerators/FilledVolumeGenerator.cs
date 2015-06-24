
using Voxels.Objects;

namespace Voxels.VoxelGenerators
{
    /// <summary>
    /// Always returns a voxel with a weight of 1
    /// </summary>
    public class FilledVolumeGenerator : VoxelGenerator
    {
        public override Voxel GenerateVoxel(IntVector3 position)
        {
            return new Voxel { Weight = 255 };
        }
    }
}