// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Seethrough/Seethrough Fresnel Effect"
{
	/*
	Niels de Jong (Red Owl Games) - 2016 - Credit appreciated, not required

	Note: This shader was part of a seethrough pack
	Note2: This shader was written as an exercise while I was learning to write shaders, use at your own risk.
	*/
	Properties
	{
		_MainTex("Texture",2D) = "white" {}
		_BackTex("Seethrough Texture",2D) ="black" {}
		//_fresnellerp("Fr",Range(0,1)) = 0.5
		_fresnelcol("Fresnel Color",Color) = (1,1,1,1)
		//_Bias("Fresnel Bias",Range(0,1)) = 0
		_PulseStrength("Pulse Strength",Range(0,0.1)) = 0.01

		_Scale("Fresnel Scale",Range(0,2)) = 0.5
		_Power("Fresnel Power",Range(0,10)) = 0.5
		
	}
		SubShader
	{
		Tags{ "RenderType" = "Opaque" "Queue" = "Geometry" }
		LOD 100

		

			/*First Pass, renders the normal texture, sets values in the stencil buffer for the pixels where the depth test fails (ie. behind objects)*/
			Pass
		{

			Stencil{
			Ref 1
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
			UNITY_TRANSFER_FOG(o,o.vertex);
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

			Stencil{
			Ref 1
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
			float3 normal:NORMAL;
		};

		struct v2f
		{
			float2 uv : TEXCOORD0;
			float4 vertex : SV_POSITION;
			float R : TEXCOORD1;
		};

		sampler2D _BackTex;
		float4 _fresnelcol;
		float4 _BackTex_ST;
		float _fresnellerp;
		float _Bias;
		float _Scale;
		float _Power;

		float _PulseStrength;

		v2f vert(appdata v)
		{
			v2f o;

			//fresnel

			v.vertex.xyz *= 1 - _PulseStrength - 0.1 + (0 + _PulseStrength)*sin(_Time.z);
			v.vertex.x *= 1 + 0.05*sin(_Time.w);
			v.vertex.y *= 1 + 0.05*sin(_Time.x);
			v.vertex.z *= 1 + 0.05*sin(_Time.y);

			o.vertex = UnityObjectToClipPos(v.vertex);
			o.uv = TRANSFORM_TEX(v.uv, _BackTex);
			UNITY_TRANSFER_FOG(o, o.vertex);

			float3 posWorld = mul(unity_ObjectToWorld, v.vertex).xyz;
			float3 normWorld = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));

			float3 I = normalize(posWorld - _WorldSpaceCameraPos.xyz);
			o.R = /*_Bias*/ +_Scale * pow(1.0 + dot(I, normWorld), _Power);

			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{
			// sample the texture
			fixed4 col = tex2D(_BackTex, i.uv);
		//fixed4 fresnelcol = fixed4(1, 1, 0, 1);
		//fixed4 returncol = (1 - returncol.r, 1 - returncol.g, 1 - returncol.b, returncol.a); 
		return lerp(col, _fresnelcol, i.R);
		}
			ENDCG
		}
	
	
	}
}
