Shader "Custom/BillboardingShader"
{
     Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _FlipX("_FlipX", Float) = 0.0
    }
   
    SubShader
    {
        Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "DisableBatching" = "True" }

        ZWrite Off
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float _FlipX;

            struct appdata
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            const float3 vect3Zero = float3(0.0, 0.0, 0.0);

            sampler2D _MainTex;

            v2f vert(appdata v)
            {
                v2f o;
                float flip = (_FlipX == 1.0) ? -1.0 : 1.0;

                float4 offset = float4(v.pos.x * flip, v.pos.y, 0.0, 0.0);

                float4 BillboardVertex = mul(UNITY_MATRIX_P,
                    mul(UNITY_MATRIX_MV, float4(0.0, 0.0, 0.0, 1.0))
                    + offset
                    * float4(
                        length(unity_ObjectToWorld._m00_m10_m20),
                        length(unity_ObjectToWorld._m01_m11_m21), 1.0, 1.0));

                o.pos = BillboardVertex;
                o.uv = v.uv;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // Don't need to do anything special, just render the texture
                return tex2D(_MainTex, uv);
            }
            ENDCG
        }
    }
}
