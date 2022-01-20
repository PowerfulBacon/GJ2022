#version 330 core

in vec2 UV;
in vec4 texData;

out vec4 result;

uniform float spriteWidth;
uniform float spriteHeight;
uniform sampler2D textureSampler;

const float border = 0.001;

const vec3 topLeftColour = vec3(66.0/255.0, 91.0/255.0, 135.0/255.0);
const vec3 bottomRightColour = vec3(46.0/255.0, 48.0/255.0, 51/255.0);

const float sqrt_two = sqrt(2.0);

//Xn+1 = (aXn + c) mod m

void main()
{
	float factor = sqrt(1-UV.x + UV.y) / sqrt_two;

	result = vec4(topLeftColour * factor + bottomRightColour * (1 - factor), 1.0);

} 