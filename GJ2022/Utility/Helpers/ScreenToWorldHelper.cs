using GJ2022.Rendering;
using GJ2022.Utility.MathConstructs;
using GLFW;

namespace GJ2022.Utility.Helpers
{
    public static class ScreenToWorldHelper
    {

        public static Vector<float> GetWorldCoordinates(Vector<float> screenCoordinates, Vector<float> screenWidth)
        {
            float cursorX = screenCoordinates[0];
            float cursorY = screenCoordinates[1];
            //Calculate the proportional position of the mouse
            cursorX /= screenWidth[0];
            cursorY /= screenWidth[1];
            //Convert from bounds [0, 1] to [-1, 1]
            cursorX = cursorX * 2 - 1;
            cursorY = cursorY * 2 - 1;
            //Guess
            Matrix cameraMatrix = RenderMaster.mainCamera.ProjectionMatrix * RenderMaster.mainCamera.ViewMatrix;
            float translationX = cameraMatrix[4, 1];
            float translationY = cameraMatrix[4, 2];
            float scaleX = cameraMatrix[1, 1];
            float scaleY = cameraMatrix[2, 2];
            //Convert the cursor screen pos into world position.
            cursorX += translationX;
            cursorY += -translationY;
            cursorX /= scaleX;
            cursorY /= -scaleY;
            return new Vector<float>(cursorX, cursorY);
        }

        public static Vector<float> GetWorldCoordinates(Window window)
        {
            //Fetch position of the mouse
            double cursorX;
            double cursorY;
            Glfw.GetCursorPosition(window, out cursorX, out cursorY);
            //Get the size of the window
            int windowWidth;
            int windowHeight;
            Glfw.GetWindowSize(window, out windowWidth, out windowHeight);
            //Calculate the proportional position of the mouse
            cursorX /= windowWidth;
            cursorY /= windowHeight;
            //Convert from bounds [0, 1] to [-1, 1]
            cursorX = cursorX * 2 - 1;
            cursorY = cursorY * 2 - 1;
            //Guess
            Matrix cameraMatrix = RenderMaster.mainCamera.ProjectionMatrix * RenderMaster.mainCamera.ViewMatrix;
            float translationX = cameraMatrix[4, 1];
            float translationY = cameraMatrix[4, 2];
            float scaleX = cameraMatrix[1, 1];
            float scaleY = cameraMatrix[2, 2];
            //Convert the cursor screen pos into world position.
            cursorX += translationX;
            cursorY += -translationY;
            cursorX /= scaleX;
            cursorY /= -scaleY;
            return new Vector<float>((float)cursorX, (float)cursorY);
        }

    }
}
