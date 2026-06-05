Shader "UI/InverseRectMask2D"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)
    }

        SubShader
        {
            Tags
            {
                "Queue" = "Transparent"
                "IgnoreProjector" = "True"
                "RenderType" = "Transparent"
                "PreviewType" = "Plane"
                "CanUseSpriteAtlas" = "True"
            }

            Cull Off
            Lighting Off
            ZWrite Off
            ZTest[building_internal] // UIの標準設定
            Blend SrcAlpha OneMinusSrcAlpha

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma target 2.0

                #include "UnityCG.cginc"
                #include "UnityUI.cginc"

                #pragma multi_compile_local _ UNITY_UI_CLIP_RECT

                struct appdata_t
                {
                    float4 vertex   : POSITION;
                    float4 color    : COLOR;
                    float2 texcoord : TEXCOORD0;
                };

                struct v2f
                {
                    float4 vertex   : SV_POSITION;
                    fixed4 color : COLOR;
                    float2 texcoord : TEXCOORD0;
                    float4 worldPosition : TEXCOORD1;
                };

                fixed4 _Color;
                sampler2D _MainTex;
                float4 _ClipRect;

                v2f vert(appdata_t v)
                {
                    v2f OUT;
                    OUT.worldPosition = v.vertex;
                    OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
                    OUT.texcoord = v.texcoord;
                    OUT.color = v.color * _Color;
                    return OUT;
                }

                fixed4 frag(v2f IN) : SV_Target
                {
                    half4 color = tex2D(_MainTex, IN.texcoord) * IN.color;

                    #ifdef UNITY_UI_CLIP_RECT
                    // 標準のマスク判定（範囲内なら1、範囲外なら0）
                    float maskAlpha = UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                    // ★ここがポイント：アルファを反転させる（1から引く）
                    color.a *= (1.0 - maskAlpha);
                    #endif

                    return color;
                }
                ENDCG
            }
        }
}