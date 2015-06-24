using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Voxels.Objects;

namespace Meshing.Generators
{
    public abstract class MeshGenerator
    {
        public MeshGenerator()
        {

        }

        public abstract Mesh GenerateMesh(VoxelContainer data, IntVector3 start, IntVector3 end);
    }
}
