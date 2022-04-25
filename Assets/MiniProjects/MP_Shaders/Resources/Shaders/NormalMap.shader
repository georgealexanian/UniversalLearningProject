Shader "Example/NormalMap"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _BumpMap ("Bump Map", 2D) = "bump" {}
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
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;
            CBUFFER_END

            struct VERTEXDATA
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct V2F
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            V2F vert(VERTEXDATA vertexdata)
            {
                V2F v2f;
                v2f.vertex = TransformObjectToHClip(vertexdata.vertex.xyz);
                v2f.uv = TRANSFORM_TEX(vertexdata.uv, _MainTex);
                return v2f;
            }

            half4 frag(V2F v2f) : SV_TARGET
            {
                half4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v2f.uv);
                return color;
            }
            ENDHLSL
        }
    }
}