Shader "Unlit/3DShader"
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
            Cull off
            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"
                #pragma target 3.0

                sampler3D _MainTex3D;
                float _SOX, _SOY, _SOZ, _sep, _wid, _len;

                struct vertInput
                {
                    float4 pos : POSITION;
                    float4 texcoord : TEXCOORD0;
                };

                struct vertOutput
                {
                    float4 pos : SV_POSITION;
                    float4 texcoord : TEXCOORD0;
                };

                vertOutput vert(vertInput input)
                {
                    vertOutput o;
                    o.pos = UnityObjectToClipPos(input.pos);
                    o.texcoord = input.texcoord;
                    return o;
                }

                half4 frag(vertOutput o) : SV_Target
                {
                    //half4 mainColor = tex3D(_MainTex3D, o.texcoord);
                    //return mainColor;
                    half4 mainColor = {1.0, 1.0, 1.0, 0.0};
                    float x = _SOX * o.texcoord.x;
                    float y = _SOY * o.texcoord.y;
                    float z = _SOZ * o.texcoord.z;
                    if ((abs((x + _sep / 2.0f) % _sep - _sep / 2.0f) < _len &&
                            abs((y + _sep / 2.0f) % _sep - _sep / 2.0f) < _wid &&
                            abs((z + _sep / 2.0f) % _sep - _sep / 2.0f) < _wid) ||

                            (abs((x + _sep / 2.0f) % _sep - _sep / 2.0f) < _wid &&
                            abs((y + _sep / 2.0f) % _sep - _sep / 2.0f) < _len &&
                            abs((z + _sep / 2.0f) % _sep - _sep / 2.0f) < _wid) ||

                            (abs((x + _sep / 2.0f) % _sep - _sep / 2.0f) < _wid &&
                            abs((y + _sep / 2.0f) % _sep - _sep / 2.0f) < _wid &&
                            abs((z + _sep / 2.0f) % _sep - _sep / 2.0f) < _len))
                    {
                        mainColor.a = 0.1;
                    }
                    //if (o.texcoord.x < 0.75 && o.texcoord.x > 0.25 && o.texcoord.y < 0.75 && o.texcoord.y > 0.25) { mainColor.a = 1.0; mainColor.r = 0.0; }
                    return mainColor;
                }
                ENDCG
            }
        }
            FallBack "Diffuse"
}
