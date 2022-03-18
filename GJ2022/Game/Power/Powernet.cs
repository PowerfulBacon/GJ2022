using GJ2022.Entities.Structures;
using GJ2022.Entities.Structures.Power;
using GJ2022.Game.GameWorld;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Game.Power
{
    /// <summary>
    /// Powernets:
    ///  - Powernets are based on power/time.
    ///  - Supply = Maximum power that can be put in
    ///  - Demand = Power requested.
    ///  
    /// If you have a battery that is outputting 200kW power into the net, the powernet will have
    /// 200kW of supply.
    /// The battery will only output the demand of the powernet into the powernet, meaning that even if you
    /// are putting 200kW into the powernet of supply, the thing outputting will only actually put that power in
    /// when it is needed.
    /// The powernet only holds as much physical power units as the supply since it needs to hold something so it isn't lost
    /// during concurrent processing).
    /// If more power is sent into the powernet than demand, the additional power will be lost
    /// </summary>
    public class Powernet
    {

        private static int PowernetCount = 0;

        public int PowernetId { get; } = PowernetCount++;

        public HashSet<PowerConduit> conduits = new HashSet<PowerConduit>();

        public float Supply { get; private set; }
        public float Demand { get; private set; }
        public float Stored { get; private set; }

        public void AdjustSupply(float adjustAmount)
        {
            Supply += adjustAmount;
        }

        public void AdjustDemand(float adjustAmount)
        {
            Demand += adjustAmount;
        }

        public float ReceievePower(float wantedAmount)
        {
            float amount = Math.Min(wantedAmount, Stored);
            Stored -= amount;
            return amount;
        }

        /// <summary>
        /// Send power into the network.
        /// If more power is sent than is being used, the excess will be destroyed.
        /// The powernet can take 50% more power than demand  so that adding power to the network won't disable machines temporarilly.
        /// </summary>
        public void SendPower(float powerToSend)
        {
            Stored = Math.Max(Math.Min(Stored + powerToSend, Demand * 1.5f), 0);
        }

        /// <summary>
        /// Adopt another powernet and assimmilate all the things on it
        /// </summary>
        public void Adopt(Powernet powernet)
        {
            foreach (PowerConduit conduit in powernet.conduits)
            {
                //adopted
                conduit._powernet = this;
                conduit.attachedTextObject.Text = $"{PowernetId}";
                conduits.Add(conduit);
                //Update attached powernets
                foreach (PowernetInteractor interactor in World.Current.GetPowernetInteractors((int)conduit.Position.X, (int)conduit.Position.Y))
                {
                    interactor.AttachedPowernet = this;
                }
            }
            powernet.conduits = null;
            //Take this supply and demand also
            Supply += powernet.Supply;
            Demand += powernet.Demand;
        }

    }
}
