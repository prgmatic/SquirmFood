using UnityEngine;
using System.Collections.Generic;

public class PipeSpriteRenderer : MonoBehaviour 
{
	public float Scale = 1f;
	public Camera Camera;

	private SpriteRenderer[] _renderers;

	public void Awake()
	{
		var sprites = Resources.LoadAll<Sprite>("Pipes4");
		_renderers = new SpriteRenderer[sprites.Length];
		var rot = Quaternion.Euler(Camera.transform.rotation.eulerAngles.x, 0, 0);
		for(int i = 0; i < _renderers.Length; i++)
		{
			var go = new GameObject();
			go.name = "Pipe Row";
			go.layer = this.gameObject.layer;
			go.transform.rotation = rot;
			go.transform.SetParent(this.transform);
			_renderers[i] = go.AddComponent<SpriteRenderer>();
			var r = _renderers[i];
			r.sortingOrder = (_renderers.Length - i) * 10;
			r.sprite = sprites[i];
		}
		UpdateSpriteTransforms();
	}

	private void UpdateSpriteTransforms()
	{
		foreach(var r in _renderers)
		{
			var up = r.transform.up; //* this.transform.localScale.x;
			r.transform.localScale = Vector3.one;
			r.transform.localPosition = up * (-(float)r.sprite.texture.height / 2 / r.sprite.pixelsPerUnit);
			r.transform.localPosition += up * (r.sprite.rect.center.y / r.sprite.pixelsPerUnit);
		}
	}
}
