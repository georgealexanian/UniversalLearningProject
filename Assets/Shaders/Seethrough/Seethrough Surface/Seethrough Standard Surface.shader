Shader "Seethrough/Seethrough Standard Surface"
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
		_BackTex("Showthrough Texture",2D) = "white" {}
		_BackColor("Showthrough Color",Color) = (0,1,1,1)
		_BackGlossiness("Back Smoothness", Range(0,1)) = 0.5
		_BackMetallic("Back Metallic", Range(0,1)) = 0.0
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

			ZTest Greater
			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
#pragma surface surf Standard fullforwardshadows

			// Use shader model 3.0 target, to get nicer looking lighting
#pragma target 3.0

			sampler2D _BackTex;

		struct Input {
			float2 uv_BackTex;
		};

		half _BackGlossiness;
		half _BackMetallic;
		fixed4 _BackColor;

		void surf(Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_BackTex, IN.uv_BackTex) * _BackColor;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _BackMetallic;
			o.Smoothness = _BackGlossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}