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
                IEquippable equippable = item as IEquippable;
                //Convert slot flags to an actual slot
                InventorySlot wantedSlot;
                if ((equippable.Slots & InventorySlot.SLOT_BACK) != 0)
                    wantedSlot = InventorySlot.SLOT_BACK;
                else if ((equippable.Slots & InventorySlot.SLOT_BODY) != 0)
                    wantedSlot = InventorySlot.SLOT_BODY;
                else if ((equippable.Slots & InventorySlot.SLOT_HEAD) != 0)
                    wantedSlot = InventorySlot.SLOT_HEAD;
                else
                {
                    completed = true;
                    return;
                }
                parent.Owner.TryEquipItem(wantedSlot, equippable);
            }
            completed = true;
        }

        public override void PerformProcess(PawnBehaviour parent)
        {
            return;
        }
    }
}
