using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Utility.MathConstructs;
using System.Threading;
using System.Threading.Tasks;

namespace GJ2022.Entities.Effects
{
    public abstract class Effect : Entity, IDestroyable
    {

        protected abstract int ExistanceTime { get; }

        public bool Destroyed { get; private set; }

        public Effect(Vector<float> position, float layer) : base(position, layer)
        {
            Task.Run(DeleteThread);
        }

        public override bool Destroy()
        {
            Destroyed = true;
            return base.Destroy();
        }

        private void DeleteThread()
        {
            Thread.Sleep(ExistanceTime);
            Destroy();
        }

    }
}
