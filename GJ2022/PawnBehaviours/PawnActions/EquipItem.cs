using GJ2022.Components;
using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Entities.Items;
using GJ2022.Entities.Pawns;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.PawnBehaviours.PawnActions
{
    public class EquipItem : PawnAction
    {
        public override bool Overriding => false;

        public override int Priority => 10;

        private bool completed = false;

        private Item item;

        public EquipItem(Item item)
        {
            this.item = item;
        }

        public override bool CanPerform(PawnBehaviour parent)
        {
            return false;
        }

        public override bool Completed(PawnBehaviour parent)
        {
            return completed;
        }

        public override void OnActionCancel(PawnBehaviour parent)
        {
            return;
        }

        public override void OnActionEnd(PawnBehaviour parent)
        {
            completed = false;
        }

        public override void OnActionStart(PawnBehaviour parent)
        {
            //Path to the target
            parent.Owner.MoveTowardsEntity(item);
        }

        public override void OnActionUnreachable(PawnBehaviour parent, Vector<float> unreachableLocation)
        {
            completed = true;
        }

        public override void OnPawnReachedLocation(PawnBehaviour parent)
        {
            //Equip
            if (!item.Destroyed && parent.Owner.InReach(item))
            {
                //Equip the item
                item.SendSignal(Signal.SIGNAL_ITEM_EQUIP_TO_PAWN, parent.Owner);
            }
            completed = true;
        }

        public override void PerformProcess(PawnBehaviour parent)
        {
            return;
        }
    }
}
