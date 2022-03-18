using GJ2022.Atmospherics;
using GJ2022.Entities.Pawns;
using GJ2022.Entities.Turfs;
using GJ2022.Game.GameWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Components.Items
{
    public class Component_InternalEntity_Hardsuit : Component_InternalEntity
    {

        private bool helmetDeployed = false;

        public override void OnComponentAdd()
        {
            base.OnComponentAdd();
            Parent.RegisterSignal(Signal.SIGNAL_ITEM_EQUIPPED, -1, OnEquipped);
            Parent.RegisterSignal(Signal.SIGNAL_ITEM_UNEQUIPPED, -1, OnUnequipped);
        }

        public override void OnComponentRemove()
        {
            base.OnComponentRemove();
            Parent.UnregisterSignal(Signal.SIGNAL_ITEM_EQUIPPED, OnEquipped);
            Parent.UnregisterSignal(Signal.SIGNAL_ITEM_UNEQUIPPED, OnUnequipped);
        }

        private object OnEquipped(object source, params object[] parameters)
        {
            Pawn equipper = (Pawn)parameters[0];
            equipper.RegisterSignal(Signal.SIGNAL_ENTITY_MOVED, -1, OnPawnMoved);
            return null;
        }

        private object OnUnequipped(object source, params object[] parameters)
        {

            Pawn equipper = (Pawn)parameters[0];
            equipper.UnregisterSignal(Signal.SIGNAL_ENTITY_MOVED, OnPawnMoved);
            return null;
        }

        private object OnPawnMoved(object source, params object[] parmeters)
        {
            Pawn mover = (Pawn)source;
            if (ShouldDeployHelmet(mover))
            {
                if (!helmetDeployed)
                    DeployHardsuitHelmet(mover);
            }
            else if (helmetDeployed)
                UndeployHardsuitHelmet(mover);
            return null;
        }

        private bool ShouldDeployHelmet(Pawn pawn)
        {
            Turf turf = World.Current.GetTurf((int)pawn.Position.X, (int)pawn.Position.Y);
            Atmosphere locatedAtmosphere = turf?.Atmosphere?.ContainedAtmosphere;
            if (locatedAtmosphere == null)
                return true;
            if (locatedAtmosphere.KiloPascalPressure < 50 || locatedAtmosphere.KiloPascalPressure > 150)
                return true;
            return false;
        }

        private void DeployHardsuitHelmet(Pawn pawn)
        {
            helmetDeployed = true;
            StoredEntity.SendSignal(Signal.SIGNAL_ITEM_EQUIP_TO_PAWN, pawn);
        }

        private void UndeployHardsuitHelmet(Pawn pawn)
        {
            helmetDeployed = false;
        }

    }
}
