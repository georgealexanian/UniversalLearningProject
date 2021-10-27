// 2 pass shader to draw object with blurry texture and slightly transparent when behind walls
Shader "Custom/ShowThrough"
{
    Properties
    {
        _Color("Main Color", Color) = (1,1,1,1)
        _MainTex("Base (RGB)", 2D) = "white" {}
        _OccludeColor("Occlusion Color", Color) = (0,0,1,1)
    }
    SubShader
    {
        // occluded pass
        Pass
        {
            Tags
            {
                "Queue" = "Opaque"
            }
            
            ZTest Greater
            Color[_OccludeColor]
        }
        
        Pass
        {
            Tags
            {
                "Queue" = "Opaque"
            }
            
            ZTest Lequal
            Color[_Color]
        }


        //Vertex lights
        Pass
        {
            Tags
            {
                "Queue" = "Opaque"
            }
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse", 1
}