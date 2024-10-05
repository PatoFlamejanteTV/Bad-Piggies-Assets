Shader "e2d/Curve" {
	Properties {
		_ControlSize ("Control Size", Float) = 1
		_InvControlSize ("Inv Control Size", Float) = 1
		_InvControlSizeHalf ("Half of Inv Control Size", Float) = 0.5
		_Control ("Control (RGBA)", 2D) = "red" {}
		_Splat0 ("Layer 0 (R)", 2D) = "white" {}
		_SplatParams0 ("Splat Params 0", Vector) = (1,1,0,0)
		_Splat1 ("Layer 1 (G)", 2D) = "white" {}
		_SplatParams1 ("Splat Params 1", Vector) = (1,1,0,0)
		_MainTex ("BaseMap (RGB)", 2D) = "white" {}
		_Color ("Main Color", Vector) = (1,1,1,1)
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "False" "QUEUE" = "Transparent" "RenderType" = "Opaque" }
		Pass {
			Tags { "IGNOREPROJECTOR" = "False" "QUEUE" = "Transparent" "RenderType" = "Opaque" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			GpuProgramID 56035
			Program "vp" {
				SubProgram "d3d11 " {
					"vs_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					layout(std140) uniform VGlobals {
						vec4 unused_0_0[2];
						float _InvControlSize;
						float _InvControlSizeHalf;
						vec4 _SplatParams0;
					};
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						vec4 unused_1_1[7];
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_2_0[17];
						mat4x4 unity_MatrixVP;
						vec4 unused_2_2[2];
					};
					in  vec4 in_COLOR0;
					in  vec4 in_POSITION0;
					in  vec2 in_TEXCOORD0;
					out vec2 vs_TEXCOORD0;
					out vec2 vs_TEXTCOORD1;
					vec4 u_xlat0;
					vec4 u_xlat1;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = u_xlat0 + unity_ObjectToWorld[3];
					    u_xlat1 = u_xlat0.yyyy * unity_MatrixVP[1];
					    u_xlat1 = unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    gl_Position = unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
					    vs_TEXCOORD0.x = in_TEXCOORD0.y * _InvControlSize + _InvControlSizeHalf;
					    vs_TEXTCOORD1.x = in_TEXCOORD0.x * _SplatParams0.x;
					    vs_TEXCOORD0.y = 0.0;
					    vs_TEXTCOORD1.y = in_COLOR0.x;
					    return;
					}"
				}
			}
			Program "fp" {
				SubProgram "d3d11 " {
					"ps_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					uniform  sampler2D _Splat1;
					uniform  sampler2D _Control;
					uniform  sampler2D _Splat0;
					in  vec2 vs_TEXCOORD0;
					in  vec2 vs_TEXTCOORD1;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec3 u_xlat3;
					void main()
					{
					    u_xlat0 = texture(_Control, vs_TEXCOORD0.xy);
					    u_xlat0.x = floor(u_xlat0.y);
					    u_xlat1 = texture(_Splat1, vs_TEXTCOORD1.xy);
					    u_xlat2 = texture(_Splat0, vs_TEXTCOORD1.xy);
					    u_xlat3.xyz = u_xlat1.xyz + (-u_xlat2.xyz);
					    SV_Target0.xyz = u_xlat3.xyz * u_xlat0.xxx + u_xlat2.xyz;
					    SV_Target0.w = u_xlat2.w;
					    return;
					}"
				}
			}
		}
	}
	Fallback "VertexLit"
}