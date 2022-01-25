#version 330 core

in vec2 UV;
in float buttonState;
in vec4 colour;

out vec4 result;

void main()
{
	result = colour + (1.0 - UV.x) * vec4(0.3, 0.3, 0.3, 0.0);
	if(buttonState == 1.0)
	{
		result *= 1.4;
	}
}
