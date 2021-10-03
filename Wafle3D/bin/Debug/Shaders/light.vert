#version 330 core

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoords;

out vec3 Normal;
out vec3 FragPos;

out vec2 TexCoords;

uniform mat4 rotation; //model
uniform mat4 position; //view
uniform mat4 scale; //scale
uniform mat4 projection; //camera projection

void main(void)
{
    gl_Position = vec4(aPos, 1.0) * scale * rotation * position * projection;
    FragPos = vec3(rotation * vec4(aPos, 1.0));
    Normal = aNormal * mat3(transpose(inverse(rotation)));
    TexCoords = aTexCoords;
}