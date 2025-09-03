Shader "AestheticFluid/Main"
{
    Properties
    {
        _MainTex ("Fluid Texture", 2D) = "white" {}
        _Time ("Time", Float) = 0
        _Magnitude ("Distortion Magnitude", Range(0, 1)) = 0.15
        _Speed ("Wave Speed", Range(1, 50)) = 15
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Background" }
        LOD 100
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Time;
            float _Magnitude;
            float _Speed;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // Calculate wavy coordinates based on the original JavaScript code
                float2 wavyCoord;
                wavyCoord.x = i.uv.x + (sin(_Time + i.uv.y * _Speed) * _Magnitude);
                wavyCoord.y = i.uv.y + (cos(_Time + i.uv.x * _Speed) * _Magnitude);
                
                // Sample the fluid texture with wavy distortion
                fixed4 frameColor = tex2D(_MainTex, wavyCoord);
                
                return frameColor;
            }
            ENDCG
        }
    }
    
    // URP version
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" "Queue"="Background" }
        LOD 100
        
        Pass
        {
            Name "AestheticFluidPass"
            Tags { "LightMode"="UniversalForward" }
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float4 positionHCS : SV_POSITION;
            };
            
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            
            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float _Time;
                float _Magnitude;
                float _Speed;
            CBUFFER_END
            
            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                return output;
            }
            
            half4 frag(Varyings input) : SV_Target
            {
                // Calculate wavy coordinates
                float2 wavyCoord;
                wavyCoord.x = input.uv.x + (sin(_Time + input.uv.y * _Speed) * _Magnitude);
                wavyCoord.y = input.uv.y + (cos(_Time + input.uv.x * _Speed) * _Magnitude);
                
                // Sample the fluid texture with wavy distortion
                half4 frameColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, wavyCoord);
                
                return frameColor;
            }
            ENDHLSL
        }
    }
}