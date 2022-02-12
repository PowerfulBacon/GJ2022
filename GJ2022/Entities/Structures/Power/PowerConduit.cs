using GJ2022.Game.GameWorld;
using GJ2022.Rendering.RenderSystems.Renderables;
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
                Destroy();
            }
            else
            {
                AddNode();
            }
        }

        public override Renderable Renderable { get; set; } = new StandardRenderable("power_cond_heavy.node");

        public Directions ConnectionDirections { get; set; } = Directions.NONE;

        public void AddNode()
        {
            int x = (int)Position[0];
            int y = (int)Position[1];
            UpdateIconState();
            LocateConduit(x, y + 1)?.UpdateIconState();
            LocateConduit(x + 1, y)?.UpdateIconState();
            LocateConduit(x, y - 1)?.UpdateIconState();
            LocateConduit(x - 1, y)?.UpdateIconState();
        }

        public void RemoveNode()
        {
            int x = (int)Position[0];
            int y = (int)Position[1];
            UpdateIconState();
            LocateConduit(x, y + 1)?.UpdateIconState();
            LocateConduit(x + 1, y)?.UpdateIconState();
            LocateConduit(x, y - 1)?.UpdateIconState();
            LocateConduit(x - 1, y)?.UpdateIconState();
        }

        public void UpdateIconState()
        {
            int x = (int)Position[0];
            int y = (int)Position[1];
            //Locate adjacent nodes
            PowerConduit northConduit = LocateConduit(x, y + 1);
            PowerConduit eastConduit = LocateConduit(x + 1, y);
            PowerConduit southConduit = LocateConduit(x, y - 1);
            PowerConduit westConduit = LocateConduit(x - 1, y);
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
