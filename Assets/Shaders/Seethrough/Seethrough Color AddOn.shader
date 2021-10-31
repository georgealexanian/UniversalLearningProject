// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Seethrough/Seethrough Color AddOn"
{
	/*
	Niels de Jong (Red Owl Games) - 2016 - Credit appreciated, not required

	Note: This shader was part of a seethrough pack
	Note2: This shader was written as an exercise while I was learning to write shaders, use at your own risk.
	*/
	Properties
	{
		_MainTex("Standard Texture",2D) = "black" {}
		_BackTex("Showthrough Texture",2D) = "white" {}
		_BackColor("Showthrough Color",Color) = (0,1,1,1)
	}
		SubShader
	{
		Tags{ "RenderType" = "Opaque" "Queue" = "Geometry" }
		LOD 100

		/*First Pass, sets values in the stencil buffer for the pixels where the depth test fails (ie. behind objects)*/

		/*Second Pass, render only pixels that have value 1 in the stencil buffer (ie. the pixels that weren't drawn the previous pass) uses the second texture for this*/
		Pass
	{

		Ztest Greater

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
}
