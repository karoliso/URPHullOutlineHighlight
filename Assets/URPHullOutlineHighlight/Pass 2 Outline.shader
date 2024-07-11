Shader "Universal Render Pipeline/Highlighter/Pass 2 Outline"
{
	Properties 
	{
		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_Outline ("Outline width", Range (.002, 0.1)) = .005
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque"}

		Cull Off
        ZTest Always

        Stencil
        {
             Ref 1
             Comp NotEqual
             Pass Keep
             Fail Keep
             ZFail Keep
        }
		
		Pass 
		{
			Name "OUTLINE"
			
            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
			#pragma vertex vert
			#pragma fragment frag
			
            CBUFFER_START(UnityPerMaterial)
            float _Outline;
            float4 _OutlineColor;
            CBUFFER_END
			
            struct Attributes 
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
        
            struct Varyings 
            {
                float4 positionHCS : SV_POSITION;
                half fogCoord : TEXCOORD0;
                half4 color : COLOR;
                UNITY_VERTEX_OUTPUT_STEREO
            };
            
            Varyings vert(Attributes input) 
            {
                Varyings output = (Varyings)0;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);

                float3 clipNormal = TransformObjectToHClip(input.normalOS * 100);
                clipNormal = normalize(float3(clipNormal.xy, 0));

                output.positionHCS.xyz += normalize(clipNormal) * _Outline * output.positionHCS.w;
                
                output.color = _OutlineColor;
                return output;
            }
			
			half4 frag(Varyings i) : SV_Target
			{
				return i.color;
			}
            ENDHLSL
		}
	}
}
