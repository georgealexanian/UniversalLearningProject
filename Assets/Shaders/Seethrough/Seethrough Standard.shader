// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Seethrough/Seethrough Standard"
{
	/*
	Niels de Jong (Red Owl Games) - 2016 - Credit appreciated, not required

	Note: This shader was part of a seethrough pack
	Note2: This shader was written as an exercise while I was learning to write shaders, use at your own risk.
	*/
	Properties
	{
		
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo", 2D) = "white" {}

	_Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5

		_Glossiness("Smoothness", Range(0.0, 1.0)) = 0.5
		[Gamma] _Metallic("Metallic", Range(0.0, 1.0)) = 0.0
		_MetallicGlossMap("Metallic", 2D) = "white" {}

	_BumpScale("Scale", Float) = 1.0
		_BumpMap("Normal Map", 2D) = "bump" {}

	_Parallax("Height Scale", Range(0.005, 0.08)) = 0.02
		_ParallaxMap("Height Map", 2D) = "black" {}

	_OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
		_OcclusionMap("Occlusion", 2D) = "white" {}

	_EmissionColor("Color", Color) = (0,0,0)
		_EmissionMap("Emission", 2D) = "white" {}

	_DetailMask("Detail Mask", 2D) = "white" {}

	_DetailAlbedoMap("Detail Albedo x2", 2D) = "grey" {}
	_DetailNormalMapScale("Scale", Float) = 1.0
		_DetailNormalMap("Normal Map", 2D) = "bump" {}

	[Enum(UV0,0,UV1,1)] _UVSec("UV Set for secondary textures", Float) = 0


		// Blending state
		[HideInInspector] _Mode("__mode", Float) = 0.0
		[HideInInspector] _SrcBlend("__src", Float) = 1.0
		[HideInInspector] _DstBlend("__dst", Float) = 0.0
		[HideInInspector] _ZWrite("__zw", Float) = 1.0

		//SEETHROUGH
		_BackTex("Showthrough Texture",2D) = "white" {}
		_BackColor("Showthrough Color",Color) = (0,1,1,1)
	}

		CGINCLUDE
#define UNITY_SETUP_BRDF_INPUT MetallicSetup
		ENDCG

		SubShader
		{
		Tags{ "RenderType" = "Opaque" "Queue"="Geometry" "PerformanceChecks"="False"}
		LOD 100

		/*First Pass, render the normal object, sets values in the stencil buffer for the pixels where the depth test fails (ie. behind objects)*/
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

			// ------------------------------------------------------------------
			//  Base forward pass (directional light, emission, lightmaps, ...)

			Name "FORWARD"
			Tags{ "LightMode" = "ForwardBase" }

			Blend[_SrcBlend][_DstBlend]
			ZWrite[_ZWrite]

			CGPROGRAM
#pragma target 2.0

#pragma shader_feature _NORMALMAP
#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
#pragma shader_feature _EMISSION 
#pragma shader_feature _METALLICGLOSSMAP 
#pragma shader_feature ___ _DETAIL_MULX2
			// SM2.0: NOT SUPPORTED shader_feature _PARALLAXMAP

#pragma skip_variants SHADOWS_SOFT DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE

#pragma multi_compile_fwdbase
#pragma multi_compile_fog

#pragma vertex vertBase
#pragma fragment fragBase
#include "UnityStandardCoreForward.cginc"

			ENDCG
		}

			// ------------------------------------------------------------------
			//  Additive forward pass (one light per pass)
			Pass
		{
			Name "FORWARD_DELTA"
			Tags{ "LightMode" = "ForwardAdd" }
			Blend[_SrcBlend] One
			Fog{ Color(0,0,0,0) } // in additive pass fog should be black
			ZWrite Off
			ZTest LEqual

			CGPROGRAM
#pragma target 2.0

#pragma shader_feature _NORMALMAP
#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
#pragma shader_feature _METALLICGLOSSMAP
#pragma shader_feature ___ _DETAIL_MULX2
			// SM2.0: NOT SUPPORTED shader_feature _PARALLAXMAP
#pragma skip_variants SHADOWS_SOFT

#pragma multi_compile_fwdadd_fullshadows
#pragma multi_compile_fog

#pragma vertex vertAdd
#pragma fragment fragAdd
#include "UnityStandardCoreForward.cginc"

			ENDCG
		}

			// ------------------------------------------------------------------
			//  Shadow rendering pass
			Pass{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }

			ZWrite On ZTest LEqual

			CGPROGRAM
#pragma target 2.0

#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
#pragma skip_variants SHADOWS_SOFT
#pragma multi_compile_shadowcaster

#pragma vertex vertShadowCaster
#pragma fragment fragShadowCaster

#include "UnityStandardShadow.cginc"

			ENDCG
		}

			// ------------------------------------------------------------------
			// Extracts information for lightmapping, GI (emission, albedo, ...)
			// This pass it not used during regular rendering.
			Pass
		{
			Name "META"
			Tags{ "LightMode" = "Meta" }

			Cull Off

			CGPROGRAM
#pragma vertex vert_meta
#pragma fragment frag_meta

#pragma shader_feature _EMISSION
#pragma shader_feature _METALLICGLOSSMAP
#pragma shader_feature ___ _DETAIL_MULX2

#include "UnityStandardMeta.cginc"
			ENDCG
		}

		/*Second Pass, render only pixels that have value 1 in the stencil buffer (ie. the pixels that weren't drawn the previous pass) uses the second texture for this*/
		Pass
		{
			Tags { "RenderType" = "Opaque" "Queue"="Geometry"}

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
