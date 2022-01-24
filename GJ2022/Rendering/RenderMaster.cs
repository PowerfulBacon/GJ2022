using GJ2022.Rendering.RenderSystems;
using GJ2022.Rendering.RenderSystems.LineRenderer;
using GLFW;
using static OpenGL.Gl;

namespace GJ2022.Rendering
{
    static class RenderMaster
    {

        //A list of all render systems in use by the game.
        //Each frame, these will all be executed, rendering anything
        //they should render.
        private static RenderSystem[] renderSystems;

        //The main camera
        public static Camera mainCamera;

        /// <summary>
        /// Initialize the renderer:
        /// Create the camera.
        /// Create the shaders.
        /// Set the openGl stuff.
        /// </summary>
        public static unsafe void Initialize()
        {

            //Create the camera
            mainCamera = new Camera();

            //Set the openGL stuff
            SetOpenGlFlags();

            //Load the render systems
            InitRenderSystems();
        }

        private static void InitRenderSystems()
        {
            renderSystems = new RenderSystem[] {
                new LineRenderer(),
                new TextRenderSystem(),
                new CircleRenderSystem(),
                new BlueprintRenderSystem(),
                new InstanceRenderSystem(),
                new OutlineQuadRenderSystem(),
                new BackgroundRenderSystem(),
            };
        }

        private static void SetOpenGlFlags()
        {
            //Enable backface culling
            glEnable(GL_CULL_FACE);
            glCullFace(GL_BACK);
            glFrontFace(GL_CW);
            //Enable depth test -> We use z to represent the layer an object is on
            glEnable(GL_DEPTH_TEST);
            //Use the lowest number fragment
            glDepthFunc(GL_LEQUAL);
            //Enable blending for transparent objects
            glEnable(GL_BLEND);
            glBlendFuncSeparate(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA, GL_ONE, GL_ZERO);
            //Set a background colour
            glClearColor(255 / 255f, 105 / 255f, 180 / 255f, 1.0f);
        }

        /// <summary>
        /// Handles rendering everything that needs to be rendered
        /// </summary>
        public static unsafe void RenderWorld(Window window)
        {

            //Swap framebuffers (What does this actuall do? (dunno but its expensive))
            Glfw.SwapBuffers(window);
            //Poll for system events (Keypresses etc.)
            Glfw.PollEvents();

            //Clear the screen and reset it to the background colour
            glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

            mainCamera.DebugMove(window);

            //Process each render system
            foreach (RenderSystem renderSystem in renderSystems)
            {
                renderSystem.BeginRender(mainCamera);
                renderSystem.RenderModels(mainCamera);
                renderSystem.EndRender();
            }

        }

    }
}
