using Engine.MeshGeneration.Texturing;
using Meshing.VertexDefinitions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Voxels.Objects;

namespace Meshing
{
    public static class VoxelHelper
    {
        /// <summary>
        /// Returns faces that are visible for the voxel at the given index in the given volume.
        /// </summary>
        /// <returns>The faces for.</returns>
        /// <param name="volume">Volume.</param>
        /// <param name="voxel">Voxel.</param>
        public static IEnumerable<Face> VisibleFacesFor(VoxelContainer chunk, IntVector3 voxel)
        {
            var result = new List<Face>();
            var faceVector = new IntVector3(voxel);

            if (chunk.GetVoxel(voxel).Weight > 0)
            {
                faceVector.Y += 1;
                if (chunk.GetVoxel(faceVector).Weight <= 0)
                    result.Add(Face.Top);

                faceVector.Y -= 2;
                if (chunk.GetVoxel(faceVector).Weight <= 0)
                    result.Add(Face.Bottom);

                faceVector.Y += 1;
                faceVector.X -= 1;
                if (chunk.GetVoxel(faceVector).Weight <= 0)
                    result.Add(Face.Left);

                faceVector.X += 2;
                if (chunk.GetVoxel(faceVector).Weight <= 0)
                    result.Add(Face.Right);

                faceVector.X -= 1;
                faceVector.Z -= 1;
                if (chunk.GetVoxel(faceVector).Weight <= 0)
                    result.Add(Face.Front);

                faceVector.Z += 2;
                if (chunk.GetVoxel(faceVector).Weight <= 0)
                    result.Add(Face.Back);
            }

            return result;
        }

        /// <summary>
        /// Determines if the voxel is considered to be in an "active" state
        /// at the given position in the voxel array.
        /// </summary>
        /// <param name="voxels">the array</param>
        /// <param name="pos">the position</param>
        /// <returns></returns>
        public static bool IsActive(Voxel[, ,] voxels, IntVector3 pos)
        {
            if (InBounds(voxels, pos))
            {
                return voxels[pos.X, pos.Y, pos.Z].Weight > 0;
            }

            return false;
        }

