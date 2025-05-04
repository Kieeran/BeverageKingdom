Shader "UI/GradientGlint_NoTex"
{
    Properties
    {
        _LeftColor    ("Left Color",   Color) = (0,0.5,1,1)
        _RightColor   ("Right Color",  Color) = (0,1,0.3,1)
        _FillAmount   ("Fill Amount",  Range(0,1)) = 0.5
        _GlintColor   ("Glint Color",  Color) = (1,1,1,0.8)
        _GlintSpeed   ("Glint Speed",  Float) = 1
        _GlintWidth   ("Glint Width",  Float) = 0.2
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

            float4 _LeftColor;
            float4 _RightColor;
            float  _FillAmount;
            float4 _GlintColor;
            float  _GlintSpeed;
            float  _GlintWidth;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
                float4 color  : COLOR;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
                float4 col : COLOR;
            };

            v2f vert(appdata IN)
            {
                v2f OUT;
                OUT.pos = UnityObjectToClipPos(IN.vertex);
                OUT.uv  = IN.uv;
                OUT.col = IN.color;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                float u = IN.uv.x;

                // 1) Linear gradient giữa LeftColor và RightColor
                fixed4 baseCol = lerp(_LeftColor, _RightColor, u);

                // 2) Tính vùng glint (highlight) chạy ngang
                float t = frac(_Time.y * _GlintSpeed);
                float glow = smoothstep(t - _GlintWidth, t, u)
                           - smoothstep(t, t + _GlintWidth, u);

                // 3) Áp glint lên baseCol
                baseCol.rgb += _GlintColor.rgb * glow;
                baseCol.a   *= (u <= _FillAmount ? 1 : 0);

                return baseCol * IN.col;
            }
            ENDCG
        }
    }
}
