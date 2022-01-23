#version 330 core

in vec2 UV;

out vec4 result;

const vec3 topLeftColour = vec3(66.0/255.0, 91.0/255.0, 135.0/255.0);
const vec3 bottomRightColour = vec3(46.0/255.0, 48.0/255.0, 51/255.0);

void main()
{
	float factor = (1-UV.x + UV.y) / 2;

	result = vec4(topLeftColour * factor + bottomRightColour * (1 - factor), 1.0);

} 