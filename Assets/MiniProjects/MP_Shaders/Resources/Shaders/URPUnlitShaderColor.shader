Shader "Example/URPUnlitShaderColor"
{
    Properties
    {

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
                return half4(0.5, 1, 0, 1);
            }
            
            ENDHLSL
        }
    }
}