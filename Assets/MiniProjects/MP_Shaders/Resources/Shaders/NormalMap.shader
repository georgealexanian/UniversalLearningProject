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

        CGPROGRAM
        #pragma surface surf Lambert

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
        };

        sampler2D _MainTex;
        sampler2D _BumpMap;

        void surf(Input input, inout SurfaceOutput surfaceOutput)
        {
            surfaceOutput.Albedo = tex2D(_MainTex, input.uv_MainTex).rgb;
            surfaceOutput.Normal = UnpackNormal(tex2D(_BumpMap, input.uv_BumpMap));
        }
        ENDCG
    }
    FallBack "Diffuse"
}