        /// <summary>
        /// Determines if the given IntVector3 is within the bounds of the given voxel array
        /// </summary>
        /// <param name="voxels">the array</param>
        /// <param name="pos">the IntVector3</param>
        /// <returns></returns>
        public static bool InBounds(Voxel[, ,] voxels, IntVector3 pos)
        {
            return (pos.X >= 0 && pos.Y >= 0 && pos.Z >= 0) &&
               (pos.X < voxels.GetLength(0) && pos.Y < voxels.GetLength(1) && pos.Z < voxels.GetLength(2));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="voxels"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static IEnumerable<Vector3> Verts(Voxel[, ,] voxels, IntVector3 index)
        {
            // assuming this block is not null or empty we can proceed
            if (voxels[index.X, index.Y, index.Z].Weight > 0)
            {                                       // Visibility rules for faces are ... 
                if (IsVisibleFace(voxels, index, Face.Top))
                {
                    var verts = FaceVerts(index, Face.Top);
                    foreach (Vector3 vert in verts)
                    {
                        yield return vert;
                    }
                }

                if (IsVisibleFace(voxels, index, Face.Bottom))
                {
                    var verts = FaceVerts(index, Face.Bottom);
                    foreach (Vector3 vert in verts)
                    {
                        yield return vert;
                    }
                }


                if (IsVisibleFace(voxels, index, Face.Front))
                {
                    var verts = FaceVerts(index, Face.Front);
                    foreach (Vector3 vert in verts)
                    {
                        yield return vert;
                    }
                }

                if (IsVisibleFace(voxels, index, Face.Back))
                {
                    var verts = FaceVerts(index, Face.Back);
                    foreach (Vector3 vert in verts)
                    {
                        yield return vert;
                    }
                }

                if (IsVisibleFace(voxels, index, Face.Left))
                {
                    var verts = FaceVerts(index, Face.Left);
                    foreach (Vector3 vert in verts)
                    {
                        yield return vert;
                    }
                }

                if (IsVisibleFace(voxels, index, Face.Right))
                {
                    var verts = FaceVerts(index, Face.Right);
                    foreach (Vector3 vert in verts)
                    {
                        yield return vert;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="voxels"></param>
        /// <param name="index"></param>
        /// <param name="face"></param>
        /// <returns></returns>
        public static bool IsVisibleFace(Voxel[, ,] voxels, IntVector3 index, Face face)
        {
            if (IsActive(voxels, index))
            {
                switch (face)
                {
                    case Face.Top:                                // Visibility rules for faces are ... 
                        if (index.Y == voxels.GetLength(1) - 1 ||       // active volume edge block (assume edge of volume is visible)
                           voxels[index.X, index.Y + 1, index.Z].Weight < 0)  // adjacent block is empty
                        {
                            return true;
                        }
                        break;

                    case Face.Bottom:
                        if (index.Y == 0 || voxels[index.X, index.Y - 1, index.Z].Weight < 0)
                        {
                            return true;
                        }
                        break;

                    case Face.Front:
                        if (index.Z == 0 || voxels[index.X, index.Y, index.Z - 1].Weight < 0)
                        {
                            return true;
                        }
                        break;

                    case Face.Back:
                        if (index.Z == voxels.GetLength(2) - 1 || voxels[index.X, index.Y, index.Z + 1].Weight < 0)
                        {
                            return true;
                        }
                        break;

                    case Face.Left:
                        if (index.X == 0 || voxels[index.X - 1, index.Y, index.Z].Weight < 0)
                        {
                            return true;
                        }
                        break;

                    case Face.Right:
                        if (index.X == voxels.GetLength(0) - 1 || voxels[index.X + 1, index.Y, index.Z].Weight < 0)
                        {
                            return true;
                        }
                        break;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns the corner points of the given face for a voxel 
        /// at the given index within the array.
        /// The locations given are relative to the arrays 0,0,0 point.
        /// </summary>
        /// <returns>The verts.</returns>
        /// <param name="face">Face.</param>
        public static IEnumerable<Vector3> FaceVerts(IntVector3 vIndex, Face face)
        {
            switch (face)
            {
                case Face.Top:
                    yield return new Vector3(-0.5f + vIndex.X, 0.5f + vIndex.Y, 0.5f + vIndex.Z);
                    yield return new Vector3(0.5f + vIndex.X, 0.5f + vIndex.Y, 0.5f + vIndex.Z);
                    yield return new Vector3(0.5f + vIndex.X, 0.5f + vIndex.Y, -0.5f + vIndex.Z);
                    yield return new Vector3(-0.5f + vIndex.X, 0.5f + vIndex.Y, -0.5f + vIndex.Z);
                    break;
                case Face.Bottom:
                    yield return new Vector3(-0.5f + vIndex.X, -0.5f + vIndex.Y, -0.5f + vIndex.Z);
                    yield return new Vector3(0.5f + vIndex.X, -0.5f + vIndex.Y, -0.5f + vIndex.Z);
                    yield return new Vector3(0.5f + vIndex.X, -0.5f + vIndex.Y, 0.5f + vIndex.Z);
                    yield return new Vector3(-0.5f + vIndex.X, -0.5f + vIndex.Y, 0.5f + vIndex.Z);
                    break;
                case Face.Left:
                    yield return new Vector3(-0.5f + vIndex.X, 0.5f + vIndex.Y, 0.5f + vIndex.Z);
                    yield return new Vector3(-0.5f + vIndex.X, 0.5f + vIndex.Y, -0.5f + vIndex.Z);
                    yield return new Vector3(-0.5f + vIndex.X, -0.5f + vIndex.Y, -0.5f + vIndex.Z);
                    yield return new Vector3(-0.5f + vIndex.X, -0.5f + vIndex.Y, 0.5f + vIndex.Z);
                    break;
                case Face.Right:
                    yield return new Vector3(0.5f + vIndex.X, 0.5f + vIndex.Y, -0.5f + vIndex.Z);
                    yield return new Vector3(0.5f + vIndex.X, 0.5f + vIndex.Y, 0.5f + vIndex.Z);
                    yield return new Vector3(0.5f + vIndex.X, -0.5f + vIndex.Y, 0.5f + vIndex.Z);
                    yield return new Vector3(0.5f + vIndex.X, -0.5f + vIndex.Y, -0.5f + vIndex.Z);
                    break;
                case Face.Front:
                    yield return new Vector3(-0.5f + vIndex.X, 0.5f + vIndex.Y, -0.5f + vIndex.Z);
                    yield return new Vector3(0.5f + vIndex.X, 0.5f + vIndex.Y, -0.5f + vIndex.Z);
                    yield return new Vector3(0.5f + vIndex.X, -0.5f + vIndex.Y, -0.5f + vIndex.Z);
                    yield return new Vector3(-0.5f + vIndex.X, -0.5f + vIndex.Y, -0.5f + vIndex.Z);
                    break;
                case Face.Back:
                    yield return new Vector3(0.5f + vIndex.X, 0.5f + vIndex.Y, 0.5f + vIndex.Z);
                    yield return new Vector3(-0.5f + vIndex.X, 0.5f + vIndex.Y, 0.5f + vIndex.Z);
                    yield return new Vector3(-0.5f + vIndex.X, -0.5f + vIndex.Y, 0.5f + vIndex.Z);
                    yield return new Vector3(0.5f + vIndex.X, -0.5f + vIndex.Y, 0.5f + vIndex.Z);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="voxelType"></param>
        /// <param name="face"></param>
        /// <param name="atlas"></param>
        /// <returns></returns>
        public static IEnumerable<Vector2> UVCoords(byte voxelType, Face face, TextureAtlas atlas)
        {
            return atlas.Textures
                .Where(a => a.VoxelType == voxelType && a.Faces.Contains(face))
                .First()
                .Bounds;
        }

        /// <summary>
        /// The complete set of vertices for a single textured cube voxel.
        /// </summary>
        /// <returns></returns>
        public static TexturedVertex[] VertsForTexturedVoxel()
        {
            return new[]
                {
                    // 3D coordinates              UV Texture coordinates
                    new TexturedVertex(new Vector3(-1.0f, -1.0f, -1.0f), new Vector2(0.0f, 1.0f)), // Front
                    new TexturedVertex(new Vector3(-1.0f,  1.0f, -1.0f), new Vector2(0.0f, 0.0f)),
                    new TexturedVertex(new Vector3( 1.0f,  1.0f, -1.0f), new Vector2(1.0f, 0.0f)),
                    new TexturedVertex(new Vector3(-1.0f, -1.0f, -1.0f), new Vector2(0.0f, 1.0f)),
                    new TexturedVertex(new Vector3( 1.0f,  1.0f, -1.0f), new Vector2(1.0f, 0.0f)),
                    new TexturedVertex(new Vector3( 1.0f, -1.0f, -1.0f), new Vector2(1.0f, 1.0f)),

                    new TexturedVertex(new Vector3(-1.0f, -1.0f,  1.0f), new Vector2(1.0f, 0.0f)), // BACK
                    new TexturedVertex(new Vector3( 1.0f,  1.0f,  1.0f), new Vector2(0.0f, 1.0f)),
                    new TexturedVertex(new Vector3(-1.0f,  1.0f,  1.0f), new Vector2(1.0f, 1.0f)),
                    new TexturedVertex(new Vector3(-1.0f, -1.0f,  1.0f), new Vector2(1.0f, 0.0f)),
                    new TexturedVertex(new Vector3( 1.0f, -1.0f,  1.0f), new Vector2(0.0f, 0.0f)),
                    new TexturedVertex(new Vector3( 1.0f,  1.0f,  1.0f), new Vector2(0.0f, 1.0f)),

                    new TexturedVertex(new Vector3(-1.0f, 1.0f, -1.0f), new Vector2(0.0f, 1.0f)), // Top
                    new TexturedVertex(new Vector3(-1.0f, 1.0f,  1.0f), new Vector2(0.0f, 0.0f)),
                    new TexturedVertex(new Vector3( 1.0f, 1.0f,  1.0f), new Vector2(1.0f, 0.0f)),
                    new TexturedVertex(new Vector3(-1.0f, 1.0f, -1.0f), new Vector2(0.0f, 1.0f)),
                    new TexturedVertex(new Vector3( 1.0f, 1.0f,  1.0f), new Vector2(1.0f, 0.0f)),
                    new TexturedVertex(new Vector3( 1.0f, 1.0f, -1.0f), new Vector2(1.0f, 1.0f)),

                    new TexturedVertex(new Vector3(-1.0f,-1.0f, -1.0f), new Vector2(1.0f, 0.0f)), // Bottom
                    new TexturedVertex(new Vector3( 1.0f,-1.0f,  1.0f), new Vector2(0.0f, 1.0f)),
                    new TexturedVertex(new Vector3(-1.0f,-1.0f,  1.0f), new Vector2(1.0f, 1.0f)),
                    new TexturedVertex(new Vector3(-1.0f,-1.0f, -1.0f), new Vector2(1.0f, 0.0f)),
                    new TexturedVertex(new Vector3( 1.0f,-1.0f, -1.0f), new Vector2(0.0f, 0.0f)),
                    new TexturedVertex(new Vector3( 1.0f,-1.0f,  1.0f), new Vector2(0.0f, 1.0f)),

                    new TexturedVertex(new Vector3(-1.0f, -1.0f, -1.0f), new Vector2(0.0f, 1.0f)), // Left
                    new TexturedVertex(new Vector3(-1.0f, -1.0f,  1.0f), new Vector2(0.0f, 0.0f)),
                    new TexturedVertex(new Vector3(-1.0f,  1.0f,  1.0f), new Vector2(1.0f, 0.0f)),
                    new TexturedVertex(new Vector3(-1.0f, -1.0f, -1.0f), new Vector2(0.0f, 1.0f)),
                    new TexturedVertex(new Vector3(-1.0f,  1.0f,  1.0f), new Vector2(1.0f, 0.0f)),
                    new TexturedVertex(new Vector3(-1.0f,  1.0f, -1.0f), new Vector2(1.0f, 1.0f)),

                    new TexturedVertex(new Vector3( 1.0f, -1.0f, -1.0f), new Vector2(1.0f, 0.0f)), // Right
                    new TexturedVertex(new Vector3( 1.0f,  1.0f,  1.0f), new Vector2(0.0f, 1.0f)),
                    new TexturedVertex(new Vector3( 1.0f, -1.0f,  1.0f), new Vector2(1.0f, 1.0f)),
                    new TexturedVertex(new Vector3( 1.0f, -1.0f, -1.0f), new Vector2(1.0f, 0.0f)),
                    new TexturedVertex(new Vector3( 1.0f,  1.0f, -1.0f), new Vector2(0.0f, 0.0f)),
                    new TexturedVertex(new Vector3( 1.0f,  1.0f,  1.0f), new Vector2(0.0f, 1.0f))
            };
        }
    }
}
