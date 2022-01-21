using GJ2022.Rendering.Models;
using GJ2022.Rendering.Textures;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Rendering
{
    public abstract class Renderable
    {

        //====================
        // Object specific - Change as much as you like
        //====================

        //The object matrix -> Our translation from the origin of the world (Only changes if we move)
        public virtual Matrix ObjectMatrix { get; set; } = Matrix.Identity[4];

        //The model associated with this object
        //We need to sort in terms of models for optimisations
        public virtual ModelData ModelData { get; set; } = BlockModelData.Singleton;


        //The position of the object in 3D space
        public Vector position = new Vector(3);

        public Renderable(Vector position)
        {
            this.position = position;
        }

        //Is this object being rendered?
        public bool IsRendering { get; set; } = false;

        /// <summary>
        /// Get the renderable texture data that can be used by the renderer
        /// </summary>
        public abstract RendererTextureData GetRendererTexture();

    }
}
