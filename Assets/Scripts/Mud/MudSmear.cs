using UnityEngine;
using System.Collections.Generic;

public class MudSmear : MonoBehaviour 
{
	[Range (0f, 1f)]
	public float Expansion = 0f;
	public int X = 5;
	public int Y = 4;
	SpriteRenderer _renderer;

	void Awake()
	{
		_renderer = GetComponentInChildren<SpriteRenderer>();
	}

	void Update()
	{
		float xScale = 0.5f * Expansion + 0.5f;
		transform.localScale = transform.localScale.SetX(xScale);
		Vector3 expandOffset = transform.right * Expansion / 2;
		this.transform.position = Gameboard.Instance.GridPositionToWorldPosition(X, Y) + new Vector3(0.5f, -0.5f) + expandOffset;
	}

	public void Animate(float animateTime)
	{
		StartCoroutine("AnimationRoutine", animateTime);
	}

	private System.Collections.IEnumerator AnimationRoutine(float animateTime)
	{
		float time = 0f;
		float targetTime = animateTime;

		while(time < targetTime)
		{
			time += Time.deltaTime;
			Expansion = Mathf.Lerp(0, 1, time / targetTime);
			yield return null;
		}
		Expansion = 1f;
	}
}
