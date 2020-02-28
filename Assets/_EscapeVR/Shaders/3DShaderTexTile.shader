Shader "Unlit/3DShaderTexTile"
{
    Properties
    {
        _MainTex("Albedo (RGBA)", 2D) = "white" {}
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

            sampler2D _MainTex;
            float4 _MainTex_ST;

            struct vertInput
            {
                float4 pos : POSITION;
                float4 texcoord : TEXCOORD0;
            };

            struct vertOutput
            {
                float4 pos : SV_POSITION;
                float2 texcoord : TEXCOORD0;
            };

            vertOutput vert(vertInput input)
            {
                vertOutput o;
                o.pos = UnityObjectToClipPos(input.pos);
                o.texcoord = TRANSFORM_TEX(input.texcoord, _MainTex);
                return o;
            }

            half4 frag(vertOutput o) : SV_Target
            {
                half4 white = {1.0, 1.0, 1.0, 0.1};
                half4 mainColor = tex2D(_MainTex, o.texcoord);
                if (mainColor.r > 0.0) return white;
                else white.a = 0.0;
                return white;
            }
            ENDCG
        }
    }
        FallBack "Diffuse"
}