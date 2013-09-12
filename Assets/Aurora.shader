Shader "Custom/Aurora" {
	Properties {
        _UAnim ("U Anim Params", Vector) = (1, 1, 1, 1)
        _VAnim ("V Anim Params", Vector) = (1, 1, 1, 1)
	}
	SubShader {
        pass {
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

struct v2f {
    float4 pos : SV_POSITION;
    float2 uv : TEXCOORD0;
};

float4 _UAnim;
float4 _VAnim;
float4 _MainTex_ST;

v2f vert (appdata_base v)
{
    v2f o;
    o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
    o.uv = v.texcoord.xy;
    return o;
}

float wave (float x, float4 param)
{
    x = sin(x * param.x + _Time.y * param.y);
    x = sin(x * param.z + _Time.y * param.w);
    return (1.0f + x) * 0.5f;
}

half4 frag (v2f i) : COLOR
{
    float x = wave(i.uv.x, _UAnim) * wave(i.uv.y, _VAnim);
    return half4 (x, x, x, 1);
}
ENDCG
        }
	} 
	FallBack off
}