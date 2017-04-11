/// ----------------------------------------
/// Author: Grant Smith (40111906 / migiesmith)
/// ----------------------------------------

Shader "Space/Outline" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Outline("Outline Size", Range(0.0, 0.1)) = 0.005
	}


	SubShader {
      	Tags { "RenderType"="Opaque" }
        Cull Front
        ZWrite On
        ZTest LEqual
		LOD 200

		  
		Pass{

		CGPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
			

            struct v2f {
                float4 pos : SV_POSITION;
            };

			float4 _Color;
			float _Outline;


            v2f vert(appdata_base i)
            {
                v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, i.vertex);
				
				float3 norm   = mul ((float3x3)UNITY_MATRIX_IT_MV, i.normal);
				float2 offset = TransformViewToProjection(norm.xy);
				
				o.pos.xy += offset * o.pos.z * _Outline;
                return o;
            }

			
            half4 frag(v2f i) : COLOR
            {
				return _Color;
            }
		ENDCG
		}

		Cull Back
	  	UsePass "Valve/vr_standard/FORWARD"


/*
		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color;
		float4 _RimColor;
		float _RimPower;
*/
			/*
		void surf (Input IN, inout SurfaceOutput o)
		{
			
		}
		*/
	}

	FallBack "Diffuse"
}
