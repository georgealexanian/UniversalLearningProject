Shader "Example/URPUnlitShaderTexture"
{
    Properties
    {
        [MainColor] _BaseColor ("Base Color", Color) = (0, 0, 0, 1)
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
                half4 position : POSITION;
            };

            struct OUTPUT
            {
                half4 positionSV : SV_POSITION;
            };

            OUTPUT vert(INPUT input)
            {
                OUTPUT output;
                output.positionSV = TransformObjectToHClip(input.position.xyz);
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