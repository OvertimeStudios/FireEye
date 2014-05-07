using UnityEngine;
using System.Collections;

public class AutoDestruct : MonoBehaviour 
{
	public enum Mode
	{
		FadeOut,
		Destroy
	}

	public Mode mode;

	public float time;

	private SpriteRenderer spriteRenderer;
	private float initialAlpha;

	void OnEnable()
	{
		StartCoroutine(Remove(time));
	}

	void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		initialAlpha = spriteRenderer.color.a;
	}

	void Update()
	{
		if(mode == Mode.FadeOut)
		{
			Color color = spriteRenderer.color;
			color.a -= Time.deltaTime * initialAlpha / time;

			spriteRenderer.color = color;
		}
	}

	IEnumerator Remove(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);

		Color color = spriteRenderer.color;
		color.a = initialAlpha;
		spriteRenderer.color = color;

		transform.parent.gameObject.SetActive(false);
	}
}
