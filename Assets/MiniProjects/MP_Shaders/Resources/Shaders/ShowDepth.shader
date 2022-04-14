Shader "Example/ShowDepth"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 1)
        _DepthStrength("DepthStrength", Float) = 1
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
        }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert;
            #pragma fragment frag;

            #include "UnityCG.cginc"

            CBUFFER_START(UnityPerMaterial)
            half4 _Color;
            float _DepthStrength;
            CBUFFER_END

            struct INPUT
            {
                float4 position : POSITION;
            };

            struct VTOF
            {
                float4 position : SV_POSITION;
                float4 depth : DEPTH;
            };

            VTOF vert(INPUT input)
            {
                VTOF vtof;
                vtof.position = UnityObjectToClipPos(input.position);
                vtof.depth = -UnityObjectToViewPos(input.position).z * _ProjectionParams.w;
                return vtof;
            }

            float4 frag(VTOF vtof) : SV_TARGET
            {
                half invert = 1 - vtof.depth * _DepthStrength;
                return float4(invert, invert, invert, 1) * _Color;
            }
            ENDHLSL
        }
    }
}