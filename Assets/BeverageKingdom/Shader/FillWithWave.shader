Shader "UI/FillWithWaveHorizontal"
{
    Properties
    {
        _MainTex        ("Sprite Texture",    2D)    = "white" {}
        _FillAmount     ("Fill Amount",       Range(0,1)) = 0.5
        _WaveSpeed      ("Wave Speed",        Float) = 1
        _WaveHeight     ("Wave Height",       Float) = 0.02
        _WaveFrequency  ("Wave Frequency",    Float) = 10
        _TintColor      ("Tint",              Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
        }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Lighting Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4   _MainTex_ST;
            float    _FillAmount;
            float    _WaveSpeed;
            float    _WaveHeight;
            float    _WaveFrequency;
            float4   _TintColor;

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos   : SV_POSITION;
                fixed4 color : COLOR;
                float2 uv    : TEXCOORD0;
            };

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.pos   = UnityObjectToClipPos(IN.vertex);
                OUT.color = IN.color * _TintColor;
                OUT.uv    = TRANSFORM_TEX(IN.texcoord, _MainTex);
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                float2 uv = IN.uv;

                // 1) Tạo wave chạy dọc (theo trục y)
                float t = _Time.y * _WaveSpeed;
                float wave = sin(uv.y * _WaveFrequency + t) * _WaveHeight;
                uv.x += wave;    // đẩy ngang theo sóng

                // 2) Sample texture
                fixed4 col = tex2D(_MainTex, uv) * IN.color;

                // 3) Clip theo FillAmount ngang: để chỉ hiển thị đến mức x <= _FillAmount
                if (uv.x > _FillAmount)
                    col.a = 0;

                return col;
            }
            ENDCG
        }
    }
    FallBack "UI/Default"
}
