using GJ2022.Subsystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Pawns
{
    public partial class Pawn
    {

        public void Death(string cause)
        {
            Log.WriteLine("i died lol");
            PawnControllerSystem.Singleton.StopProcessing(this);
            //throw new System.NotImplementedException();
        }

    }
}
