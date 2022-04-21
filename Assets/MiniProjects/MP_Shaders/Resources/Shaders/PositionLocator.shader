Shader "Example/PositionLocator"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "black" {}
        _Radius ("Radius", Range(0, 3)) = 1
        _Position ("Position", Vector) = (0, 0, 0, 0)
        _CircleColor ("Circle Color", Color) = (1, 1, 1, 1)
        _EdgeThickness ("_EdgeThickness", Range(0, 3)) = 0.01
        _LineWidth ("_LineWidth", Range(0, 3)) = 0.1
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

            sampler2D _MainTex;
            float _Radius;
            float _EdgeThickness;
            float _LineWidth;
            float4 _Position;
            fixed4 _CircleColor;

            struct V2F
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 worldPos : TEXCOORD2;
            };

            float CIRCLE(float2 pt, float2 center, float radius, float lineWidth, float edgeThickness)
            {
                float2 p = pt - center;
                float len = length(p);
                float halfLineWidth = lineWidth / 2.0;
                float result = smoothstep(radius - halfLineWidth - edgeThickness, radius - halfLineWidth, len) -
                    smoothstep(radius + halfLineWidth, radius + halfLineWidth + edgeThickness, len);

                return result;
            }

            V2F vert(appdata_base appdata)
            {
                V2F v2f;

                v2f.vertex = UnityObjectToClipPos(appdata.vertex);
                v2f.uv = UnityObjectToClipPos(appdata.texcoord);
                v2f.worldPos = mul(unity_ObjectToWorld, appdata.vertex);

                return v2f;
            }

            half4 frag(V2F v2f) : SV_TARGET
            {
                half4 color;
                color = tex2D(_MainTex, v2f.uv);

                float inCircle = CIRCLE(v2f.worldPos.xz, _Position.xz, _Radius, _Radius * _LineWidth, _Radius * _EdgeThickness);
                fixed4 finalColor = lerp(color, _CircleColor, inCircle);

                return finalColor;
            }
            ENDCG
        }
    }
}