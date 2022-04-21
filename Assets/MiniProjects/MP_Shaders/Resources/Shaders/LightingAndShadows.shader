Shader "Example/LightingAndShadows"
{
    Properties {}

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
            
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"
 
            struct V2F
            {
                half4 vertex : SV_POSITION;
                half3 normal : TEXCOORD0;
                fixed4 light : COLOR0; //diffuse lighting color
                fixed3 ambient : COLOR1;
                SHADOW_COORDS(1)
            };

            V2F vert(appdata_base appdata)
            {
                V2F v2f;

                v2f.vertex = UnityObjectToClipPos(appdata.vertex);
                v2f.normal = UnityObjectToWorldNormal(appdata.normal);

                half light = max(0, dot(v2f.normal, _WorldSpaceLightPos0.xyz));
                v2f.light = light * _LightColor0;
                v2f.ambient = ShadeSH9(half4(v2f.normal, 1));
                TRANSFER_SHADOW(v2f)

                return v2f;
            }

            half4 frag(V2F v2f) : SV_TARGET
            {
                half4 color;
                color.rgba = 1;
                //To render negative normal vector components, use the compression technique.
                //To compress the range of normal component values (-1..1) to color value range (0..1), change the following line:

                fixed shadow = SHADOW_ATTENUATION(v2f);
                fixed4 finalLighting = half4(v2f.light * shadow);
                color *= finalLighting;
                return color;
            }
            ENDCG
        }
    }
}