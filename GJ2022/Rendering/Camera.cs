﻿using GJ2022.Audio;
using GJ2022.Utility.MathConstructs;
using GLFW;
using System;
using static OpenGL.Gl;

namespace GJ2022.Rendering
{
    public class Camera
    {

        //Position of the camera in world space
        private Vector<float> Position = new Vector<float>(0, 0, 0);

        private Vector<float> Scale = new Vector<float>(1, 1, 1);

        //View matrix of the camera
        public Matrix ViewMatrix { get; private set; } = Matrix.Identity[4];

        //public Matrix ProjectionMatrix { get; private set; } = Matrix.GetPerspectiveMatrix(90.0f, 0.01f, 100.0f);
        public Matrix ProjectionMatrix { get; private set; } = Matrix.GetScaleMatrix(1080 / 1920f, 1, 0.01f);

        private float speed = 0.04f;

        private double lastTime = 0;

        private Vector<float> GetForwardVector()
        {
            Matrix rotation = Matrix.GetRotationMatrix(0, 0, 0) * Matrix.GetRotationMatrix(0, 0, 0);
            Matrix unitVector = new Matrix(new float[,]
            {
                { 0 },
                { 0 },
                { 1 },
                { 1 },
            });
            Matrix output = rotation * unitVector;
            return new Vector<float>(output[1, 1], output[1, 2], output[1, 3]);
        }

        private Vector<float> GetUpVector()
        {
            Matrix rotation = Matrix.GetRotationMatrix(0, 0, 0) * Matrix.GetRotationMatrix(0, 0, 0);
            Matrix unitVector = new Matrix(new float[,]
            {
                { 0 },
                { 1 },
                { 0 },
                { 1 },
            });
            Matrix output = rotation * unitVector;
            return new Vector<float>(output[1, 1], output[1, 2], output[1, 3]);
        }

        private Vector<float> GetRightVector()
        {
            Matrix rotation = Matrix.GetRotationMatrix(0, 0, 0) * Matrix.GetRotationMatrix(0, 0, 0);
            Matrix unitVector = new Matrix(new float[,]
            {
                { 1 },
                { 0 },
                { 0 },
                { 1 },
            });
            Matrix output = rotation * unitVector;
            return new Vector<float>(output[1, 1], output[1, 2], output[1, 3]);
        }

        public void DebugMove(Window window)
        {
            double currentTime = Glfw.Time;
            speed = (4f / Scale[0]) * (float)(currentTime - lastTime);
            lastTime = currentTime;
            if (Glfw.GetKey(window, Keys.D) == InputState.Press)
            {
                Position += GetRightVector() * speed;
            }
            else if (Glfw.GetKey(window, Keys.A) == InputState.Press)
            {
                Position -= GetRightVector() * speed;
            }
            if (Glfw.GetKey(window, Keys.W) == InputState.Press)
            {
                Position += GetUpVector() * speed;
            }
            else if (Glfw.GetKey(window, Keys.S) == InputState.Press)
            {
                Position -= GetUpVector() * speed;
            }
            if (Glfw.GetKey(window, Keys.R) == InputState.Press)
            {
                glPolygonMode(GL_FRONT_AND_BACK, GL_LINE);
            }
            if (Glfw.GetKey(window, Keys.T) == InputState.Press)
            {
                glPolygonMode(GL_FRONT_AND_BACK, GL_FILL);
            }

            //ProjectionMatrix = Matrix.Identity[4];
            //ViewMatrix = Matrix.GetTranslationMatrix(3 * (float)Math.Sin(Glfw.Time), 3 * (float)Math.Cos(Glfw.Time), -5);
            //ViewMatrix = Matrix.GetTranslationMatrix((float)Math.Sin(Glfw.Time) * 5.0f, (float)Math.Cos(Glfw.Time) * 5.0f, 0);
            ViewMatrix = Matrix.GetScaleMatrix(Scale[0], Scale[1], Scale[2]) * Matrix.GetTranslationMatrix(Position.X, Position.Y, Position.Z);

            //Update audio
            AudioMaster.UpdateListener(Position.X, Position.Y, 0);

        }

        public void OnScroll(double offset)
        {
            Scale[0] = Math.Max(Math.Min(Scale[0] * (Math.Sign((float)offset) * 0.1f + 1), 2.0f), 0.05f);
            Scale[1] = Math.Max(Math.Min(Scale[1] * (Math.Sign((float)offset) * 0.1f + 1), 2.0f), 0.05f);
        }

        public void OnWindowResized(int width, int height)
        {
            //todo
            //ProjectionMatrix = Matrix.GetScaleMatrix((float)height / width, height / 1080f, 0.01f);
            //float xScale = width / 1920.0f;
            //float yScale = height / 1080.0f;
            //float aspectRatio = (float)height / width;
            //ProjectionMatrix = Matrix.GetTranslationMatrix(-xScale * 0.5f, 0, 0) * Matrix.GetScaleMatrix(xScale, yScale, 0.01f);
        }

    }
}
