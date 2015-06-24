using System.Collections.Generic;

namespace Voxels.Objects
{
    public class Chunk : VoxelContainer
    {
        public Chunk()
            : base()
        {

        }

        public Chunk(IntVector3 start, IntVector3 end)
            : base(start, end)
        {

        }
    }
}