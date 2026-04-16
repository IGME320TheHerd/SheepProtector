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

                float4 camPos = float4(UnityObjectToViewPos(vect3Zero).xyz, 1.0);    // UnityObjectToViewPos(pos) is equivalent to mul(UNITY_MATRIX_MV, float4(pos, 1.0)).xyz,
                                                                                    // This gives us the camera's origin in 3D space (the position (0,0,0) in Camera Space)

                float4 viewDir = float4(v.pos.x, v.pos.y, 0.0, 0.0);            // Since w is 0.0, in homogeneous coordinates this represents a vector direction instead of a position

                float4 outPos = mul(UNITY_MATRIX_P, camPos + viewDir);            // Add the camera position and direction, then multiply by UNITY_MATRIX_P to get the new projected vert position

                float4 BillboardVertex = mul(UNITY_MATRIX_P,
                    mul(UNITY_MATRIX_MV, float4(0.0, 0.0, 0.0, 1.0))
                    + float4(v.pos.x, v.pos.y, 0.0, 0.0)
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

                if (_FlipX == 1.0)
                {
                    uv.x = 1.0 - uv.x;
                }

                // Don't need to do anything special, just render the texture
                return tex2D(_MainTex, uv);
            }
            ENDCG
        }
    }
}
