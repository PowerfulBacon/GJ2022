using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Components.Atmospherics
{
    public class Component_Equippable : Component
    {

        /// <summary>
        /// The overlay texture to apply to pawns when equipped.
        /// </summary>
        public string EquipTexture { get; private set; }

        /// <summary>
        /// Should we append the equipped slot to the item name?
        /// </summary>
        public bool AppendSlotToIconState { get; private set; } = false;

        public override void OnComponentAdd()
        {

            return;
        }

        public override void OnComponentRemove()
        {
            return;
        }

        public override void SetProperty(string name, object property)
        {
            return;
        }

    }
}
