﻿#version 140

uniform ProjectionMatrixBuffer
{
    mat4 projection_matrix;
};

uniform ModelViewMatrixBuffer
{
    mat4 modelview_matrix;
};

in vec3 in_position;
in vec4 in_color;

out vec4 color;

void main()
{
    gl_Position = projection_matrix * modelview_matrix * vec4(in_position, 1);
    color = in_color;
}
