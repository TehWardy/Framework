using Meshing.Generators;
using System;
using System.Collections.Generic;
using UnityEngine;
using Voxels.Objects;

public class SurfaceMeshGenerator : MeshGenerator
{
    public override Mesh GenerateMesh(VoxelContainer data, IntVector3 start, IntVector3 end)
    {
        byte surfaceCrossValue = 128;
        Voxel v0, v1, v2, v3, v4, v5, v6, v7;

        int vertexIndex = 0, crossBitMap, edgeBits, triangleIndex, index1, index2, index3;

        var interpolatedValues = new Vector3[12];
        var vertices = new List<Vector3>();
        var triangleIndices = new List<int>();
        var colours = new List<Color>();
        var temp = new IntVector3();

        for (int x = start.X; x < end.X - 1; x++)
        {
            for (int y = start.Y; y < end.Y - 1; y++)
            {
                for (int z = start.Z; z < end.Z - 1; z++)
                {
                    if (vertices.Count > 64500)
                    {
                        //Maximum vertex count for a mesh is 65k
                        //If reaching this limit we should be making smaller or less complex meshes
                        break;
                    }

                    temp.X = x;
                    temp.Y = y;
                    temp.Z = z;

                    //Get the 8 corners of this cube
                    v0 = data.GetVoxel(temp); temp.X++;
                    v1 = data.GetVoxel(temp); temp.X--; temp.Y++;
                    v2 = data.GetVoxel(temp); temp.X++;
                    v3 = data.GetVoxel(temp); temp.X--; temp.Y--; temp.Z++;
                    v4 = data.GetVoxel(temp); temp.X++;
                    v5 = data.GetVoxel(temp); temp.X--; temp.Y++;
                    v6 = data.GetVoxel(temp); temp.X++;
                    v7 = data.GetVoxel(temp);

                    crossBitMap = 0;
                    if (v0.Weight < surfaceCrossValue) crossBitMap |= 1;
                    if (v1.Weight < surfaceCrossValue) crossBitMap |= 2;
                    if (v2.Weight < surfaceCrossValue) crossBitMap |= 8;
                    if (v3.Weight < surfaceCrossValue) crossBitMap |= 4;
                    if (v4.Weight < surfaceCrossValue) crossBitMap |= 16;
                    if (v5.Weight < surfaceCrossValue) crossBitMap |= 32;
                    if (v6.Weight < surfaceCrossValue) crossBitMap |= 128;
                    if (v7.Weight < surfaceCrossValue) crossBitMap |= 64;

                    //Use the edge look up table to determine the configuration of edges
                    edgeBits = Contouring3D.EdgeTableLookup[crossBitMap];

                    //The surface did not cross any edges, this cube is either complelety inside, or completely outside the volume
                    if (edgeBits == 0)
                        continue;

                    float interpolatedCrossingPoint = 0f;

                    //Calculate the interpolated positions for each edge that has a crossing value

                    //Bottom four edges
                    if ((edgeBits & 1) > 0)
                    {
                        interpolatedCrossingPoint = (surfaceCrossValue - v0.Weight) / (v1.Weight - v0.Weight);
                        interpolatedValues[0] = Vector3.Lerp(new Vector3(x, y, z), new Vector3(x + 1, y, z), interpolatedCrossingPoint);
                    }
                    if ((edgeBits & 2) > 0)
                    {
                        interpolatedCrossingPoint = (surfaceCrossValue - v1.Weight) / (v3.Weight - v1.Weight);
                        interpolatedValues[1] = Vector3.Lerp(new Vector3(x + 1, y, z), new Vector3(x + 1, y + 1, z), interpolatedCrossingPoint);
                    }
                    if ((edgeBits & 4) > 0)
                    {
                        interpolatedCrossingPoint = (surfaceCrossValue - v2.Weight) / (v3.Weight - v2.Weight);
                        interpolatedValues[2] = Vector3.Lerp(new Vector3(x, y + 1, z), new Vector3(x + 1, y + 1, z), interpolatedCrossingPoint);
                    }
                    if ((edgeBits & 8) > 0)
                    {
                        interpolatedCrossingPoint = (surfaceCrossValue - v0.Weight) / (v2.Weight - v0.Weight);
                        interpolatedValues[3] = Vector3.Lerp(new Vector3(x, y, z), new Vector3(x, y + 1, z), interpolatedCrossingPoint);
                    }

                    //Top four edges
                    if ((edgeBits & 16) > 0)
                    {
                        interpolatedCrossingPoint = (surfaceCrossValue - v4.Weight) / (v5.Weight - v4.Weight);
                        interpolatedValues[4] = Vector3.Lerp(new Vector3(x, y, z + 1), new Vector3(x + 1, y, z + 1), interpolatedCrossingPoint);
                    }
                    if ((edgeBits & 32) > 0)
                    {
                        interpolatedCrossingPoint = (surfaceCrossValue - v5.Weight) / (v7.Weight - v5.Weight);
                        interpolatedValues[5] = Vector3.Lerp(new Vector3(x + 1, y, z + 1), new Vector3(x + 1, y + 1, z + 1), interpolatedCrossingPoint);
                    }
                    if ((edgeBits & 64) > 0)
                    {
                        interpolatedCrossingPoint = (surfaceCrossValue - v6.Weight) / (v7.Weight - v6.Weight);
                        interpolatedValues[6] = Vector3.Lerp(new Vector3(x, y + 1, z + 1), new Vector3(x + 1, y + 1, z + 1), interpolatedCrossingPoint);
                    }
                    if ((edgeBits & 128) > 0)
                    {
                        interpolatedCrossingPoint = (surfaceCrossValue - v4.Weight) / (v6.Weight - v4.Weight);
                        interpolatedValues[7] = Vector3.Lerp(new Vector3(x, y, z + 1), new Vector3(x, y + 1, z + 1), interpolatedCrossingPoint);
                    }

                    //Side four edges
                    if ((edgeBits & 256) > 0)
                    {
                        interpolatedCrossingPoint = (surfaceCrossValue - v0.Weight) / (v4.Weight - v0.Weight);
                        interpolatedValues[8] = Vector3.Lerp(new Vector3(x, y, z), new Vector3(x, y, z + 1), interpolatedCrossingPoint);
                    }
                    if ((edgeBits & 512) > 0)
                    {
                        interpolatedCrossingPoint = (surfaceCrossValue - v1.Weight) / (v5.Weight - v1.Weight);
                        interpolatedValues[9] = Vector3.Lerp(new Vector3(x + 1, y, z), new Vector3(x + 1, y, z + 1), interpolatedCrossingPoint);
                    }
                    if ((edgeBits & 1024) > 0)
                    {
                        interpolatedCrossingPoint = (surfaceCrossValue - v3.Weight) / (v7.Weight - v3.Weight);
                        interpolatedValues[10] = Vector3.Lerp(new Vector3(x + 1, y + 1, z), new Vector3(x + 1, y + 1, z + 1), interpolatedCrossingPoint);
                    }
                    if ((edgeBits & 2048) > 0)
                    {
                        interpolatedCrossingPoint = (surfaceCrossValue - v2.Weight) / (v6.Weight - v2.Weight);
                        interpolatedValues[11] = Vector3.Lerp(new Vector3(x, y + 1, z), new Vector3(x, y + 1, z + 1), interpolatedCrossingPoint);
                    }

                    //Shift the cross bit map to use as an index into the triangle look up table
                    crossBitMap <<= 4;

                    triangleIndex = 0; index1 = 0; index2 = 0; index3 = 0;
                    while (Contouring3D.TriangleLookupTable[crossBitMap + triangleIndex] != -1)
                    {
                        //For each triangle in the look up table, create a triangle and add it to the list.
                        index1 = Contouring3D.TriangleLookupTable[crossBitMap + triangleIndex];
                        index2 = Contouring3D.TriangleLookupTable[crossBitMap + triangleIndex + 1];
                        index3 = Contouring3D.TriangleLookupTable[crossBitMap + triangleIndex + 2];

                        vertices.Add(new Vector3(interpolatedValues[index1].x, interpolatedValues[index1].y, interpolatedValues[index1].z));
                        vertices.Add(new Vector3(interpolatedValues[index2].x, interpolatedValues[index2].y, interpolatedValues[index2].z));
                        vertices.Add(new Vector3(interpolatedValues[index3].x, interpolatedValues[index3].y, interpolatedValues[index3].z));

                        triangleIndices.Add(vertexIndex);
                        triangleIndices.Add(vertexIndex + 1);
                        triangleIndices.Add(vertexIndex + 2);

                        //uvs.Add();
                        //uvs.Add();
                        //uvs.Add();

                        vertexIndex += 3;
                        triangleIndex += 3;
                    }
                }
            }
        }

        //Create texture coordinates for all the vertices
        List<Vector2> texCoords = new List<Vector2>();
        Vector2 emptyTexCoords0 = new Vector2(0, 0);
        Vector2 emptyTexCoords1 = new Vector2(0, 1);
        Vector2 emptyTexCoords2 = new Vector2(1, 1);

        for (int texturePointer = 0; texturePointer < vertices.Count; texturePointer += 3)
        {
            //There should be as many texture coordinates as vertices.
            //This example does not support textures, so fill with zeros
            texCoords.Add(emptyTexCoords1);
            texCoords.Add(emptyTexCoords2);
            texCoords.Add(emptyTexCoords0);
        }

        //Generate the mesh using the vertices and triangle indices we just created
        var mesh = new Mesh { 
            vertices = vertices.ToArray(), 
            triangles = triangleIndices.ToArray(),
            uv = texCoords.ToArray(), 
            colors = colours.ToArray()
        };

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.Optimize();
        return mesh;
    }
}


