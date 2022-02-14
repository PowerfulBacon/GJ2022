#version 330 core

in vec2 UV;
in vec4 texData;
in vec4 iColour;

out vec4 result;

uniform float spriteWidth;
uniform float spriteHeight;
uniform sampler2D textureSampler;

const float border = 0.001;

void main()
{

    vec2 transformedUV = (UV * (1-2*border)) + border;

    transformedUV = vec2(1.0 - transformedUV.y, 1.0 - transformedUV.x);

    float spritesheetWidth = spriteWidth / 32.0;
    float spritesheetHeight = spriteHeight / 32.0;

    transformedUV += vec2(border, border);
    transformedUV *= vec2(texData[2] / spriteWidth, texData[3] / spriteHeight);
    transformedUV += vec2(texData[0] / spritesheetWidth, (spritesheetHeight - texData[1] - 1) / spritesheetHeight);

    result = texture(textureSampler, transformedUV).rgba * iColour.rgba;

    if(result.a == 0)
    {
        discard;
    }

} 