using System.Runtime.InteropServices;
using UnityEngine;

namespace Voxels.DXWrapper.VertexDefinitions
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ColouredVertex
    {
        public Vector3 Position { get; set; }
        public int Colour { get; set; }
    }
}
