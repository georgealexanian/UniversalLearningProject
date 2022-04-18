Shader "Example/BubbleTextures"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
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
               fixed4 color = tex2D(_MainTex, v2f.uv);
               return color;
           }
           ENDCG
        }
    }
}