// Made with Amplify Shader Editor v1.9.1.5
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Dary_Palasky_Built-in/Fabric Shader Built-in"
{
	Properties
	{
		[Header(____COLOR____)]_ChangeBaseColor("ChangeBaseColor", Color) = (1,1,1,0)
		[Toggle]_EnableWhiteColor("EnableWhiteColor", Float) = 0
		_Part_R("Part_R", Color) = (1,1,1,0)
		_Part_G("Part_G", Color) = (1,1,1,0)
		_Part_B("Part_B", Color) = (1,1,1,0)
		[Header(_____BLOOD_____)]_BloodColor("Blood Color", Color) = (0.227451,0,0,1)
		_Blood_Intensity("Blood_Intensity", Range( 0 , 1)) = 0
		_Blood_Roughness("Blood_Roughness", Range( 0 , 1)) = 0
		[Header(_____DIRT_____)]_DirtColor("DirtColor", Color) = (0.2235294,0.1372549,0.04705883,1)
		_Dirt_Intensity("Dirt_Intensity", Range( 0 , 2)) = 0
		_Power("Power", Range( 0 , 3)) = 0
		_UVtilling("UVtilling", Range( 0 , 10)) = 1
		_Dirt_Roughness("Dirt_Roughness", Range( 0 , 0.5)) = 0
		[Header(____ROUGHNESS____)]_Base_Roughness("Base_Roughness", Range( 0 , 2)) = 0
		[Toggle]_InvertTexture("InvertTexture", Float) = 0
		[Header(____METALLIC____)]_Metallic_Intensity("Metallic_Intensity", Range( 0 , 1)) = 0
		[Header(____OPACITY____)]_Hide("Hide", Range( 0 , 1)) = 0
		_AlphaClip("AlphaClip", Range( 0 , 1)) = 0.5
		[NoScaleOffset]_Opacity("Opacity", 2D) = "white" {}
		[Header(____TEXTURES____)][NoScaleOffset]_BaseColor_1("BaseColor_1", 2D) = "white" {}
		[NoScaleOffset]_BaseColor_2("BaseColor_2", 2D) = "white" {}
		[NoScaleOffset]_Normal("Normal", 2D) = "white" {}
		[NoScaleOffset]_RMA("RMA", 2D) = "white" {}
		[NoScaleOffset]_DirtMask("DirtMask", 2D) = "white" {}
		[NoScaleOffset]_RGBmask("RGBmask", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "AlphaTest+0" }
		Cull Off
		AlphaToMask On
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Normal;
		uniform float _EnableWhiteColor;
		uniform sampler2D _BaseColor_1;
		uniform sampler2D _BaseColor_2;
		uniform float4 _ChangeBaseColor;
		uniform float4 _Part_R;
		uniform sampler2D _RGBmask;
		uniform float4 _Part_G;
		uniform float4 _Part_B;
		uniform float4 _DirtColor;
		uniform float _Dirt_Intensity;
		uniform sampler2D _DirtMask;
		uniform float _UVtilling;
		uniform float _Power;
		uniform float4 _BloodColor;
		uniform float _Blood_Intensity;
		uniform sampler2D _RMA;
		uniform float _Metallic_Intensity;
		uniform float _InvertTexture;
		uniform float _Base_Roughness;
		uniform float _Dirt_Roughness;
		uniform float _Blood_Roughness;
		uniform sampler2D _Opacity;
		uniform float _Hide;
		uniform float _AlphaClip;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal48 = i.uv_texcoord;
			o.Normal = UnpackNormal( tex2D( _Normal, uv_Normal48 ) );
			float2 uv_BaseColor_11 = i.uv_texcoord;
			float2 uv_BaseColor_287 = i.uv_texcoord;
			float4 lerpResult9 = lerp( (( _EnableWhiteColor )?( tex2D( _BaseColor_2, uv_BaseColor_287 ) ):( tex2D( _BaseColor_1, uv_BaseColor_11 ) )) , ( (( _EnableWhiteColor )?( tex2D( _BaseColor_2, uv_BaseColor_287 ) ):( tex2D( _BaseColor_1, uv_BaseColor_11 ) )) * _ChangeBaseColor ) , 1.0);
			float2 uv_RGBmask100 = i.uv_texcoord;
			float4 tex2DNode100 = tex2D( _RGBmask, uv_RGBmask100 );
			float4 lerpResult97 = lerp( lerpResult9 , ( lerpResult9 * _Part_R ) , tex2DNode100.r);
			float4 lerpResult98 = lerp( lerpResult97 , ( lerpResult97 * _Part_G ) , tex2DNode100.g);
			float4 lerpResult295 = lerp( lerpResult98 , ( lerpResult98 * _Part_B ) , tex2DNode100.b);
			float lerpResult118 = lerp( 0.0 , _Dirt_Intensity , pow( tex2D( _DirtMask, ( i.uv_texcoord * _UVtilling ) ).g , _Power ));
			float clampResult120 = clamp( lerpResult118 , 0.0 , 1.0 );
			float4 lerpResult111 = lerp( lerpResult295 , _DirtColor , clampResult120);
			float lerpResult124 = lerp( 0.0 , _Blood_Intensity , tex2DNode100.a);
			float clampResult123 = clamp( lerpResult124 , 0.0 , 1.0 );
			float4 lerpResult113 = lerp( lerpResult111 , _BloodColor , clampResult123);
			o.Albedo = lerpResult113.rgb;
			float2 uv_RMA155 = i.uv_texcoord;
			float4 tex2DNode155 = tex2D( _RMA, uv_RMA155 );
			o.Metallic = ( tex2DNode155.b * _Metallic_Intensity );
			float lerpResult160 = lerp( ( (( _InvertTexture )?( ( 1.0 - tex2DNode155.g ) ):( tex2DNode155.g )) * _Base_Roughness ) , _Dirt_Roughness , clampResult120);
			float lerpResult158 = lerp( lerpResult160 , _Blood_Roughness , clampResult123);
			o.Smoothness = lerpResult158;
			o.Occlusion = tex2DNode155.r;
			o.Alpha = 1;
			float2 uv_Opacity25 = i.uv_texcoord;
			float lerpResult168 = lerp( 0.0 , tex2D( _Opacity, uv_Opacity25 ).r , ( 1.0 - _Hide ));
			float clampResult167 = clamp( lerpResult168 , 0.0 , 1.0 );
			clip( clampResult167 - _AlphaClip );
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
			AlphaToMask Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.6
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
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
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
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
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
Node;AmplifyShaderEditor.CommentaryNode;171;-833.8847,4267.34;Inherit;False;851.9762;304.2454;Metallic;2;163;164;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;157;-2065.799,3770.396;Inherit;False;608.0111;281.892;RMA;2;155;156;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;86;-853.2256,4700.892;Inherit;False;1261.192;404.0884;Opacity;6;167;168;169;26;25;303;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;85;-1316.874,495.6986;Inherit;False;1797.233;1580.529;Color;20;2;10;13;11;9;1;88;93;91;92;94;98;100;101;96;95;97;295;87;312;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;83;-789.5438,3386.653;Inherit;False;630.8391;289.4064;Normal;2;49;48;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;82;-838.5442,3770.343;Inherit;False;1586.137;419.4397;Roughness;6;160;57;56;158;161;159;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-12.67809,661.9016;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;9;260.3086,579.3976;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;92;-468.6636,1455.126;Inherit;False;Property;_Part_G;Part_G;3;0;Create;True;0;0;0;False;0;False;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;93;-472.5727,1617.478;Inherit;False;Property;_Part_B;Part_B;4;0;Create;True;0;0;0;False;0;False;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;96;-181.7222,1660.641;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;95;-186.1453,1491.451;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;94;-196.0976,1317.838;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;109;-1354.511,2286.814;Inherit;False;1845.991;540.3239;Dirt Mask;12;119;301;120;146;111;311;309;310;308;307;182;118;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;110;-788.0328,2868.819;Inherit;False;1278.492;451.4852;Blood Mask;5;125;147;124;123;113;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;1;-1010.776,558.1917;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;25;-557.2683,4750.893;Inherit;True;Property;_TextureSample4;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;167;3.309446,4754.254;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;163;-493.5909,4319.126;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;158;581.7344,3851.825;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;160;196.8428,3857.026;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;113;319.6069,3115.333;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;123;-286.1235,3153.805;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;124;-472.9485,3155.405;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;118;-81.46078,2542.914;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;168;-176.1684,4755.854;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;100;-247.0512,1054.657;Inherit;True;Property;_TextureSample2;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;10;-19.4302,762.0187;Inherit;False;Constant;_Float0;Float 0;3;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;97;152.3851,1298.409;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;98;147.1551,1483.571;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;295;128.3517,1640.365;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1814.556,3966.49;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Dary_Palasky_Built-in/Fabric Shader Built-in;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;;0;False;;False;0;False;;0;False;;False;0;Custom;0.5;True;True;0;True;Opaque;;AlphaTest;All;12;all;True;True;True;True;0;False;;False;193;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;1;False;;1;False;;0;1;False;;1;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;True;0;0;False;;-1;0;True;_AlphaClip;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.ColorNode;91;-467.2484,1289.787;Inherit;False;Property;_Part_R;Part_R;2;0;Create;True;0;0;0;False;0;False;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;164;-783.8846,4444.422;Inherit;False;Property;_Metallic_Intensity;Metallic_Intensity;15;1;[Header];Create;True;1;____METALLIC____;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;147;-332.3566,2943.447;Inherit;False;Property;_BloodColor;Blood Color;5;1;[Header];Create;True;1;_____BLOOD_____;0;0;False;0;False;0.227451,0,0,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;182;-251.6512,2658.275;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;2;-1253.944,555.3956;Inherit;True;Property;_BaseColor_1;BaseColor_1;19;2;[Header];[NoScaleOffset];Create;True;1;____TEXTURES____;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;302;1394.2,4170.745;Inherit;False;Property;_AlphaClip;AlphaClip;17;0;Create;True;0;0;0;False;0;False;0.5;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;159;313.1537,4051.151;Inherit;False;Property;_Blood_Roughness;Blood_Roughness;7;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;161;-56.53535,4057.978;Inherit;False;Property;_Dirt_Roughness;Dirt_Roughness;12;0;Create;True;0;0;0;False;0;False;0;0;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;26;-803.2256,4754.039;Inherit;True;Property;_Opacity;Opacity;18;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;169;-689.3575,4952.563;Inherit;False;Property;_Hide;Hide;16;1;[Header];Create;True;1;____OPACITY____;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;303;-378.4414,4989.178;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;49;-739.5442,3446.059;Inherit;True;Property;_Normal;Normal;21;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerNode;48;-479.5053,3436.653;Inherit;True;Property;_TextureSample7;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;156;-2015.799,3820.396;Inherit;True;Property;_RMA;RMA;22;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerNode;155;-1779.387,3822.287;Inherit;True;Property;_TextureSample6;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;125;-750.5931,3172.517;Inherit;False;Property;_Blood_Intensity;Blood_Intensity;6;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;310;-1032.148,2597.443;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;309;-1300.132,2595.737;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;111;306.1313,2514.473;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;146;12.80042,2347.586;Inherit;False;Property;_DirtColor;DirtColor;8;1;[Header];Create;True;1;_____DIRT_____;0;0;False;0;False;0.2235294,0.1372549,0.04705883,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;120;82.07416,2542.588;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;308;-887.9937,2377.324;Inherit;True;Property;_TextureSample3;Texture Sample 3;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;307;-1166.494,2382.246;Inherit;True;Property;_DirtMask;DirtMask;23;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;301;-546.3464,2677.282;Inherit;False;Property;_Power;Power;10;0;Create;True;0;0;0;False;0;False;0;0;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;119;-551.5239,2388.183;Inherit;False;Property;_Dirt_Intensity;Dirt_Intensity;9;0;Create;True;0;0;0;False;0;False;0;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;311;-1328.299,2728.322;Float;False;Property;_UVtilling;UVtilling;11;0;Create;True;0;0;0;False;0;False;1;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;13;-283.5646,704.9106;Inherit;False;Property;_ChangeBaseColor;ChangeBaseColor;0;1;[Header];Create;True;1;____COLOR____;0;0;False;0;False;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;88;-1231.973,809.544;Inherit;True;Property;_BaseColor_2;BaseColor_2;20;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerNode;87;-997.6699,809.7914;Inherit;True;Property;_TextureSample1;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ToggleSwitchNode;312;-591.2054,578.9161;Inherit;False;Property;_EnableWhiteColor;EnableWhiteColor;1;0;Create;True;0;0;0;False;0;False;0;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;313;-1260.215,3930.435;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;314;-1070.859,3854.654;Inherit;False;Property;_InvertTexture;InvertTexture;14;0;Create;True;0;0;0;False;0;False;0;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;57;-789.7086,4055.292;Inherit;False;Property;_Base_Roughness;Base_Roughness;13;1;[Header];Create;True;1;____ROUGHNESS____;0;0;False;0;False;0;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-559.6693,3861.139;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;101;-474.42,1064.822;Inherit;True;Property;_RGBmask;RGBmask;24;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
WireConnection;11;0;312;0
WireConnection;11;1;13;0
WireConnection;9;0;312;0
WireConnection;9;1;11;0
WireConnection;9;2;10;0
WireConnection;96;0;98;0
WireConnection;96;1;93;0
WireConnection;95;0;97;0
WireConnection;95;1;92;0
WireConnection;94;0;9;0
WireConnection;94;1;91;0
WireConnection;1;0;2;0
WireConnection;25;0;26;0
WireConnection;167;0;168;0
WireConnection;163;0;155;3
WireConnection;163;1;164;0
WireConnection;158;0;160;0
WireConnection;158;1;159;0
WireConnection;158;2;123;0
WireConnection;160;0;56;0
WireConnection;160;1;161;0
WireConnection;160;2;120;0
WireConnection;113;0;111;0
WireConnection;113;1;147;0
WireConnection;113;2;123;0
WireConnection;123;0;124;0
WireConnection;124;1;125;0
WireConnection;124;2;100;4
WireConnection;118;1;119;0
WireConnection;118;2;182;0
WireConnection;168;1;25;1
WireConnection;168;2;303;0
WireConnection;100;0;101;0
WireConnection;97;0;9;0
WireConnection;97;1;94;0
WireConnection;97;2;100;1
WireConnection;98;0;97;0
WireConnection;98;1;95;0
WireConnection;98;2;100;2
WireConnection;295;0;98;0
WireConnection;295;1;96;0
WireConnection;295;2;100;3
WireConnection;0;0;113;0
WireConnection;0;1;48;0
WireConnection;0;3;163;0
WireConnection;0;4;158;0
WireConnection;0;5;155;1
WireConnection;0;10;167;0
WireConnection;182;0;308;2
WireConnection;182;1;301;0
WireConnection;303;0;169;0
WireConnection;48;0;49;0
WireConnection;155;0;156;0
WireConnection;310;0;309;0
WireConnection;310;1;311;0
WireConnection;111;0;295;0
WireConnection;111;1;146;0
WireConnection;111;2;120;0
WireConnection;120;0;118;0
WireConnection;308;0;307;0
WireConnection;308;1;310;0
WireConnection;87;0;88;0
WireConnection;312;0;1;0
WireConnection;312;1;87;0
WireConnection;313;0;155;2
WireConnection;314;0;155;2
WireConnection;314;1;313;0
WireConnection;56;0;314;0
WireConnection;56;1;57;0
ASEEND*/
//CHKSM=7E02117C344484DFCF6D1378437DC5BFDBFCFFDD