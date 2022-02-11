Shader "Example/URPUnlitShaderNormal"
{
    Properties
    {
        [MainColor] _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
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
                half3 normal : NORMAL;
            };

            struct OUTPUT
            {
                half4 position : SV_POSITION;
                half3 normal : TEXCOORD0;
            };

            OUTPUT vert(INPUT input)
            {
                OUTPUT output;
                output.position = TransformObjectToHClip(input.position.xyz);
                // Use the TransformObjectToWorldNormal function to transform the
                // normals from object to world space. This function is from the
                // SpaceTransforms.hlsl file, which is referenced in Core.hlsl.
                output.normal = TransformObjectToWorldNormal(input.normal);
                // output.normal = TransformWorldToObjectNormal(input.normal); //object normal
                return output;
            }

            half4 frag(OUTPUT output) : SV_TARGET
            {
                half4 color = 1;
                //To render negative normal vector components, use the compression technique.
                //To compress the range of normal component values (-1..1) to color value range (0..1), change the following line:
                color.rgb = output.normal * 0.5 + 0.5;
                return color;
            }
            ENDHLSL
        }
    }
}