using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Abstract
{
    public class Blueprint : Entity, IDestroyable
    {

        private bool isDestroyed = false;

        public Blueprint(Vector position) : base(position)
        { }

        public bool Destroy()
        {
            //Set destroyed
            isDestroyed = true;
            //Stop rendering
            
            return true;
        }

        public bool IsDestroyed()
        {
            return isDestroyed;
        }
    }
}
