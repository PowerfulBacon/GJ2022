using GJ2022.Components;
using GJ2022.Entities;
using GJ2022.Entities.Pawns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.PawnBehaviours.InteractionEvents
{
    public class MineInteraction : IInteractionEvent
    {

        public void Interact(Entity target, Pawn interactor)
        {
            //Send a mine signal to the target
            target.SendSignal(Signal.SIGNAL_ENTITY_MINE, interactor);
        }

    }
}
