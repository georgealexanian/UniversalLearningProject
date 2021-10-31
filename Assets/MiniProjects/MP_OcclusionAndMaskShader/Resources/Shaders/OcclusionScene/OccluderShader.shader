Shader "Custom/Occluder"
{
    Properties
    {
        [MainTexture] _BaseMap("Base Map (RGB) Smoothness / Alpha (A)", 2D) = "white" {}
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Geometry"
        }

        Stencil
        {
            Ref 11
            Comp Always
            Pass Replace
        }

        Pass
        {
            ZWrite Off
            ColorMask 0

            HLSLPROGRAM
            #pragma vertex LitPassVertexSimple
            #pragma fragment LitPassFragmentSimple

            #include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitForwardPass.hlsl"
            ENDHLSL
        }
    }
}