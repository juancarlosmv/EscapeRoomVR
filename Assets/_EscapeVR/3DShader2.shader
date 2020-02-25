// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/3DShader2"
{
    Properties
    {
        _MainTex3D("Albedo (RGBA)", 3D) = "white" {}
        _SOX("Size x", Float) = 0.0
        _SOY("Size y", Float) = 0.0
        _SOZ("Size z", Float) = 0.0
        _sep("Grid size", Float) = 0.2
        _wid("Line width", Float) = 0.01
        _len("Line length", Float) = 0.04
    }
        SubShader
        {
            Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }
            LOD 200
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off
            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"
                #pragma target 3.0

                sampler3D _MainTex3D;
                float _SOX, _SOY, _SOZ, _sep, _wid, _len;

                struct v2f {
                    float4 pos : SV_POSITION; // Clip space
                    float3 wPos : TEXCOORD1; // World position
                };

                // Vertex function
                v2f vert(appdata_full v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.wPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                    return o;
                }

                struct vertOutput
                {
                    float4 pos : SV_POSITION;
                    float4 texcoord : TEXCOORD0;
                };

                fixed4 frag(v2f i) : SV_Target
                {
                }
                ENDCG
            }
        }
            FallBack "Diffuse"
}
