#version 330 core

layout(location = 0) in vec3 aPos;

layout(location = 1) in vec2 aTexCoord;

out vec2 texCoord;

uniform mat4 rotation; //model
uniform mat4 position; //view
uniform mat4 scale; //scale
uniform mat4 projection; //camera projection

void main(void)
{
    //normal = normalize((rotation * vec4(vertexNormal, 0)).xyz);
    gl_Position = vec4(aPos, 1.0) * scale * rotation * position * projection;
    texCoord = vec2(aTexCoord.x, aTexCoord.y);
}