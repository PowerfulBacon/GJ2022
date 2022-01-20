﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenGL.Gl;

namespace GJ2022.Rendering.RenderSystems.LineRenderer
{
    class LineRenderer : RenderSystem
    {

        public static LineRenderer Singleton;

        //List of lines we are rendering
        public List<Line> rendering { get; } = new List<Line>();

        protected override string SystemShaderName => "simple";

        //Location of uniform variables
        private int objectMatrixUniformLocation;
        private int viewMatrixUniformLocation;
        private int projectionMatrixUniformLocation;
        private int colourUniformLocation;

        //Buffer indexes
        private uint lineVertexArrayObject;
        private uint lineVertexBufferObject;
        private uint lineUvBuffer;

        public LineRenderer()
        {
            Singleton = this;
        }

        /// <summary>
        /// Start rendering a line.
        /// The line will continue to render until StopRendering(line)
        /// is called.
        /// </summary>
        /// <param name="line">The line to start rendering</param>
        public void StartRendering(Line line)
        {
            rendering.Add(line);
        }

        /// <summary>
        /// Stops rendering a line.
        /// If the line is not being rendered already, nothing will
        /// happen.
        /// </summary>
        /// <param name="line">The line to stop rendering</param>
        public void StopRendering(Line line)
        {
            rendering.Remove(line);
        }

        public unsafe override void Initialize()
        {

            //Load the line model and UV into buffers
            float[] lineModel = new float[]
            {
                0f, 0f, 0f,
                1f, 1f, 1f
            };

            float[] lineUvs = new float[]
            {
                0f, 0f,
                1f, 1f
            };

            //Generate the line VAO
            lineVertexArrayObject = glGenVertexArray();
            glBindVertexArray(lineVertexArrayObject);

            //Generate the VBO
            lineVertexBufferObject = glGenBuffer();
            glBindBuffer(GL_ARRAY_BUFFER, lineVertexBufferObject);

            //Fill the vertex buffer object with data about our vertices.
            fixed (float* v = &lineModel[0])
            {
                glBufferData(GL_ARRAY_BUFFER, sizeof(float) * lineModel.Length, v, GL_STATIC_DRAW);
            }

            //Generate the UV buffer
            lineUvBuffer = glGenBuffer();
            glBindBuffer(GL_ARRAY_BUFFER, lineUvBuffer);

            //Generate the UV Buffer data
            fixed (float* u = &lineUvs[0])
            {
                glBufferData(GL_ARRAY_BUFFER, sizeof(float) * lineUvs.Length, u, GL_STATIC_DRAW);
            }

            //Attach the shader set so we can grab uniform locations
            //Link program and use program are required here, not sure what they do exactly.
            SystemShaders.AttachShaders(programUint);
            glLinkProgram(programUint);

            //Get the uniform locations
            viewMatrixUniformLocation = glGetUniformLocation(programUint, "viewMatrix");
            projectionMatrixUniformLocation = glGetUniformLocation(programUint, "projectionMatrix");
            objectMatrixUniformLocation = glGetUniformLocation(programUint, "objectMatrix");
            colourUniformLocation = glGetUniformLocation(programUint, "colour");

            //Detatch the shaders
            SystemShaders.DetatchShaders(programUint);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainCamera"></param>
        /// <param name="programUint"></param>
        public unsafe override void BeginRender(Camera mainCamera)
        {

            //Attach the shader set
            //SystemShaders.AttachShaders(programUint);
            glUseProgram(programUint);

            //Load the camera's view matrix
            //Put the matrix into that uniform variable
            glUniformMatrix4fv(viewMatrixUniformLocation, 1, false, mainCamera.ViewMatrix.GetPointer());

            //Load the camera's projection matrix
            //Put the matrix into that uniform variable
            glUniformMatrix4fv(projectionMatrixUniformLocation, 1, false, mainCamera.ProjectionMatrix.GetPointer());

            //Bind the vertex buffer object and the vertex array object
            BindAttribArray(0, lineVertexBufferObject, 3);
            BindAttribArray(1, lineUvBuffer, 2);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="programUint"></param>
        public override void EndRender()
        {

            //Disable the vertex arrays
            glDisableVertexAttribArray(0);
            glDisableVertexAttribArray(1);

            //Detatch the shader set?
            //glUseProgram(0);
            //SystemShaders.DetatchShaders(programUint);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="programUint"></param>
        public unsafe override void RenderModels(Camera mainCamera)
        {

            //Render each line seperately
            //A lot slower than the instance renderer,
            //however lines will rarely be rendered outside
            //of debugging.
            //We should probably disable the line renderer
            //all together if in release build.
            foreach (Line line in rendering)
            {
                //Send in data to the uniform values
                glUniformMatrix4fv(objectMatrixUniformLocation, 1, false, line.ObjectMatrix.GetPointer());

                //Send in data to the uniform values
                glUniform4f(colourUniformLocation, line.Colour.red, line.Colour.green, line.Colour.blue, line.Colour.alpha);

                //Draw the line
                glDrawArrays(GL_LINES, 0, 2);
            }

        }

    }
}
