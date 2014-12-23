/// Smoothly combines up to 4 textures together (_Splat0 - _Splat3). The mix is directed by the _Control texture where
/// its R, G, B and A components correspond to the splat textures and function as weight for the sum of the colors.
/// The alpha is faded to zero when the V coordinate exceeds the fade threshold of the textures. This helps the terrain
/// surface blend into the fill texture.
/// The SplatParams vector consist of:
///  - world width
///  - world height
///  - fixed angle (boolean)
///  - fade threshold
/// The shader takes per/vertex data in uv, uv2 and colors.
/// UV:
///  - distance along the surface curve
///  - control texture U coordinate
/// UV2:
///  - vertex X position in the local space
///  - vertex Y position in the local space
/// COLOR:
///  - V coordinate (1 when on directly on the curve, 0 otherwise)
///
/// Based on Hidden/TerrainEngine/Splatmap/Lightmap-FirstPass

Shader "Shinigami/Terrain/Curve Normal" {
Properties {
	_ControlSize ("Control Size", Float) = 1.0
	_Control ("Control (RGBA)", 2D) = "red" {}

	_Splat0 ("Layer 0 (R)", 2D) = "white" {}
	_Normal0 ("Normal 0 (R)", 2D) = "normal" {}
	_SplatParams0 ("Splat Params 0", Vector) = (1, 1, 0, 0)

	_Splat1 ("Layer 1 (G)", 2D) = "white" {}
	_Normal1 ("Normal 1 (G)", 2D) = "normal" {}
	_SplatParams1 ("Splat Params 1", Vector) = (1, 1, 0, 0)

	_Splat2 ("Layer 2 (B)", 2D) = "white" {}
	_Normal2 ("Normal 2 (B)", 2D) = "normal" {}
	_SplatParams2 ("Splat Params 2", Vector) = (1, 1, 0, 0)

	_Splat3 ("Layer 3 (A)", 2D) = "white" {}
	_Normal3 ("Normal 3 (A)", 2D) = "normal" {}
	_SplatParams3 ("Splat Params 3", Vector) = (1, 1, 0, 0)

	// used in fallback on old cards
	_MainTex ("BaseMap (RGB)", 2D) = "white" {}
	_Color ("Main Color", Color) = (1,1,1,1)
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
#pragma surface surf ColoredSpecular alpha vertex:vert
#pragma target 3.0
#include "UnityCG.cginc"    // Get standard Unity constants

float _ControlSize;
sampler2D _Control;
sampler2D _Splat0,_Splat1,_Splat2,_Splat3,_Normal0,_Normal1,_Normal2,_Normal3;
float4 _SplatParams0, _SplatParams1, _SplatParams2, _SplatParams3;

struct Input {
	// custom data
	float3 Control_uv;
	float4 Splat01_uv;
	float4 Splat23_uv;
	float4 color : COLOR;
};

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
	o.Control_uv.x = (v.texcoord.y +0.5) / _ControlSize;
	o.Control_uv.y = 0;
	o.Control_uv.z = v.color.x;
	o.Splat01_uv.xy = _SplatParams0.z ? (v.texcoord1.xy / _SplatParams0.xy) : float2(v.texcoord.x  / _SplatParams0.x, v.color.x);
	o.Splat01_uv.zw = _SplatParams1.z ? (v.texcoord1.xy / _SplatParams1.xy) : float2(v.texcoord.x  / _SplatParams1.x, v.color.x);
	o.Splat23_uv.xy = _SplatParams2.z ? (v.texcoord1.xy / _SplatParams2.xy) : float2(v.texcoord.x  / _SplatParams2.x, v.color.x);
	o.Splat23_uv.zw = _SplatParams3.z ? (v.texcoord1.xy / _SplatParams3.xy) : float2(v.texcoord.x  / _SplatParams3.x, v.color.x);
}

void surf(Input IN, inout SurfaceOutput o)
{
	// need to offset U by half of the unit to center the texture around the curve nodes
	half4 splatControl = tex2D (_Control, IN.Control_uv.xy);
	
	
	half4 splat1 = tex2D (_Splat0, IN.Splat01_uv.xy).rgba;
	half4 splat2 = tex2D (_Splat1, IN.Splat01_uv.zw).rgba;
	half4 splat3 = tex2D (_Splat2, IN.Splat23_uv.xy).rgba;
	half4 splat4 = tex2D (_Splat3, IN.Splat23_uv.zw).rgba;

	half4 normal1 = tex2D (_Normal0, IN.Splat01_uv.xy).rgba;
	half4 normal2 = tex2D (_Normal1, IN.Splat01_uv.zw).rgba;
	half4 normal3 = tex2D (_Normal2, IN.Splat23_uv.xy).rgba;
	half4 normal4 = tex2D (_Normal3, IN.Splat23_uv.zw).rgba;

	o.Gloss = IN.color.b*100;
	o.Specular = IN.color.g;

	// mix the color
	half4 color = half4(0, 0, 0,0);
	color += splatControl.r * splat1 * splat1.a;
	color += splatControl.g * splat2 * splat2.a;
	color += splatControl.b * splat3 * splat3.a;
	color += splatControl.a * splat4 * splat4.a;

	half4 normal = half4(0,0,0,0);
	normal += splatControl.r * normal1;
	normal += splatControl.g * normal2;
	normal += splatControl.b * normal3;
	normal += splatControl.a * normal4;
	
	// set the mixed color
	o.Albedo = color.rgb;
	o.Normal = UnpackNormal(normal);
	
	
	// mix the alpha coef for fading out based on the textures we have
	float alphaThreshold = 0;
	alphaThreshold += splatControl.r * _SplatParams0.w;
	alphaThreshold += splatControl.g * _SplatParams1.w;
	alphaThreshold += splatControl.b * _SplatParams2.w;
	alphaThreshold += splatControl.a * _SplatParams3.w;
	
	// compute alpha using the threshold
	half alpha = splatControl.r* splat1.a + splatControl.g* splat2.a + splatControl.b* splat3.a + splatControl.a* splat4.a;
	alpha *= saturate(IN.Control_uv.z / alphaThreshold);
	o.Alpha = alpha;
}

ENDCG  
}

// Fallback to Diffuse
Fallback "Bumped Specular"
}
