using GJ2022.Audio;
using GJ2022.Entities;
using GJ2022.Entities.Items.Stacks;
using GJ2022.Entities.Pawns;
using GJ2022.Entities.Pawns.Mobs;
using GJ2022.Entities.Pawns.Mobs.Humans;
using GJ2022.Entities.Structures.Power;
using GJ2022.EntityLoading;
using GJ2022.Game.Construction;
using GJ2022.Game.GameWorld;
using GJ2022.Managers;
using GJ2022.PawnBehaviours.Behaviours;
using GJ2022.Rendering;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Rendering.Text;
using GJ2022.Rendering.Textures;
using GJ2022.Subsystems;
using GJ2022.UserInterface;
using GJ2022.Utility.MathConstructs;
using GLFW;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using static OpenGL.Gl;

namespace GJ2022
{

    public class Program
    {

        public static bool UsingOpenGL = false;

        /// <summary>
        /// Entry point of the program
        /// 
        /// Args:
        /// -s [port] (Host the server with given port)
        /// -c [address] [port] (Host the client with given address and port)
        /// </summary>
        private static void Main(string[] args)
        {

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;

            EntityLoader.LoadEntities();
            //return;

            //Start texture loading
            BlueprintLoader.LoadBlueprints();

            //Set the window hints
            SetWindowHints();

            //Create the window
            Window window = SetupWindow();
            UsingOpenGL = true;

            //Initialize the renderer
            RenderMaster.Initialize();

            //Load texture data after openGL
            TextureCache.LoadTextureDataJson();

            //Setup open AL
            try
            {
                AudioMaster.Initialize();
            }
            catch (System.TypeInitializationException e)
            {
                Log.WriteLine("===FAILED TO LOAD AUDIO===", LogType.ERROR);
                Log.WriteLine(e, LogType.ERROR);
            }
            //new AudioSource().PlaySound("effects/picaxe1.wav", 0, 0, repeating: true);
            //new AudioSource().PlaySound("effects/picaxe1.wav", 0, -60, repeating: true);

            //Create callbacks
            SetCallbacks(window);

            //Start subsystems
            Subsystem.InitializeSingletons();

            //Load text
            TextLoader.LoadText();

            //Wait until texture loading is done
            Log.WriteLine("Waiting for async loading to complete...", LogType.DEBUG);
            while (!TextureCache.LoadingComplete || !BlueprintLoader.BlueprintsLoaded) Thread.Sleep(1);
            Log.WriteLine("Done loading", LogType.DEBUG);

            TextureCache.InitializeTextureObjects();

            //Create the error texture (so it has uint of 0)
            TextureCache.GetTexture("error");

            //Trigger on world init
            Subsystem.InitializeSubsystems(window);

            //Create the background first
            new BackgroundRenderable().StartRendering();

            for (int i = 0; i < 4; i++)
            {
                Human p = new Human(new Vector<float>(2.3f, 7.3f));
                EntityCreator.CreateEntity<Entity>("OxygenTank", new Vector<float>(1, 3)).SendSignal(Components.Signal.SIGNAL_ITEM_EQUIP_TO_PAWN, p);
                EntityCreator.CreateEntity<Entity>("SpaceSuit", new Vector<float>(1, 3)).SendSignal(Components.Signal.SIGNAL_ITEM_EQUIP_TO_PAWN, p);
                EntityCreator.CreateEntity<Entity>("SpaceHelmet", new Vector<float>(1, 3)).SendSignal(Components.Signal.SIGNAL_ITEM_EQUIP_TO_PAWN, p);
                EntityCreator.CreateEntity<Entity>("BreathMask", new Vector<float>(1, 3)).SendSignal(Components.Signal.SIGNAL_ITEM_EQUIP_TO_PAWN, p);
                new CrewmemberBehaviour(p);
            }

            for (int x = 4; x < 6; x++)
            {
                for (int y = 4; y < 6; y++)
                {
                    EntityCreator.CreateEntity<Entity>("Iron", new Vector<float>(x, y))
                        .SendSignal(Components.Signal.SIGNAL_SET_STACK_SIZE, 50);
                }
            }

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    EntityCreator.CreateEntity("Plating", new Vector<float>(x, y));
                }
            }

            //new BreathMask(new Vector<float>(3, 3));

            Dog dog = new Dog(new Vector<float>(2, 2));
            new DogBehaviour(dog);

            new PacmanGenerator(new Vector<float>(7, 4));

            Random r = new Random();
            for (int x = 20; x < 100; x += r.Next(1, 10))
            {
                for (int y = 20; y < 100; y += r.Next(1, 10))
                {
                    EntityCreator.CreateEntity<Entity>("Gold", new Vector<float>(x, y))
                        .SendSignal(Components.Signal.SIGNAL_SET_STACK_SIZE, 50);
                }
            }

            UserInterfaceCreator.CreateUserInterface();

            //This is last
            Subsystem.WorldInitialize();

            //Rendering Loop
            double lastTime = 0;
            while (!Glfw.WindowShouldClose(window))
            {
                try
                {
                    double currentTime = Glfw.Time;
                    TimeManager.UpdateTime(currentTime - lastTime);
                    lastTime = currentTime;
                    //Perform rendering
                    RenderMaster.RenderWorld(window);
                }
                catch (System.Exception e)
                {
                    Log.WriteLine(e, LogType.ERROR);
                }
            }

            //Cleanup openAL
            AudioMaster.Cleanup();

            //Kill subsystems
            Subsystem.KillSubsystems();

            //Terminate GLFW
            Glfw.Terminate();

        }

        /// <summary>
        /// Set the window hints
        /// </summary>
        private static void SetWindowHints()
        {
            Glfw.WindowHint(Hint.ContextVersionMajor, 3);
            Glfw.WindowHint(Hint.ContextVersionMinor, 3);
            Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);
            Glfw.WindowHint(Hint.Resizable, true);
        }

        /// <summary>
        /// Setup the window, make it 1920 x 1080 by default
        /// TODO: Have it scale to screen resolution
        /// </summary>
        private static Window SetupWindow()
        {
            Window window = Glfw.CreateWindow(1920, 1080, "GJ2022", GLFW.Monitor.None, Window.None);
            Glfw.MakeContextCurrent(window);
            Import(Glfw.GetProcAddress);
            return window;
        }

        /// <summary>
        /// Setup the callback methods
        /// </summary>
        private static void SetCallbacks(Window window)
        {
            Glfw.SetWindowSizeCallback(window, (IntPtr windowPtr, int width, int height) => WindowSizeCallback(windowPtr, width, height));
            Glfw.SetScrollCallback(window, (IntPtr windowPtr, double x, double y) => RenderMaster.mainCamera.OnScroll(y)); ;
        }

        /// <summary>
        /// Called when the window has been resized.
        /// Recalculate the view matrix of the camera
        /// </summary>
        private static void WindowSizeCallback(IntPtr window, int width, int height)
        {
            RenderMaster.mainCamera.OnWindowResized(width, height);
            glViewport(0, 0, width, height);
        }

    }
}
