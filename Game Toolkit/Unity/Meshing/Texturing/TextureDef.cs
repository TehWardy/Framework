using UnityEngine;
using Voxels.Objects;

namespace Engine.MeshGeneration.Texturing
{
    /// <summary>
    /// Represents an area within the atlas texture 
    /// from which a single texture can be pulled.
    /// </summary>
    public class TextureDef
    {
        /// <summary>
        /// The voxel block type to use this texture for.
        /// </summary>
        public byte VoxelType { get; set; }

        /// <summary>
        /// Faces this texture should be applied to on voxels of the above type.
        /// </summary>
        public Face[] Faces { get; set; }

        /// <summary>
        /// Atlas start ref
        /// </summary>
        public Vector2[] Bounds { get; set; }
    }
}
