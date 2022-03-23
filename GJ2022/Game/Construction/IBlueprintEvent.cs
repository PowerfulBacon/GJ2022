using GJ2022.EntityLoading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Game.Construction
{
    public interface IBlueprintEvent : IInstantiatable
    {

        /// <summary>
        /// Called when the blueprint set is selected by the player.
        /// </summary>
        void OnBlueprintSelected();

        /// <summary>
        /// Called when the blueprint set is deselected by the player.
        /// </summary>
        void OnBlueprintDeselected();

    }
}
