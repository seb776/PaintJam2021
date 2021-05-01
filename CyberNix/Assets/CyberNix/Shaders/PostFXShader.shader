Shader "Unlit/PostFXShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_PixelSize ("PixelSize", Range(0,1)) = 0.1
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

            sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _MainTex_TexelSize;
			float _PixelSize;

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

            fixed4 frag (v2f i) : SV_Target
            {
				fixed2 uv = i.uv;
				float pxs = _PixelSize;
				fixed2 px = fixed2(pxs, pxs)*(_MainTex_TexelSize.zz) / _MainTex_TexelSize.zw;
				fixed2 puv = floor(uv / px)*px;
				fixed4 col = tex2D(_MainTex, uv);
                UNITY_APPLY_FOG(i.fogCoord, col);

				col = pixelize(uv);

                return col;
            }
            ENDCG
        }
    }
}
