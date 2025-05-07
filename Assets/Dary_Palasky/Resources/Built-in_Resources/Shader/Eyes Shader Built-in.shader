// Made with Amplify Shader Editor v1.9.1.5
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Dary_Palasky_Built-in/Eyes Shader Built-in"
{
	Properties
	{
		[Header(____Color____)]_Eyes_Color("Eyes_Color", Color) = (1,1,1,0)
		_Brightness("Brightness", Range( 0 , 5)) = 1.5
		_Roughness("Roughness", Range( 0 , 1)) = 0.89
		[NoScaleOffset]_Base_Color("Base_Color", 2D) = "white" {}
		[NoScaleOffset]_IrisMask("IrisMask", 2D) = "white" {}
		[Header(____Emmision____)]_Emission_Intensity("Emission_Intensity", Range( 0 , 100)) = 0
		_Emission_Color("Emission_Color", Color) = (1,0,0,0)
		[Toggle(_ENABLE_EMISSION_MASK_ON)] _Enable_Emission_Mask("Enable_Emission_Mask", Float) = 0
		[NoScaleOffset]_Emission_Mask("Emission_Mask", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature_local _ENABLE_EMISSION_MASK_ON
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
		};

		uniform sampler2D _Base_Color;
		uniform float _Brightness;
		uniform float4 _Eyes_Color;
		uniform sampler2D _IrisMask;
		uniform float4 _Emission_Color;
		uniform float _Emission_Intensity;
		uniform sampler2D _Emission_Mask;
		uniform float _Roughness;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Base_Color1 = i.uv_texcoord;
			float4 temp_output_3_0 = ( tex2D( _Base_Color, uv_Base_Color1 ) * _Brightness );
			float2 uv_IrisMask9 = i.uv_texcoord;
			float4 lerpResult8 = lerp( temp_output_3_0 , ( temp_output_3_0 * _Eyes_Color ) , tex2D( _IrisMask, uv_IrisMask9 ).r);
			o.Albedo = lerpResult8.rgb;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV78 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode78 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV78, -1.0 ) );
			float4 temp_output_76_0 = ( ( _Emission_Color * ( _Emission_Intensity * 10000.0 ) ) * fresnelNode78 );
			float2 uv_Emission_Mask69 = i.uv_texcoord;
			#ifdef _ENABLE_EMISSION_MASK_ON
				float4 staticSwitch82 = ( temp_output_76_0 * tex2D( _Emission_Mask, uv_Emission_Mask69 ) );
			#else
				float4 staticSwitch82 = temp_output_76_0;
			#endif
			o.Emission = staticSwitch82.rgb;
			o.Smoothness = _Roughness;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19105
Node;AmplifyShaderEditor.CommentaryNode;72;-1326.743,-748.0321;Inherit;False;1449.987;662.8954;Base_Color;9;3;8;5;1;2;4;7;9;10;;1,1,1,1;0;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;940.2587,224.339;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Dary_Palasky_Built-in/Eyes Shader Built-in;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-492.3842,-682.5329;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;8;-141.7556,-698.032;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-391.4259,-509.5639;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;1;-1015.831,-669.6854;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;4;-988.173,-473.9441;Inherit;False;Property;_Brightness;Brightness;1;0;Create;True;0;0;0;False;0;False;1.5;1.34;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;9;-444.2321,-319.1933;Inherit;True;Property;_TextureSample1;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;10;-699.8446,-315.1362;Inherit;True;Property;_IrisMask;IrisMask;4;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerNode;69;-495.546,688.0314;Inherit;True;Property;_TextureSample2;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;70;-728.6232,693.3715;Inherit;True;Property;_Emission_Mask;Emission_Mask;8;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;74;28.19849,669.038;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;76;-876.4598,526.7725;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;75;-877.1878,340.6799;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FresnelNode;78;-1211.191,548.1877;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;51;639.5397,312.9555;Inherit;False;Property;_Roughness;Roughness;2;0;Create;True;0;0;0;False;0;False;0.89;0.918;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;77;-1118.413,339.9637;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;10000;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;82;162.1585,292.2797;Inherit;False;Property;_Enable_Emission_Mask;Enable_Emission_Mask;7;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;81;-1104.426,110.3616;Inherit;False;Property;_Emission_Color;Emission_Color;6;0;Create;True;0;0;0;False;0;False;1,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;7;-925.0526,-386.8353;Inherit;False;Property;_Eyes_Color;Eyes_Color;0;1;[Header];Create;True;1;____Color____;0;0;False;0;False;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;2;-1276.743,-677.0007;Inherit;True;Property;_Base_Color;Base_Color;3;1;[NoScaleOffset];Create;True;1;____HUMAN____;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;79;-1442.898,315.4632;Inherit;False;Property;_Emission_Intensity;Emission_Intensity;5;1;[Header];Create;True;1;____Emmision____;0;0;False;0;False;0;0.918;0;100;0;1;FLOAT;0
WireConnection;0;0;8;0
WireConnection;0;2;82;0
WireConnection;0;4;51;0
WireConnection;3;0;1;0
WireConnection;3;1;4;0
WireConnection;8;0;3;0
WireConnection;8;1;5;0
WireConnection;8;2;9;1
WireConnection;5;0;3;0
WireConnection;5;1;7;0
WireConnection;1;0;2;0
WireConnection;9;0;10;0
WireConnection;69;0;70;0
WireConnection;74;0;76;0
WireConnection;74;1;69;0
WireConnection;76;0;75;0
WireConnection;76;1;78;0
WireConnection;75;0;81;0
WireConnection;75;1;77;0
WireConnection;77;0;79;0
WireConnection;82;1;76;0
WireConnection;82;0;74;0
ASEEND*/
//CHKSM=7D099D77085C7C5D55D399CB47D58C26293FB608