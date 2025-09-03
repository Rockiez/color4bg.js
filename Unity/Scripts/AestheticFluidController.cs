using UnityEngine;
using UnityEngine.Rendering;

namespace AestheticFluid
{
    [RequireComponent(typeof(Renderer))]
    public class AestheticFluidController : MonoBehaviour
    {
        [Header("Colors")]
        [SerializeField] private Color[] colors = new Color[6]
        {
            new Color(1f, 0.04f, 0.1f, 1f),      // Red
            new Color(0.95f, 0.65f, 0f, 1f),     // Orange
            new Color(0.96f, 0.93f, 0.04f, 1f),  // Yellow
            new Color(0.22f, 0.91f, 0.05f, 1f),  // Green
            new Color(0.1f, 0.37f, 0.82f, 1f),   // Blue
            new Color(1f, 0.04f, 0.1f, 1f)       // Red (repeat)
        };
        
        [Header("Animation")]
        [SerializeField] private bool animate = true;
        [SerializeField] private float animationSpeed = 1f;
        [SerializeField] public float distortionMagnitude = 0.15f;
        [SerializeField] public float waveSpeed = 15f;
        
        [Header("Fluid Parameters")]
        [SerializeField] private float radiusInner = 0.1f;
        [SerializeField] private float radiusOuter = 0.3f;
        [SerializeField] private int randomSeed = 1000;
        
        [Header("Render Settings")]
        [SerializeField] private int textureResolution = 512;
        
        // Private fields
        private Material rttMaterial;
        private Material mainMaterial;
        private RenderTexture fluidTexture;
        private Camera renderCamera;
        private GameObject rttQuad;
        private Renderer mainRenderer;
        private float currentTime = 0f;
        private Vector4[] dyePositions = new Vector4[6];
        
        // Random number generator
        private System.Random rng;
        
        void Start()
        {
            InitializeComponents();
            SetupRenderToTexture();
            GenerateDyePositions();
            UpdateShaderProperties();
        }
        
        void InitializeComponents()
        {
            mainRenderer = GetComponent<Renderer>();
            rng = new System.Random(randomSeed);
            
            // Create materials
            var rttShader = Shader.Find("AestheticFluid/RTT");
            var mainShader = Shader.Find("AestheticFluid/Main");
            
            if (rttShader == null)
            {
                Debug.LogError("AestheticFluid/RTT shader not found!");
                return;
            }
            
            if (mainShader == null)
            {
                Debug.LogError("AestheticFluid/Main shader not found!");
                return;
            }
            
            rttMaterial = new Material(rttShader);
            mainMaterial = new Material(mainShader);
            
            // Assign main material to renderer
            mainRenderer.material = mainMaterial;
        }
        
        void SetupRenderToTexture()
        {
            // Create render texture
            fluidTexture = new RenderTexture(textureResolution, textureResolution, 0, RenderTextureFormat.ARGB32);
            fluidTexture.Create();
            
            // Create camera for RTT
            var cameraGO = new GameObject("FluidRTTCamera");
            cameraGO.transform.SetParent(transform);
            cameraGO.transform.localPosition = Vector3.forward;
            
            renderCamera = cameraGO.AddComponent<Camera>();
            renderCamera.orthographic = true;
            renderCamera.orthographicSize = 0.5f;
            renderCamera.nearClipPlane = 0.1f;
            renderCamera.farClipPlane = 10f;
            renderCamera.targetTexture = fluidTexture;
            renderCamera.clearFlags = CameraClearFlags.SolidColor;
            renderCamera.backgroundColor = Color.black;
            renderCamera.enabled = false; // We'll render manually
            
            // Create quad for RTT
            rttQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            rttQuad.name = "FluidRTTQuad";
            rttQuad.transform.SetParent(cameraGO.transform);
            rttQuad.transform.localPosition = Vector3.zero;
            rttQuad.transform.localRotation = Quaternion.identity;
            rttQuad.transform.localScale = Vector3.one;
            
            // Assign RTT material to quad
            rttQuad.GetComponent<Renderer>().material = rttMaterial;
            
            // Set main material texture
            mainMaterial.SetTexture("_MainTex", fluidTexture);
        }
        
        void GenerateDyePositions()
        {
            for (int i = 0; i < 6; i++)
            {
                float x = (float)rng.NextDouble();
                float y = (float)rng.NextDouble();
                float inner = (float)rng.NextDouble() * radiusInner + radiusInner;
                float outer = (float)rng.NextDouble() * radiusOuter + radiusOuter;
                
                dyePositions[i] = new Vector4(x, y, inner, outer);
            }
        }
        
        void UpdateShaderProperties()
        {
            if (rttMaterial == null || mainMaterial == null) return;
            
            // Update RTT material
            rttMaterial.SetVector("_Resolution", new Vector2(textureResolution, textureResolution));
            rttMaterial.SetFloat("_Time", currentTime);
            
            // Set colors
            for (int i = 0; i < 6; i++)
            {
                rttMaterial.SetColor($"_Color{i}", colors[i]);
                rttMaterial.SetVector($"_Dye{i}", dyePositions[i]);
            }
            
            // Update main material
            mainMaterial.SetFloat("_Time", currentTime);
            mainMaterial.SetFloat("_Magnitude", distortionMagnitude);
            mainMaterial.SetFloat("_Speed", waveSpeed);
        }
        
        void Update()
        {
            if (!animate) return;
            
            currentTime += Time.deltaTime * animationSpeed;
            UpdateShaderProperties();
            
            // Render to texture
            if (renderCamera != null)
            {
                renderCamera.Render();
            }
        }
        
        public void SetColors(Color[] newColors)
        {
            if (newColors.Length == 6)
            {
                colors = newColors;
                UpdateShaderProperties();
            }
        }
        
        public void SetAnimationSpeed(float speed)
        {
            animationSpeed = speed;
        }
        
        public void SetDistortionMagnitude(float magnitude)
        {
            distortionMagnitude = magnitude;
            UpdateShaderProperties();
        }
        
        public void RegenerateDyePositions()
        {
            GenerateDyePositions();
            UpdateShaderProperties();
        }
        
        public void SetSeed(int seed)
        {
            randomSeed = seed;
            rng = new System.Random(seed);
            GenerateDyePositions();
            UpdateShaderProperties();
        }
        
        void OnDestroy()
        {
            if (fluidTexture != null)
            {
                fluidTexture.Release();
                DestroyImmediate(fluidTexture);
            }
            
            if (rttMaterial != null)
                DestroyImmediate(rttMaterial);
                
            if (mainMaterial != null)
                DestroyImmediate(mainMaterial);
        }
        
        void OnValidate()
        {
            if (Application.isPlaying)
            {
                UpdateShaderProperties();
            }
        }
    }
}