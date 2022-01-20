﻿using System;
using System.Collections.Generic;
using System.IO;
using static OpenGL.Gl;

namespace GJ2022.Rendering.Shaders
{
    public class ShaderSet
    {

        //Relative directory of the shaders to the .exe
        public const string SHADER_DIRECTORY = "./Rendering/Shaders/";

        private static Dictionary<string, ShaderSet> loadedShaders = new Dictionary<string, ShaderSet>();

        string name;
        uint vertex_shader;
        uint fragment_shader;

        public ShaderSet(string name, string base_directory = SHADER_DIRECTORY)
        {
            //Set the name
            this.name = name;
            //Check for cached shaders
            if (loadedShaders.ContainsKey(name))
            {
                vertex_shader = loadedShaders[name].vertex_shader;
                fragment_shader = loadedShaders[name].fragment_shader;
                return;
            }
            //Generate the shaders
            vertex_shader = glCreateShader(GL_VERTEX_SHADER);
            fragment_shader = glCreateShader(GL_FRAGMENT_SHADER);
            //Load and compile shaders
            try
            {
                //Read the shader sources
                glShaderSource(vertex_shader, File.ReadAllText($"{base_directory}{name}.vert"));
                glShaderSource(fragment_shader, File.ReadAllText($"{base_directory}{name}.frag"));
                //Compile the shaders
                glCompileShader(vertex_shader);
                glCompileShader(fragment_shader);
                //Print the result of compiling the shader
                Log.WriteLine($"{name}.vert: {glGetShaderInfoLog(vertex_shader)}");
                Log.WriteLine($"{name}.frag: {glGetShaderInfoLog(fragment_shader)}");
                //Print a debug message
                Log.WriteLine($"Loaded {name} shaders successfully.");
                //Cache the shaders for faster loading later
                loadedShaders.Add(name, this);
            }
            catch (Exception e)
            {
                //Error in the file.
                Log.WriteLine($"ERROR: Failed to compile vertex shaders. {name}.vert and {name}.frag!", LogType.ERROR);
                Log.WriteLine(e.Message, LogType.ERROR);
                //Delete shaders.
                DeleteShaders();
                return;
            }
        }

        public void AttachShaders(uint program_uint)
        {
            //Attaches the shaders to the program.
            glAttachShader(program_uint, vertex_shader);
            glAttachShader(program_uint, fragment_shader);
        }

        public void DetatchShaders(uint programUint)
        {
            glDetachShader(programUint, vertex_shader);
            glDetachShader(programUint, fragment_shader);
        }

        public void DeleteShaders()
        {
            //Deletes the shaders for shutdown.
            glDeleteShader(vertex_shader);
            glDeleteShader(fragment_shader);
        }

        public uint GetVertexShader()
        {
            return vertex_shader;
        }

        public uint GetFragmentShader()
        {
            return fragment_shader;
        }

    }
}
