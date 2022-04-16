Shader "Example/SimpleNoise"
{
    Properties
    {
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
           
           struct V2F
           {
               float4 vertex : SV_POSITION;
               float2 uv : TEXCOORD0;
           };

           float random(float2 pt, float seed)
           {
               const float a = 12.9898;
               const float b = 78.233;
               const float c = 43758.543123;
               return frac(sin(dot(pt, float2(a, b)) + seed) * c);
           } 
           
           V2F vert(appdata_base appdata)
           {
               V2F v2f;
               v2f.vertex = UnityObjectToClipPos(appdata.vertex);
               v2f.uv = appdata.texcoord;
               return v2f;
           }
           
           fixed4 frag(V2F v2f) : SV_Target
           {
               fixed3 color = random(v2f.uv, _Time.y) * fixed3(1, 1, 1);
               return fixed4(color, 1.0);
           }
           ENDCG
        }
    }
}