using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Game.Construction;
using GJ2022.Game.Construction.Walls;
using GJ2022.Rendering.Textures;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Abstract
{
    public class Blueprint : Entity, IStandardRenderable, IDestroyable
    {

        private bool isDestroyed = false;

        public BlueprintDetail BlueprintDetail { get; set; } = new FoundationBlueprint();

        public Blueprint(Vector position) : base(position)
        { }

        public bool Destroy()
        {
            //Set destroyed
            isDestroyed = true;
            //Stop rendering
            
            return true;
        }

        public bool IsDestroyed()
        {
            return isDestroyed;
        }

        public override RendererTextureData GetRendererTexture()
        {
            return TextureCache.GetTexture("stone");
        }

    }
}
