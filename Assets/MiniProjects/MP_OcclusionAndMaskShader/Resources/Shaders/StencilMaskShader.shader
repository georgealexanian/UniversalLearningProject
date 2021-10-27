Shader "Custom/StencilMaskShader"
{
    SubShader
    {
        Pass
        {
            Stencil
            {
                Ref 1
                Pass Replace     
            }

            ZWrite Off
            ColorMask 0
        }
    }
}