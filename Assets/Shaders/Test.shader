Shader "Sunspots" 
{
	Properties
	{
		_MainTex("Texture1 (RGB)", 2D) = "white" {}
		_Color("Main Color", Color) = (1,1,1,1)
	}

	Category
	{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		Lighting Off

		//screen 1 - (1 - a) * (1 - b)

		SubShader
		{
			Pass
			{
				ZWrite Off
				Cull Off
				Blend One One
				BlendOp Sub
				SetTexture[_MainTex]
				{
					constantColor(1,1,1,1)
					Combine constant
				}
			}

			Pass
			{
				ZWrite Off
				Cull Off
				Blend Zero OneMinusSrcColor
				BlendOp Add
				SetTexture[_MainTex]
				{
					constantColor[_Color]
					Combine texture * constant
				}
			}
			Pass
			{
				ZWrite Off
				Cull Off
				Blend One One
				BlendOp Sub
				SetTexture[_MainTex]
				{
					constantColor[_Color]
				}
				SetTexture[_MainTex]
				{
					constantColor(1,1,1,1)
					Combine constant, previous
				}
			}
		}
	}
}