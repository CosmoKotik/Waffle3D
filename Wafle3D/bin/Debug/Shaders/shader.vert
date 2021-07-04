#version 330

attribute vec3 aPos;

attribute vec2 aTexCoord;

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