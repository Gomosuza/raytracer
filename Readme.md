# C# Raytracer

A raytracer built with Monogame and .Net Core.

There are multiple backend implementations that can be swapped out on the fly:

* single-threaded software-based
* multi-threaded software-based

TODO:

* GPU-based (vertex/pixel shader)
* compute shader

Each implementation uses the same logic to determine resolution:

Given a resolution it will attempt to hit the configured FPS target (default: 60 FPS), failing to do so it will half the resolution.