// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

/// The shader renders only single bush of grass waving it according to the parameters.
/// The wave speed is the same for all bushes but it is offset based on the position of grass.
/// Custom data are passed through the color bindings:
///  - grass ID
///  - wave amplitude / 10
///  - wave offset / 10
///  - nothing (0)
/// Currently the shader supports up to 4 textures but this could be extended by adding more texture parameters.

Shader "Shinigami/Terrain/Grass Normal" {
Properties {
	_Grass0 ("Grass 0 (R)", 2D) = "white" {}
	_Grass1 ("Grass 1 (G)", 2D) = "white" {}
	_Grass2 ("Grass 2 (B)", 2D) = "white" {}
	_Grass3 ("Grass 3 (A)", 2D) = "white" {}

	_Normal0 ("Normal 0 (R)", 2D) = "normal" {}
	_Normal1 ("Normal 1 (G)", 2D) = "normal" {}
	_Normal2 ("Normal 2 (B)", 2D) = "normal" {}
	_Normal3 ("Normal 3 (A)", 2D) = "normal" {}
	_WaveFrequency ("WaveFrequency", Float) = 1
	
	_SpecBrightness ("Spec Brightness", Vector) = (0, 0, 0, 0)
	_RimPower ("Rim Power", Vector) = (1, 1, 1, 1)

	// used in fallback on old cards
	_MainTex ("Fallback Texture (RGB)", 2D) = "white" {}
	_Color ("Fallback Color", Color) = (1,1,1,1)

	_Cutoff ("Alpha Cut-Off Threshold", Vector) = (0.5, 0.5, 0.5, 0.5)
}
	
SubShader {
	Tags {
		"Queue" = "Transparent"
		"IgnoreProjector"="True"
		"RenderType" = "TransparentCutout"
	}
	ZWrite On Blend One OneMinusSrcAlpha
	Lighting On
	
CGPROGRAM
#pragma exclude_renderers gles
#pragma surface surf ColoredSpecular  alpha vertex:vert
#pragma target 3.0
#include "UnityCG.cginc"    // Get standard Unity constants
struct Input {
	float2 uv_Grass0 : TEXCOORD0;
	float4 color : COLOR;
};

sampler2D _Grass0, _Grass1, _Grass2, _Grass3,_Normal0,_Normal1,_Normal2,_Normal3;
float _WaveFrequency;
uniform float3 _obstacle;
uniform float _affectDist, _bendAmount;
float4 _SpecBrightness;
float4 _RimPower;

float round(float x)
{
	return sign(x)*floor(abs(x)+0.5);
}

inline half4 LightingColoredSpecular (SurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
{
  half NdotL = max (0, dot (s.Normal, normalize(lightDir)));
  half diff = NdotL;
  //half diff = pow (NdotL * 0.5 + 0.5, 2);

  half specCol = pow (NdotL, s.Gloss) * s.Specular;

  half4 c;
  c.rgb = (s.Albedo  * diff + specCol*s.Alpha* 2)* _LightColor0.rgb*atten;
  c.a = s.Alpha;
  return c;
}

void vert(inout appdata_full v, out Input o)
{
	float2 worldPos = mul ((float4x4)unity_ObjectToWorld, v.vertex).xy;

	float2 bendDir = normalize (worldPos.xy - _obstacle.xy);//direction of obstacle bend
    float distMulti = (_affectDist-min(_affectDist,distance(worldPos.xy,_obstacle.xy)))/_affectDist; //distance falloff


	float2 waveDir = v.texcoord1.xy;
	float waveAmplitude = v.color.y * 10;
	float offset = v.color.z * 10;

	float wind = waveAmplitude * sin(_WaveFrequency * 2 * 3.14 * _Time.y + offset);
	v.vertex.xy += wind * (v.texcoord.y > 0.5) * waveDir;
	v.vertex.xy += bendDir * distMulti * _bendAmount*(v.texcoord.y > 0.5) * waveDir;
}

void surf (Input IN, inout SurfaceOutput o)
{
	float2 uv = IN.uv_Grass0;
	float type = round(IN.color.x * 100);
	
	half Rim = 0;
	Rim += (type==0) * _RimPower.r;
	Rim += (type==1) * _RimPower.g;
	Rim += (type==2) * _RimPower.b;
	Rim += (type==3) * _RimPower.a;
	o.Gloss = Rim;

	half Spec = 0;
	Spec += (type==0) * _SpecBrightness.r;
	Spec += (type==1) * _SpecBrightness.g;
	Spec += (type==2) * _SpecBrightness.b;
	Spec += (type==3) * _SpecBrightness.a;
	o.Specular = Spec;

	half4 color = half4(0, 0, 0, 0);
	color += (type==0) * tex2D(_Grass0, uv).rgba;
	color += (type==1) * tex2D(_Grass1, uv).rgba;
	color += (type==2) * tex2D(_Grass2, uv).rgba;
	color += (type==3) * tex2D(_Grass3, uv).rgba;

	half4 normal = half4(0, 0, 0, 0);
	normal += (type==0) * tex2D(_Normal0, uv).rgba;
	normal += (type==1) * tex2D(_Normal1, uv).rgba;
	normal += (type==2) * tex2D(_Normal2, uv).rgba;
	normal += (type==3) * tex2D(_Normal3, uv).rgba;
	o.Normal = UnpackNormal(normal);
	o.Albedo = color.rgb;
	o.Alpha = color.a * IN.color.a;
}

ENDCG  
}

// Fallback to Diffuse
Fallback "Bumped Specular"
}
