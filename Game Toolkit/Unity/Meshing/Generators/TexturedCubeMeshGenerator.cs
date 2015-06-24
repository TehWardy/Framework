using Engine.MeshGeneration.Texturing;
using Meshing;
using Meshing.Generators;
using Meshing.VertexDefinitions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Voxels.Objects;

/// <summary>
/// Generates a "Minecraft Style" textured cube mesh for chunks 
/// </summary>
public class TexturedCubeMeshGenerator : MeshGenerator
{
	public TextureAtlas Atlas { get; set; }

	public TexturedCubeMeshGenerator()
	{

	}

	public override Mesh GenerateMesh(VoxelContainer data, IntVector3 start, IntVector3 end)
	{
	    var verts = GenerateVertsFor(data, start, end);

	    if (!verts.Any())
	        return null;

	    var result = new Mesh()
	    {
	        vertices = verts.Select(v => v.Position).ToArray(),
	        uv = verts.Select(v => v.Uv).ToArray()
	    };
	    result.SetTriangles(GenerateIndicesFor(verts).ToArray(), 0);
	    return result;
	}

	List<TexturedVertex> GenerateVertsFor(VoxelContainer data, IntVector3 start, IntVector3 end)
	{
	    var verts = new List<TexturedVertex>();
	    var here = new IntVector3(data.Start);
	    //Voxel currentVoxel;
	    Vector3[] currentVerts;
	    Vector2[] currentUVs;

	    for (here.X = start.X; here.X <= end.X; here.X++)
	        for (here.Z = start.Z; here.Z <= end.Z; here.Z++)
	            for (here.Y = start.Y; here.Y <= end.Y; here.Y++)
	            {
	                //currentVoxel = data.GetVoxel(here);
	                foreach (var face in VoxelHelper.VisibleFacesFor(data, here))
	                {
	                    currentVerts = VoxelHelper.FaceVerts(here, face).ToArray();
	                    currentUVs = VoxelHelper.UVCoords(data.GetVoxel(here).Type, face, Atlas).ToArray();

	                    verts.AddRange(new []{
	                        new TexturedVertex(currentVerts[0], currentUVs[0]),
	                        new TexturedVertex(currentVerts[1], currentUVs[1]),
	                        new TexturedVertex(currentVerts[2], currentUVs[2]),
	                        new TexturedVertex(currentVerts[3], currentUVs[3])
	                    });
	                }
	            }

	    return verts;
	}

	private List<int> GenerateIndicesFor(List<TexturedVertex> verts)
	{
	    // verts are defined in quads per voxel face, so ...
	    // verts / 4 = number of rendered faces
	    // faces * 2 = number of tris (2 tris make a quad)
	    // faces * 6 = number of indices (links for each of the 6 corners that make up the 2 tris from all quads)
	    //var faces = data.Mesh.vertexCount / 4;
	    //var indexCount = faces * 6;
	    var indexes = new int[(verts.Count / 4) * 6];
	    var index = 0;

	    // setup index info for each face
	    for (int vert = 0; vert < verts.Count; vert += 4)
	    {
	        // tri 1
	        indexes[index++] = vert;
	        indexes[index++] = vert + 1;
	        indexes[index++] = vert + 2;

	        // tri 2
	        indexes[index++] = vert;
	        indexes[index++] = vert + 2;
        indexes[index++] = vert + 3;
    }

    return indexes.ToList();
}
}