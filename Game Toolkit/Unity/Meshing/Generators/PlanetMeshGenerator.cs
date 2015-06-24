using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Voxels.Objects;

namespace Meshing.Generators
{
    public class PlanetMeshGenerator
    {
        public Mesh GenerateMesh(Voxels.Objects.PlanetRegion region, int planetSize, int verticals, int horizontals)
        {
            Vector3[] verts;
            Vector2[] uvs;

            BuildVerts(region, planetSize, verticals, horizontals, out verts, out uvs);
            var result = new Mesh { vertices = verts, uv = uvs };
            result.SetTriangles(BuildIndexes(verts, verticals, horizontals), 0);

            return result;
        }

        public void BuildVerts(Voxels.Objects.PlanetRegion region, int planetSize, int verticals, int horizontals, out Vector3[] verts, out Vector2[] uvs)
        {
            // determine range and stepping variable
            var range = region.ToRadians();
            var verticalStep = (range.North - range.South) / (verticals - 1);
            var horizontalStep = (range.East - range.West) / (horizontals - 1);

            //range.South -= verticalStep;
            //range.East += horizontalStep;

            // define result containers
            var vertList = new List<Vector3>();
            var uvList = new List<Vector2>();

            // ok lets do this 
            for (double inc = range.North; inc >= range.South; inc -= verticalStep)
            { 
                for (double az = range.West; az <= range.East; az += horizontalStep)
                {
                    // translate the angles to cartesian space for this point
					vertList.Add(new Vector3(
						(float)(planetSize * Math.Sin(az) * Math.Cos(inc)),
						(float)(planetSize * Math.Sin(inc)),
						(float)(planetSize * Math.Cos(az) * Math.Cos(inc)))
	             	);

                    /*
                     * Ok by knowing the start and end of our range, and the current pos
                     * we calc the angular distance and that gets projected to our texture as a uv
                     * 
                    var start = new SphericalPosition(planetSize, (float)range.South, (float)range.West);
                    var end = new SphericalPosition(planetSize, (float)range.North, (float)range.East);
                    var currentPoint = new SphericalPosition(planetSize, (float)inc, (float)az);

                    var u = (currentPoint.Azimuth - end.Azimuth) / (end.Azimuth - start.Azimuth);
                    var v = (currentPoint.Inclination - end.Inclination) / (end.Inclination - start.Inclination);
                    */
					uvList.Add(new Vector2(
						(float)Math.Abs((az - range.East) / (range.East - range.West)),
						(float)Math.Abs((inc - range.North) / (range.North - range.South)))
		           	);
                }
            }

            verts = vertList.ToArray();
            uvs = uvList.ToArray();
        }

        public int[] BuildIndexes(Vector3[] verts, int verticals, int horizontals)
        {
            var indexes = new List<int>();
            var lastVert = verts.Length - horizontals - 2;

            for (int vert = 0; vert < lastVert; vert++)
            {
                //setup 2 tris
                // tri 1
                indexes.Add(vert);
                indexes.Add(vert + horizontals + 1);
                indexes.Add(vert + 1);
                
                // tri 2
                indexes.Add(vert + 1);
                indexes.Add(vert + horizontals + 1);
                indexes.Add(vert + horizontals + 2);
            }

            return indexes.ToArray();
        }
    }
}
