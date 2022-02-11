Shader "Example/TransparentAlphaBlending"
{
    Properties
    {
        [MainColor] _BaseColor ("Base Color", Color) = (0, 0, 1, 1)
        [MainTexture] _BaseMap ("Base Map", 2D) = "Black"
    }

    SubShader
    {
        Tags
        {
            "RenderQueue" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Transparent"
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
            half4 _BaseColor;
            float4 _BaseMap_ST;
            CBUFFER_END

            struct INPUT
            {
                half4 position : POSITION;
                half2 uv : TEXCOORD0;
            };

            struct VERTEXTOFRAGMENT
            {
                half4 position : SV_POSITION;
                half2 uv : TEXCOORD0;
            };

            VERTEXTOFRAGMENT vert(INPUT input)
            {
                VERTEXTOFRAGMENT v2f;
                v2f.position = TransformObjectToHClip(input.position);
                v2f.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                return v2f;
            }

            half4 frag(VERTEXTOFRAGMENT v2f) : SV_TARGET
            {
                half4 color = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, v2f.uv);
                if (color.a == 0)
                {
                    color.rgba = _BaseColor;
                }
                return color;
            }
            ENDHLSL
        }
    }
}