Shader "Unlit/ChromaKeyUnlit"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_ChromaKey("Chroma key", Color) = (1.0,0.0,0.0,1.0)
		_Threshold("Threshold", Range(0,1)) = 0.5
		_ColorShade("Color", Color) = (1.0,1.0,1.0,1.0)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        ZWrite On
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
			float4 _ChromaKey;
			float4 _ColorShade;
			float _Threshold;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

				if (length(col.xyz - _ChromaKey.xyz) < _Threshold)
					return fixed4(col.xyz, 0.0);

                return fixed4(col.xyz * _ColorShade, 1.0);
            }
            ENDCG
        }
    }
}
