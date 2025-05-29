// Made with Amplify Shader Editor v1.9.1.5
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Dary_Palasky_Built-in/Body Shader Built-in_Haunted_Doll"
{
	Properties
	{
		[Header(_____SKIN_____)]_Skin_Color("Skin_Color", Color) = (1,1,1,0)
		_Skin_Roughness("Skin_Roughness", Range( 0 , 2)) = 0.8
		[Toggle]_InvertTexture("InvertTexture", Float) = 0
		[Header(_____BLOOD_____)]_BloodColor("Blood Color", Color) = (0.227451,0,0,1)
		_Blood_Roughness("Blood _Roughness", Range( 0 , 1)) = 0
		_Blood_Fullbody("Blood_Fullbody", Range( 0 , 1)) = 0
		_Blood_torso("Blood_torso", Range( 0 , 1)) = 0
		_Blood_legs("Blood_legs", Range( 0 , 1)) = 0
		_Blood_hands("Blood_hands", Range( 0 , 1)) = 0
		[Header(_____DIRT_____)]_DirtColor("DirtColor", Color) = (0.2235294,0.1372549,0.04705883,1)
		_Dirt_Roughness("Dirt _Roughness", Range( 0 , 0.5)) = 0
		_Dirt_Body("Dirt_Body", Range( 0 , 2)) = 0
		_Dirt_Fullbody("Dirt_Fullbody", Range( 0 , 2)) = 0
		[Header(_____OPACITY_____)]_Hide_Foots("Hide_Foots", Range( 0 , 1)) = 0
		_Hide_TorsoBlouse("Hide_TorsoBlouse", Range( 0 , 1)) = 0
		_Hide_Legs("Hide_Legs", Range( 0 , 1)) = 0
		_Hide_Hands("Hide_Hands", Range( 0 , 1)) = 0
		_Hide_TorsoCoat("Hide_TorsoCoat", Range( 0 , 1)) = 0
		_Hide_TorsoVest("Hide_TorsoVest", Range( 0 , 1)) = 0
		_AlphaClip("AlphaClip", Range( 0 , 1)) = 0.99
		[Header(_____TEXTURES_____)][NoScaleOffset]_Base_Color("Base_Color", 2D) = "white" {}
		[NoScaleOffset]_Normal("Normal", 2D) = "white" {}
		[NoScaleOffset]_RMA("RMA", 2D) = "white" {}
		[NoScaleOffset]_Dirt_Mask("Dirt_Mask", 2D) = "white" {}
		[NoScaleOffset]_Blood_Mask("Blood_Mask", 2D) = "white" {}
		[NoScaleOffset]_Opacity("Opacity", 2D) = "white" {}
		[NoScaleOffset]_Opacity_2("Opacity_2", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "AlphaTest+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Normal;
		uniform sampler2D _Base_Color;
		uniform float4 _Skin_Color;
		uniform float4 _DirtColor;
		uniform float _Dirt_Fullbody;
		uniform sampler2D _Dirt_Mask;
		uniform float _Dirt_Body;
		uniform float4 _BloodColor;
		uniform float _Blood_hands;
		uniform sampler2D _Blood_Mask;
		uniform float _Blood_legs;
		uniform float _Blood_torso;
		uniform float _Blood_Fullbody;
		uniform float _InvertTexture;
		uniform sampler2D _RMA;
		uniform float _Skin_Roughness;
		uniform float _Dirt_Roughness;
		uniform float _Blood_Roughness;
		uniform float _Hide_Foots;
		uniform sampler2D _Opacity;
		uniform float _Hide_Legs;
		uniform float _Hide_TorsoBlouse;
		uniform float _Hide_Hands;
		uniform sampler2D _Opacity_2;
		uniform float _Hide_TorsoCoat;
		uniform float _Hide_TorsoVest;
		uniform float _AlphaClip;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal276 = i.uv_texcoord;
			o.Normal = UnpackNormal( tex2D( _Normal, uv_Normal276 ) );
			float2 uv_Base_Color259 = i.uv_texcoord;
			float4 tex2DNode259 = tex2D( _Base_Color, uv_Base_Color259 );
			float4 lerpResult266 = lerp( tex2DNode259 , ( _Skin_Color * tex2DNode259 ) , 1.0);
			float2 uv_Dirt_Mask32 = i.uv_texcoord;
			float4 tex2DNode32 = tex2D( _Dirt_Mask, uv_Dirt_Mask32 );
			float lerpResult38 = lerp( 0.0 , _Dirt_Fullbody , tex2DNode32.b);
			float clampResult40 = clamp( lerpResult38 , 0.0 , 1.0 );
			float4 lerpResult47 = lerp( lerpResult266 , _DirtColor , clampResult40);
			float lerpResult128 = lerp( 0.0 , _Dirt_Body , tex2DNode32.a);
			float clampResult131 = clamp( lerpResult128 , 0.0 , 1.0 );
			float4 lerpResult133 = lerp( lerpResult47 , _DirtColor , clampResult131);
			float2 uv_Blood_Mask41 = i.uv_texcoord;
			float4 tex2DNode41 = tex2D( _Blood_Mask, uv_Blood_Mask41 );
			float lerpResult43 = lerp( 0.0 , _Blood_hands , tex2DNode41.r);
			float clampResult46 = clamp( lerpResult43 , 0.0 , 1.0 );
			float4 lerpResult155 = lerp( lerpResult133 , _BloodColor , clampResult46);
			float lerpResult135 = lerp( 0.0 , _Blood_legs , tex2DNode41.g);
			float clampResult134 = clamp( lerpResult135 , 0.0 , 1.0 );
			float4 lerpResult158 = lerp( lerpResult155 , _BloodColor , clampResult134);
			float lerpResult138 = lerp( 0.0 , _Blood_torso , tex2DNode41.b);
			float clampResult137 = clamp( lerpResult138 , 0.0 , 1.0 );
			float4 lerpResult157 = lerp( lerpResult158 , _BloodColor , clampResult137);
			float lerpResult141 = lerp( 0.0 , _Blood_Fullbody , pow( tex2DNode41.a , 2.0 ));
			float clampResult140 = clamp( lerpResult141 , 0.0 , 1.0 );
			float4 lerpResult48 = lerp( lerpResult157 , _BloodColor , clampResult140);
			o.Albedo = lerpResult48.rgb;
			float2 uv_RMA51 = i.uv_texcoord;
			float4 tex2DNode51 = tex2D( _RMA, uv_RMA51 );
			float lerpResult132 = lerp( clampResult40 , 1.0 , clampResult131);
			float lerpResult63 = lerp( ( (( _InvertTexture )?( ( 1.0 - tex2DNode51.r ) ):( tex2DNode51.r )) * _Skin_Roughness ) , _Dirt_Roughness , lerpResult132);
			float lerpResult151 = lerp( clampResult46 , 1.0 , clampResult134);
			float lerpResult152 = lerp( lerpResult151 , 1.0 , clampResult137);
			float lerpResult154 = lerp( lerpResult152 , 1.0 , clampResult140);
			float lerpResult64 = lerp( lerpResult63 , _Blood_Roughness , lerpResult154);
			o.Smoothness = lerpResult64;
			o.Occlusion = tex2DNode51.b;
			o.Alpha = 1;
			float2 uv_Opacity220 = i.uv_texcoord;
			float4 tex2DNode220 = tex2D( _Opacity, uv_Opacity220 );
			float lerpResult234 = lerp( 0.0 , _Hide_Foots , tex2DNode220.r);
			float clampResult233 = clamp( lerpResult234 , 0.0 , 1.0 );
			float lerpResult244 = lerp( 1.0 , 0.0 , clampResult233);
			float lerpResult236 = lerp( 0.0 , _Hide_Legs , tex2DNode220.g);
			float clampResult235 = clamp( lerpResult236 , 0.0 , 1.0 );
			float lerpResult245 = lerp( lerpResult244 , 0.0 , clampResult235);
			float lerpResult238 = lerp( 0.0 , _Hide_TorsoBlouse , tex2DNode220.b);
			float clampResult237 = clamp( lerpResult238 , 0.0 , 1.0 );
			float lerpResult246 = lerp( lerpResult245 , 0.0 , clampResult237);
			float2 uv_Opacity_2242 = i.uv_texcoord;
			float4 tex2DNode242 = tex2D( _Opacity_2, uv_Opacity_2242 );
			float lerpResult225 = lerp( 0.0 , _Hide_Hands , tex2DNode242.b);
			float clampResult224 = clamp( lerpResult225 , 0.0 , 1.0 );
			float lerpResult248 = lerp( lerpResult246 , 0.0 , clampResult224);
			float lerpResult227 = lerp( 0.0 , _Hide_TorsoCoat , tex2DNode242.r);
			float clampResult226 = clamp( lerpResult227 , 0.0 , 1.0 );
			float lerpResult249 = lerp( lerpResult248 , 0.0 , clampResult226);
			float lerpResult229 = lerp( 0.0 , _Hide_TorsoVest , tex2DNode242.a);
			float clampResult228 = clamp( lerpResult229 , 0.0 , 1.0 );
			float lerpResult250 = lerp( lerpResult249 , 0.0 , clampResult228);
			clip( lerpResult250 - _AlphaClip );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19105
Node;AmplifyShaderEditor.CommentaryNode;124;-1898.177,309.4966;Inherit;False;1613.884;725.0905;Dirt Mask;12;37;32;133;47;132;131;40;130;128;38;49;39;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;123;-2100.908,1066.388;Inherit;False;1824.397;1022.847;Blood Mask;23;48;157;158;155;154;152;151;42;41;142;139;136;141;140;138;137;135;134;45;43;46;50;288;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;122;-2873.944,2339.88;Inherit;False;605.4539;283.1589;RMA;2;51;52;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;121;-1828.869,2334.521;Inherit;False;1332.985;420.0544;Roughness;6;59;65;63;66;64;56;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-1470.944,2412.556;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;218;-1850.19,3262.815;Inherit;False;1572.101;1066.176;Opacity;28;244;234;243;242;219;220;232;231;230;239;241;240;238;237;236;235;233;229;228;227;226;225;224;245;248;249;250;287;;1,1,1,1;0;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1001.184,2558.565;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Dary_Palasky_Built-in/Body Shader Built-in_Haunted_Doll;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Custom;0.5;True;True;0;True;Opaque;;AlphaTest;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;;-1;0;True;_AlphaClip;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.LerpOp;47;-468.1578,525.2893;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;133;-465.8958,779.4453;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;155;-447.3628,1312.902;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;158;-453.2458,1464.254;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;157;-453.2458,1620.832;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;38;-1012.433,575.1952;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;40;-823.9977,568.0783;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;32;-1626.329,609.0621;Inherit;True;Property;_TextureSample4;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;46;-1053.093,1351.374;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;43;-1239.918,1352.974;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;134;-1051.516,1485.222;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;135;-1238.34,1486.822;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;137;-1046.279,1617.911;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;138;-1233.102,1619.511;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;140;-1043.837,1744.588;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;141;-1230.661,1746.188;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;41;-1822.927,1394.665;Inherit;True;Property;_TextureSample5;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;49;-889.0569,381.7835;Inherit;False;Property;_DirtColor;DirtColor;9;1;[Header];Create;True;1;_____DIRT_____;0;0;False;0;False;0.2235294,0.1372549,0.04705883,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;50;-1099.326,1141.016;Inherit;False;Property;_BloodColor;Blood Color;3;1;[Header];Create;True;1;_____BLOOD_____;0;0;False;0;False;0.227451,0,0,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;132;-631.5117,662.9001;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;128;-1013.527,764.5703;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;131;-820.6457,762.9593;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;64;-732.2343,2400.923;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;63;-1117.126,2406.124;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;258;-1881.285,-298.2018;Inherit;False;1438.693;513.4558;Color;6;270;260;267;266;265;259;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;275;-1837.371,2865.251;Inherit;False;686.8623;331.9106;Normal;2;286;276;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;59;-1783.58,2601.017;Inherit;False;Property;_Skin_Roughness;Skin_Roughness;1;0;Create;True;0;0;0;False;0;False;0.8;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;52;-2823.944,2389.88;Inherit;True;Property;_RMA;RMA;22;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TexturePropertyNode;37;-1860.182,605.9032;Inherit;True;Property;_Dirt_Mask;Dirt_Mask;23;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.LerpOp;266;-646.8198,-39.5631;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;259;-1593.173,-34.5252;Inherit;True;Property;_TextureSample2;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;260;-1837.675,-35.74736;Inherit;True;Property;_Base_Color;Base_Color;20;2;[Header];[NoScaleOffset];Create;True;1;_____TEXTURES_____;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;265;-897.9102,28.18941;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;267;-896.3549,128.2997;Inherit;False;Constant;_Float1;Float 0;1;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;270;-1518.373,-233.9915;Inherit;False;Property;_Skin_Color;Skin_Color;0;1;[Header];Create;True;1;_____SKIN_____;0;0;False;0;False;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;130;-1323.001,602.8036;Inherit;False;Property;_Dirt_Fullbody;Dirt_Fullbody;12;0;Create;True;0;0;0;False;0;False;0;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-1310.59,781.2801;Inherit;False;Property;_Dirt_Body;Dirt_Body;11;0;Create;True;0;0;0;False;0;False;0;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-1517.563,1371.041;Inherit;False;Property;_Blood_hands;Blood_hands;8;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;136;-1513.271,1492.673;Inherit;False;Property;_Blood_legs;Blood_legs;7;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;139;-1510.748,1637.578;Inherit;False;Property;_Blood_torso;Blood_torso;6;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;48;-461.6639,1754.469;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;142;-1508.307,1765.652;Inherit;False;Property;_Blood_Fullbody;Blood_Fullbody;5;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;51;-2590.091,2393.039;Inherit;True;Property;_TextureSample6;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;66;-1000.815,2600.249;Inherit;False;Property;_Blood_Roughness;Blood _Roughness;4;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-1370.504,2607.076;Inherit;False;Property;_Dirt_Roughness;Dirt _Roughness;10;0;Create;True;0;0;0;False;0;False;0;0;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;276;-1498.413,2915.251;Inherit;True;Property;_TextureSample11;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;286;-1787.371,2925.262;Inherit;True;Property;_Normal;Normal;21;1;[NoScaleOffset];Create;True;1;_____TEXTURES_____;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.ClampOpNode;224;-715.8109,3888.692;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;225;-902.6358,3890.292;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;226;-710.7537,4032.849;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;227;-897.5788,4034.449;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;228;-717.9048,4175.859;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;229;-904.7298,4177.459;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;233;-727.692,3364.403;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;235;-726.1137,3498.251;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;236;-912.9389,3499.852;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;237;-720.877,3630.938;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;238;-907.7018,3632.537;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;234;-914.5168,3366.003;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;244;-474.8766,3346.771;Inherit;False;3;0;FLOAT;1;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;245;-470.0017,3485.687;Inherit;False;3;0;FLOAT;1;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;248;-456.8679,3910.915;Inherit;False;3;0;FLOAT;1;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;249;-451.993,4049.835;Inherit;False;3;0;FLOAT;1;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;246;-460.8555,3621.596;Inherit;False;3;0;FLOAT;1;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;220;-1551.73,3361.877;Inherit;True;Property;_TextureSample12;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;242;-1537.45,3828.096;Inherit;True;Property;_TextureSample13;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ToggleSwitchNode;290;-2010.981,2404.397;Inherit;False;Property;_InvertTexture;InvertTexture;2;0;Create;True;0;0;0;False;0;False;0;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;289;-2191.173,2459.097;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;288;-1460.625,1853.637;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;151;-834.7888,1386.533;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;152;-826.9357,1549.869;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;154;-824.869,1680.451;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;42;-2058.39,1393.111;Inherit;True;Property;_Blood_Mask;Blood_Mask;24;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.LerpOp;250;-451.4371,4183.28;Inherit;False;3;0;FLOAT;1;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;287;-1476.367,4194.321;Inherit;False;Property;_AlphaClip;AlphaClip;19;0;Create;True;0;0;0;False;0;False;0.99;0.99;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;232;-1182.374,4195.527;Inherit;False;Property;_Hide_TorsoVest;Hide_TorsoVest;18;0;Create;True;1;;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;231;-1177.845,4063.458;Inherit;False;Property;_Hide_TorsoCoat;Hide_TorsoCoat;17;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;230;-1178.181,3906.993;Inherit;False;Property;_Hide_Hands;Hide_Hands;16;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;241;-1190.602,3648.386;Inherit;False;Property;_Hide_TorsoBlouse;Hide_TorsoBlouse;14;0;Create;True;1;Head_Blood;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;239;-1203.346,3523.225;Inherit;False;Property;_Hide_Legs;Hide_Legs;15;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;240;-1192.162,3382.199;Inherit;False;Property;_Hide_Foots;Hide_Foots;13;1;[Header];Create;True;1;_____OPACITY_____;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;219;-1785.583,3358.718;Inherit;True;Property;_Opacity;Opacity;25;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TexturePropertyNode;243;-1784.011,3827.534;Inherit;True;Property;_Opacity_2;Opacity_2;26;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
WireConnection;56;0;290;0
WireConnection;56;1;59;0
WireConnection;0;0;48;0
WireConnection;0;1;276;0
WireConnection;0;4;64;0
WireConnection;0;5;51;3
WireConnection;0;10;250;0
WireConnection;47;0;266;0
WireConnection;47;1;49;0
WireConnection;47;2;40;0
WireConnection;133;0;47;0
WireConnection;133;1;49;0
WireConnection;133;2;131;0
WireConnection;155;0;133;0
WireConnection;155;1;50;0
WireConnection;155;2;46;0
WireConnection;158;0;155;0
WireConnection;158;1;50;0
WireConnection;158;2;134;0
WireConnection;157;0;158;0
WireConnection;157;1;50;0
WireConnection;157;2;137;0
WireConnection;38;1;130;0
WireConnection;38;2;32;3
WireConnection;40;0;38;0
WireConnection;32;0;37;0
WireConnection;46;0;43;0
WireConnection;43;1;45;0
WireConnection;43;2;41;1
WireConnection;134;0;135;0
WireConnection;135;1;136;0
WireConnection;135;2;41;2
WireConnection;137;0;138;0
WireConnection;138;1;139;0
WireConnection;138;2;41;3
WireConnection;140;0;141;0
WireConnection;141;1;142;0
WireConnection;141;2;288;0
WireConnection;41;0;42;0
WireConnection;132;0;40;0
WireConnection;132;2;131;0
WireConnection;128;1;39;0
WireConnection;128;2;32;4
WireConnection;131;0;128;0
WireConnection;64;0;63;0
WireConnection;64;1;66;0
WireConnection;64;2;154;0
WireConnection;63;0;56;0
WireConnection;63;1;65;0
WireConnection;63;2;132;0
WireConnection;266;0;259;0
WireConnection;266;1;265;0
WireConnection;266;2;267;0
WireConnection;259;0;260;0
WireConnection;265;0;270;0
WireConnection;265;1;259;0
WireConnection;48;0;157;0
WireConnection;48;1;50;0
WireConnection;48;2;140;0
WireConnection;51;0;52;0
WireConnection;276;0;286;0
WireConnection;224;0;225;0
WireConnection;225;1;230;0
WireConnection;225;2;242;3
WireConnection;226;0;227;0
WireConnection;227;1;231;0
WireConnection;227;2;242;1
WireConnection;228;0;229;0
WireConnection;229;1;232;0
WireConnection;229;2;242;4
WireConnection;233;0;234;0
WireConnection;235;0;236;0
WireConnection;236;1;239;0
WireConnection;236;2;220;2
WireConnection;237;0;238;0
WireConnection;238;1;241;0
WireConnection;238;2;220;3
WireConnection;234;1;240;0
WireConnection;234;2;220;1
WireConnection;244;2;233;0
WireConnection;245;0;244;0
WireConnection;245;2;235;0
WireConnection;248;0;246;0
WireConnection;248;2;224;0
WireConnection;249;0;248;0
WireConnection;249;2;226;0
WireConnection;246;0;245;0
WireConnection;246;2;237;0
WireConnection;220;0;219;0
WireConnection;242;0;243;0
WireConnection;290;0;51;1
WireConnection;290;1;289;0
WireConnection;289;0;51;1
WireConnection;288;0;41;4
WireConnection;151;0;46;0
WireConnection;151;2;134;0
WireConnection;152;0;151;0
WireConnection;152;2;137;0
WireConnection;154;0;152;0
WireConnection;154;2;140;0
WireConnection;250;0;249;0
WireConnection;250;2;228;0
ASEEND*/
//CHKSM=CE4D4396F209E2B90D4E4ABA1EDD27FD4903715A