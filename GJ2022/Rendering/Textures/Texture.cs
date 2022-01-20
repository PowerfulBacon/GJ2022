namespace GJ2022.Rendering.Textures
{
    public abstract class Texture
    {

        //Full width of the texture file (The entire spritesheet)
        public int Width { get; protected set; }
        public int Height { get; protected set; }

        //The texture ID of the loaded texture, used by OpenGL
        public uint TextureId { get; protected set; }

        /// <summary>
        /// Reads the texture file and loads it to openGl
        /// </summary>
        public abstract unsafe void ReadTexture(string fileName);

    }
}
