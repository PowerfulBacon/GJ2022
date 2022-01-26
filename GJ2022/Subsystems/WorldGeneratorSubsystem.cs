using GJ2022.Rendering;
using GJ2022.Utility.MathConstructs;
using GJ2022.WorldGeneration;
using GLFW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Subsystems
{
    public class WorldGeneratorSubsystem : Subsystem
    {

        public static WorldGeneratorSubsystem Singleton { get; } = new WorldGeneratorSubsystem();

        public override int sleepDelay => 100;

        public override SubsystemFlags SubsystemFlags => SubsystemFlags.NO_PROCESSING;

        private AsteroidGenerator asteroidGenerator;

        public HashSet<Vector<int>> GeneratedPositions = new HashSet<Vector<int>>();

        public override void Fire(Window window)
        {
            //Get the camera's position
            float cam_x = RenderMaster.mainCamera.ViewMatrix[4, 1] / RenderMaster.mainCamera.ViewMatrix[1, 1];
            float cam_y = RenderMaster.mainCamera.ViewMatrix[4, 2] / RenderMaster.mainCamera.ViewMatrix[2, 2]; ;
            for (int x = (int)cam_x - 50; x <= cam_x + 50; x++)
            {
                for (int y = (int)cam_y - 50; y <= cam_y + 50; y++)
                {
                    if (GeneratedPositions.Contains(new Vector<int>(x, y)))
                        continue;
                    GeneratedPositions.Add(new Vector<int>(x, y));
                    asteroidGenerator.GeneratePosition(x, y);
                }
            }
        }

        public override void InitSystem()
        {
            asteroidGenerator = new AsteroidGenerator();
        }

        protected override void AfterWorldInit()
        {
            return;
        }
    }
}
