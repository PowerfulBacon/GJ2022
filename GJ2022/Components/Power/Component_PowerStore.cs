using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Components.Power
{
    public class Component_PowerStore : Component
    {

        public float StoredPower { get; set; } = 0;
        public float MaxPower { get; set; } = 0;
        public float ChargeRate { get; set; } = 0;

        public override void OnComponentAdd()
        {
            Parent.RegisterSignal(Signal.SIGNAL_GET_STORED_POWER, 5, ReturnPower);
            Parent.RegisterSignal(Signal.SIGNAL_ITEM_TAKE_POWER, 5, TakePower);
            Parent.RegisterSignal(Signal.SIGNAL_ITEM_GIVE_POWER, 5, GivePower);
            Parent.RegisterSignal(Signal.SIGNAL_GET_POWER_DEMAND, 5, GetDemand);
        }

        public override void OnComponentRemove()
        {
            Parent.UnregisterSignal(Signal.SIGNAL_GET_STORED_POWER, ReturnPower);
            Parent.UnregisterSignal(Signal.SIGNAL_GET_STORED_POWER, TakePower);
            Parent.UnregisterSignal(Signal.SIGNAL_ITEM_GIVE_POWER, GivePower);
            Parent.UnregisterSignal(Signal.SIGNAL_GET_POWER_DEMAND, GetDemand);
        }

        private object GivePower(object source, params object[] arguments)
        {
            float amount = Convert.ToSingle(arguments[0]);
            float powerGiven = Math.Min(amount, MaxPower - StoredPower);
            StoredPower += powerGiven;
            return powerGiven;
        }

        private object TakePower(object source, params object[] arguments)
        {
            float amount = Convert.ToSingle(arguments[0]);
            float powerTaken = Math.Min(amount, StoredPower);
            StoredPower -= powerTaken;
            return powerTaken;
        }

        private object ReturnPower(object source, params object[] arguments) => StoredPower;

        private object GetDemand(object source, params object[] arguments) => Math.Min(ChargeRate * Convert.ToSingle(arguments[0]), MaxPower - StoredPower);

        public override void SetProperty(string name, object property)
        {
            switch (name)
            {
                case "ChargeRate":
                    ChargeRate = Convert.ToSingle(property);
                    return;
                case "MaxPower":
                    MaxPower = Convert.ToSingle(property);
                    return;
                case "StoredPower":
                    StoredPower = Convert.ToSingle(property);
                    return;
            }
            throw new NotImplementedException($"Component_PowerStore has no {name} property.");
        }

    }
}
