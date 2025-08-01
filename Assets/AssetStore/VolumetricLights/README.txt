********************************
*       VOLUMETRIC LIGHTS      *
*      Created by Kronnect     *   
*          README FILE         *
********************************

Requirements & Setup
--------------------
1) Volumetric Lights works only with Universal Rendering Pipeline
2) Make sure you have Universal RP package imported in the project before using Volumetric Lights.
3) Make sure you have a Universal Rendering Pipeline asset assigned to Project Settings / Graphics. There's a URP sample asset in Demo/URP Pipeline Settings folder.


Demo Scene
----------
There's a demo scene which lets you quickly check if Volumetric Lights is working correctly in your project.


Documentation
-------------
Please read the documentation folder for additional instructions and options.


Support
-------
* Support-Web: https://kronnect.com/support
* Support-Discord: https://discord.gg/EH2GMaM
* Email: contact@kronnect.com
* Twitter: @Kronnect


Future updates
--------------
All our assets follow an incremental development process by which a few beta releases are published on our support forum (kronnect.com).
We encourage you to signup and engage our forum. The forum is the primary support and feature discussions medium.

Of course, all updates of Volumetric Lights will be eventually available on the Asset Store.


Version history
---------------

v10.4
- Improved support for custom shaders used in translucency: added custom texture name and color options to Volumetric Light Translucency component

v10.3.1
- [Fix] Fixed Unity 6.1 shader struct GBufferData naming conflict

v10.3
- Added "Ignore Overlay Camera" option to all render features

v10.2
- Added "Ignore Rotation Change" option to baked shadows. This option allows baked shadows to rotate with the light, improving performance.
- Improved performance while in edit mode

v10.1.1
- Added DOTs instancing support to depth prepass shaders
- Added helpful links and tips to the inspector

v10.0.2
- Performance optimizations for disabled lights

v10.0
- Added support for Render Graph (Unity 2023.3)

v7.1.1
- [Fix] Fixed VR issue using OpenXR

v7.1
- Small improvements to light diffusion effect

v7.0
- Added "Shadow Color" option. Shadow color can now be customized (also alpha) to create many special effects with Blend Mode.

v6.9
- Added Near Clip Distance to spotlights

v6.8.2
- [Fix] Fixed material issue when a light is duplicated in Unity Editor

- Added "Dust Prewarm" option. By default, dust particles are prewarmed/populated during activation. This behaviour can be disabled to gain performance when activating many lights at the same time.

v6.7
- Improved rendering of large area lights

v6.6.1
- Optimized mesh data reuse / generation based on settings change

v6.6
- Added "Local Space" option to "Use Custom Bounds"
- Improved generation of volumetric light area on directional lights

v6.5.2
- Option to ignore reflection probes in the depth pre-pass render feature
- The volumetric lights render feature now excludes reflection probes based on its culling mask
- [Fix] Fixed issue with translucency rendering in newly created lights that required additional initialization

v6.4
- Translucency script: option to preserve original transparent shader
- Translucency effect uses also Lit shader base color
- Added cookie offset parameter

v6.3
- Added "Blend Mode" option to Cast Direct Light feature

v6.2
- Added "Substractive" blend mode
- Translucent shadow map: supports transparent texture scale/offset properties
- Changed dust particles wind speed behaviour so a value of zero stops particles completely (as expected)

v6.1
- Improved blur quality
- Added button to post process inspector to quickly return to latest selected volumetric light
- Fixes

v6.0
- Added support for translucent shadow maps
- New demo scene: Church
- Added "Cast Direct Light" option (requires deferred rendering path)
- Added Unity 2022 compatibility
- Light shadow optimizations in Unity 2021.3 and above by using dedicated depth renderer
- Added cookie scale & speed options


v5.1.2
- Performance improvements related to particles when not in playmode

v5.1.1
- [Fix] Fixed depth test issue on WebGL
- [Fix] Fixed effect disappearing issue when saving the scene

v5.1
- Added "Raymarch Max Steps" setting which can be used to top the number of samples and improve performance
- Shader optimizations

v5.0.3
- Minimum required Unity version is now 2020.3.16
- [Fix] Fixed particles intensity not reflecting light intensity changes

v5.0.2
- Updated documentation with built optimization tips

v5.0.1
- [Fix] Fixed material keywords being reseted when saving scene in Unity 2021.3

v5.0
- Added Bake Mode option for point light shadows. Choose between Half Sphere, Cubemap (1 face per frame) or Cubemap (6 faces per frame)
- [Fix] Fixed lengthy compilation time in Unity 2021.2

v4.7
- Added new demo scene (temple)

v4.6.2
- Added Xbox compatibility

v4.6.1
- Added dithering option to Volumetric Lights Render Feature
- Volumetric Lights Render Feature optimizations
- Shadow occlusion render texture optimization

