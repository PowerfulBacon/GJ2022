﻿#version 330 core
//The vertex data relative to the model.
layout (location = 0) in vec3 pos;
layout (location = 1) in vec2 vertexUv;
layout (location = 2) in vec3 instancePos;
layout (location = 3) in float instanceRotation;
layout (location = 4) in vec4 textureData;

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
        -1.0, 0.0, 0.0, 0.0,
        0.0, -1.0, 0.0, 0.0,
        0.0, 0.0, -1.0, 0.0,
        0.0, 0.0, 0.0, 1.0
    );
    float cosRot = cos(instanceRotation);
    float sinRot = sin(instanceRotation);
    mat3 rotationMatrix = mat3(
        cosRot, -sinRot, 0.0,
        sinRot, cosRot, 0.0,
        0.0, 0.0, 1.0
    );
    mat4 MVP = projectionMatrix * viewMatrix;
    gl_Position = MVP * correctionMatrix * vec4((rotationMatrix * pos) + instancePos, 1.0);
    gl_Position.x = -gl_Position.x;
    gl_Position.y = -gl_Position.y;
    //Output the vertex UV to the fragment shader
    UV = vertexUv;
    texData = textureData;
}