using UnityEngine;
using Voxels.Objects;

namespace Engine.MeshGeneration.Texturing
{
    /// <summary>
    /// Packed texture set to be used for mapping texture info on 
    /// dynamically generated meshes.
    /// </summary>
    public class TextureAtlas
    {
        /// <summary>
        /// Texture definitions within the atlas.
        /// </summary>
        public TextureDef[] Textures { get; set; }

        public TextureAtlas()
        {
            SetupTextures();
        }

        protected virtual void SetupTextures()
        {
            // default for bas atlas is a material with a single texture in the atlas
            Textures = new TextureDef[]
            {
                new TextureDef 
                { 
                    VoxelType = 0, 
                    Faces =  new[] { Face.Top, Face.Bottom, Face.Left, Face.Right, Face.Front, Face.Back },
                    Bounds = new[] {
                        new Vector2(0,1), 
                        new Vector2(1, 1),
                        new Vector2(1,0),
                        new Vector2(0, 0)
                    }
                }
            };
        }


        public static TextureDef[] GenerateTextureSet(IntVector2 textureSizeInPixels, IntVector2 atlasSizeInPixels)
        {
            int x = atlasSizeInPixels.X / textureSizeInPixels.X;
            int z = atlasSizeInPixels.Z / textureSizeInPixels.Z;
            int i = 0;
            var result = new TextureDef[x * z];
            var uvSize = new Vector2(1f / ((float)x), 1f / ((float)z));

            for (int tx = 0; tx < x; tx++)
                for (int tz = 0; tz < z; tz++)
                {
					// for perf, types are limited to 255 (1 byte)
					if(i < 255)
					{
	                    result[i] = new TextureDef
	                    {
	                        VoxelType = (byte)i,
	                        Faces = new[] { Face.Top, Face.Bottom, Face.Left, Face.Right, Face.Front, Face.Back },
	                        Bounds = new[] {
	                            new Vector2(tx * uvSize.x, (tz + 1f) * uvSize.y), 
	                            new Vector2((tx + 1f) * uvSize.x, (tz + 1f) * uvSize.y),
	                            new Vector2((tx + 1f) * uvSize.x, tz * uvSize.y),
	                            new Vector2(tx * uvSize.x, tz * uvSize.y)
	                        }
	                    };

	                    i++;
					}
					else
						break;
                }

             return result;
        }
    }
}
