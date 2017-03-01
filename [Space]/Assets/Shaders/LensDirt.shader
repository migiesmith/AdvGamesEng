Shader "Hidden/LensDirt" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_fintensity ("Intensity", Range (0, 1)) = 0
		_DirtTex ("Dirt Texture", 2D) = "red" {}
	}
	SubShader {
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform sampler2D _DirtTex;
			uniform float _fintensity;


			float4 frag(v2f_img i) : COLOR {
				float4 colour = tex2D(_MainTex, i.uv);
				float4 dirt = tex2D(_DirtTex, i.uv);

				float4 outColour = lerp(colour, dirt, dirt.a * _fintensity);
				outColour.a = colour.a;
				return outColour;
			}
			ENDCG
		}
	}
}