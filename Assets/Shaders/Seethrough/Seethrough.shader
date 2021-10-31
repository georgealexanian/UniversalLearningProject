// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Seethrough/Seethrough"
{
    /*
    Niels de Jong (Red Owl Games) - 2016 - Credit appreciated, not required

    Note: This shader was part of a seethrough pack
    Note2: This shader was written as an exercise while I was learning to write shaders, use at your own risk.
    */
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BackTex("Show through Texture",2D) = "black" {}
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque" "Queue"="Geometry"
        }
        LOD 100

        /*First Pass, renders the normal texture, sets values in the stencil buffer for the pixels where the depth test fails (ie. behind objects)*/
        Pass
        {

            Stencil
            {
                Ref 11
                ReadMask 255
                WriteMask 255
                Comp always
                Pass keep
                //Fail zero -> irrelevant, never fails
                ZFail replace
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
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }

        /*Second Pass, render only pixels that have value 1 in the stencil buffer (ie. the pixels that weren't drawn the previous pass) uses the second texture for this*/
        Pass
        {

            Ztest always

            Stencil
            {
                Ref 11
                ReadMask 255
                WriteMask 255
                Comp Equal
                Pass keep
                Fail zero
                ZFail zero
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

            sampler2D _BackTex;
            float4 _BackTex_ST;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _BackTex);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_BackTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}