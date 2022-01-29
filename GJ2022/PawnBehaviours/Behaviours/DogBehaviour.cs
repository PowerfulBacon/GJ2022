﻿using GJ2022.Entities.Pawns;
using GJ2022.PawnBehaviours.PawnActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.PawnBehaviours.Behaviours
{
    class DogBehaviour : PawnBehaviour
    {

        public DogBehaviour(Pawn owner) : base(owner)
        { }

        public override Dictionary<PawnAction, double> Actions { get; } = new Dictionary<PawnAction, double>()
        {
            { new HaulItems(), 0 },
            { new ReturnToSpawn(), 0 }
        };

    }
}
