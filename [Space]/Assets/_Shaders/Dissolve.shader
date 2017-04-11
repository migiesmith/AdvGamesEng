Shader "Space/Dissolve"
{
	Properties
	{		
		_Color( "Color", Color ) = ( 1, 1, 1, 1 )
      	_DissolveTex ("Dissolve Map", 2D) = "white" {}
	}

    SubShader
    {
      Tags { "Queue" = "Transparent" "RenderType"="Transparent" }
      Blend SrcAlpha OneMinusSrcAlpha
	  
	  GrabPass{
		  "_PrePass"
	  }

	  UsePass "Valve/vr_standard/FORWARD"
	  
      Pass{
		  //CULL OFF
		  CGPROGRAM
			#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
      
            struct v2f {
                float4 pos : SV_POSITION;
                float4 prePos : TEXCOORD0;
          		float2 uv : TEXCOORD1;
            };

      		sampler2D _PrePass;
      		sampler2D _DissolveTex;
			float4 _DissolveTex_ST;
   
			float4 _Color;

            v2f vert(appdata_base i)
            {
                v2f o;
                o.pos = mul(UNITY_MATRIX_MVP, i.vertex);
                o.prePos = ComputeGrabScreenPos(UnityObjectToClipPos(i.vertex));
            	o.uv = TRANSFORM_TEX(i.texcoord, _DissolveTex);
                return o;
            }
           
            half4 frag(v2f i) : COLOR
            {
				float4 preCol = tex2Dproj(_PrePass, i.prePos);
				float4 dCol = tex2D(_DissolveTex, i.uv);
				if(dCol.r - _Color.a < 0.0){
					return lerp(float4(0,0,0,0), _Color, 1.0 - _Color.a);
				}else{
					return preCol;
				}
            }
           
            ENDCG
	  }
    }
    Fallback "Diffuse"
 }

 