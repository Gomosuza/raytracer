# C# Raytracer

A raytracer built with Monogame and .Net Core.

:warning: WIP

Initial working software tracer with basic reflective surfaces and simple soft shadows:

![software](screenshots/software_v1.png)

___

There are multiple backend implementations that can be swapped out on the fly:

* single-threaded software-based
* multi-threaded software-based (same logic but makes use of multiple cores)

Each implementation uses the same logic to determine resolution:

Given a resolution it will attempt to hit the configured FPS target (e.g. 60 FPS), failing to do so it will half the resolution.

This will cause the images to become blurrier until all movement is stopped and it can upscale again.

For CPU based tracing this behaviour can be observed at almost all (decent) resolutions as CPUs aren't powerful enough for realtime raytracing.

# Features

* configuration options (currently limited to file/command line based at startup)
* simple reflective colored surfaces
* hard/soft shadows (`sampleCount` = 1 for hard, larger number for softer shadows)

TODO:

* GPU-based (vertex/pixel shader)
* compute shader
* load scene from file
* GUI overlay to edit configuration values on the fly
* performance test/tracing