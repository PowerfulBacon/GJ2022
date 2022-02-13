using GJ2022.Game;
using GJ2022.Game.GameWorld;
using GJ2022.Game.Power;
using GJ2022.Managers.TaskManager;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Rendering.Text;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.Structures.Power
{
    public class PowerConduit : Structure
    {
        public PowerConduit(Vector<float> position) : base(position, Layers.LAYER_CONDUIT)
        {
            if (LocateConduit((int)position[0], (int)position[1]) != null)
            {
                Destroy(false);
            }
            else
            {
                textObjectOffset = new Vector<float>(0, -0.6f);
                attachedTextObject = new TextObject($"{Powernet?.PowernetId}", Colour.White, Position + textObjectOffset, TextObject.PositionModes.WORLD_POSITION, 0.4f);
                AddNode();
                World.SetPowerCable((int)position[0], (int)position[1], this);
            }
        }

        public override Renderable Renderable { get; set; } = new StandardRenderable("power_cond_heavy.node");

        //Directions this conduit is attached to
        public Directions ConnectionDirections { get; set; } = Directions.NONE;

        //Powernet reference
        public Powernet _powernet;
        public Powernet Powernet
        {
            get => _powernet;
            set {
                _powernet?.conduits.Remove(this);
                _powernet = value;
                attachedTextObject.Text = $"{_powernet?.PowernetId}";
                _powernet.conduits.Add(this);
            }
        }

        //Adjacent Conduit nodes
        private PowerConduit northConduit;
        private PowerConduit eastConduit;
        private PowerConduit southConduit;
        private PowerConduit westConduit;

        public bool Destroy(bool initialized = true)
        {
            if (!initialized)
                return base.Destroy();
            if (!base.Destroy())
                return false;
            RemoveNode();
            World.SetPowerCable((int)Position[0], (int)Position[1], null);
            return true;
        }

        /// <summary>
        /// Create 2 new powernets and propogate the networks to see if they can be merged
        /// </summary>
        private void SplitPowernetPropogation()
        {
            //Do this on a single thread to prevent 2 powernets fighting forever
            ThreadSafeTaskManager.ExecuteThreadSafeActionUnblocking(ThreadSafeTaskManager.TASK_POWER_CONDUIT_PROPOGATION, () =>
            {
                //Change the powernet to something else
                Powernet = new Powernet();
                //Start the processing queue
                Queue<PowerConduit> processingQueue = new Queue<PowerConduit>();
                processingQueue.Enqueue(this);
                //Begin processing
                while (processingQueue.Count > 0)
                {
                    PowerConduit current = processingQueue.Dequeue();
                    //Already has our power net
                    if (current.Powernet == Powernet)
                        continue;
                    //Adopt into powernet
                    current.Powernet = Powernet;
                    //Get adjacent notes
                    if (current.northConduit != null)
                        processingQueue.Enqueue(current.northConduit);
                    if (current.eastConduit != null)
                        processingQueue.Enqueue(current.eastConduit);
                    if (current.southConduit != null)
                        processingQueue.Enqueue(current.southConduit);
                    if (current.westConduit != null)
                        processingQueue.Enqueue(current.westConduit);
                }
                return true;
            });
        }

        public void AddNode()
        {
            UpdateIconState();
            //Update adjacent icon states
            Powernet parentNet = null;
            //North
            northConduit?.UpdateIconState();
            if (northConduit?.Powernet != null)
                parentNet = northConduit.Powernet;
            //East
            eastConduit?.UpdateIconState();
            if (parentNet != null)
            {
                if (eastConduit?.Powernet != parentNet)
                {
                    if (eastConduit?.Powernet != null)
                        parentNet.Adopt(eastConduit.Powernet);
                    else if (eastConduit != null)
                        eastConduit.Powernet = parentNet;
                }
            }
            else if (eastConduit?.Powernet != null)
                parentNet = eastConduit.Powernet;
            //South
            southConduit?.UpdateIconState();
            if (parentNet != null)
            {
                if (southConduit?.Powernet != parentNet)
                {
                    if (southConduit?.Powernet != null)
                        parentNet.Adopt(southConduit.Powernet);
                    else if (southConduit != null)
                        southConduit.Powernet = parentNet;
                }
            }
            else if (southConduit?.Powernet != null)
                parentNet = southConduit.Powernet;
            //West
            westConduit?.UpdateIconState();
            if (parentNet != null)
            {
                if (westConduit?.Powernet != parentNet)
                {
                    if (westConduit?.Powernet != null)
                        parentNet.Adopt(westConduit.Powernet);
                    else if (westConduit != null)
                        westConduit.Powernet = parentNet;
                }
            }
            else if (westConduit?.Powernet != null)
                parentNet = westConduit.Powernet;
            //Merge / adopt adjacent powernets
            if (parentNet == null)
                Powernet = new Powernet();
            else
                Powernet = parentNet;
        }

        public void RemoveNode()
        {
            UpdateIconState();
            northConduit?.UpdateIconState();
            northConduit?.SplitPowernetPropogation();
            eastConduit?.UpdateIconState();
            eastConduit?.SplitPowernetPropogation();
            southConduit?.UpdateIconState();
            southConduit?.SplitPowernetPropogation();
            westConduit?.UpdateIconState();
            westConduit?.SplitPowernetPropogation();
        }

        public void LocateAdjacentConduits()
        {
            int x = (int)Position[0];
            int y = (int)Position[1];
            //Locate adjacent nodes
            northConduit = LocateConduit(x, y + 1);
            eastConduit = LocateConduit(x + 1, y);
            southConduit = LocateConduit(x, y - 1);
            westConduit = LocateConduit(x - 1, y);
        }

        public void UpdateIconState()
        {
            LocateAdjacentConduits();
            //Set our icon state
            int count = (northConduit != null ? 1 : 0) + (eastConduit != null ? 1 : 0) + (southConduit != null ? 1 : 0) + (westConduit != null ? 1 : 0);
            int i = 0;
            string[] stringsToJoin;
            if (count == 1)
            {
                stringsToJoin = new string[2];
                stringsToJoin[i++] = "0";
            }
            else
            {
                stringsToJoin = new string[count];
            }
            if (northConduit != null)
                stringsToJoin[i++] = "1";
            if (southConduit != null)
                stringsToJoin[i++] = "2";
            if (eastConduit != null)
                stringsToJoin[i++] = "4";
            if (westConduit != null)
                stringsToJoin[i++] = "8";
            Texture = $"power_cond_heavy.{(count == 0 ? "node" : string.Join("-", stringsToJoin))}";
        }

        private PowerConduit LocateConduit(int x, int y)
        {
            List<Structure> locatedStructures = World.GetStructures(x, y);
            for (int i = locatedStructures.Count - 1; i >= 0; i = Math.Min(i - 1, locatedStructures.Count - 1))
            {
                Structure structure = locatedStructures[i];
                //Ignore ourself
                if (structure == this)
                    continue;
                if (structure is PowerConduit)
                    return structure as PowerConduit;
            }
            return null;
        }

    }
}
