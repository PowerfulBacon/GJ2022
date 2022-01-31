using GJ2022.Game.GameWorld;

namespace GJ2022.Rendering.Textures
{
    /// <summary>
    /// Texture data used by the renderer
    /// </summary>
    public struct RendererTextureData
    {

        public RendererTextureData(Texture texture, TextureJson json)
        {
            TextureUint = texture.TextureId;
            Width = json.Width;
            Height = json.Height;
            IndexX = json.IndexX;
            IndexY = json.IndexY;
            FileWidth = texture.Width;
            FileHeight = texture.Height;
            DirectionalMode = json.DirectionalModes;
        }

        public uint TextureUint { get; }

        public int FileWidth { get; }

        public int FileHeight { get; }

        public int Width { get; }

        public int Height { get; }

        public int IndexX { get; }

        public int IndexY { get; }

        public DirectionalModes DirectionalMode { get; }

    }
}
