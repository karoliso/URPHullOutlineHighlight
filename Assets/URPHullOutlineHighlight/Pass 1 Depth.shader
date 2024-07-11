Shader "Universal Render Pipeline/Highlighter/Pass 1 Depth"
{
    Properties
    { }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalRenderPipeline" }

        Cull Back
        ZWrite On
        ZTest Always
        ColorMask 0

        Stencil
        {
             Ref 1
             Comp Always
             Pass Replace
             Fail Replace
             ZFail Replace
        }

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"            

            struct Attributes
            {
                float4 positionOS   : POSITION;                 
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
            };            

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);

                return OUT;
            }
        
            half4 frag() : SV_Target
            {
                return 0;
            }
            ENDHLSL
        }
    }
}