/// @file
/// @author Ondrej Mocny http://www.hardwire.cz
/// See LICENSE.txt for license information.
///
/// Simply textures the mesh using the diffuse method.
///
/// Based on Transparent/Cutout/Diffuse

Shader "Shinigami/Terrain/FillNormal" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_Normal ("Normal", 2D) = "normal" {}

	_SpecBrightness ("Spec Brightness", Range(0.0,1.0)) = 0
	_RimPower ("Rim Power", Range (0.1,40)) = 1
}

SubShader {
	Tags {
		"Queue"="Transparent"
		"IgnoreProjector"="True"
		"RenderType"="Opaque"
	}
	ZWrite On Blend One OneMinusSrcAlpha
	Lighting On
CGPROGRAM
#pragma surface surf ColoredSpecular alpha 

sampler2D _MainTex,_Normal;
fixed4 _Color;
float _SpecBrightness;
float _RimPower;

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

struct Input {
	float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	o.Normal = UnpackNormal(tex2D(_Normal, IN.uv_MainTex));
	o.Albedo = c.rgb;
	o.Gloss = _RimPower;
	o.Specular = _SpecBrightness;
	o.Alpha = c.a;
}
ENDCG
}

Fallback "Bumped Specular"
}
