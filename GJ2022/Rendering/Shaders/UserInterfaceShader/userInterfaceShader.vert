﻿#version 330 core
//The vertex data relative to the model.
layout (location = 0) in vec3 pos;
layout (location = 1) in vec2 vertexUv;
layout (location = 2) in vec4 instancePos;
layout (location = 3) in vec4 textureData;
layout (location = 4) in vec2 scale;

//UV data
out vec2 UV;
out vec4 texData;

// The translation matrix (Model, View)
//uniform mat4 objectMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;
 
void main()
{
    //??? For some reason the coords outputted by this shader have X and Z inverted and swapped 
    //when compared to our physical coords.
    //correct that here.
    mat4 correctionMatrix = mat4(
        -scale[0], 0.0, 0.0, 0.0,
        0.0, -scale[1], 0.0, 0.0,
        0.0, 0.0, -1.0, 0.0,
        0.0, 0.0, 0.0, 1.0
    );
    mat4 MVP = projectionMatrix;
    if(instancePos[3] == 1.0)
    {
        MVP = MVP * viewMatrix;
    }
    gl_Position = MVP * correctionMatrix * vec4(pos + instancePos.xyz, 1.0);
    gl_Position.x = -gl_Position.x;
    gl_Position.y = -gl_Position.y;
    //Output the vertex UV to the fragment shader
    UV = vertexUv;
    texData = textureData;
}