#version 330 core

in vec2 UV;
in vec3 circleColour;

out vec4 result;

void main()
{
    float centeredX = UV.x * 2.0 - 1.0;
    float centeredY = UV.y * 2.0 - 1.0;
    float dist = sqrt(centeredX * centeredX + centeredY * centeredY);
    result = vec4(dist < 0.9 ? circleColour * 0.95 : circleColour, dist <= 1.0 ? 1.0 : 0.0);
}
