using GLFW;
using GJ2022.Rendering.RenderSystems.LineRenderer;
using GJ2022.Utility.MathConstructs;
using System;
using static OpenGL.Gl;

namespace GJ2022.Rendering
{
    public class Camera
    {

        //Position of the camera in world space
        private Vector Position = new Vector(3);

        //Rotation of the camera in euler angles
        private Vector Rotation = new Vector(3);

        //View matrix of the camera
        public Matrix ViewMatrix { get; private set; } = Matrix.Identity[4];

        public Matrix ProjectionMatrix { get; private set; } = Matrix.GetPerspectiveMatrix(90.0f, 0.01f, 100.0f);

        private float speed = 0.04f;

        private Vector GetForwardVector()
        {
            Matrix rotation = Matrix.GetRotationMatrix(0, -Rotation[1], 0) * Matrix.GetRotationMatrix(-Rotation[0], 0, 0);
            Matrix unitVector = new Matrix(new float[,]
            {
                { 0 },
                { 0 },
                { 1 },
                { 1 },
            });
            Matrix output = rotation * unitVector;
            return new Vector(3, output[1, 1], output[1, 2], output[1, 3]);
        }

        private Vector GetUpVector()
        {
            Matrix rotation = Matrix.GetRotationMatrix(0, -Rotation[1], 0) * Matrix.GetRotationMatrix(-Rotation[0], 0, 0);
            Matrix unitVector = new Matrix(new float[,]
            {
                { 0 },
                { 1 },
                { 0 },
                { 1 },
            });
            Matrix output = rotation * unitVector;
            return new Vector(3, output[1, 1], output[1, 2], output[1, 3]);
        }

        private Vector GetRightVector()
        {
            Matrix rotation = Matrix.GetRotationMatrix(0, -Rotation[1], 0) * Matrix.GetRotationMatrix(-Rotation[0], 0, 0);
            Matrix unitVector = new Matrix(new float[,]
            {
                { 1 },
                { 0 },
                { 0 },
                { 1 },
            });
            Matrix output = rotation * unitVector;
            return new Vector(3, output[1, 1], output[1, 2], output[1, 3]);
        }

        private Line savedLine;

        public void DebugMove(Window window)
        {
            if (Glfw.GetKey(window, Keys.W) == InputState.Press)
            {
                Position += GetForwardVector() * speed;
            }
            else if (Glfw.GetKey(window, Keys.S) == InputState.Press)
            {
                Position -= GetForwardVector() * speed;
            }
            if (Glfw.GetKey(window, Keys.D) == InputState.Press)
            {
                Position += GetRightVector() * speed;
            }
            else if (Glfw.GetKey(window, Keys.A) == InputState.Press)
            {
                Position -= GetRightVector() * speed;
            }
            if (Glfw.GetKey(window, Keys.Space) == InputState.Press)
            {
                Position += GetUpVector() * speed;
            }
            else if (Glfw.GetKey(window, Keys.LeftShift) == InputState.Press)
            {
                Position -= GetUpVector() * speed;
            }
            if (Glfw.GetKey(window, Keys.O) == InputState.Press)
            {
                speed *= 2;
            }
            if (Glfw.GetKey(window, Keys.I) == InputState.Press)
            {
                speed /= 2;
            }
            if (Glfw.GetKey(window, Keys.R) == InputState.Press)
            {
                glPolygonMode(GL_FRONT_AND_BACK, GL_LINE);
            }
            if (Glfw.GetKey(window, Keys.T) == InputState.Press)
            {
                glPolygonMode(GL_FRONT_AND_BACK, GL_FILL);
            }
            double mouseX;
            double mouseY;
            Glfw.GetCursorPosition(window, out mouseX, out mouseY);
            double deltaX = (1920 / 2) - mouseX;
            double deltaY = (1080 / 2) - mouseY;
            Glfw.SetCursorPosition(window, 1920 / 2, 1080 / 2);

            Rotation[1] -= (float) deltaX * 0.001f;
            Rotation[0] -= (float) deltaY * 0.001f;

            //ProjectionMatrix = Matrix.Identity[4];
            //ViewMatrix = Matrix.GetTranslationMatrix(3 * (float)Math.Sin(Glfw.Time), 3 * (float)Math.Cos(Glfw.Time), -5);
            //ViewMatrix = Matrix.GetTranslationMatrix((float)Math.Sin(Glfw.Time) * 5.0f, (float)Math.Cos(Glfw.Time) * 5.0f, 0);
            ViewMatrix = Matrix.Identity[4];
            ViewMatrix *= Matrix.GetRotationMatrix(Rotation[0], 0, Rotation[2]);
            ViewMatrix *= Matrix.GetRotationMatrix(0, Rotation[1], 0);
            ViewMatrix *= Matrix.GetTranslationMatrix(Position[0], Position[1], Position[2]);
        }

    }
}