v4.6
- Added downscaling option to Volumetric Lights Renderer feature

v4.5.1
- Improved edge preservation when blur option is used

v4.5
- Added custom depth prepass for improved support of transparent and semi-transparent objects

v4.4.1
- Target camera property is now exposed for all light types (used for autoToggle options)
- [Fix] Fixed mesh being generated by inspector when volumetric light is disabled
- [Fix] Fixed PS4 compilation issue

v4.4
- New option Shadow Orientation for point lights
- Optimization related to baked shadows for point lights in OnStart mode

v4.2
- Added auto-toggle "Time Check Interval" parameter.
- Removed macro console warning in Unity 2020.1
- When "Sync With Profile" is enabled, properties in inspector are now readonly
- Using cookie texture no longer requires occlusion cam improving performance

v4.1
- Simplification. It's no longer required to create a volumetric light profile to use the effect.
- Timeline support is now simpler. The VolumetricLightAnimation is no longer used. Instead you can modify directly the VolumetricLight fields.

v4.0
- Added global blur option (check documentation)
- Removed top limit for shadow intensity
- Prefab instances no longer forces unpack
- [Fix] Fixed an issue when creating a volumetric point light in prefab mode

v3.6.5
- Improved compatibility with Unity 2020.3
- Removed a harmeless Editor warning message related to 3D textures

v3.6.4
- Profile must now created specifically when using volumetric lights in prefabs

v3.6.3
- Improved dust particle rendering

v3.6
- Shadow pass optimization when dust particles are also enabled
- Added support for Timeline/Animation (check documentation to enable this option)
- [Fix] Fixed an issue with occlusion camera being destroyed during a physic event resulting in a console error
- [Fix] Fixed/changed the way spotlights render when angle is >90

v3.5
- Added support for "Colored Cookie Texture" (new option in profile)

v3.4
- Added ability to dim and/or deactivate volumetric effect based on distance. Added "Autotoggle", "Distance Start Dimming" and "Distance Deactivation" properties
- [Fix] Fixed an issue that cause the effect no rendering properly when changing light range at runtime

v3.3
- When duplicating an existing volumetric light, settings are no longer shared unless they use an previously created profile asset
- When creating a Volumetric Fog Profile, the resulting asset is now placed in the current selected folder
- Volumetric area lights can now be linked to directional lights to keep their rotation and color synced
- Material handling optimizations

v3.2
- Added Raymarch Presets which provides reference quality/performance settings
- Improved default settings when adding new volumetric lights to the scene
- [Fix] Fixed dithering issue that could add an halo artifact around certain lights

v2.5.3
- [Fix] Fixed editor issue when dragging a volumetric light into a new prefab

v2.5.2
- Minor shader optimizations

v2.5.1
- Improved behaviour when instantiated from a prefab

v2.5
- Ensures light transform scale is normalized to prevent scaling issues when parent transform is changed
- Reduced shader variants by 1/4

v2.4 11-Sep-2020
- API: added "settings" property to allow modifications of individual lights without affecting a shared profile
- [Fix] Fixed particles not appearing immediately when disabling/reenabling light

v2.3 8-Sep-2020
- Added Shadow Auto Toggle and Shadow Visible Distance to optimize shadow rendering based on light distance to camera
- Added further optimizations for dust particles and shadows when not visible in frustum


v2.2 22-Aug-2020
- Added support for orthographic camera
- Added "Raymarching Min Step" parameter which can improve performance
- [Fix] Fixed VR issue due to URP not setting inverted VP matrices correctly
- [Fix] Fixed rare clipping issue on Android due to lack of floating point precision 

v2.1.4 28-Jun-2020
- Improved fit for rect light shadow map

v2.1.3 23-Jun-2020
- Particle system user modifications are now preserved
- Improved bluenoise sampling
- Added "Attenuation Mode" option (Simple and Quadratic modes supported with ability to specify quadratic coeficients)
- Added "Blend Mode" : PreMultiply
- Dust Particles: added "Distance Attenuation" and "Auto Toggle" options based on distance
- Improved profile editor UI
- [Fix] Fixed clipping issue on some platforms

v1.4 14-JUN-2020
- Enhanced blue noise jitter operator

v1.3 9-MAY-2020
- Added blue noise option
- Changed default render queue to 3101 for improved compatibility with Volumetric Fog & Mist 2

v1.2 April 2020
- Shadow occlusion optimizations
- [Fix] Fixed VR issues

v1.1 April 2020
- Added warning to inspector if Depth Texture option is not enabled in URP settings override in Project Settings / Quality
- API: added ScheduleShadowUpdate() method to issue a manual shadow update when shadow bake at start is enabled

v1.0 March / 2020
First release