Shader "Unlit/PortalGlossyShader"
{
    Properties
    {
        _MainTex ("Sprite", 2D) = "white" {}
        _ScrollSpeed ("Gloss Scroll Speed", Float) = 0.3
        _TintColor ("Gloss Color", Color) = (0.7, 1, 1, 0.5)
        _GlossIntensity ("Gloss Intensity", Float) = 0.5
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha One
        Cull Off ZWrite Off
        Lighting Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _ScrollSpeed;
            float4 _TintColor;
            float _GlossIntensity;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 glossUV : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.glossUV = o.uv + float2(0, _Time.y * _ScrollSpeed);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 baseCol = tex2D(_MainTex, i.uv);
                
                float wave = sin(i.glossUV.y * 4 + _Time.y * _ScrollSpeed);
                wave = smoothstep(0.2, 0.8, wave * 0.5 + 0.5);
                
                float alpha = baseCol.a;
                fixed4 gloss = _TintColor * wave * _GlossIntensity;

                fixed4 finalColor = lerp(baseCol, baseCol + gloss, alpha);

                return finalColor;
            }
            ENDCG
        }
    }
}
