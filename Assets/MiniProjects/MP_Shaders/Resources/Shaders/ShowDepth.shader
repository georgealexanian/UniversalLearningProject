Shader "Example/ShowDepth"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
        }
 
        Pass
        {
            CGPROGRAM
            #pragma vertex vert;
            #pragma fragment frag;

            #include "UnityCG.cginc"

            half4 _Color;
            
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
                vtof.depth = -mul(UNITY_MATRIX_MV, input.position).z * _ProjectionParams.w;
                return vtof;
            }

            float4 frag(VTOF vtof) : SV_TARGET
            {
                half invert = 1 - vtof.depth;
                return float4(invert, invert, invert, 1) * _Color;
            }
            ENDCG
        }
    }
    
    
}