﻿using GJ2022.Entities.Debug;
using GJ2022.Entities.Items.Clothing.Back;
using GJ2022.Entities.Items.Clothing.Body;
using GJ2022.Entities.Items.Stacks;
using GJ2022.Entities.Items.Tools.Mining;
using GJ2022.Entities.Pawns;
using GJ2022.Entities.Turfs.Standard.Floors;
using GJ2022.Game.Construction;
using GJ2022.Managers.Stockpile;
using GJ2022.PawnBehaviours.Behaviours;
using GJ2022.Rendering;
using GJ2022.Rendering.RenderSystems;
using GJ2022.Rendering.RenderSystems.LineRenderer;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Rendering.Text;
using GJ2022.Rendering.Textures;
using GJ2022.Subsystems;
using GJ2022.UserInterface;
using GJ2022.UserInterface.Components;
using GJ2022.UserInterface.Components.Advanced;
using GJ2022.UserInterface.Factory;
using GJ2022.Utility.Helpers;
using GJ2022.Utility.MathConstructs;
using GJ2022.WorldGeneration;
using GLFW;
using System;
using System.Threading;
using static GJ2022.UserInterface.Components.UserInterfaceButton;
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
        static void Main(string[] args)
        {

            //Start texture loading
            TextureCache.LoadTextureDataJson();
            BlueprintLoader.LoadBlueprints();

            //Set the window hints
            SetWindowHints();

            //Create the window
            Window window = SetupWindow();
            UsingOpenGL = true;

            //Create callbacks
            SetCallbacks(window);

            //Start subsystems
            Subsystem.InitializeSingletons();

            //Load text
            TextLoader.LoadText();

            //Initialize the renderer
            RenderMaster.Initialize();

            //Wait until texture loading is done
            Log.WriteLine("Waiting for async loading to complete...", LogType.DEBUG);
            while (!TextureCache.LoadingComplete || !BlueprintLoader.BlueprintsLoaded) { }
            Log.WriteLine("Done loading", LogType.DEBUG);

            //Create the error texture (so it has uint of 0)
            TextureCache.GetTexture("error");

            //Trigger on world init
            Subsystem.InitializeSubsystems(window);

            //Create the background first
            new BackgroundRenderable().StartRendering();

            Pawn jetpackPawn = null;
            for (int i = 0; i < 4; i++)
            {
                Human p = new Human(new Vector<float>(2.3f, 7.3f));
                p.TryEquipItem(InventorySlot.SLOT_BODY, new SpaceSuit(new Vector<float>(0, 0)));
                jetpackPawn = p;
                new CrewmemberBehaviour(p);
            }

            new SyndicateHardsuit(new Vector<float>(4, 7));

            for (int x = 4; x < 6; x++)
            {
                for (int y = 4; y < 6; y++)
                {
                    new Iron(new Vector<float>(x, y), 50, 50);
                }
            }

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    new Plating(x, y);
                }
            }

            new Jetpack(new Vector<float>(9, 8));
            new Pickaxe(new Vector<float>(3, 2));

            jetpackPawn.TryEquipItem(InventorySlot.SLOT_BACK, new Jetpack(new Vector<float>(9, 8)));

            Random r = new Random();
            for (int x = 20; x < 100; x += r.Next(1, 10))
            {
                for (int y = 20; y < 100; y += r.Next(1, 10))
                {
                    new Gold(new Vector<float>(x, y), 50, 50);
                }
            }

            UserInterfaceCreator.CreateUserInterface();

            //This is last
            Subsystem.WorldInitialize();

            //Rendering Loop
            while (!Glfw.WindowShouldClose(window))
            {
                try
                {
                    //Perform rendering
                    RenderMaster.RenderWorld(window);
                }
                catch (System.Exception e)
                {
                    Log.WriteLine(e, LogType.ERROR);
                }
            }

            //Kill subsystems
            Subsystem.KillSubsystems();

            //Terminate GLFW
            Glfw.Terminate();
        }

        /// <summary>
        /// Set the window hints
        /// </summary>
        static void SetWindowHints()
        {
            Glfw.WindowHint(Hint.ContextVersionMajor, 3);
            Glfw.WindowHint(Hint.ContextVersionMinor, 3);
            Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);
            Glfw.WindowHint(Hint.Resizable, false);
        }

        /// <summary>
        /// Setup the window, make it 1920 x 1080 by default
        /// TODO: Have it scale to screen resolution
        /// </summary>
        static Window SetupWindow()
        {
            Window window = Glfw.CreateWindow(1920, 1080, "GJ2022", GLFW.Monitor.None, Window.None);
            Glfw.MakeContextCurrent(window);
            Import(Glfw.GetProcAddress);
            return window;
        }

        /// <summary>
        /// Setup the callback methods
        /// </summary>
        static void SetCallbacks(Window window)
        {
            Glfw.SetWindowSizeCallback(window, (IntPtr windowPtr, int width, int height) => WindowSizeCallback(windowPtr, width, height));
            Glfw.SetScrollCallback(window, (IntPtr windowPtr, double x, double y) => RenderMaster.mainCamera.OnScroll(y)); ;
        }

        /// <summary>
        /// Called when the window has been resized.
        /// Recalculate the view matrix of the camera
        /// </summary>
        static void WindowSizeCallback(IntPtr window, int width, int height)
        {
            RenderMaster.mainCamera.OnWindowResized(width, height);
        }

    }
}
