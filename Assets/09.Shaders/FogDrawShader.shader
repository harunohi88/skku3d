Shader "Unlit/FogRevealShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "black" {}
        _Center ("Reveal Center", Vector) = (0.5, 0.5, 0, 0)
        _Radius ("Reveal Radius", Float) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            ZTest Always Cull Off ZWrite Off

            // 밝은 부분을 덮어쓰기 위해 최대 밝기 유지
            BlendOp Max
            Blend One One

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _Center;
            float _Radius;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // 중심에서 거리 계산
                float dist = distance(i.uv, _Center.xy);

                // fade는 중심일수록 0, 바깥일수록 1
                float fade = smoothstep(_Radius, _Radius * 1.2, dist);

                // 밝은 영역만 남기기 (검정 → 밝음)
                float light = 1.0 - fade;

                return fixed4(light, light, light, 1.0);
            }
            ENDCG
        }
    }
}
