Shader "Unlit/RocketEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

			float _cir(fixed2 uv, float r)
			{
				return length(uv) - r;
			}

			fixed4 BRRRR(fixed2 uv, float shp)
			{
				float acc = 100.;

				acc = min(acc, _cir(uv, .25));

				for (int i = 0; i < 30; ++i)
				{
					float f = float(i);
					float s = lerp(0.05,0.2, sin(f)*.5+.5);
					fixed2 dir = fixed2(fmod(_Time.x*70.+f+sin(f), 0.5), sin(f)*.1*abs(uv.x));
					acc = min(acc, _cir(uv + dir,s));
				}
				
				return saturate(fixed4(fixed3(227, 43, 62)/255., 1. - saturate(acc*shp)));
			}

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
				fixed4 col = fixed4(0.,0.,0.,0.);// tex2D(_MainTex, i.uv);
				col = BRRRR(i.uv-fixed2(1.0,0.5),400.);
				fixed4 white = BRRRR(2.*(i.uv - fixed2(1.0, 0.5)),10.).xxxw;
				col += fixed4(fixed3(1.,1.,1.)*white.w*.25, col.w);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

                return col;
            }
            ENDCG
        }
    }
}
