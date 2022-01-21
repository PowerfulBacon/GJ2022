#version 330 core

in vec2 UV;
in vec4 texData;

out vec4 result;

uniform float spriteWidth;
uniform float spriteHeight;
uniform sampler2D textureSampler;

const float border = 0.001;

const vec3 topLeftColour = vec3(217.0/255.0, 157.0/255.0, 54.0/255.0);
const vec3 bottomRightColour = vec3(189.0/255.0, 153.0/255.0, 92.0/255.0);

void main()
{
	float factor = (1-UV.x + UV.y) / 2;

	result = vec4(topLeftColour * factor + bottomRightColour * (1 - factor), 1.0);

} 