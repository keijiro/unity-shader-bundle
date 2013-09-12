Shader "Custom/Aurora" {
	Properties {
        _UAnimR ("U Anim Params (R)", Vector) = (1, 1, 1, 1)
        _VAnimR ("V Anim Params (R)", Vector) = (1, 1, 1, 1)
        _UAnimG ("U Anim Params (G)", Vector) = (1, 1, 1, 1)
        _VAnimG ("V Anim Params (G)", Vector) = (1, 1, 1, 1)
        _UAnimB ("U Anim Params (B)", Vector) = (1, 1, 1, 1)
        _VAnimB ("V Anim Params (B)", Vector) = (1, 1, 1, 1)
	}
	SubShader {
        pass {
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

struct v2f {
    float4 pos : SV_POSITION;
    float4 col : COLOR0;
};

float4 _UAnimR;
float4 _VAnimR;
float4 _UAnimG;
float4 _VAnimG;
float4 _UAnimB;
float4 _VAnimB;
float4 _MainTex_ST;

float wave (float x, float4 param)
{
    x = sin(x * param.x + _Time.y * param.y);
    x = sin(x * param.z + _Time.y * param.w);
    return (1.0f + x) * 0.5f;
}

v2f vert (appdata_base v)
{
	float x = v.texcoord.x;
	float y = v.texcoord.y;

    float r = wave(x, _UAnimR) * wave(y, _VAnimR);
    float g = wave(x, _UAnimG) * wave(y, _VAnimG);
    float b = wave(x, _UAnimB) * wave(y, _VAnimB);

    v2f o;
    o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
    o.col = float4 (r, g, b, 1);
    return o;
}

half4 frag (v2f i) : COLOR
{
    return i.col;
}
ENDCG
        }
	} 
	FallBack off
}