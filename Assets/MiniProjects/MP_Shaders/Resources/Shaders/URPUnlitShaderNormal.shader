Shader "Example/URPUnlitShaderNormal"
{
    Properties
    {
        [MainColor] _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
        [MainTexture] _BaseMap ("Base Texture", 2D) = "White"
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
        }

        Pass
        {
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
                float2 uv : TEXCOORD0;
            };

            struct OUTPUT
            {
                half4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
 
            OUTPUT vert(INPUT input)
            {
                OUTPUT output;
                output.position = TransformObjectToHClip(input.position.xyz);
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);

                return output;
            }

            half4 frag(INPUT input) : SV_TARGET
            {
                half4 color = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv);
                return color;
            }
            
            ENDHLSL
        }
    }
}