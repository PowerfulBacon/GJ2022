using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJ2022.Components;
using GJ2022.Components.Bespoke;
using GJ2022.Entities;
using GJ2022.Entities.Items;
using GJ2022.EntityLoading;
using GJ2022.EntityLoading.XmlDataStructures;
using GJ2022.Game.Construction;
using GJ2022.Game.GameWorld;
using GJ2022.Utility.MathConstructs;
using GLFW;

namespace GJ2022.Subsystems
{
    public class BlueprintSystem : Subsystem
    {

        public static BlueprintSystem Singleton { get; } = new BlueprintSystem();

        public override int sleepDelay => 0;

        public override SubsystemFlags SubsystemFlags => SubsystemFlags.NO_UPDATE;

        /// <summary>
        /// Associates component handlers with their respective blueprints.
        /// </summary>
        private Dictionary<Entity, BlueprintData> blueprintEntities { get; } = new Dictionary<Entity, BlueprintData>();

        /// <summary>
        /// List of all blueprint sts
        /// </summary>
        public List<BlueprintCategory> Blueprints { get; } = new List<BlueprintCategory>();

        public override void InitSystem()
        { }

        protected override void AfterWorldInit()
        { }

        public override void Fire(Window window)
        {
            throw new NotImplementedException();
        }

        public void QueueBlueprint(Vector<float> position, BlueprintData blueprintData)
        {
            //Check for existing blueprints and return if necessary
            foreach (IComponentHandler componentHandler in World.GetThings("blueprint", (int)position.X, (int)position.Y))
            {
                //Get the blueprint at this location
                BlueprintData existingBlueprint = GetBlueprintData(componentHandler as Entity);
                //If the existing blueprint has a greater priority, ignore
                if (existingBlueprint.Layer == blueprintData.Layer)
                {
                    //We need to do something
                    //The existing blueprint has a greater priority than us
                    if (existingBlueprint.Priority > blueprintData.Priority)
                        return;
                    //We need to delete the existing blueprint
                    Entity existingBlueprintEntity = componentHandler as Entity;
                    existingBlueprintEntity.Destroy();
                }
            }
            //Create a new entity from the blueprint typeDef and add it to our blueprint entities list
            Entity createdEntity = EntityCreator.CreateEntity<Entity>("blueprint", position);
            blueprintEntities.Add(createdEntity, blueprintData);
        }

        public bool BlueprintHasMaterials(Component_Blueprint blueprint)
        {
            throw new NotImplementedException();
        }

        public void CompleteBlueprint(Component_Blueprint blueprint)
        {
            throw new NotImplementedException();
        }

        public bool BlueprintRequiresMaterial(Component_Blueprint blueprint, EntityDef typeDef)
        {
            throw new NotImplementedException();
        }

        public void BlueprintInsertMaterials(Component_Blueprint blueprint, Item item)
        {
            throw new NotImplementedException();
        }

        public void DeleteBlueprintData(Entity entity)
        {
            blueprintEntities.Remove(entity);
        }

        public BlueprintData GetBlueprintData(Entity entity)
        {
            return blueprintEntities[entity];
        }

        public bool HasBlueprints()
        {
            return blueprintEntities.Count > 0;
        }

    }
}
