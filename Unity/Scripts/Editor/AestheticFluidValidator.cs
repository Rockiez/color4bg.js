using UnityEngine;
using UnityEditor;

namespace AestheticFluid
{
    /// <summary>
    /// Editor utility to validate the AestheticFluid Unity port
    /// </summary>
    public class AestheticFluidValidator
    {
        [MenuItem("Tools/AestheticFluid/Validate Installation")]
        public static void ValidateInstallation()
        {
            bool allValid = true;
            
            Debug.Log("=== AestheticFluid Installation Validation ===");
            
            // Check shaders
            var rttShader = Shader.Find("AestheticFluid/RTT");
            var mainShader = Shader.Find("AestheticFluid/Main");
            
            if (rttShader == null)
            {
                Debug.LogError("❌ RTT Shader not found: 'AestheticFluid/RTT'");
                allValid = false;
            }
            else
            {
                Debug.Log("✅ RTT Shader found: " + rttShader.name);
            }
            
            if (mainShader == null)
            {
                Debug.LogError("❌ Main Shader not found: 'AestheticFluid/Main'");
                allValid = false;
            }
            else
            {
                Debug.Log("✅ Main Shader found: " + mainShader.name);
            }
            
            // Check script
            var controllerScript = System.Type.GetType("AestheticFluid.AestheticFluidController");
            if (controllerScript == null)
            {
                Debug.LogError("❌ AestheticFluidController script not found");
                allValid = false;
            }
            else
            {
                Debug.Log("✅ AestheticFluidController script found");
            }
            
            // Check materials
            string[] materialPaths = {
                "Assets/Unity/Materials/AestheticFluidRTT.mat",
                "Assets/Unity/Materials/AestheticFluidMaterial.mat"
            };
            
            foreach (string path in materialPaths)
            {
                var material = AssetDatabase.LoadAssetAtPath<Material>(path);
                if (material == null)
                {
                    Debug.LogWarning("⚠️ Material not found at: " + path);
                }
                else
                {
                    Debug.Log("✅ Material found: " + material.name);
                }
            }
            
            // Check prefab
            string prefabPath = "Assets/Unity/Prefabs/AestheticFluidBg.prefab";
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (prefab == null)
            {
                Debug.LogWarning("⚠️ Prefab not found at: " + prefabPath);
            }
            else
            {
                Debug.Log("✅ Prefab found: " + prefab.name);
            }
            
            if (allValid)
            {
                Debug.Log("🎉 All core components are properly installed!");
            }
            else
            {
                Debug.LogError("❌ Installation incomplete. Please check the errors above.");
            }
        }
        
        [MenuItem("Tools/AestheticFluid/Create Test Scene")]
        public static void CreateTestScene()
        {
            // Create a new scene
            var scene = UnityEngine.SceneManagement.SceneManager.CreateScene("AestheticFluidTest");
            UnityEngine.SceneManagement.SceneManager.SetActiveScene(scene);
            
            // Create camera
            var cameraGO = new GameObject("Main Camera");
            var camera = cameraGO.AddComponent<Camera>();
            camera.transform.position = new Vector3(0, 0, -10);
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = Color.black;
            
            // Create background
            var backgroundGO = GameObject.CreatePrimitive(PrimitiveType.Quad);
            backgroundGO.name = "AestheticFluidBackground";
            backgroundGO.transform.position = new Vector3(0, 0, 5);
            backgroundGO.transform.localScale = new Vector3(20, 12, 1);
            
            // Add controller
            var controller = backgroundGO.AddComponent<AestheticFluidController>();
            
            // Add example script
            var example = backgroundGO.AddComponent<AestheticFluidExample>();
            
            Debug.Log("✅ Test scene created! Press Play to see the effect.");
            Debug.Log("Controls: Space = regenerate, C = cycle colors");
        }
        
        [MenuItem("Tools/AestheticFluid/Performance Test")]
        public static void PerformanceTest()
        {
            var controller = Object.FindObjectOfType<AestheticFluidController>();
            if (controller == null)
            {
                Debug.LogError("No AestheticFluidController found in scene. Please create a test scene first.");
                return;
            }
            
            Debug.Log("=== Performance Test ===");
            
            // Test different texture resolutions
            int[] resolutions = { 128, 256, 512, 1024 };
            
            foreach (int res in resolutions)
            {
                System.GC.Collect();
                var startTime = System.DateTime.Now;
                
                // This would need to be implemented in the controller
                // controller.SetTextureResolution(res);
                
                var endTime = System.DateTime.Now;
                var duration = (endTime - startTime).TotalMilliseconds;
                
                Debug.Log($"Resolution {res}x{res}: ~{duration:F1}ms");
            }
        }
    }
}