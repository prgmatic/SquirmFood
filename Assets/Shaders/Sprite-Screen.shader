// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Sprite-Screen" 
{
	Properties 
	{
		[PerRendererData]
		_MainTex("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)

		[MaterialToggle]
		PixelSnap ("PixelSnap", Float) = 0
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
		Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha

		GrabPass { }

		Pass
		{
			CGPROGRAM
	
			#include "UnityCG.cginc"
			#pragma vertex ScreenVertex
			#pragma fragment ScreenFragment

			sampler2D _MainTex;
			sampler2D _GrabTexture;
			fixed4 _Color;

			struct VertexInput
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct VertexOutput
			{
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				half2 texcoord : TEXCOORD0;
				float4 screenPos : TEXCOORD1;
			};

			VertexOutput ScreenVertex(VertexInput input)
			{
				VertexOutput output;
				output.vertex = UnityObjectToClipPos(input.vertex);
				output.screenPos = output.vertex;
				output.texcoord = input.texcoord;
				output.color = input.color * _Color;
				#ifdef PIXELSNAP_ON
				output.vertex = UnityPixelSnap(output.vertex);
				#endif
				return output;
			}

			fixed4 ScreenFragment(VertexOutput input) : SV_Target
			{
				half4 color = tex2D(_MainTex, input.texcoord);

				float2 grabTexcoord = input.screenPos.xy / input.screenPos.w;
				grabTexcoord.x = (grabTexcoord.x + 1.0) * .5;
				grabTexcoord.y = (grabTexcoord.y + 1.0) * .5;
				grabTexcoord.y = 1.0 - grabTexcoord.y;

				fixed4 grabColor = tex2D(_GrabTexture, grabTexcoord);
				fixed4 result = 1.0 - (1.0 - grabColor) * (1.0 - color);
				result.a = color.a;
				return result;
			}
			ENDCG
		}
	} 
	FallBack "Sprites/Diffuse"
}