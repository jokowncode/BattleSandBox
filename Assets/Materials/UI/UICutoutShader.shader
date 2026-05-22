Shader "Custom/GuideMask"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (0,0,0,0.5) // 遮罩颜色，默认半透明黑
        
        _Center ("Center", Vector) = (0,0,0,0) // 镂空中心点(屏幕坐标)
        _Size ("Size", Vector) = (100, 100, 0, 0) // 镂空大小 (宽, 高)
        _Softness ("Softness", Range(0, 50)) = 5 // 边缘平滑度
        _CornerRadius ("Corner Radius", Range(0, 100)) = 10 // 圆角半径
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
        
        Cull Off Lighting Off ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                fixed4 color : COLOR;
            };

            fixed4 _Color;
            float4 _Center;
            float4 _Size;
            float _Softness;
            float _CornerRadius;

            v2f vert (appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPosition = v.vertex; 
                o.texcoord = v.texcoord;
                o.color = v.color * _Color;
                return o;
            }
            
            float sdRoundRect(float2 p, float2 b, float r) {
                float2 d = abs(p) - b + float2(r, r);
                return min(max(d.x, d.y), 0.0) + length(max(d, 0.0)) - r;
            }

            fixed4 frag (v2f i) : SV_Target {
                float2 pixelPos = i.worldPosition.xy;
                float2 rectCenter = _Center.xy;
                
                float d = sdRoundRect(pixelPos - rectCenter, _Size.xy * 0.5, _CornerRadius);
                float outAlpha = smoothstep(-_Softness, 0, d);
                
                fixed4 col = i.color;
                col.a *= outAlpha;
                return col;
            }
            ENDCG
        }
    }
}
