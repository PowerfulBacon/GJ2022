﻿using System;

namespace GJ2022.Game.Construction.Walls
{
    public class FoundationBlueprint : BlueprintDetail
    {

        public override string Name => "Metal Foundations";

        public override bool IsRoom => true;

        public override Type BorderType => typeof(Entities.Debug.DebugSolid);

        public override Type FloorType => typeof(Entities.Debug.DebugEntity);

        public override string BorderTexture => "stone";

        public override string FloorTexture => base.FloorTexture;

        public override int BlueprintLayer => 0;
    }
}
