Shader "Example/URPUnlitShaderTexture"
{
    Properties
    {
        [MainColor] _BaseColor ("Base Color", Color) = (0, 0, 0, 1)
        [MainTexture] _MainTex ("Base Map", 2D) = "white"
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

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
            half4 _BaseColor;
            float4 _MainTex_ST;
            CBUFFER_END

            struct INPUT
            {
                half4 position : POSITION;
                float2 uvCoords : TEXCOORD0;
            };

            struct OUTPUT
            {
                half4 positionSV : SV_POSITION;
                float2 uvCoords : TEXCOORD0;
            };

            OUTPUT vert(INPUT input)
            {
                OUTPUT output;
                output.positionSV = TransformObjectToHClip(input.position.xyz);
                output.uvCoords = TRANSFORM_TEX(input.uvCoords, _MainTex);
                return output;
            }

            half4 frag(INPUT input) : SV_TARGET
            {
                half4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uvCoords);
                color *= _BaseColor;
                return color;
            }
            
            ENDHLSL
        }
    }
}