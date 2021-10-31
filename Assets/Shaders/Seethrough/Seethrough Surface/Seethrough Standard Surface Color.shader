// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Seethrough/Seethrough Standard Surface Color"
{
	/*
		Niels de Jong (Red Owl Games) - 2016 - Credit appreciated, not required

		Note: This shader was part of a seethrough pack
		Note2: This shader was written as an exercise while I was learning to write shaders, use at your own risk.
	*/
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0

		//SEETHROUGH
		_BackColor("Showthrough Color",Color) = (0,1,1,1)
		_BackTex("Showthrough Texture",2D) = "white" {}
	}
	SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		/*First Pass, render the normal object, sets values in the stencil buffer for the pixels where the depth test fails (ie. behind objects)*/
		//Pass
		//{
			/*Stencil{
				Ref 1
				ReadMask 255
				WriteMask 255
				Comp always
				Pass keep
				//Fail zero -> irrelevant, never fails
				ZFail replace
			}*/

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

			// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf(Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG

		Pass{
			/*Second Pass, render only pixels that have value 1 in the stencil buffer (ie. the pixels that weren't drawn the previous pass) uses the second texture for this*/

			Ztest Greater

			/*Stencil{
				Ref 1
				ReadMask 255
				WriteMask 255
				Comp Equal
				Pass keep
				Fail zero
				ZFail zero
			}*/

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
			float4 _BackColor;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _BackTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_BackTex, i.uv);
				col *= _BackColor;
				return col;
			}
			ENDCG
		}			
	}
	FallBack "Diffuse"
}