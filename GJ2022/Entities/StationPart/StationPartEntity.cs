using GJ2022.Rendering.Models;
using GJ2022.Rendering.RenderSystems;
using GJ2022.Rendering.RenderSystems.Interfaces;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.StationPart
{
    public abstract class StationPartEntity : Entity, IInstanceRenderable
    {

        public override ModelData ModelData { get; set; } = QuadModelData.Singleton;

        //List of rooms connected to this one
        public List<StationPartEntity> AttachedRooms { get; } = new List<StationPartEntity>();

        //Set the relative connection points
        public virtual Vector[] RelativeConnectionPoints { get; protected set; }

        //Ctor
        public StationPartEntity(Vector position) : base(position) { }

        private Dictionary<RenderBatchSet, int> renderableBatchIndex = new Dictionary<RenderBatchSet, int>();

        public void SetRenderableBatchIndex(RenderBatchSet associatedSet, int index)
        {
            if (renderableBatchIndex.ContainsKey(associatedSet))
                renderableBatchIndex[associatedSet] = index;
            else
                renderableBatchIndex.Add(associatedSet, index);
        }

        /// <summary>
        /// Returns the renderable batch index in the provided set.
        /// Returns -1 if failed.
        /// </summary>
        public int GetRenderableBatchIndex(RenderBatchSet associatedSet)
        {
            if (renderableBatchIndex.ContainsKey(associatedSet))
                return renderableBatchIndex[associatedSet];
            else
                return -1;
        }

        public Vector GetInstancePosition()
        {
            return position;
        }

        public Vector GetInstanceScale()
        {
            return new Vector(2, 3, 1);
        }
    }
}
