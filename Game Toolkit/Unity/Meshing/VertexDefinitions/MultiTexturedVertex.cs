

using UnityEngine;
namespace Meshing.VertexDefinitions
{
    public struct MultiTexturedVertex
    {
        public Vector3 Position { get; set; }
        public Vector3 TextureWeights { get; set; }
        public Vector2 UV { get; set; }
    }
}
