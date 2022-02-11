Shader "Example/TransparentAlphaBlending"
{
    Properties
    {
        [MainColor] _BaseColor ("Base Color", Color) = (0, 0, 1, 1)
    }

    SubShader
    {
        Tags
        {
            "RenderQueue" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
        }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
            half4 _BaseColor;
            CBUFFER_END

            struct INPUT
            {
                half4 position : POSITION;
                half3 normal : NORMAL;
            };

            struct VERTEXTOFRAGMENT
            {
                half4 position : SV_POSITION;
                half3 normal : TEXCOORD0;
            };

            VERTEXTOFRAGMENT vert(INPUT input)
            {
                VERTEXTOFRAGMENT v2f;
                v2f.position = TransformObjectToHClip(input.position);
                v2f.normal = TransformObjectToWorldNormal(input.normal);
                return v2f;
            }

            half4 frag(VERTEXTOFRAGMENT v2f) : SV_TARGET
            {
                half4 color = 1;
                color.rgb = v2f.normal * 0.5 + 0.5;
                return color;
            }
            ENDHLSL
        }
    }
}