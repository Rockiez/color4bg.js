# AestheticFluidBg Unity Port

This folder contains the Unity implementation of the AestheticFluidBg.js effect.

## Overview

The AestheticFluidBg effect creates a dynamic, colorful fluid background using:
- Multiple colored "dye" spots with blur gradients
- Animated wavy distortion
- Real-time color blending

## Files Structure

```
Unity/
├── Shaders/
│   ├── AestheticFluidBg.shader     # Main surface shader
│   └── AestheticFluidRTT.shader    # Render-to-texture shader
├── Scripts/
│   └── AestheticFluidController.cs # Component to control the effect
├── Materials/
│   ├── AestheticFluidMaterial.mat  # Main material
│   └── AestheticFluidRTT.mat       # RTT material
├── Prefabs/
│   └── AestheticFluidBg.prefab     # Ready-to-use prefab
└── README.md                       # This file
```

## Installation

1. Copy the entire `Unity/` folder into your Unity project's `Assets/` directory
2. Create a quad or plane GameObject in your scene
3. Attach the `AestheticFluidController.cs` script to the GameObject
4. Assign the `AestheticFluidMaterial.mat` to the GameObject's Renderer
5. Configure colors and parameters in the inspector

## Usage

### Basic Setup
1. Drag the `AestheticFluidBg.prefab` into your scene
2. Scale and position as needed for your background

### Custom Setup
1. Create a Quad GameObject
2. Add the `AestheticFluidController` component
3. The component will automatically create and configure materials
4. Adjust colors and animation parameters in the inspector

## Parameters

- **Colors (6)**: Six colors used for the fluid effect
- **Animation Speed**: Controls the speed of the wavy distortion
- **Distortion Magnitude**: Controls the intensity of the wavy effect
- **Blur Inner/Outer Radius**: Controls the size and softness of the dye spots
- **Seed**: Random seed for dye spot positioning

## Performance Notes

- Uses render-to-texture for the fluid effect
- Optimized for modern graphics hardware
- Consider lowering texture resolution for mobile platforms

## Compatibility

- Unity 2021.3 LTS or higher
- Universal Render Pipeline (URP) compatible
- Built-in Render Pipeline compatible
- Mobile-friendly with adjustable quality settings