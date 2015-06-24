
using UnityEngine;
namespace Meshing.VertexDefinitions
{
    public struct TexturedVertex
    {
        public Vector3 Position { get; set; }
        public Vector2 Uv { get; set; }

        public TexturedVertex(Vector3 position, Vector2 uv) : this()
        {
            Position = position;
            Uv = uv;
        }
    }
}
