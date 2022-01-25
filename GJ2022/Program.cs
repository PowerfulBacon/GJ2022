﻿using GJ2022.Entities.Debug;
using GJ2022.Entities.Items.Stacks;
using GJ2022.Entities.Pawns;
using GJ2022.Managers;
using GJ2022.Rendering;
using GJ2022.Rendering.RenderSystems;
using GJ2022.Rendering.RenderSystems.LineRenderer;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Rendering.Text;
using GJ2022.Rendering.Textures;
using GJ2022.Subsystems;
using GJ2022.UserInterface.Components;
using GJ2022.UserInterface.Components.Advanced;
using GJ2022.UserInterface.Factory;
using GJ2022.Utility.Helpers;
using GJ2022.Utility.MathConstructs;
using GLFW;
using System;
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

            //World creation here


            //Initialize the renderer
            RenderMaster.Initialize();

            //Trigger on world init
            Subsystem.InitializeSubsystems(window);
            Subsystem.WorldInitialize();

            //Wait until texture loading is done
            Log.WriteLine("Waiting for async loading to complete...", LogType.DEBUG);
            while (!TextureCache.LoadingComplete) { }
            Log.WriteLine("Done loading", LogType.DEBUG);

            //Create the background first
            new BackgroundRenderable().StartRendering();
            //new BackgroundEntity(new Vector(3));

            Line.StartDrawingLine(new Vector<float>(-5, 0, 0), new Vector<float>(5, 0, 0), Colour.Red);
            Line.StartDrawingLine(new Vector<float>(0, -5, 0), new Vector<float>(0, 5, 0), Colour.Green);
            Line.StartDrawingLine(new Vector<float>(0, 0, -5), new Vector<float>(0, 0, 5), Colour.Blue);

            BuildModeSubsystem.Singleton.ActivateBuildMode();

            for (int i = 0; i < 5; i++)
            {
                new Pawn(new Vector<float>(2.3f, 7.3f, 5));
            }

            Iron iron = new Iron(new Vector<float>(20, 20, 20), 50, 50);
            StockpileManager.AddItem(iron);

            //new TextObject("Debug Screen", Colour.Red, new Vector<float>(0, 0), TextObject.PositionModes.SCREEN_POSITION, 0.2f);
            new TextObject("Debug World", Colour.Red, new Vector<float>(0, 0), TextObject.PositionModes.WORLD_POSITION, 1.0f);

            new DebugEntity(new Vector<float>(-5, -5));

            new UserInterfaceTextIcon(
                "Iron - 50",
                "iron",
                Colour.White,
                CoordinateHelper.PixelsToScreen(new Vector<float>(-1920 + 60, 1080 - 70)),
                CoordinateHelper.PixelsToScreen(new Vector<float>(100, 100)),
                TextObject.PositionModes.SCREEN_POSITION,
                CoordinateHelper.PixelsToScreen(100),
                10);

            DropdownFactory.CreateDropdown(
                CoordinateHelper.PixelsToScreen(new Vector<float>(-1920 + 150, -1080 + 40)),
                "Building",
                new string[] { "Foundation" },
                new OnButtonPressed[] { null });
            DropdownFactory.CreateDropdown(
                CoordinateHelper.PixelsToScreen(new Vector<float>(-1920 + 450, -1080 + 40)),
                "Actions",
                new string[] { "Deconstruct", "Mine" },
                new OnButtonPressed[] { null, null });
            DropdownFactory.CreateDropdown(
                CoordinateHelper.PixelsToScreen(new Vector<float>(-1920 + 750, -1080 + 40)),
                "Debug",
                new string[] { "1", "2", "3", "4", "5", "6", "7", "8" },
                new OnButtonPressed[] { null, null, null, null, null, null, null, null, null });

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
