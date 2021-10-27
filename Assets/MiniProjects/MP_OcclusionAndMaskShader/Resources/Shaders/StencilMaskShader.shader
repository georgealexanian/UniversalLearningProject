Shader "Custom/StencilMaskShader"
{
    SubShader
    {
        Tags 
        { 
            "Queue" = "Background+1"
            "RenderType" = "Opaque" 
        }

        Pass
        {
            Stencil
            {
                Ref 1
                Comp Always
                Pass Replace     
            }

            ZTest LEqual
            ZWrite Off
            Blend Zero One
        }
    }
}