using GJ2022.Rendering;
using GJ2022.Utility.MathConstructs;
using GLFW;

namespace GJ2022.Utility.Helpers
{
    public static class WorldToScreenHelper
    {

        public static Vector<float> GetScreenCoordinates(Window window, Vector<float> worldPosition)
        {
            float worldX = worldPosition[0];
            float worldY = worldPosition[1];
            //Guess
            Matrix cameraMatrix = RenderMaster.mainCamera.ProjectionMatrix * RenderMaster.mainCamera.ViewMatrix;
            float translationX = cameraMatrix[4, 1];
            float translationY = cameraMatrix[4, 2];
            float scaleX = cameraMatrix[1, 1];
            float scaleY = cameraMatrix[2, 2];
            //Convert the cursor screen pos into world position.
            worldX *= scaleX;
            worldY *= scaleY;
            worldX -= translationX;
            worldY -= translationY;
            float worldScaleCorrector = 1920.0f / 1080.0f;
            worldX = worldX * worldScaleCorrector;
            return new Vector<float>((float)worldX, (float)worldY);
        }

    }
}
