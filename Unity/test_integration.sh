#!/bin/bash

# AestheticFluid Unity Port - Quick Integration Test Script
# This script helps verify the Unity port can be properly integrated

echo "=== AestheticFluid Unity Port Integration Test ==="
echo ""

# Check if we're in the right directory
if [ ! -d "Unity" ]; then
    echo "❌ Error: Run this script from the color4bg.js repository root"
    exit 1
fi

echo "✅ Repository structure confirmed"

# Check shader files
echo ""
echo "Checking shader files..."
for shader in Unity/Shaders/*.shader; do
    if [ -f "$shader" ]; then
        echo "✅ Found: $(basename "$shader")"
        # Check if shader has the correct shader name
        if grep -q "Shader \"AestheticFluid/" "$shader"; then
            echo "   → Shader name format correct"
        else
            echo "   ⚠️  Warning: Shader name format may be incorrect"
        fi
    fi
done

# Check script files
echo ""
echo "Checking C# scripts..."
for script in Unity/Scripts/*.cs Unity/Scripts/Editor/*.cs; do
    if [ -f "$script" ]; then
        echo "✅ Found: $(basename "$script")"
        # Check if script has namespace
        if grep -q "namespace AestheticFluid" "$script"; then
            echo "   → Namespace found"
        else
            echo "   ⚠️  Warning: No namespace found"
        fi
    fi
done

# Check materials
echo ""
echo "Checking material files..."
for material in Unity/Materials/*.mat; do
    if [ -f "$material" ]; then
        echo "✅ Found: $(basename "$material")"
    fi
done

# Check documentation
echo ""
echo "Checking documentation..."
for doc in Unity/*.md; do
    if [ -f "$doc" ]; then
        echo "✅ Found: $(basename "$doc")"
    fi
done

# Show file sizes
echo ""
echo "File statistics:"
echo "Shaders: $(find Unity/Shaders -name "*.shader" | wc -l) files"
echo "Scripts: $(find Unity/Scripts -name "*.cs" | wc -l) files"
echo "Materials: $(find Unity/Materials -name "*.mat" | wc -l) files"
echo "Documentation: $(find Unity -name "*.md" | wc -l) files"

echo ""
echo "Total lines of code:"
echo "Shaders: $(cat Unity/Shaders/*.shader | wc -l) lines"
echo "C# Scripts: $(cat Unity/Scripts/*.cs Unity/Scripts/Editor/*.cs | wc -l) lines"

echo ""
echo "=== Integration Instructions ==="
echo "1. Copy the Unity/ folder to your Unity project's Assets/ directory"
echo "2. Open your Unity project and let it compile"
echo "3. Use Tools → AestheticFluid → Validate Installation to verify"
echo "4. Use Tools → AestheticFluid → Create Test Scene for a quick test"
echo "5. Or drag Unity/Prefabs/AestheticFluidBg.prefab into your scene"
echo ""
echo "🎉 Unity port is ready for integration!"

exit 0