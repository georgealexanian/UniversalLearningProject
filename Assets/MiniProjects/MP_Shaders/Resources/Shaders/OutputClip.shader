Shader "Example/OutputClip"
{
    Properties {}

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Opaque"
        }

        Pass 
        {
            CGPROGRAM
            #pragma vertex vert;
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct V2F
            {
                float4 vertex : SV_POSITION;
                float4 worldPos : TEXCOORD2;
            };

            V2F vert(appdata_base appdata)
            {
                V2F v2f;
                v2f.vertex = UnityObjectToClipPos(appdata.vertex);
                v2f.worldPos = appdata.vertex;
                
                return v2f;
            }

            half4 frag(V2F v2f) : SV_TARGET
            {
                half4 color = 0;
                float offset = 0.2;
                clip(v2f.worldPos.xyz + float3(offset, offset, offset));
                
                return color;
            }
            ENDCG
        }

    }
}