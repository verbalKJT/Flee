// Made with Amplify Shader Editor v1.9.1.5
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Dary_Palasky_Built-in/Head Shader Built-in"
{
	Properties
	{
		[Header(_____SKIN_____)]_Human_Skin_Color("Human_Skin_Color", Color) = (1,1,1,0)
		_Skin_Roughness("Skin_Roughness", Range( 0 , 2)) = 0.8
		[Toggle]_InvertTexture("InvertTexture", Float) = 0
		_Metallic("Metallic", Range( 0 , 1)) = 0.8
		[Header(_____BLOOD_____)]_Blood_Color("Blood_Color", Color) = (0.227451,0,0,1)
		_Blood_Roughness("Blood_Roughness", Range( 0 , 1)) = 0
		_Blood_Intensity_1("Blood_Intensity_1", Range( 0 , 1)) = 0
		_Blood_Intensity_2("Blood_Intensity_2", Range( 0 , 1)) = 0
		_Blood_Intensity_3("Blood_Intensity_3", Range( 0 , 1)) = 0
		_Blood_Fullbody("Blood_Fullbody", Range( 0 , 1)) = 0
		[Header(_____DIRT_____)]_Dirt_Color("Dirt_Color", Color) = (0.172549,0.09411765,0.03921569,1)
		_Dirt_Roughness("Dirt_Roughness", Range( 0 , 0.5)) = 0
		_Dirt_Head_Intensity("Dirt_Head_Intensity", Range( 0 , 2)) = 0
		_Dirt_Fullbody("Dirt_Fullbody", Range( 0 , 2)) = 0
		[HideInInspector][Header(_____OPACITY_____)]_Neck_1("Neck_1", Range( 0 , 1)) = 0
		[HideInInspector]_Neck_2("Neck_2", Range( 0 , 1)) = 0
		[HideInInspector]_AlphaClip("AlphaClip", Range( 0 , 1)) = 0.99
		[Header(_____TEXTURES_____)][NoScaleOffset]_Base_Color("Base_Color", 2D) = "white" {}
		[NoScaleOffset]_Normal("Normal", 2D) = "white" {}
		[NoScaleOffset]_RMA("RMA", 2D) = "white" {}
		[NoScaleOffset]_Dirt_Mask("Dirt_Mask", 2D) = "white" {}
		[NoScaleOffset]_Blood_Mask("Blood_Mask", 2D) = "white" {}
		[HideInInspector][NoScaleOffset]_Opacity("Opacity", 2D) = "white" {}
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
		uniform float4 _Human_Skin_Color;
		uniform float4 _Dirt_Color;
		uniform float _Dirt_Head_Intensity;
		uniform sampler2D _Dirt_Mask;
		uniform float _Dirt_Fullbody;
		uniform float4 _Blood_Color;
		uniform float _Blood_Intensity_1;
		uniform sampler2D _Blood_Mask;
		uniform float _Blood_Intensity_2;
		uniform float _Blood_Intensity_3;
		uniform float _Blood_Fullbody;
		uniform sampler2D _RMA;
		uniform float _Metallic;
		uniform float _InvertTexture;
		uniform float _Skin_Roughness;
		uniform float _Dirt_Roughness;
		uniform float _Blood_Roughness;
		uniform float _Neck_1;
		uniform sampler2D _Opacity;
		uniform float _Neck_2;
		uniform float _AlphaClip;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal71 = i.uv_texcoord;
			o.Normal = UnpackNormal( tex2D( _Normal, uv_Normal71 ) );
			float2 uv_Base_Color1 = i.uv_texcoord;
			float4 tex2DNode1 = tex2D( _Base_Color, uv_Base_Color1 );
			float4 lerpResult5 = lerp( tex2DNode1 , ( _Human_Skin_Color * tex2DNode1 ) , 1.0);
			float2 uv_Dirt_Mask258 = i.uv_texcoord;
			float4 tex2DNode258 = tex2D( _Dirt_Mask, uv_Dirt_Mask258 );
			float lerpResult255 = lerp( 0.0 , _Dirt_Head_Intensity , tex2DNode258.r);
			float clampResult256 = clamp( lerpResult255 , 0.0 , 1.0 );
			float4 lerpResult303 = lerp( lerpResult5 , _Dirt_Color , clampResult256);
			float lerpResult299 = lerp( 0.0 , _Dirt_Fullbody , tex2DNode258.g);
			float clampResult300 = clamp( lerpResult299 , 0.0 , 1.0 );
			float4 lerpResult257 = lerp( lerpResult303 , _Dirt_Color , clampResult300);
			float2 uv_Blood_Mask270 = i.uv_texcoord;
			float4 tex2DNode270 = tex2D( _Blood_Mask, uv_Blood_Mask270 );
			float lerpResult261 = lerp( 0.0 , _Blood_Intensity_1 , tex2DNode270.r);
			float clampResult260 = clamp( lerpResult261 , 0.0 , 1.0 );
			float4 lerpResult275 = lerp( lerpResult257 , _Blood_Color , clampResult260);
			float lerpResult263 = lerp( 0.0 , _Blood_Intensity_2 , tex2DNode270.g);
			float clampResult262 = clamp( lerpResult263 , 0.0 , 1.0 );
			float4 lerpResult276 = lerp( lerpResult275 , _Blood_Color , clampResult262);
			float lerpResult265 = lerp( 0.0 , _Blood_Intensity_3 , tex2DNode270.b);
			float clampResult264 = clamp( lerpResult265 , 0.0 , 1.0 );
			float4 lerpResult277 = lerp( lerpResult276 , _Blood_Color , clampResult264);
			float lerpResult267 = lerp( 0.0 , _Blood_Fullbody , pow( tex2DNode270.a , 2.0 ));
			float clampResult266 = clamp( lerpResult267 , 0.0 , 1.0 );
			float4 lerpResult279 = lerp( lerpResult277 , _Blood_Color , clampResult266);
			o.Albedo = lerpResult279.rgb;
			float2 uv_RMA51 = i.uv_texcoord;
			float4 tex2DNode51 = tex2D( _RMA, uv_RMA51 );
			float lerpResult336 = lerp( 0.0 , tex2DNode51.a , _Metallic);
			o.Metallic = lerpResult336;
			float lerpResult302 = lerp( clampResult256 , 1.0 , clampResult300);
			float lerpResult63 = lerp( ( (( _InvertTexture )?( ( 1.0 - tex2DNode51.r ) ):( tex2DNode51.r )) * _Skin_Roughness ) , _Dirt_Roughness , lerpResult302);
			float lerpResult272 = lerp( clampResult260 , 1.0 , clampResult262);
			float lerpResult273 = lerp( lerpResult272 , 1.0 , clampResult264);
			float lerpResult274 = lerp( lerpResult273 , 1.0 , clampResult266);
			float lerpResult64 = lerp( lerpResult63 , _Blood_Roughness , lerpResult274);
			o.Smoothness = lerpResult64;
			o.Occlusion = tex2DNode51.b;
			o.Alpha = 1;
			float2 uv_Opacity220 = i.uv_texcoord;
			float4 tex2DNode220 = tex2D( _Opacity, uv_Opacity220 );
			float lerpResult234 = lerp( 0.0 , _Neck_1 , tex2DNode220.r);
			float clampResult233 = clamp( lerpResult234 , 0.0 , 1.0 );
			float lerpResult244 = lerp( 1.0 , 0.0 , clampResult233);
			float lerpResult236 = lerp( 0.0 , _Neck_2 , tex2DNode220.g);
			float clampResult235 = clamp( lerpResult236 , 0.0 , 1.0 );
			float lerpResult245 = lerp( lerpResult244 , 0.0 , clampResult235);
			clip( lerpResult245 - _AlphaClip );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19105
Node;AmplifyShaderEditor.CommentaryNode;125;-1923.101,-466.3704;Inherit;False;1343.964;670.8484;Color;6;5;3;6;2;1;4;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;122;-2881.107,2363.864;Inherit;False;605.4539;283.1589;RMA;2;51;52;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;121;-1828.869,2334.521;Inherit;False;1332.985;420.0544;Roughness;6;59;65;63;66;64;56;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;120;-1856.281,2821.682;Inherit;False;725.536;298.7959;Normal;2;72;71;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-1470.944,2412.556;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;64;-732.2343,2400.923;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;63;-1117.126,2406.124;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;218;-1845.106,3156.652;Inherit;False;1567.22;536.5701;Opacity;11;240;245;244;234;219;220;241;236;235;233;332;;1,1,1,1;0;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1001.184,2558.565;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Dary_Palasky_Built-in/Head Shader Built-in;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Custom;0.5;True;True;0;True;Opaque;;AlphaTest;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;;-1;0;True;_AlphaClip;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.CommentaryNode;251;-1922.321,258.534;Inherit;False;1613.884;725.0905;Dirt Mask;12;293;259;258;257;256;255;253;299;300;301;302;303;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;252;-2125.049,1015.425;Inherit;False;1874.45;1246.895;Blood Mask;23;296;295;281;280;279;277;276;275;274;273;272;271;270;267;266;265;264;263;262;261;260;254;335;;1,1,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;255;-1036.577,524.2329;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;256;-848.142,517.1158;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;258;-1650.472,558.0999;Inherit;True;Property;_TextureSample8;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;260;-1077.238,1300.411;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;261;-1264.062,1302.011;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;262;-1075.66,1434.26;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;263;-1262.484,1435.861;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;264;-1070.423,1566.948;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;265;-1257.247,1568.548;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;266;-1067.982,1693.626;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;267;-1254.806,1695.226;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;270;-1847.072,1343.703;Inherit;True;Property;_TextureSample11;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;275;-471.5074,1261.94;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;276;-477.3892,1413.292;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;277;-477.3892,1569.87;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;280;-1534.893,1586.616;Inherit;False;Property;_Blood_Intensity_3;Blood_Intensity_3;8;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;296;-1540.13,1455.632;Inherit;False;Property;_Blood_Intensity_2;Blood_Intensity_2;7;0;Create;True;1;Head_Blood;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;295;-1541.708,1320.079;Inherit;False;Property;_Blood_Intensity_1;Blood_Intensity_1;6;0;Create;True;1;Head_Blood;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;274;-857.3628,1638.685;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;273;-851.0808,1498.906;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;272;-858.934,1335.571;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;299;-1042.198,683.1522;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;300;-849.3159,681.5413;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;302;-682.2146,605.6683;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;257;-478.5576,752.3713;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;303;-491.495,498.3208;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;293;-1343.787,540.174;Inherit;False;Property;_Dirt_Head_Intensity;Dirt_Head_Intensity;12;0;Create;True;0;0;0;False;0;False;0;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;301;-1349.407,699.0934;Inherit;False;Property;_Dirt_Fullbody;Dirt_Fullbody;13;0;Create;True;0;0;0;False;0;False;0;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;51;-2597.254,2417.023;Inherit;True;Property;_TextureSample6;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;52;-2831.107,2413.864;Inherit;True;Property;_RMA;RMA;19;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TexturePropertyNode;259;-1884.326,554.9409;Inherit;True;Property;_Dirt_Mask;Dirt_Mask;20;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.ColorNode;254;-1123.471,1090.054;Inherit;False;Property;_Blood_Color;Blood_Color;4;1;[Header];Create;True;1;_____BLOOD_____;0;0;False;0;False;0.227451,0,0,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;253;-913.2011,330.8209;Inherit;False;Property;_Dirt_Color;Dirt_Color;10;1;[Header];Create;True;1;_____DIRT_____;0;0;False;0;False;0.172549,0.09411765,0.03921569,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;66;-1000.815,2600.249;Inherit;False;Property;_Blood_Roughness;Blood_Roughness;5;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-1370.504,2607.076;Inherit;False;Property;_Dirt_Roughness;Dirt_Roughness;11;0;Create;True;0;0;0;False;0;False;0;0;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;233;-722.6078,3258.241;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;235;-721.0296,3392.089;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;236;-907.8548,3393.69;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;234;-909.4327,3259.84;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;244;-469.7927,3240.608;Inherit;False;3;0;FLOAT;1;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;245;-464.9177,3379.525;Inherit;False;3;0;FLOAT;1;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;71;-1517.322,2871.682;Inherit;True;Property;_TextureSample10;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;241;-1185.501,3413.461;Inherit;False;Property;_Neck_2;Neck_2;15;1;[HideInInspector];Create;True;1;Head_Blood;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;240;-1187.079,3277.108;Inherit;False;Property;_Neck_1;Neck_1;14;2;[HideInInspector];[Header];Create;True;1;_____OPACITY_____;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;332;-1555.106,3593.721;Inherit;False;Property;_AlphaClip;AlphaClip;16;1;[HideInInspector];Create;True;0;0;0;False;0;False;0.99;0.99;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;219;-1780.5,3252.555;Inherit;True;Property;_Opacity;Opacity;22;2;[HideInInspector];[NoScaleOffset];Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerNode;220;-1546.647,3255.714;Inherit;True;Property;_TextureSample12;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;72;-1799.08,2872.892;Inherit;True;Property;_Normal;Normal;18;1;[NoScaleOffset];Create;True;1;_____TEXTURES_____;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.ToggleSwitchNode;333;-2019.525,2437.448;Inherit;False;Property;_InvertTexture;InvertTexture;2;0;Create;True;0;0;0;False;0;False;0;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;334;-2199.717,2492.148;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;279;-480.3544,1709.596;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;281;-1532.451,1714.69;Inherit;False;Property;_Blood_Fullbody;Blood_Fullbody;9;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;335;-1590.308,1802.698;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;4;-1491.365,-379.318;Inherit;False;Property;_Human_Skin_Color;Human_Skin_Color;0;1;[Header];Create;True;1;_____SKIN_____;0;0;False;0;False;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-1564.846,-178.8205;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;2;-1809.348,-180.0428;Inherit;True;Property;_Base_Color;Base_Color;17;2;[Header];[NoScaleOffset];Create;True;1;_____TEXTURES_____;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;6;-1044.482,6.107661;Inherit;False;Constant;_Float0;Float 0;1;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-1035.402,-108.0219;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;5;-802.3309,-175.5963;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;271;-2080.924,1340.544;Inherit;True;Property;_Blood_Mask;Blood_Mask;21;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.LerpOp;336;-2113.697,2709.35;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;59;-1783.58,2601.017;Inherit;False;Property;_Skin_Roughness;Skin_Roughness;1;0;Create;True;0;0;0;False;0;False;0.8;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;337;-2387.297,2779.75;Inherit;False;Property;_Metallic;Metallic;3;0;Create;True;0;0;0;False;0;False;0.8;0;0;1;0;1;FLOAT;0
WireConnection;56;0;333;0
WireConnection;56;1;59;0
WireConnection;64;0;63;0
WireConnection;64;1;66;0
WireConnection;64;2;274;0
WireConnection;63;0;56;0
WireConnection;63;1;65;0
WireConnection;63;2;302;0
WireConnection;0;0;279;0
WireConnection;0;1;71;0
WireConnection;0;3;336;0
WireConnection;0;4;64;0
WireConnection;0;5;51;3
WireConnection;0;10;245;0
WireConnection;255;1;293;0
WireConnection;255;2;258;1
WireConnection;256;0;255;0
WireConnection;258;0;259;0
WireConnection;260;0;261;0
WireConnection;261;1;295;0
WireConnection;261;2;270;1
WireConnection;262;0;263;0
WireConnection;263;1;296;0
WireConnection;263;2;270;2
WireConnection;264;0;265;0
WireConnection;265;1;280;0
WireConnection;265;2;270;3
WireConnection;266;0;267;0
WireConnection;267;1;281;0
WireConnection;267;2;335;0
WireConnection;270;0;271;0
WireConnection;275;0;257;0
WireConnection;275;1;254;0
WireConnection;275;2;260;0
WireConnection;276;0;275;0
WireConnection;276;1;254;0
WireConnection;276;2;262;0
WireConnection;277;0;276;0
WireConnection;277;1;254;0
WireConnection;277;2;264;0
WireConnection;274;0;273;0
WireConnection;274;2;266;0
WireConnection;273;0;272;0
WireConnection;273;2;264;0
WireConnection;272;0;260;0
WireConnection;272;2;262;0
WireConnection;299;1;301;0
WireConnection;299;2;258;2
WireConnection;300;0;299;0
WireConnection;302;0;256;0
WireConnection;302;2;300;0
WireConnection;257;0;303;0
WireConnection;257;1;253;0
WireConnection;257;2;300;0
WireConnection;303;0;5;0
WireConnection;303;1;253;0
WireConnection;303;2;256;0
WireConnection;51;0;52;0
WireConnection;233;0;234;0
WireConnection;235;0;236;0
WireConnection;236;1;241;0
WireConnection;236;2;220;2
WireConnection;234;1;240;0
WireConnection;234;2;220;1
WireConnection;244;2;233;0
WireConnection;245;0;244;0
WireConnection;245;2;235;0
WireConnection;71;0;72;0
WireConnection;220;0;219;0
WireConnection;333;0;51;1
WireConnection;333;1;334;0
WireConnection;334;0;51;1
WireConnection;279;0;277;0
WireConnection;279;1;254;0
WireConnection;279;2;266;0
WireConnection;335;0;270;4
WireConnection;1;0;2;0
WireConnection;3;0;4;0
WireConnection;3;1;1;0
WireConnection;5;0;1;0
WireConnection;5;1;3;0
WireConnection;5;2;6;0
WireConnection;336;1;51;4
WireConnection;336;2;337;0
ASEEND*/
//CHKSM=D630B52E51F113D00A6B6102AC2217AAEAD64493