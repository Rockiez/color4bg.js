using UnityEngine;

namespace AestheticFluid
{
    /// <summary>
    /// Example script demonstrating how to use the AestheticFluidController
    /// Attach this to any GameObject in your scene to test the fluid background
    /// </summary>
    public class AestheticFluidExample : MonoBehaviour
    {
        [Header("Example Controls")]
        [SerializeField] private KeyCode regenerateKey = KeyCode.Space;
        [SerializeField] private KeyCode colorCycleKey = KeyCode.C;
        [SerializeField] private bool showUI = true;
        
        private AestheticFluidController fluidController;
        private int currentColorScheme = 0;
        
        // Predefined color schemes
        private Color[][] colorSchemes = new Color[][]
        {
            // Vivid
            new Color[]
            {
                new Color(1f, 0.04f, 0.1f, 1f),      // Red
                new Color(0.95f, 0.65f, 0f, 1f),     // Orange  
                new Color(0.96f, 0.93f, 0.04f, 1f),  // Yellow
                new Color(0.22f, 0.91f, 0.05f, 1f),  // Green
                new Color(0.1f, 0.37f, 0.82f, 1f),   // Blue
                new Color(1f, 0.04f, 0.1f, 1f)       // Red
            },
            // Pastel
            new Color[]
            {
                new Color(0.82f, 0.68f, 1f, 1f),     // Pastel Purple
                new Color(0.6f, 0.84f, 0.61f, 1f),   // Pastel Green
                new Color(0.98f, 0.89f, 0.56f, 1f),  // Pastel Yellow
                new Color(1f, 0.68f, 0.85f, 1f),     // Pastel Pink
                new Color(0.49f, 0.84f, 1f, 1f),     // Pastel Blue
                new Color(0.91f, 1f, 0.88f, 1f)      // Pastel Mint
            },
            // Blue Gradient
            new Color[]
            {
                new Color(0f, 0.5f, 1f, 1f),         // Blue
                new Color(0.19f, 0.6f, 1f, 1f),      // Light Blue
                new Color(0.38f, 0.7f, 1f, 1f),      // Lighter Blue
                new Color(0.56f, 0.8f, 1f, 1f),      // Even Lighter
                new Color(0.75f, 0.9f, 1f, 1f),      // Pale Blue
                new Color(0.94f, 1f, 1f, 1f)         // Very Pale Blue
            }
        };
        
        void Start()
        {
            // Find or create fluid controller
            fluidController = FindObjectOfType<AestheticFluidController>();
            
            if (fluidController == null)
            {
                Debug.Log("No AestheticFluidController found. Creating example background...");
                CreateExampleBackground();
            }
        }
        
        void CreateExampleBackground()
        {
            // Create a quad for the background
            GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            quad.name = "AestheticFluidBackground";
            quad.transform.position = new Vector3(0, 0, 10);
            quad.transform.localScale = new Vector3(20, 12, 1);
            
            // Add the fluid controller
            fluidController = quad.AddComponent<AestheticFluidController>();
            
            Debug.Log("Created AestheticFluidBackground. Use Space to regenerate, C to cycle colors.");
        }
        
        void Update()
        {
            if (fluidController == null) return;
            
            // Handle input
            if (Input.GetKeyDown(regenerateKey))
            {
                fluidController.RegenerateDyePositions();
                Debug.Log("Regenerated dye positions");
            }
            
            if (Input.GetKeyDown(colorCycleKey))
            {
                CycleColorScheme();
            }
        }
        
        void CycleColorScheme()
        {
            currentColorScheme = (currentColorScheme + 1) % colorSchemes.Length;
            fluidController.SetColors(colorSchemes[currentColorScheme]);
            
            string[] schemeNames = { "Vivid", "Pastel", "Blue Gradient" };
            Debug.Log($"Switched to {schemeNames[currentColorScheme]} color scheme");
        }
        
        void OnGUI()
        {
            if (!showUI || fluidController == null) return;
            
            float panelWidth = 300;
            float panelHeight = 200;
            float margin = 20;
            
            GUI.Box(new Rect(margin, margin, panelWidth, panelHeight), "Aesthetic Fluid Background");
            
            float y = margin + 30;
            float spacing = 25;
            
            GUI.Label(new Rect(margin + 10, y, panelWidth - 20, 20), $"Press '{regenerateKey}' to regenerate");
            y += spacing;
            
            GUI.Label(new Rect(margin + 10, y, panelWidth - 20, 20), $"Press '{colorCycleKey}' to cycle colors");
            y += spacing;
            
            string[] schemeNames = { "Vivid", "Pastel", "Blue Gradient" };
            GUI.Label(new Rect(margin + 10, y, panelWidth - 20, 20), $"Current: {schemeNames[currentColorScheme]}");
            y += spacing;
            
            // Distortion magnitude slider
            GUI.Label(new Rect(margin + 10, y, 100, 20), "Distortion:");
            float newMagnitude = GUI.HorizontalSlider(new Rect(margin + 110, y + 5, 150, 20), 
                fluidController.distortionMagnitude, 0f, 0.5f);
            if (newMagnitude != fluidController.distortionMagnitude)
            {
                fluidController.SetDistortionMagnitude(newMagnitude);
            }
            y += spacing;
            
            // Animation speed slider
            GUI.Label(new Rect(margin + 10, y, 100, 20), "Speed:");
            float newSpeed = GUI.HorizontalSlider(new Rect(margin + 110, y + 5, 150, 20), 
                fluidController.animationSpeed, 0f, 3f);
            if (newSpeed != fluidController.animationSpeed)
            {
                fluidController.SetAnimationSpeed(newSpeed);
            }
        }
    }
}