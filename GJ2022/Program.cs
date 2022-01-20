using GLFW;
using GJ2022.Rendering;
using GJ2022.Rendering.RenderSystems.LineRenderer;
using GJ2022.Rendering.Textures;
using GJ2022.Subsystems;
using GJ2022.Utility.MathConstructs;
using System.Collections.Generic;
using static OpenGL.Gl;
using GJ2022.Entities.Debug;
using System.Numerics;
using GJ2022.Rendering.RenderSystems;
using System.Drawing;

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

            //Start subsystems
            Subsystem.InitializeSubsystems();

            Subsystem.WorldInitialize();

            //Start texture loading
            TextureCache.LoadTextureDataJson();

            //Set the window hints
            SetWindowHints();

            //Create the window
            Window window = SetupWindow();
            UsingOpenGL = true;

            //Initialize the renderer
            RenderMaster.Initialize();

            //Wait until texture loading is done
            Log.WriteLine("Waiting for async loading to complete...", LogType.DEBUG);
            while (!TextureCache.LoadingComplete) { }
            Log.WriteLine("Done loading", LogType.DEBUG);

            //Create a debug thingy
            InstanceRenderSystem.Singleton.StartRendering(new DebugEntity(new Vector(3, 0, 0, -5)));
            InstanceRenderSystem.Singleton.StartRendering(new DebugEntity(new Vector(3, 0, 0, 5)));
            InstanceRenderSystem.Singleton.StartRendering(new DebugEntity(new Vector(3, 5, 0, 0)));
            InstanceRenderSystem.Singleton.StartRendering(new DebugEntity(new Vector(3, -5, 0, 0)));

            Line.StartDrawingLine(new Vector(3, -5, 0, 0), new Vector(3, 5, 0, 0), Colour.Red);
            Line.StartDrawingLine(new Vector(3, 0, -5, 0), new Vector(3, 0, 5, 0), Colour.Green);
            Line.StartDrawingLine(new Vector(3, 0, 0, -5), new Vector(3, 0, 0, 5), Colour.Blue);

            //Rendering Loop
            while (!Glfw.WindowShouldClose(window))
            {
                try
                {
                    //Perform rendering
                    RenderMaster.RenderWorld(window);
                }
                catch (Exception e)
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

    }
}
