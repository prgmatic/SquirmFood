Shader "Sprite/Crossfade"
{
	Properties
	{
		_Blend("Blend", Range(0, 1)) = 0.5
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		[PerRendererData] _Texture2("Texture 2", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#include "UnityCG.cginc"

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				float2 texcoord2 : TEXCOORD2;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color : COLOR;
				half2 texcoord  : TEXCOORD0;
				half2 texcoord2 : TEXCOORD2;
			};

			fixed4 _Color;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.texcoord2 = IN.texcoord2;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap(OUT.vertex);
				#endif
				return OUT;
			}
			sampler2D _MainTex;
			sampler2D _Texture2;
			float _Blend;
			fixed4 frag(v2f IN) : SV_Target
			{
				//fixed4 c = tex2D(_MainTex, IN.texcoord) * IN.color;
				//c.rgb *= c.a;
				//return c;
				fixed4 t1 = tex2D(_MainTex, IN.texcoord) * _Color;
				t1.rgb *= t1.a;
				//return t1;
				fixed4 t2 = tex2D(_Texture2, IN.texcoord2) * _Color;
				t2.rgb *= t2.a;
				return lerp(t1, t2, _Blend);
			}
			ENDCG
		}
	}
}
