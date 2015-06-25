using UnityEngine;
using System.Collections.Generic;

public class Crossfader : MonoBehaviour 
{

	public Texture FadeTexture;
	//public SpriteRenderer CrossfadeWith;
	[Range(0, 1)]
	public float FadeValue = 0f;

	private SpriteRenderer _renderer;
	MaterialPropertyBlock _mpb;

	void Awake()
	{
		_mpb = new MaterialPropertyBlock();
		_renderer = GetComponent<SpriteRenderer>();
		_renderer.GetPropertyBlock(_mpb);
		_mpb.SetTexture("_Texture2", FadeTexture);
		_renderer.SetPropertyBlock(_mpb);
	}

	void Update()
	{
		//_renderer.SetPropertyBlock(_mpb);

		//dest.uv
		/*
		Color c = _renderer.color;
		c.a = 1.0f - FadeValue;
		_renderer.color = c;
		c = CrossfadeWith.color;
		c.a = FadeValue;
		CrossfadeWith.color = c;
		*/
	}
}
