Shader "Example/URPUnlitShaderColor"
{
    Properties
    {
        [MainColor] _BaseColor ("Color To Show", Color) = (0.5, 0, 1, 1)
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

            CBUFFER_START(UnityPerMaterial)
            half4 _BaseColor;
            CBUFFER_END

            struct INPUT
            {
                half4 inputPosition : POSITION;
            };

            struct OUTPUT
            {
                half4 outputPosition : SV_POSITION;
            };

            OUTPUT vert(INPUT input)
            {
                OUTPUT output;
                output.outputPosition = TransformObjectToHClip(input.inputPosition.xyz);
                return output;
            }

            half4 frag() : SV_TARGET
            {
                return _BaseColor;
            }
            
            ENDHLSL
        }
    }
}