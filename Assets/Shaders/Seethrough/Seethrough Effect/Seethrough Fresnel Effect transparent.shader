// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Seethrough/Seethrough Fresnel Effect Transparent"
{

	/*
	Niels de Jong (Red Owl Games) - 2016 - Credit appreciated, not required

	Note: This shader was part of a seethrough pack
	Note2: This shader was written as an exercise while I was learning to write shaders, use at your own risk.
	*/
	Properties
	{
		_MainTex("Texture",2D) = "white" {}
		//_fresnellerp("Fr",Range(0,1)) = 0.5
		_fresnelcol("Fresnel Color",Color) = (1,1,1,1)
		//_Bias("Fresnel Bias",Range(0,1)) = 0
		_PulseStrength("Pulse Strength",Range(0,0.1)) = 0.01
		_PulseModifier("Pulse Modifier",Range(0,6.4)) = 0
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

			Pass
		{

			Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
			ZWrite On
			Blend SrcAlpha OneMinusSrcAlpha

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

			uniform float4 _Tint;
		uniform float4 _fresnelcol;
		uniform float _Scale;
		float _PulseStrength;
		float _PulseModifier;

		struct vIN
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
		};

		struct vOUT
		{
			float4 pos : SV_POSITION;
			float3 posWorld : TEXCOORD0;
			float3 normWorld : TEXCOORD1;

		};

		vOUT vert(vIN v)
		{
			vOUT o;
			v.vertex.xyz *= 1 - _PulseStrength - 0.1 + (0 + _PulseStrength)*sin(_Time.z+_PulseModifier);
			v.vertex.x *= 1 + 0.05*sin(_Time.w+_PulseModifier);
			v.vertex.y *= 1 + 0.05*sin(_Time.x+_PulseModifier);
			v.vertex.z *= 1 + 0.05*sin(_Time.y+_PulseModifier);
			o.pos = UnityObjectToClipPos(v.vertex);
			
			o.posWorld = mul(unity_ObjectToWorld, v.vertex);
			o.normWorld = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));

			return o;
		}

		float4 frag(vOUT i) : COLOR
		{
			float3 I = normalize(i.posWorld - _WorldSpaceCameraPos.xyz);

			float refFactor = max(0.0, min(1.0,_Scale * pow(1.0 + dot(I, i.normWorld), 1.4)));
			return  lerp(float4(1.0, 1.0, 1.0, 0.0), _fresnelcol,refFactor);
		}

			ENDCG
		}

				Pass
		{

			Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
			ZWrite On
			Blend SrcAlpha OneMinusSrcAlpha

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

			uniform float4 _Tint;
		uniform float4 _fresnelcol;
		uniform float _Scale;
		float _PulseStrength;
		float _PulseModifier;

		struct vIN
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
		};

		struct vOUT
		{
			float4 pos : SV_POSITION;
			float3 posWorld : TEXCOORD0;
			float3 normWorld : TEXCOORD1;

		};

		vOUT vert(vIN v)
		{
			vOUT o;
			v.vertex.xyz *= 1 - _PulseStrength - 0.1 + (0 + _PulseStrength)*sin(_Time.y + _PulseModifier);
			v.vertex.x *= 1 + 0.05*sin(_Time.x + _PulseModifier);
			v.vertex.y *= 1 + 0.05*sin(_Time.w + _PulseModifier);
			v.vertex.z *= 1 + 0.05*sin(_Time.z + _PulseModifier);
			o.pos = UnityObjectToClipPos(v.vertex);

			o.posWorld = mul(unity_ObjectToWorld, v.vertex);
			o.normWorld = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));

			return o;
		}

		float4 frag(vOUT i) : COLOR
		{
			float3 I = normalize(i.posWorld - _WorldSpaceCameraPos.xyz);

			float refFactor = max(0.0, min(1.0,_Scale * pow(1.0 + dot(I, i.normWorld), 1.4)));
			return  lerp(float4(1.0, 1.0, 1.0, 0.0), _fresnelcol,refFactor);
		}

			ENDCG
		}
				

		
	
	}
}
