// Made with Amplify Shader Editor v1.9.1.5
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Dary_Palasky_Built-in/Hair Shader Built-in"
{
	Properties
	{
		[Header(____COLOR____)][NoScaleOffset]_Color_Map("Color_Map", 2D) = "white" {}
		_Base_Color("Base_Color", Color) = (1,1,1,0)
		_Tip_Color("Tip_Color", Color) = (1,1,1,0)
		_Root_Color("Root_Color", Color) = (1,1,1,0)
		_RootPower("RootPower", Range( -5 , 1)) = 0.5
		_TipPower("TipPower", Range( -5 , 1)) = 0.5
		[NoScaleOffset]_Root_Map("Root_Map", 2D) = "white" {}
		[Header(____WIND____)][NoScaleOffset]_WindMask("WindMask", 2D) = "white" {}
		_WindEffect("WindEffect", Range( 0 , 2)) = 0
		_WindPosition_1("WindPosition_1", Color) = (0,0.2196079,0,1)
		_WindPosition_2("WindPosition_2", Color) = (0,0,0.1921569,1)
		[Header(____OPACITY____)]_Disappearing("Disappearing", Range( 0 , 2)) = 0
		_AlphaClip("AlphaClip", Float) = 1.5
		[NoScaleOffset]_Opacity_Map("Opacity_Map", 2D) = "white" {}
		[Header(____SMOOTHNESS____)]_Roughness("Roughness", Range( 0 , 1)) = 0
		[Header(____NORMAL____)][NoScaleOffset][Normal]_Normal_Map("Normal_Map", 2D) = "white" {}
		_Normal_Strength("Normal_Strength", Range( 0 , 8)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Transparent+0" }
		Cull Off
		AlphaToMask On
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
		};

		uniform float4 _WindPosition_1;
		uniform float4 _WindPosition_2;
		uniform sampler2D _WindMask;
		uniform float _WindEffect;
		uniform sampler2D _Normal_Map;
		uniform float _Normal_Strength;
		uniform sampler2D _Color_Map;
		uniform float4 _Base_Color;
		uniform float4 _Tip_Color;
		uniform sampler2D _Root_Map;
		uniform float _TipPower;
		uniform float4 _Root_Color;
		uniform float _RootPower;
		uniform float _Roughness;
		uniform sampler2D _Opacity_Map;
		uniform float _AlphaClip;
		uniform float _Disappearing;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float simplePerlin2D94 = snoise( ( ase_worldPos + ( _Time.y * 0.2 ) ).xy*2.0 );
			simplePerlin2D94 = simplePerlin2D94*0.5 + 0.5;
			float simplePerlin2D93 = snoise( ( ase_worldPos + ( ( 1.0 - _Time.y ) * 0.1 ) ).xy*3.0 );
			simplePerlin2D93 = simplePerlin2D93*0.5 + 0.5;
			float lerpResult97 = lerp( simplePerlin2D94 , simplePerlin2D93 , 0.5);
			float4 lerpResult99 = lerp( _WindPosition_1 , _WindPosition_2 , lerpResult97);
			float2 uv_WindMask178 = v.texcoord;
			float4 tex2DNode178 = tex2Dlod( _WindMask, float4( uv_WindMask178, 0, 0.0) );
			float temp_output_180_0 = ( tex2DNode178.r + tex2DNode178.g );
			v.vertex.xyz = ( float4( ase_vertex3Pos , 0.0 ) + ( ( lerpResult99 * temp_output_180_0 ) * _WindEffect ) ).rgb;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal_Map122 = i.uv_texcoord;
			float3 tex2DNode122 = UnpackNormal( tex2D( _Normal_Map, uv_Normal_Map122 ) );
			float4 color194 = IsGammaSpace() ? float4(0,0,1,0) : float4(0,0,1,0);
			float4 lerpResult192 = lerp( color194 , float4( tex2DNode122 , 0.0 ) , _Normal_Strength);
			o.Normal = BlendNormals( tex2DNode122 , lerpResult192.rgb );
			float2 uv_Color_Map2 = i.uv_texcoord;
			float4 tex2DNode2 = tex2D( _Color_Map, uv_Color_Map2 );
			float4 lerpResult183 = lerp( tex2DNode2 , ( _Base_Color * tex2DNode2 ) , 1.0);
			float2 uv_Root_Map143 = i.uv_texcoord;
			float4 tex2DNode143 = tex2D( _Root_Map, uv_Root_Map143 );
			float lerpResult162 = lerp( 0.0 , 1.0 , pow( tex2DNode143.r , ( 1.0 - _TipPower ) ));
			float clampResult165 = clamp( lerpResult162 , 0.0 , 1.0 );
			float4 lerpResult166 = lerp( lerpResult183 , ( lerpResult183 * _Tip_Color ) , clampResult165);
			float lerpResult171 = lerp( 0.0 , 1.0 , pow( ( 1.0 - tex2DNode143.r ) , ( 1.0 - _RootPower ) ));
			float clampResult170 = clamp( lerpResult171 , 0.0 , 1.0 );
			float4 lerpResult174 = lerp( lerpResult166 , ( lerpResult183 * _Root_Color ) , clampResult170);
			o.Albedo = lerpResult174.rgb;
			o.Smoothness = _Roughness;
			float2 uv_Opacity_Map54 = i.uv_texcoord;
			float smoothstepResult45 = smoothstep( 0.0 , _Disappearing , i.uv_texcoord.y);
			o.Alpha = ( ( tex2D( _Opacity_Map, uv_Opacity_Map54 ).r * _AlphaClip ) * pow( smoothstepResult45 , 5.0 ) );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows vertex:vertexDataFunc 

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
			sampler3D _DitherMaskLOD;
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
				vertexDataFunc( v, customInputData );
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
				surfIN.worldPos = worldPos;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
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
Node;AmplifyShaderEditor.CommentaryNode;189;-2660.823,-1996.745;Inherit;False;2207.973;1455.897;Color;29;174;1;2;166;183;182;5;176;181;143;162;165;163;164;150;170;171;168;169;173;167;175;172;177;142;12;149;151;195;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;187;-2420.764,-112.3192;Inherit;False;617.6471;280.0972;Normal;2;122;123;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;186;-2439.122,225.875;Inherit;False;1956.664;825.9719;Opacity;13;53;139;133;54;55;141;140;42;43;44;45;66;47;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;185;-2452.192,1117.626;Inherit;False;3042.5;1448.274;WindEffect;33;82;83;84;85;86;87;88;90;91;92;89;93;94;95;96;97;99;98;103;105;106;107;102;104;108;110;121;178;180;179;109;100;101;;1,1,1,1;0;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1368.42,-26.48331;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Dary_Palasky_Built-in/Hair Shader Built-in;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;;0;False;;False;0;False;;0;False;;False;0;Translucent;0.5;True;True;0;False;Opaque;;Transparent;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Absolute;0;;-1;-1;-1;-1;0;True;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.SimpleAddOpNode;82;-1603.316,1365.985;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;83;-1824.138,1396.783;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;84;-2095.059,1391.626;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;85;-1978.059,1517.626;Inherit;False;Constant;_Float8;Float 8;11;0;Create;True;0;0;0;False;0;False;0.2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;86;-1954.059,1167.626;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;87;-1628.449,1861.682;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;88;-1849.271,1892.479;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;90;-2003.192,2013.322;Inherit;False;Constant;_Float12;Float 8;11;0;Create;True;0;0;0;False;0;False;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;91;-1979.192,1663.322;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.OneMinusNode;92;-2171.563,1913.426;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;89;-2402.192,1882.322;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;93;-1373.604,1837.423;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;94;-1347.833,1385.028;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;95;-1542.833,1534.028;Inherit;False;Constant;_Float13;Float 13;11;0;Create;True;0;0;0;False;0;False;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;96;-1565.833,1749.028;Inherit;False;Constant;_Float14;Float 13;11;0;Create;True;0;0;0;False;0;False;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;97;-915.4959,1725.377;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;99;-455.0147,1613.584;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;98;-1117.14,1745.743;Inherit;False;Constant;_Float15;Float 13;11;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;103;-219.3296,1943.522;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;105;-392.723,2095.528;Inherit;False;Constant;_Float17;Float 16;13;0;Create;True;0;0;0;False;0;False;2.84;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;106;-524.723,2028.528;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.TextureCoordinatesNode;107;-834.0923,2101.816;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;102;-99.0891,1639.355;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;104;-386.723,2003.528;Inherit;False;Constant;_Float16;Float 16;13;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;108;161.0759,1633.278;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;110;437.908,1452.858;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PosVertexDataNode;121;266.7444,1308.854;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;178;-478.4865,2335.9;Inherit;True;Property;_TextureSample4;Texture Sample 2;5;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;180;-26.94165,2339.162;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;109;-109.9526,1791.562;Inherit;False;Property;_WindEffect;WindEffect;8;0;Create;True;0;0;0;False;0;False;0;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;100;-843.0679,1288.941;Inherit;False;Property;_WindPosition_1;WindPosition_1;9;0;Create;True;0;0;0;False;0;False;0,0.2196079,0,1;0,0.2196078,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;101;-851.84,1466.807;Inherit;False;Property;_WindPosition_2;WindPosition_2;10;0;Create;True;0;0;0;False;0;False;0,0,0.1921569,1;0,0,0.05660379,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;-717.8582,564.2137;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;139;-757.5094,385.2412;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;133;-1038.07,703.0868;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;54;-1162.21,287.0371;Inherit;True;Property;_TextureSample3;Texture Sample 2;5;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;43;-2389.122,736.273;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;66;-1926.402,936.4468;Inherit;False;Constant;_Float10;Float 7;5;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;55;-1466.16,275.875;Inherit;True;Property;_Opacity_Map;Opacity_Map;13;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;140;-930.5094,513.2411;Inherit;False;Property;_AlphaClip;AlphaClip;12;0;Create;True;0;0;0;False;0;False;1.5;1.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;122;-2123.917,-62.22204;Inherit;True;Property;_TextureSample2;Texture Sample 2;15;0;Create;True;0;0;0;False;0;False;123;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;174;-634.8503,-784.7857;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;2;-2370.84,-1743.417;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;166;-1167.099,-1748.591;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;5;-1710.639,-1570.672;Inherit;False;Property;_Tip_Color;Tip_Color;2;0;Create;True;0;0;0;True;0;False;1,1,1,0;1,0.7281962,0.19407,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;176;-1458.365,-1655.035;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;181;-1932.615,-1696.832;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;143;-2346.687,-1023.871;Inherit;True;Property;_TextureSample1;Texture Sample 0;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;162;-1574.318,-1270.752;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;165;-1386.018,-1278.017;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;163;-1758.088,-1359.199;Inherit;False;Constant;_Float4;Float 4;14;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;164;-1761.331,-1284.647;Inherit;False;Constant;_Float5;Float 4;14;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;150;-1750.053,-1157.012;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;170;-1385.326,-841.7957;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;171;-1555.144,-836.7813;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;168;-1745.433,-902.503;Inherit;False;Constant;_Float6;Float 4;14;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;169;-1739.46,-823.4007;Inherit;False;Constant;_Float7;Float 4;14;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;173;-1738.5,-705.1501;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;167;-1998.983,-1219.799;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;175;-1951.89,-751.5554;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;172;-1952.447,-651.2476;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;177;-1271.628,-988.2726;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;142;-2610.823,-1026.178;Inherit;True;Property;_Root_Map;Root_Map;6;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.ColorNode;12;-1581.344,-1047.164;Inherit;False;Property;_Root_Color;Root_Color;3;0;Create;True;0;0;0;False;0;False;1,1,1,0;0.4150943,0.2679641,0.06489345,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;149;-2266.23,-657.1168;Inherit;False;Property;_RootPower;RootPower;4;0;Create;True;0;0;0;False;0;False;0.5;-2.74;-5;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;151;-2320.752,-1222.914;Inherit;False;Property;_TipPower;TipPower;5;0;Create;True;0;0;0;False;0;False;0.5;-1.73;-5;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;1;-2605.293,-1743.635;Inherit;True;Property;_Color_Map;Color_Map;0;2;[Header];[NoScaleOffset];Create;True;1;____COLOR____;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TexturePropertyNode;179;-780.2266,2323.633;Inherit;True;Property;_WindMask;WindMask;7;2;[Header];[NoScaleOffset];Create;True;1;____WIND____;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;80;993.7299,84.60538;Inherit;False;Property;_Roughness;Roughness;14;1;[Header];Create;True;1;____SMOOTHNESS____;0;0;False;0;False;0;0.365;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;123;-2370.764,-62.31921;Inherit;True;Property;_Normal_Map;Normal_Map;15;3;[Header];[NoScaleOffset];[Normal];Create;True;1;____NORMAL____;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;47;-1981.558,414.5811;Inherit;False;Property;_Disappearing;Disappearing;11;1;[Header];Create;True;1;____OPACITY____;0;0;False;0;False;0;1.5;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;182;-2307.863,-1532.768;Inherit;False;Property;_Base_Color;Base_Color;1;0;Create;True;0;0;0;True;0;False;1,1,1,0;1,0.7281962,0.19407,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;183;-1713.892,-1767.087;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;195;-1861.312,-1610.953;Inherit;False;Constant;_Float1;Float 1;17;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BlendNormalsNode;190;-1330.948,-65.02661;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;192;-1513.348,59.77342;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;194;-1747.371,-41.76971;Inherit;False;Constant;_Color0;Color 0;17;0;Create;True;0;0;0;False;0;False;0,0,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;193;-1743.748,146.9734;Inherit;False;Property;_Normal_Strength;Normal_Strength;16;0;Create;True;0;0;0;False;0;False;0;0;0;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;141;-1187.881,889.0942;Inherit;False;Constant;_Float0;Float 0;14;0;Create;True;0;0;0;False;0;False;5;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;45;-1473.903,656.285;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;44;-1934.705,676.4851;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;42;-2133.722,730.3962;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
WireConnection;0;0;174;0
WireConnection;0;1;190;0
WireConnection;0;4;80;0
WireConnection;0;9;53;0
WireConnection;0;11;110;0
WireConnection;82;0;86;0
WireConnection;82;1;83;0
WireConnection;83;0;84;0
WireConnection;83;1;85;0
WireConnection;87;0;91;0
WireConnection;87;1;88;0
WireConnection;88;0;92;0
WireConnection;88;1;90;0
WireConnection;92;0;89;0
WireConnection;93;0;87;0
WireConnection;93;1;96;0
WireConnection;94;0;82;0
WireConnection;94;1;95;0
WireConnection;97;0;94;0
WireConnection;97;1;93;0
WireConnection;97;2;98;0
WireConnection;99;0;100;0
WireConnection;99;1;101;0
WireConnection;99;2;97;0
WireConnection;103;0;106;1
WireConnection;103;1;104;0
WireConnection;103;2;105;0
WireConnection;106;0;107;0
WireConnection;102;0;99;0
WireConnection;102;1;180;0
WireConnection;108;0;102;0
WireConnection;108;1;109;0
WireConnection;110;0;121;0
WireConnection;110;1;108;0
WireConnection;178;0;179;0
WireConnection;180;0;178;1
WireConnection;180;1;178;2
WireConnection;53;0;139;0
WireConnection;53;1;133;0
WireConnection;139;0;54;1
WireConnection;139;1;140;0
WireConnection;133;0;45;0
WireConnection;133;1;141;0
WireConnection;54;0;55;0
WireConnection;122;0;123;0
WireConnection;174;0;166;0
WireConnection;174;1;177;0
WireConnection;174;2;170;0
WireConnection;2;0;1;0
WireConnection;166;0;183;0
WireConnection;166;1;176;0
WireConnection;166;2;165;0
WireConnection;176;0;183;0
WireConnection;176;1;5;0
WireConnection;181;0;182;0
WireConnection;181;1;2;0
WireConnection;143;0;142;0
WireConnection;162;0;163;0
WireConnection;162;1;164;0
WireConnection;162;2;150;0
WireConnection;165;0;162;0
WireConnection;150;0;143;1
WireConnection;150;1;167;0
WireConnection;170;0;171;0
WireConnection;171;0;168;0
WireConnection;171;1;169;0
WireConnection;171;2;173;0
WireConnection;173;0;175;0
WireConnection;173;1;172;0
WireConnection;167;0;151;0
WireConnection;175;0;143;1
WireConnection;172;0;149;0
WireConnection;177;0;183;0
WireConnection;177;1;12;0
WireConnection;183;0;2;0
WireConnection;183;1;181;0
WireConnection;183;2;195;0
WireConnection;190;0;122;0
WireConnection;190;1;192;0
WireConnection;192;0;194;0
WireConnection;192;1;122;0
WireConnection;192;2;193;0
WireConnection;45;0;42;1
WireConnection;45;1;66;0
WireConnection;45;2;47;0
WireConnection;44;0;180;0
WireConnection;42;0;43;0
ASEEND*/
//CHKSM=41085B5D91993F9C75552F0CB0A9594E5916E96E