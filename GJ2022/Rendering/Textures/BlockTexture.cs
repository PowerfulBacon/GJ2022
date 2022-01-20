using GJ2022.Rendering.Models;
using System.Collections.Generic;

namespace GJ2022.Rendering.Textures
{
    class BlockTexture
    {

        public BlockTexture(TextureJson top, TextureJson bottom, TextureJson left, TextureJson right, TextureJson back, TextureJson front)
        {
            Faces = new Dictionary<CubeFaceFlags, TextureJson>();
            Faces.Add(CubeFaceFlags.FACE_ABOVE, top);
            Faces.Add(CubeFaceFlags.FACE_BELOW, bottom);
            Faces.Add(CubeFaceFlags.FACE_LEFT, left);
            Faces.Add(CubeFaceFlags.FACE_RIGHT, right);
            Faces.Add(CubeFaceFlags.FACE_FRONT, front);
            Faces.Add(CubeFaceFlags.FACE_BACK, back);
        }

        public Dictionary<CubeFaceFlags, TextureJson> Faces { get; set; }

    }
}
