Shader "AestheticFluid/RTT"
{
    Properties
    {
        _Resolution ("Resolution", Vector) = (512, 512, 0, 0)
        _Time ("Time", Float) = 0
        
        // Colors
        _Color0 ("Color 0", Color) = (1, 0, 0.1, 1)
        _Color1 ("Color 1", Color) = (0.95, 0.65, 0, 1)
        _Color2 ("Color 2", Color) = (0.96, 0.93, 0.04, 1)
        _Color3 ("Color 3", Color) = (0.22, 0.91, 0.05, 1)
        _Color4 ("Color 4", Color) = (0.1, 0.37, 0.82, 1)
        _Color5 ("Color 5", Color) = (1, 0, 0.1, 1)
        
        // Dye spots (x, y, inner_radius, outer_radius)
        _Dye0 ("Dye 0", Vector) = (0.3, 0.8, 0.1, 0.7)
        _Dye1 ("Dye 1", Vector) = (0.7, 0.8, 0.1, 0.7)
        _Dye2 ("Dye 2", Vector) = (0.7, 0.2, 0.1, 0.7)
        _Dye3 ("Dye 3", Vector) = (0.3, 0.2, 0.1, 0.7)
        _Dye4 ("Dye 4", Vector) = (0.1, 0.5, 0.1, 0.45)
        _Dye5 ("Dye 5", Vector) = (0.9, 0.5, 0.1, 0.45)
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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
            
            float2 _Resolution;
            float _Time;
            
            float4 _Color0, _Color1, _Color2, _Color3, _Color4, _Color5;
            float4 _Dye0, _Dye1, _Dye2, _Dye3, _Dye4, _Dye5;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            float4 blurDot(float3 color, float2 st, float2 pos, float inner, float outer)
            {
                float pct = distance(st, pos);
                float alpha = 1.0 - smoothstep(inner, outer, pct);
                return float4(color.rgb, alpha);
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float2 st = i.uv;
                float3 color = float3(1.0, 1.0, 1.0);
                
                // Create blur dots
                float4 dot0 = blurDot(_Color0.rgb, st, _Dye0.xy, _Dye0.z, _Dye0.w);
                float4 dot1 = blurDot(_Color1.rgb, st, _Dye1.xy, _Dye1.z, _Dye1.w);
                float4 dot2 = blurDot(_Color2.rgb, st, _Dye2.xy, _Dye2.z, _Dye2.w);
                float4 dot3 = blurDot(_Color3.rgb, st, _Dye3.xy, _Dye3.z, _Dye3.w);
                float4 dot4 = blurDot(_Color4.rgb, st, _Dye4.xy, _Dye4.z, _Dye4.w);
                float4 dot5 = blurDot(_Color5.rgb, st, _Dye5.xy, _Dye5.z, _Dye5.w);
                
                // Base gradient mixing
                color = lerp(_Color0.rgb, _Color1.rgb, st.x);
                color = lerp(color, _Color2.rgb, st.x * st.x + -0.040);
                
                // Apply blur dots
                color = lerp(color, dot0.rgb, dot0.a);
                color = lerp(color, dot1.rgb, dot1.a);
                color = lerp(color, dot2.rgb, dot2.a);
                color = lerp(color, dot3.rgb, dot3.a);
                color = lerp(color, dot4.rgb, dot4.a);
                color = lerp(color, dot5.rgb, dot5.a);
                
                return fixed4(color, 1.0);
            }
            ENDCG
        }
    }
}