Shader "UI/FogOfWarMask" {
    Properties {
        _MainTex ("Map Image", 2D) = "white" {}
        _FogMask ("Fog Mask", 2D) = "white" {}
    }
    SubShader {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }
        ZWrite Off Cull Off Blend SrcAlpha OneMinusSrcAlpha

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _FogMask;

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata_img v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord.xy;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                float fog = tex2D(_FogMask, i.uv).r;
                float brightness = saturate(fog);
                return tex2D(_MainTex, i.uv) * brightness;
            }
            ENDCG
        }
    }
}
