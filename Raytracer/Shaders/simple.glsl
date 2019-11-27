#version 430 core

layout(binding = 0, rgba32f) uniform writeonly image2D framebuffer;

layout(local_size_x = 8, local_size_y = 8) in;
void main()
{
  // most basic compute shader. Can be used to verify that everything is set up correctly
  // on execute the shader will fill the entire buffer with a single color

  ivec2 pos = ivec2(gl_GlobalInvocationID.xy);
  ivec2 size = imageSize(framebuffer);
  // discard points outside texture
  if (pos.x >= size.x || pos.y >= size.y)
  {
    return;
  }
  // make entire buffer green
  vec4 pixel = vec4(0.0, 1.0, 0.0, 1.0);
  imageStore(framebuffer, pos, pixel);
}
