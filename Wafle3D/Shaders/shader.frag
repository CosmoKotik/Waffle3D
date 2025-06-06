﻿#version 330

out vec4 FragColor;

in vec2 texCoord;

uniform sampler2D texture1;
uniform sampler2D texture2;
uniform sampler2D texture3;
uniform sampler2D texture4;
uniform sampler2D texture5;

void main()
{
    FragColor = mix(texture(texture1, texCoord), 
                    texture(texture2, texCoord), 0.2);
}