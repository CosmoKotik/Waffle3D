#version 330 core

layout(location = 0) in vec3 aPos;

layout(location = 1) in vec2 aTexCoord;

out vec2 texCoord;

uniform mat4 rotation;
uniform mat4 position;
uniform mat4 scale;
uniform mat4 projection;

void main(void)
{
    gl_Position = vec4(aPos, 1.0) * scale * rotation * position * projection;
    texCoord = vec2(aTexCoord.x, aTexCoord.y);
}