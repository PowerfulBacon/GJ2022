using GJ2022.Entities.Pawns.Health.Injuries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Pawns.Health.Bodyparts
{
    abstract class Bodypart
    {

        //List of inflicting injuries
        private List<Injury> injuries = new List<Injury>();

    }
}
