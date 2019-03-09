// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SFBayStudios/SFB_DemonessBlend"
{
	Properties
	{
		_TextureBlend("Texture Blend", Range( 0 , 1)) = 0
		_HueShift("Hue Shift", Range( -1 , 1)) = 0
		_Saturation("Saturation", Range( 0 , 1)) = 1
		_HeightMultiplier("Height Multiplier", Range( 0 , 1)) = 1
		_EmissionMultplier("Emission Multplier", Range( 0 , 10)) = 1
		[Toggle]_PulseEmission("Pulse Emission", Float) = 0
		_AlbedoOpacity("AlbedoOpacity", 2D) = "white" {}
		_Normal("Normal", 2D) = "bump" {}
		_MetalAOHeightRough("MetalAOHeightRough", 2D) = "white" {}
		_Emission("Emission", 2D) = "white" {}
		_AlbedoOpacity2("AlbedoOpacity2", 2D) = "white" {}
		_Normal2("Normal2", 2D) = "bump" {}
		_MetalAOHeightRough2("MetalAOHeightRough2", 2D) = "white" {}
		_Emission2("Emission2", 2D) = "white" {}
		[Toggle(_USEEMISSION_ON)] _UseEmission("Use Emission?", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma shader_feature _USEEMISSION_ON
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform sampler2D _Normal2;
		uniform float4 _Normal2_ST;
		uniform float _TextureBlend;
		uniform sampler2D _MetalAOHeightRough2;
		uniform float4 _MetalAOHeightRough2_ST;
		uniform float _HeightMultiplier;
		uniform float _HueShift;
		uniform sampler2D _AlbedoOpacity;
		uniform float4 _AlbedoOpacity_ST;
		uniform sampler2D _AlbedoOpacity2;
		uniform float4 _AlbedoOpacity2_ST;
		uniform float _Saturation;
		uniform float _PulseEmission;
		uniform sampler2D _Emission;
		uniform float4 _Emission_ST;
		uniform sampler2D _Emission2;
		uniform float4 _Emission2_ST;
		uniform float _EmissionMultplier;
		uniform sampler2D _MetalAOHeightRough;
		uniform float4 _MetalAOHeightRough_ST;


		float3 HSVToRGB( float3 c )
		{
			float4 K = float4( 1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0 );
			float3 p = abs( frac( c.xxx + K.xyz ) * 6.0 - K.www );
			return c.z * lerp( K.xxx, saturate( p - K.xxx ), c.y );
		}


		float3 RGBToHSV(float3 c)
		{
			float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
			float4 p = lerp( float4( c.bg, K.wz ), float4( c.gb, K.xy ), step( c.b, c.g ) );
			float4 q = lerp( float4( p.xyw, c.r ), float4( c.r, p.yzx ), step( p.x, c.r ) );
			float d = q.x - min( q.w, q.y );
			float e = 1.0e-10;
			return float3( abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			float2 uv_Normal2 = i.uv_texcoord * _Normal2_ST.xy + _Normal2_ST.zw;
			float2 uv_MetalAOHeightRough2 = i.uv_texcoord * _MetalAOHeightRough2_ST.xy + _MetalAOHeightRough2_ST.zw;
			float4 tex2DNode5 = tex2D( _MetalAOHeightRough2, uv_MetalAOHeightRough2 );
			float clampResult27 = clamp( ( _TextureBlend + ( _TextureBlend * ( tex2DNode5.b * _HeightMultiplier ) ) ) , 0.0 , 1.0 );
			float3 lerpResult12 = lerp( UnpackNormal( tex2D( _Normal, uv_Normal ) ) , UnpackNormal( tex2D( _Normal2, uv_Normal2 ) ) , clampResult27);
			float3 normalizeResult22 = normalize( lerpResult12 );
			o.Normal = normalizeResult22;
			float2 uv_AlbedoOpacity = i.uv_texcoord * _AlbedoOpacity_ST.xy + _AlbedoOpacity_ST.zw;
			float2 uv_AlbedoOpacity2 = i.uv_texcoord * _AlbedoOpacity2_ST.xy + _AlbedoOpacity2_ST.zw;
			float4 lerpResult10 = lerp( tex2D( _AlbedoOpacity, uv_AlbedoOpacity ) , tex2D( _AlbedoOpacity2, uv_AlbedoOpacity2 ) , clampResult27);
			float3 hsvTorgb49 = RGBToHSV( lerpResult10.rgb );
			float3 hsvTorgb53 = HSVToRGB( float3(( _HueShift + hsvTorgb49.x ),( _Saturation * hsvTorgb49.y ),hsvTorgb49.z) );
			o.Albedo = hsvTorgb53;
			float4 temp_cast_1 = 0;
			float2 uv_Emission = i.uv_texcoord * _Emission_ST.xy + _Emission_ST.zw;
			float2 uv_Emission2 = i.uv_texcoord * _Emission2_ST.xy + _Emission2_ST.zw;
			float4 lerpResult19 = lerp( tex2D( _Emission, uv_Emission ) , tex2D( _Emission2, uv_Emission2 ) , clampResult27);
			float temp_output_40_0 = ( ( 1 + _SinTime.w ) / 2 );
			#ifdef _USEEMISSION_ON
				float4 staticSwitch46 = lerp(lerpResult19,( ( _EmissionMultplier * temp_output_40_0 ) * lerpResult19 ),_PulseEmission);
			#else
				float4 staticSwitch46 = temp_cast_1;
			#endif
			o.Emission = staticSwitch46.rgb;
			float2 uv_MetalAOHeightRough = i.uv_texcoord * _MetalAOHeightRough_ST.xy + _MetalAOHeightRough_ST.zw;
			float4 tex2DNode3 = tex2D( _MetalAOHeightRough, uv_MetalAOHeightRough );
			float lerpResult13 = lerp( tex2DNode3.r , tex2DNode5.r , clampResult27);
			o.Metallic = lerpResult13;
			float lerpResult16 = lerp( tex2DNode3.a , tex2DNode5.a , clampResult27);
			o.Smoothness = lerpResult16;
			float lerpResult14 = lerp( tex2DNode3.g , tex2DNode5.g , clampResult27);
			o.Occlusion = lerpResult14;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16100
5;94;1226;643;1208.383;1110.724;2.296402;True;True
Node;AmplifyShaderEditor.RangedFloatNode;29;-768.1254,552.3113;Float;False;Property;_HeightMultiplier;Height Multiplier;3;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;5;-780.6876,1148.541;Float;True;Property;_MetalAOHeightRough2;MetalAOHeightRough2;13;0;Create;True;0;0;False;0;None;1adb5e8debea54237b3dea680fe59401;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;7;-784.46,463.8734;Float;False;Property;_TextureBlend;Texture Blend;0;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-331.7914,695.5187;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;38;-1544.418,977.4576;Float;False;Constant;_Int0;Int 0;12;0;Create;True;0;0;False;0;1;0;0;1;INT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-307.1769,529.9353;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinTimeNode;37;-1528.756,1069.201;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.IntNode;41;-1208.777,1125.14;Float;False;Constant;_Int1;Int 1;12;0;Create;True;0;0;False;0;2;0;0;1;INT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;23;-328.7618,359.877;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;39;-1296.043,1049.061;Float;False;2;2;0;INT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;18;-788.8021,1357.018;Float;True;Property;_Emission2;Emission2;14;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;40;-1043.192,1011.022;Float;False;2;0;FLOAT;0;False;1;INT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-1336.48,485.1831;Float;False;Property;_EmissionMultplier;Emission Multplier;4;0;Create;True;0;0;False;0;1;1;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;17;-784.7225,239.8038;Float;True;Property;_Emission;Emission;10;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;27;-170.6826,223.3824;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-783.5833,-385.6098;Float;True;Property;_AlbedoOpacity;AlbedoOpacity;7;0;Create;True;0;0;False;0;None;b3c7df93b2fbe4355ab5d4e7816274b1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;4;-781.0478,730.7184;Float;True;Property;_AlbedoOpacity2;AlbedoOpacity2;11;0;Create;True;0;0;False;0;None;83382eda3a3184296a7e151a276e1fbc;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;19;108.4915,681.194;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;10;91.4157,-85.86859;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-973.9844,661.9544;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;280.1472,836.4879;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;6;-785.1629,927.8631;Float;True;Property;_Normal2;Normal2;12;0;Create;True;0;0;False;0;None;95db230ab4a574c50954d41848166b7a;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;50;5.804572,-562.2764;Float;False;Property;_Saturation;Saturation;2;0;Create;True;0;0;False;0;1;0.6317781;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-785.4608,-188.4651;Float;True;Property;_Normal;Normal;8;0;Create;True;0;0;False;0;None;c1024ec2548d947d0b6234f5c3602df4;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;48;142.9059,-802.8046;Float;False;Property;_HueShift;Hue Shift;1;0;Create;True;0;0;False;0;0;-0.2923495;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RGBToHSVNode;49;142.9057,-290.4766;Float;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ToggleSwitchNode;44;319.5164,619.4397;Float;False;Property;_PulseEmission;Pulse Emission;5;0;Create;True;0;0;False;0;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;3;-785.4608,14.31195;Float;True;Property;_MetalAOHeightRough;MetalAOHeightRough;9;0;Create;True;0;0;False;0;None;a40f847fb32f34b60a1c8283d8876375;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;12;93.29301,38.05075;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;284.8177,-432.3914;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;52;457.9984,-692.1615;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;47;172.9095,1070.213;Float;False;Constant;_Int2;Int 2;14;0;Create;True;0;0;False;0;0;0;0;1;INT;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-1584.696,749.2214;Float;False;Property;_TimeSpeed;Time Speed;6;0;Create;True;0;0;False;0;1;1;-5;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;15;98.92572,413.5643;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;16;104.5584,543.1165;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;22;291.057,120.4525;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;14;102.6808,287.7673;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.HSVToRGBNode;53;450.7825,-288.0725;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.LerpOp;13;104.5584,165.7254;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;43;-1566.794,832.0129;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;-1244.578,778.3101;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;46;507.6198,807.7295;Float;False;Property;_UseEmission;Use Emission?;15;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;546.6746,99.22828;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;SFBayStudios/SFB_DemonessBlend;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;28;0;5;3
WireConnection;28;1;29;0
WireConnection;24;0;7;0
WireConnection;24;1;28;0
WireConnection;23;0;7;0
WireConnection;23;1;24;0
WireConnection;39;0;38;0
WireConnection;39;1;37;4
WireConnection;40;0;39;0
WireConnection;40;1;41;0
WireConnection;27;0;23;0
WireConnection;19;0;17;0
WireConnection;19;1;18;0
WireConnection;19;2;27;0
WireConnection;10;0;1;0
WireConnection;10;1;4;0
WireConnection;10;2;27;0
WireConnection;34;0;31;0
WireConnection;34;1;40;0
WireConnection;32;0;34;0
WireConnection;32;1;19;0
WireConnection;49;0;10;0
WireConnection;44;0;19;0
WireConnection;44;1;32;0
WireConnection;12;0;2;0
WireConnection;12;1;6;0
WireConnection;12;2;27;0
WireConnection;51;0;50;0
WireConnection;51;1;49;2
WireConnection;52;0;48;0
WireConnection;52;1;49;1
WireConnection;15;0;3;3
WireConnection;15;1;5;3
WireConnection;15;2;27;0
WireConnection;16;0;3;4
WireConnection;16;1;5;4
WireConnection;16;2;27;0
WireConnection;22;0;12;0
WireConnection;14;0;3;2
WireConnection;14;1;5;2
WireConnection;14;2;27;0
WireConnection;53;0;52;0
WireConnection;53;1;51;0
WireConnection;53;2;49;3
WireConnection;13;0;3;1
WireConnection;13;1;5;1
WireConnection;13;2;27;0
WireConnection;42;0;36;0
WireConnection;42;1;40;0
WireConnection;46;1;47;0
WireConnection;46;0;44;0
WireConnection;0;0;53;0
WireConnection;0;1;22;0
WireConnection;0;2;46;0
WireConnection;0;3;13;0
WireConnection;0;4;16;0
WireConnection;0;5;14;0
ASEEND*/
//CHKSM=4508E1E9CFC7D33A51EA86D84165EA0B748586BF