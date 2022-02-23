using GJ2022.Entities;
using GJ2022.Entities.Pawns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.PawnBehaviours.InteractionEvents
{
    public interface IInteractionEvent
    {

        /// <summary>
        /// A pawn interacts with a target
        /// </summary>
        void Interact(Entity target, Pawn interactor);

    }
}
