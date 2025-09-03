# Aesthetic Fluid Background - Unity Setup Guide

This guide will help you integrate the AestheticFluidBg effect into your Unity project.

## Quick Start

### Method 1: Using the Prefab (Recommended)
1. Copy the entire `Unity/` folder to your project's `Assets/` directory
2. Drag `Unity/Prefabs/AestheticFluidBg.prefab` into your scene
3. Position and scale the prefab to cover your desired background area
4. The effect will start playing automatically

### Method 2: Manual Setup
1. Create a Quad GameObject in your scene (`GameObject > 3D Object > Quad`)
2. Add the `AestheticFluidController` component to the Quad
3. The component will automatically create and configure the necessary materials
4. Adjust the parameters in the Inspector as needed

## Component Parameters

### Colors
- **Colors Array**: Six colors that define the fluid's color palette
- Default uses a vivid color scheme (red, orange, yellow, green, blue, red)

### Animation
- **Animate**: Enable/disable animation
- **Animation Speed**: Controls overall animation speed (default: 1.0)
- **Distortion Magnitude**: Intensity of the wavy distortion effect (0-1, default: 0.15)
- **Wave Speed**: Speed of the wave animation (1-50, default: 15)

### Fluid Parameters
- **Radius Inner**: Inner blur radius for dye spots (default: 0.1)
- **Radius Outer**: Outer blur radius for dye spots (default: 0.3)
- **Random Seed**: Seed for random dye spot positioning (default: 1000)

### Render Settings
- **Texture Resolution**: Resolution of the render texture (default: 512x512)

## Performance Optimization

### For Mobile Devices
1. Reduce **Texture Resolution** to 256 or 128
2. Lower **Wave Speed** to reduce GPU load
3. Consider using simplified color schemes

### For High-End Devices
1. Increase **Texture Resolution** to 1024 or higher
2. Add multiple background layers with different parameters
3. Experiment with custom color schemes

## Scripting API

### Basic Usage
```csharp
using AestheticFluid;

public class MyScript : MonoBehaviour
{
    private AestheticFluidController fluidController;
    
    void Start()
    {
        fluidController = GetComponent<AestheticFluidController>();
        
        // Change colors
        Color[] newColors = new Color[6] { /* your colors */ };
        fluidController.SetColors(newColors);
        
        // Adjust animation
        fluidController.SetAnimationSpeed(2.0f);
        fluidController.SetDistortionMagnitude(0.25f);
        
        // Regenerate pattern
        fluidController.RegenerateDyePositions();
    }
}
```

### Available Methods
- `SetColors(Color[] colors)`: Update the color palette
- `SetAnimationSpeed(float speed)`: Change animation speed
- `SetDistortionMagnitude(float magnitude)`: Adjust distortion intensity
- `RegenerateDyePositions()`: Generate new random dye positions
- `SetSeed(int seed)`: Change the random seed

## Shader Information

The effect uses two shaders:
1. **AestheticFluid/RTT**: Renders the fluid pattern to a render texture
2. **AestheticFluid/Main**: Applies wavy distortion to the final output

Both shaders are compatible with:
- Built-in Render Pipeline
- Universal Render Pipeline (URP)
- High Definition Render Pipeline (HDRP) - with minor modifications

## Troubleshooting

### Shader Not Found Error
1. Ensure all shader files are in the `Unity/Shaders/` folder
2. Check that shader names match exactly: "AestheticFluid/RTT" and "AestheticFluid/Main"
3. Try reimporting the shaders (right-click > Reimport)

### Black/Empty Background
1. Check that the GameObject has a MeshRenderer component
2. Verify the AestheticFluidController component is attached and enabled
3. Ensure the camera can see the background object

### Performance Issues
1. Reduce texture resolution in the AestheticFluidController
2. Lower the animation speed
3. Check that the background isn't rendered multiple times

### Color Issues
1. Verify all 6 colors are set in the colors array
2. Check that colors have alpha = 1
3. Try regenerating dye positions

## Advanced Customization

### Custom Color Schemes
```csharp
// Pastel colors
Color[] pastelColors = new Color[]
{
    new Color(0.82f, 0.68f, 1f, 1f),     // Pastel Purple
    new Color(0.6f, 0.84f, 0.61f, 1f),   // Pastel Green
    new Color(0.98f, 0.89f, 0.56f, 1f),  // Pastel Yellow
    new Color(1f, 0.68f, 0.85f, 1f),     // Pastel Pink
    new Color(0.49f, 0.84f, 1f, 1f),     // Pastel Blue
    new Color(0.91f, 1f, 0.88f, 1f)      // Pastel Mint
};
```

### Multiple Background Layers
Create multiple background objects with different:
- Colors
- Animation speeds
- Scales
- Z-positions

### Runtime Color Animation
```csharp
// Animate colors over time
void Update()
{
    float time = Time.time;
    Color[] animatedColors = new Color[6];
    
    for (int i = 0; i < 6; i++)
    {
        float hue = (time * 0.1f + i * 0.166f) % 1f;
        animatedColors[i] = Color.HSVToRGB(hue, 0.8f, 1f);
    }
    
    fluidController.SetColors(animatedColors);
}
```

## Example Scene Setup

For a typical background setup:
1. Create a Quad at position (0, 0, 10)
2. Scale it to (20, 12, 1) to cover a standard camera view
3. Add AestheticFluidController component
4. Set your desired colors and animation parameters
5. The background will automatically animate

## Support

For issues, questions, or contributions:
- Check the original JavaScript implementation at: https://github.com/Rockiez/color4bg.js
- Review the shader code for customization opportunities
- Test with the included AestheticFluidExample script for reference