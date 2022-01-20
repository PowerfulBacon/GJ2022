namespace GJ2022.Rendering.Textures
{
    public class TextureJson
    {
        public TextureJson(string fileName, int width, int height, int indexX, int indexY)
        {
            FileName = fileName;
            Width = width;
            Height = height;
            IndexX = indexX;
            IndexY = indexY;
        }

        public string FileName { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int IndexX { get; set; }

        public int IndexY { get; set; }

    }
}
