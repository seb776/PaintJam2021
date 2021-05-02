Shader "Unlit/PostFXShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_PixelSize ("PixelSize", Range(0,1)) = 0.1

		_Threshold("Threshold", Range(0,1)) = 0.5 // Bloom options
		_Radius("Radius", Range(1,500)) = 1
		_Intensity("Intensity", Range(0,1)) = 0.5

		_NoiseTex("Noise", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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

			sampler2D _NoiseTex;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _MainTex_TexelSize;
			float _PixelSize;

			float _Blur, _Threshold, _Radius, _Intensity;

#define pow2(x) (x * x)

			half3 linearToneMapping(half3 color)
			{
				float exposure = 1.;
				color = clamp(exposure * color, 0., 1.);
				color = pow(color, half3(1. / 2.2, 1. / 2.2, 1. / 2.2));
				return color;
			}

			float gaussian(half2 i) {
				float pi = atan(1.0) * 4.0;
				float sigma = float(_Radius) * 0.25;

				return 1.0 / (2.0 * pi * pow2(sigma)) * exp(-((pow2(i.x) + pow2(i.y)) / (2.0 * pow2(sigma))));
			}

			half3 blur(sampler2D sp, half2 uv, half2 scale) {
				half3 col = half3(0.0, 0.0, 0.0);
				float accum = 0.0;
				float weight;
				half2 offset;
				float sigma = float(_Radius) * 0.25;

				for (int x = -sigma / 2; x < sigma / 2; x += 3) {
					for (int y = -sigma / 2; y < sigma / 2; y += 3) {
						offset = half2(x, y);
						weight = gaussian(offset);
						half3 smple = tex2D(sp, uv + scale * offset).rgb;
						float luminance = 0.2126 * smple.x + 0.7152 * smple.y + 0.0722 * smple.z;
						if (luminance > _Threshold)
							col += smple * weight;
						accum += weight;
					}
				}

				return col / accum;
			}

			half3 Bloom(half2 uv, float threshold, float radius, float intensity, half2 ps)
			{
				half3 col = half3(0., 0., 0.);
				col = tex2D(_MainTex, uv).xyz;

				half3 bloomSample = blur(_MainTex, uv, ps);

				col = col + (bloomSample * intensity);
				return col;
			}

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

			fixed4 pixelize(fixed2 uv)
			{
				float pxs = _PixelSize;
				fixed2 px = fixed2(pxs, pxs)*(_MainTex_TexelSize.zz) / _MainTex_TexelSize.zw;
				fixed2 puv = floor(uv / px)*px;
				fixed4 col = tex2D(_MainTex, puv);
				return col;
			}
			float2x2 r2d(float a) { float cosa = cos(a); float sina = sin(a); return float2x2(cosa, -sina, sina, cosa); }
			float lenny(fixed2 v) { return abs(v.x) + abs(v.y); }
            fixed4 frag (v2f i) : SV_Target
            {
				fixed2 uv = i.uv;
				float pxs = _PixelSize;
				fixed2 px = fixed2(pxs, pxs)*(_MainTex_TexelSize.zz) / _MainTex_TexelSize.zw;
				fixed2 puv = floor(uv / px)*px;
				fixed4 col = tex2D(_MainTex, uv);
                UNITY_APPLY_FOG(i.fogCoord, col);

				//col = pixelize(uv);

				half2 ps = half2(1.0, 1.0) / half2(_MainTex_TexelSize.z, _MainTex_TexelSize.w);
				col = fixed4(Bloom(puv, _Threshold, _Radius, _Intensity, ps),col.w);

				col += .25*saturate(col+.2)*pow(tex2D(_NoiseTex, mul((i.uv + _Time.xx*20.), r2d(-.5))*fixed2(1.0, 0.1)*2.).x,15.)*fixed4(1.,1.,1.,1.);
				col += .5*saturate(col + .2)*pow(tex2D(_NoiseTex, mul((i.uv + _Time.xx*50.), r2d(-.5))*fixed2(1.0, 0.1)*1.).x, 15.)*fixed4(1., 1., 1., 1.);

				col += .25*(fixed4(240, 42, 81, 255) / 255.).yxzw*(1. - saturate(lenny(puv-fixed2(.5,.5))));
                return col;
            }
            ENDCG
        }
    }
}
