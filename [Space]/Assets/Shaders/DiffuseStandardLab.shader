Shader "Custom/Dissolve"
{
	Properties
	{		
		[Toggle( S_UNLIT )] g_bUnlit( "g_bUnlit", Int ) = 0

		_Color( "Color", Color ) = ( 1, 1, 1, 1 )
		_MainTex( "Albedo", 2D ) = "white" {}
		
		_Cutoff( "Alpha Cutoff", Range( 0.0, 1.0 ) ) = 0.5

		_Glossiness("Smoothness", Range(0.0, 1.0)) = 0.5
		_SpecColor("Specular", Color) = (0.2,0.2,0.2)
		_SpecGlossMap("Specular", 2D) = "white" {}

		g_flReflectanceMin( "g_flReflectanceMin", Range( 0.0, 1.0 ) ) = 0.0
		g_flReflectanceMax( "g_flReflectanceMax", Range( 0.0, 1.0 ) ) = 1.0
		g_flRoughnessFactor( "RoughnessFactor", Float ) = 14.0
		[HideInInspector] g_flReflectanceScale( "g_flReflectanceScale", Range( 0.0, 1.0 ) ) = 1.0
		[HideInInspector] g_flReflectanceBias( "g_flReflectanceBias", Range( 0.0, 1.0 ) ) = 0.0

		[Gamma] _Metallic( "Metallic", Range( 0.0, 1.0 ) ) = 0.0
		_MetallicGlossMap( "Metallic", 2D ) = "white" {}

		_BumpScale( "Scale", Float ) = 1.0
		[Normal] _BumpMap( "Normal Map", 2D ) = "bump" {}

		_Parallax ( "Height Scale", Range ( 0.005, 0.08 ) ) = 0.02
		_ParallaxMap ( "Height Map", 2D ) = "black" {}

		_OcclusionStrength( "Strength", Range( 0.0, 1.0 ) ) = 1.0
		_OcclusionMap( "Occlusion", 2D ) = "white" {}
		_OcclusionStrengthDirectDiffuse( "StrengthDirectDiffuse", Range( 0.0, 1.0 ) ) = 1.0
		_OcclusionStrengthDirectSpecular( "StrengthDirectSpecular", Range( 0.0, 1.0 ) ) = 1.0
		_OcclusionStrengthIndirectDiffuse( "StrengthIndirectDiffuse", Range( 0.0, 1.0 ) ) = 1.0
		_OcclusionStrengthIndirectSpecular( "StrengthIndirectSpecular", Range( 0.0, 1.0 ) ) = 1.0

		g_flCubeMapScalar( "Cube Map Scalar", Range( 0.0, 2.0 ) ) = 1.0

		_EmissionColor( "Color", Color ) = ( 0, 0, 0 )
		_EmissionMap( "Emission", 2D ) = "white" {}
		
		_DetailMask( "Detail Mask", 2D ) = "white" {}

		_DetailAlbedoMap( "Detail Albedo x2", 2D ) = "grey" {}
		_DetailNormalMapScale( "Scale", Float ) = 1.0
		_DetailNormalMap( "Normal Map", 2D ) = "bump" {}

		g_tOverrideLightmap( "Override Lightmap", 2D ) = "white" {}

		[Enum(UV0,0,UV1,1)] _UVSec ( "UV Set for secondary textures", Float ) = 0

		[Toggle( S_RECEIVE_SHADOWS )] g_bReceiveShadows( "g_bReceiveShadows", Int ) = 1

		[Toggle( S_RENDER_BACKFACES )] g_bRenderBackfaces( "g_bRenderBackfaces", Int ) = 0

		[Toggle( S_WORLD_ALIGNED_TEXTURE )] g_bWorldAlignedTexture( "g_bWorldAlignedTexture", Int ) = 0
		g_vWorldAlignedTextureSize( "g_vWorldAlignedTextureSize", Vector ) = ( 1.0, 1.0, 1.0, 0.0 )
		g_vWorldAlignedTextureNormal( "g_vWorldAlignedTextureNormal", Vector ) = ( 0.0, 1.0, 0.0, 0.0 )
		g_vWorldAlignedTexturePosition( "g_vWorldAlignedTexturePosition", Vector ) = ( 0.0, 0.0, 0.0, 0.0 )
		[HideInInspector] g_vWorldAlignedNormalTangentU( "g_vWorldAlignedNormalTangentU", Vector ) = ( -1.0, 0.0, 0.0, 0.0)
		[HideInInspector] g_vWorldAlignedNormalTangentV( "g_vWorldAlignedNormalTangentV", Vector ) = ( 0.0, 0.0, 1.0, 0.0)

		[HideInInspector] _SpecularMode( "__specularmode", Int ) = 1
		[HideInInspector] _Cull ( "__cull", Int ) = 2

		// Blending state
		[HideInInspector] _Mode ( "__mode", Float ) = 2.0
		[HideInInspector] _SrcBlend ( "__src", Float ) = 1.0
		[HideInInspector] _DstBlend ( "__dst", Float ) = 0.0
		[HideInInspector] _ZWrite ( "__zw", Float ) = 1.0
		[HideInInspector] _FogMultiplier ( "__fogmult", Float ) = 1.0

		
      	_DissolveTex ("Dissolve Map", 2D) = "white" {}
	}

    SubShader
    {
      Tags { "RenderType" = "AlphaBlend" }
	  

      Pass{
		  CULL OFF
		  CGPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
      
            struct v2f {
                float4 pos : SV_POSITION;
          		float2 uv : TEXCOORD0;
            };

      		sampler2D _MainTex;
      		sampler2D _DissolveTex;
   
			float4 _Color;

            v2f vert(appdata_base i)
            {
                v2f o;
                o.pos = mul(UNITY_MATRIX_MVP, i.vertex);
            	o.uv = i.texcoord.xy;
                return o;
            }
           
            half4 frag(v2f i) : COLOR
            {
				float4 dCol = tex2D(_DissolveTex, i.uv);
				clip(_Color.a - dCol.r);
                return tex2D(_MainTex, i.uv) * 0.8 * _Color;
            }
           
            ENDCG
	  }
	  UsePass "Valve/vr_standard/FORWARD"
    }
    Fallback "Diffuse"
	CustomEditor "ValveShaderGUI"
 }

 