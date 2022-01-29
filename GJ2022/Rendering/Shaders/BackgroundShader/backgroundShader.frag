#version 330 core

in vec2 UV;

out vec4 result;


const vec3 topLeftColour = vec3(0.274, 0.125, 0.435);
const vec3 bottomRightColour = vec3(0.035, 0.031, 0.149);

float rand(vec2 co){
    return fract(sin(dot(co, vec2(12.9898, 78.233))) * 43758.5453);
}

bool hasStar(vec2 co){
	return rand(co) > 0.999;
}

void main()
{

	float factor = (1-UV.x + UV.y) / 2;

	vec2 coordinate = gl_FragCoord.xy;
	bool starHere = hasStar(coordinate);

	result = vec4(vec3(1.0, 1.0, 1.0) * float(starHere), 1.0) + vec4(topLeftColour * factor + bottomRightColour * (1 - factor), 1.0);

}
