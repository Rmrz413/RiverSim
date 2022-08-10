Shader "Custom/TerrainShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        [NoScaleOffset] _StateTex("State", 2D) = "black" {}
    }
    SubShader
    {
        Tags { "Queue"="Geometry"  "RenderType"="Opaque"  "IgnoreProjector"="True"}

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"            

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _StateTex;

            #define WATER_HEIGHT(s) (s.g)
			#define TERRAIN_HEIGHT(s) (s.r)
			#define FULL_HEIGHT(s) (TERRAIN_HEIGHT(s) + WATER_HEIGHT(s))

            v2f vert (appdata_base v)
            {
                float4 state = tex2Dlod(_StateTex, float4(v.texcoord.x, v.texcoord.y, 0, 0)); 
                v.vertex.y += TERRAIN_HEIGHT(state);

                v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord * _MainTex_ST.xy + _MainTex_ST.zw;				

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                return col;
            }
            ENDCG
        }
    }    
}
