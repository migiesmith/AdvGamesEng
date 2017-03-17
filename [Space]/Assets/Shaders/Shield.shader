Shader "Space/Shield" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_ClipTex ("Clip Texture", 2D) = "white" {}
		_DistortionTex("Distortion Texture", 2D) = "white" {}
		_ClipAmount ("Clip Amount", Range(0,1)) = 0.5
		_Intensity ("Intensity", Float) = 1.0
		_RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
		_RimPower ("Rim Power", Range(0.5,8.0)) = 3.0
		_HitRange("Hit Range", Float) = 0.1
		_OscillationIntensity("Oscillation Intensity", Float) = 0.02
		
	}
	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert alpha
		#pragma vertex vert
		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		#define MAX_HITS 16

		sampler2D _MainTex;
		sampler2D _ClipTex;
		sampler2D _DistortionTex;

		struct Input {
			float2 uv_MainTex;
			float2 uv_ClipTex;
			float2 uv_DistortionTex;
          	float3 viewDir;
			float3 worldNormal;
			float3 worldPos;
		};

		fixed4 _Color;
		half _ClipAmount;
		half _Intensity;
		float4 _RimColor;
		float _RimPower;
		uniform float4 _Hits[MAX_HITS];
		float _HitRange;
		float _OscillationIntensity;

		void vert (inout appdata_full v) 
		{
			//randomNum(v.texcoord.xy)
			v.vertex.xyz += v.normal * sin(_Time) * _OscillationIntensity;
		}

		void surf (Input IN, inout SurfaceOutput o)
		{

			clip(tex2D (_ClipTex, IN.uv_ClipTex).r - _ClipAmount);

			half rim = 1.0 - saturate(dot(normalize(IN.worldNormal), IN.viewDir));

			for(int i = 0; i < MAX_HITS; i++){
				float dist = distance(_Hits[i], IN.worldPos);
				dist = 1.0 - clamp(dist, 0, _HitRange)/_HitRange;
				rim += dist * _Hits[i].w;
			}

			float dVal = tex2D (_DistortionTex, IN.uv_DistortionTex).r;
			float2 distort = float2(dVal, dVal);
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex + distort) * _Color * _Intensity;
			o.Albedo = c.rgb;
			o.Emission = _RimColor.rgb * pow (rim, _RimPower);
			o.Alpha = rim;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
