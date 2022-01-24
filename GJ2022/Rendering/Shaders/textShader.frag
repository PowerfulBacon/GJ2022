#version 330 core

in vec2 UV;
in vec4 texData;
in vec4 colour;

out vec4 result;

uniform sampler2D textureSampler;

const float border = 0.001;

void main()
{
    vec2 transformedUV = UV * (1-2*border);

    transformedUV = vec2(transformedUV.y, 1.0 - transformedUV.x);

    float spritesheetWidth = texData[3] / texData[2];
    float spritesheetHeight = texData[3] / texData[2];

    transformedUV += vec2(border, border);
    transformedUV /= vec2(spritesheetWidth, spritesheetHeight);
    transformedUV += vec2(mod(texData[0], spritesheetWidth) / spritesheetWidth, (floor(texData[0] / spritesheetWidth) / spritesheetHeight));

    result = texture(textureSampler, transformedUV).rgba;

    if(result.a < 0.4)
    {
        discard;
    }

} 