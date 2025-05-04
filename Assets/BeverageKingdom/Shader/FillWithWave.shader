Shader "UI/FillGlow"
{
    Properties
    {
        _MainTex ("Sprite", 2D) = "white" {}
        _Fill    ("Fill", Range(0,1)) = 0.5
        _Glow    ("Glow Intensity", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Cull Off ZWrite Off Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex; float4 _MainTex_ST;
            float _Fill, _Glow;

            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f    { float4 pos : SV_POSITION; float2 uv  : TEXCOORD0; };

            v2f vert(appdata v) {
                v2f o; o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                fixed4 c = tex2D(_MainTex, i.uv);
                // ẩn những phần vượt quá fill
                if (i.uv.x > _Fill) c.a = 0;
                // glow ramp ở mép fill
                float g = smoothstep(_Fill-0.05, _Fill, i.uv.x) * _Glow;
                c.rgb += g;
                return c;
            }
            ENDCG
        }
    }
}
