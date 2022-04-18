Shader "Example/BubbleTextures"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _HorizontalFlip ("_HorizontalFlip", float) = 0
        _VerticalFlip ("_VerticalFlip", float) = 0
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
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _VerticalFlip;
            float _HorizontalFlip;

            struct V2F
            {
                float4 vertex: SV_POSITION;
                float2 uv: TEXCOORD0;
            };

            V2F vert(appdata_base appdata)
            {
                V2F v2f;
                v2f.vertex = UnityObjectToClipPos(appdata.vertex);
                v2f.uv = appdata.texcoord;
                return v2f;
            }

            fixed4 frag(V2F v2f) : SV_Target
            {
                float2 newUV = v2f.uv;
                if (_VerticalFlip == 1)
                {
                    newUV.y = 1 - v2f.uv.y;
                }
                if (_HorizontalFlip == 1)
                {
                    newUV.x = 1 - v2f.uv.x;
                }

                fixed4 color = tex2D(_MainTex, newUV).rgba;
                return color;
            }
            ENDCG
        }
    }
}