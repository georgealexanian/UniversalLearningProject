Shader "Example/SimpleLighting"
{
    Properties
    {
        _Radius ("Radius", float) = 1
        _Delta ("Radius", float) = 1
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
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"

            float _Radius;
            float _Delta;

            struct V2F
            {
                half4 vertex : SV_POSITION;
                half3 normal : TEXCOORD0;
                fixed4 light : COLOR0; //diffuse lighting color
            };

            V2F vert(appdata_base appdata)
            {
                V2F v2f;

                float delta = (_SinTime.w + 1.0) / 2.0;
                float4 finalVertex = float4(normalize(appdata.vertex.xyz) * _Radius * 0.01, appdata.vertex.w);
                float4 currVertex = lerp(appdata.vertex, finalVertex, delta);
                
                v2f.vertex = UnityObjectToClipPos(currVertex);
                v2f.normal = UnityObjectToWorldNormal(currVertex);

                half light = max(0, dot(v2f.normal, _WorldSpaceLightPos0.xyz));
                v2f.light = light * _LightColor0;

                return v2f;  
            }

            half4 frag(V2F v2f) : SV_TARGET
            {
                half4 color = 1;
                //To render negative normal vector components, use the compression technique.
                //To compress the range of normal component values (-1..1) to color value range (0..1), change the following line:
                color.rgba = 1;
                color *= v2f.light;
                return color;
            }
            ENDCG
        }
        
    }
}