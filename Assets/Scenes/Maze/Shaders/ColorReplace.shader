Shader "Custom/ColorReplace"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _TargetColor ("Target Color", Color) = (1,0,0,1)
        _Strength ("Strength", Range(0,1)) = 1
    }

    SubShader
    {
        Tags 
        { 
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _TargetColor;
            float _Strength;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // RGB → HSV
            float3 rgb2hsv(float3 c)
            {
                float4 K = float4(0., -1./3., 2./3., -1.);
                float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
                float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));

                float d = q.x - min(q.w, q.y);
                float e = 1e-10;
                return float3(abs(q.z + (q.w - q.y) / (6. * d + e)), d / (q.x + e), q.x);
            }

            // HSV → RGB
            float3 hsv2rgb(float3 c)
            {
                float4 K = float4(1., 2./3., 1./3., 3.);
                float3 p = abs(frac(c.xxx + K.xyz) * 6. - K.www);
                return c.z * lerp(K.xxx, saturate(p - K.xxx), c.y);
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.uv);

                float3 hsv = rgb2hsv(col.rgb);
                float3 targetHSV = rgb2hsv(_TargetColor.rgb);

                // Replace hue + saturation, keep brightness
                float3 newHSV = float3(targetHSV.x, targetHSV.y, hsv.z);
                float3 newRGB = hsv2rgb(newHSV);

                // Blend between original and recolored
                float3 finalRGB = lerp(col.rgb, newRGB, _Strength);

                return float4(finalRGB, col.a);
            }
            ENDCG
        }
    }
}
